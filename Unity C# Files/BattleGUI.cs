using DG.Tweening;
using GameLogic.Scripts.EventBus.Events;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Lane;

public class BattleGUI : BasicMonobehaviour
{
    public GameObject goContainer;
    public GameObject goLaneContainer;
    public GameObject goBattleCharacters;

    Lane cachedWeakestLane = null;
    List<Lane> lLanes;
    List<PowerOutcome> lPowerOutcomes;
    RectTransform rtLane;
    public Button btEndTurn;
    public Button btFlee;

    Vector3 initialPosition;

    public List<SpellSlot> lSpellSlot;

    public static BattleGUI instance;

    public int turn = 0;
    public BattleGUI()
    {
        instance = this;
    }

    public Lane GetLaneOnPlacement(LanePlacement lanePlacement)
    {
        foreach (Lane lane in GetLanes())
        {
            if (lane.GetLanePlacement() == lanePlacement)
            {
                return lane;
            }
        }
        return null;
    }

    public bool IsInBattle()
    {
        return goContainer.activeSelf;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
        eventBus.Subscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Subscribe<OnBattleEnded>(OnBattleEnded);
        eventBus.Subscribe<OnAddMana>(OnAddMana);
        eventBus.Subscribe<OnResetPreview>(OnResetPreview);

        foreach (Lane lane in GetComponentsInChildren<Lane>(true))
        {
            lane.Init();
        }

        foreach (AbstractSpellSlot ass in lSpellSlot)
        {
            ass.Init();
        }

        GetComponentInChildren<ManaManager>(true).Init();
        //GetComponentInChildren<PlayerHPBar>(true).Init();
        //GetComponentInChildren<EnemyHPBar>(true).Init();

        rtLane = goLaneContainer.GetComponent<RectTransform>();
        initialPosition = rtLane.anchoredPosition;
        //lSpellSlot = new List<SpellSlot>(GetComponentsInChildren<SpellSlot>(true));
        lLanes = new List<Lane>(goLaneContainer.GetComponentsInChildren<Lane>(true));
        lPowerOutcomes = new List<PowerOutcome>(goLaneContainer.GetComponentsInChildren<PowerOutcome>(true));
    }

    private void OnAddMana(OnAddMana onAddMana)
    {
        ClearLaneCache();
    }

    public List<PowerOutcome> GetPowerOutcomes()
    {
        return lPowerOutcomes;
    }

    private void ClearLaneCache()
    {
        cachedWeakestLane = null;
    }

    private void OnResetPreview(OnResetPreview onResetPreview)
    {
        ClearLaneCache();
    }

    public Lane GetWeakestLane(bool preview)
    {
        if (cachedWeakestLane == null)
        {
            Lane weakestLane = lLanes[0];
            int minPlayerDamage = weakestLane.GetCurrentDamage(preview);
            for (int i = 1; i < lLanes.Count; i++)
            {
                if (lLanes[i].GetCurrentDamage(preview) < minPlayerDamage)
                {
                    weakestLane = lLanes[i];
                }
            }
            cachedWeakestLane = weakestLane;
            eventBus.Publish(new OnForceRefreshAllLanesValues(preview));
        }
        return cachedWeakestLane;
    }

    public List<Lane> GetLanes()
    {
        return lLanes;
    }

    public void OnBattleEnded(OnBattleEnded onBattleEnded)
    {
        goContainer.SetActive(false);
        goBattleCharacters.SetActive(false);
    }

    public void Flee()
    {
        goContainer.SetActive(false);
        goBattleCharacters.SetActive(false);
        PlayerDataInstaller.ResetData();
    }

    public void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        turn = 0;
        goContainer.SetActive(true);
        goBattleCharacters.SetActive(true);

        foreach (AbstractSpellSlot ass in lSpellSlot)
        {
            ass.OnEnable();
        }

        rtLane.anchoredPosition = new Vector2(initialPosition.x, -1000);
        rtLane.DOAnchorPosY(initialPosition.y, .5f)
              .SetEase(Ease.OutQuad);

        StartCoroutine(DelayFirstTurnAndBuff());
    }

    private IEnumerator DelayFirstTurnAndBuff()
    {
        yield return null;
        eventBus.Publish(new OnRequestAddBuffsOnBattleStart());
        yield return null;
        eventBus.Publish(new OnTurnStarted());

        eventBus.Publish(new OnRefreshEnemyHP());
        eventBus.Publish(new OnRefreshPlayerHP());

    }

    public void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        btFlee.interactable = !PlayerData.instance.IsFirstTutorialBattle();
        btFlee.gameObject.SetActive(!PlayerData.instance.IsFirstTutorialBattle());
        btEndTurn.interactable = true;
        btEndTurn.gameObject.SetActive(true);


        if (GameManager.instance.enemy.lDialoguesPerTurn.Count > turn)
        {
            if (!GameManager.instance.enemy.lDialoguesPerTurn[turn].Equals(""))
            {
                DialogueGUI.instance.LoadDialogueAndAddToQueue(GameManager.instance.enemy.lDialoguesPerTurn[turn]);
            }
        }
    }

    public void EndTurn()
    {
        turn++;
        StartCoroutine(EndTurnCoroutine());
    }

    private IEnumerator EndTurnCoroutine()
    {
        btFlee.interactable = false;
        btFlee.gameObject.SetActive(false);
        btEndTurn.interactable = false;
        btEndTurn.gameObject.SetActive(false);
        rtLane.DOAnchorPosY(initialPosition.y + 1000, .5f)
              .SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(.5f);
        eventBus.Publish(new OnTurnEnded());
        //here the ball animation starts
        yield return new WaitForSeconds(5f);
        //balls should have collided here, so show damage
        eventBus.Publish(new OnRefreshEnemyHP());
        eventBus.Publish(new OnRefreshPlayerHP());
        yield return new WaitForSeconds(.5f);
        GameManager.instance.CheckIfBattleEnded();
        if(GameManager.instance.GetBattleEnded() == GameManager.BattleResult.None) 
        { 
            yield return new WaitForSeconds(2f);
            eventBus.Publish(new OnTurnStarted());
            rtLane.anchoredPosition = new Vector2(initialPosition.x, -1000);
            rtLane.DOAnchorPosY(initialPosition.y, .5f)
                  .SetEase(Ease.OutQuad);
        }
        else
        {
            StartCoroutine(ResolveBattleEnd());
        }
    }

    private IEnumerator ResolveBattleEnd()
    {
        yield return new WaitForSeconds(1f);
        switch (GameManager.instance.GetBattleEnded())
        {
            case GameManager.BattleResult.PlayerVictory:
                if (PlayerData.instance.areaLevel == 4)
                {
                    eventBus.Publish(new OnShowGameOver("You-Win"));
                }
                else
                {
                    eventBus.Publish(new OnBattleEnded());
                }
                break;
            case GameManager.BattleResult.EnemyVictory:
            case GameManager.BattleResult.Tie:
                eventBus.Publish(new OnShowGameOver("You-Lose"));
                break;
        }
    }

}
