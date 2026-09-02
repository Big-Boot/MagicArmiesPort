using GameLogic.Scripts.EventBus.Events;
using TMPro;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class SpellSlot : AbstractSpellSlot
{
    public static float SEND_MANA_DELAY = .5f;

    public GameObject goMageSpriteContainer;
    public Animator goMageSpriteAnimator;

    Lane lane;
    public TMP_Text txLoadedPower;
    int currentPower = 0;
    int currentPreviewPower = 0;
    int previewMight = 0;
    int manaBlobsLoaded = 0;
    int previewManaBlobsLoaded = 0;
    [SerializeField] PowerOutcome powerOutcome;
    bool initialized = false;

    private int displayedPower = 0;
    private int minimumAnimatedPower = 0;

    public GameObject fxReadyActivate;
    public GameObject fxActivate;

    public GameObject prefabDistanceChargeEffect;
    public GameObject prefabDistanceBuffEffect;

    public GameObject prefabFullCharge;

    public List<ManaChargeItem> lPreloadedManaChargeItem = new List<ManaChargeItem>(10);
    int amountOfChargedItems = 0;

    public Lane GetLane()
    {
        return lane;
    }

    protected override void ClearManachargeItems()
    {
        foreach(ManaChargeItem manaChargeItem in lManaChargeItems)
        {
            manaChargeItem.gameObject.SetActive(false);
        }
        lManaChargeItems.Clear();
        amountOfChargedItems = 0;
    }

    protected override void AddManaChargeItem()
    {
        if(amountOfChargedItems >= lPreloadedManaChargeItem.Count)
        {
            return;
        }

        lManaChargeItems.Add(lPreloadedManaChargeItem[amountOfChargedItems]);
        lPreloadedManaChargeItem[amountOfChargedItems].gameObject.SetActive(true);
        amountOfChargedItems ++;
    }

    public int IncreaseCurrentPower(int amount, bool preview)
    {
        if (preview)
        {
            currentPreviewPower += amount;
        }
        else
        {
            currentPower += amount;
        }
        if (spellOwned.spellModel.spellType == RelicModel.SpellType.Offensive)
        {
            //Debug.Log("Increasing power in SpellSlot " + spellOwned.spellName + " by " + amount + " preview: " + preview);
            powerOutcome.AddPower(amount, preview);
        }
        RefreshValues(0);
        return currentPower;
    }

    public void Activate(bool preview)
    {
        eventBus.Publish(new OnActivate(false, this, preview, 0));
    }

    public int GetCurrentPower(bool preview)
    {
        if(preview)
        {
            return currentPower + currentPreviewPower;
        }

        return currentPower;
    }

    protected override void Unload()
    {
        base.Unload();
        goMageSpriteContainer.SetActive(loaded);
        if (GetComponent<SpellSlotActivateFeature>() != null)
        {
            Destroy(GetComponent<SpellSlotActivateFeature>());
        }
    }

    public override void LoadCurrentSpell()
    {
        fxReadyActivate.SetActive(false);
        base.LoadCurrentSpell();
        goMageSpriteContainer.SetActive(loaded);
    }

    public override void LoadSpell(SpellData spellOwned)
    {
        base.LoadSpell(spellOwned);
        if (GetComponent<SpellSlotActivateFeature>() != null)
        {
            Destroy(GetComponent<SpellSlotActivateFeature>());
        }
        if (loaded)
        {
            if (spellOwned.spellModel.IsActivable())
            {
                this.transform.AddComponent<SpellSlotActivateFeature>();
            }

            //TODO load sprite of the mage here
            goMageSpriteContainer.SetActive(true);
            goMageSpriteAnimator.runtimeAnimatorController = spellOwned.spellModel.runtimeAnimator;
        }
    }

    public override void Init()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;

        lane = powerOutcome.GetComponentInParent<Lane>(true);
        goMageSpriteContainer.gameObject.SetActive(loaded);
        eventBus.Subscribe<OnGameStarted>(OnGameStarted);
        RefreshValues(0);
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
        eventBus.Subscribe<OnTurnEnded>(OnTurnEnded);
        eventBus.Subscribe<OnRequestAddManaToSpellSlotOnDirection>(OnRequestAddManaToSpellSlotOnDirection);
        eventBus.Subscribe<OnResetPreview>(OnResetPreview);
        eventBus.Subscribe<OnRequestAddMightToSpellSlotOnDirection>(OnRequestAddMightToSpellSlotOnDirection);
    }

    private void OnRequestAddMightToSpellSlotOnDirection(OnRequestAddMightToSpellSlotOnDirection onRequestAddMightToSpellSlotOnDirection)
    {
        if (!loaded)
        {
            return;
        }
        if (spellOwned.spellModel.spellType == RelicModel.SpellType.Utility)
        {
            return;
        }

        bool addMight = BigbootTools.IsInDirection(onRequestAddMightToSpellSlotOnDirection.sourceSpellSlot, this, onRequestAddMightToSpellSlotOnDirection.direction);

        if (addMight)
        {
            AddMight(onRequestAddMightToSpellSlotOnDirection.might, onRequestAddMightToSpellSlotOnDirection.preview, onRequestAddMightToSpellSlotOnDirection.delay + SEND_MANA_DELAY);
            if (!onRequestAddMightToSpellSlotOnDirection.preview)
            {
                StartCoroutine(SendBuffCharge(onRequestAddMightToSpellSlotOnDirection.sourceSpellSlot.gameObject, this.gameObject, onRequestAddMightToSpellSlotOnDirection.delay));
            }

        }
    }

    public void AddMight(int might, bool preview, float delay)
    {
        if (spellOwned.spellModel.spellType == RelicModel.SpellType.Utility)
        {
            //no might on support
            return;
        }

        if (preview)
        {
            if (GetLoadedManaBlobsLoaded(preview) > 0)
            {
                currentPreviewPower += might;
                powerOutcome.AddPower(might, preview);
                RefreshValues(delay);
            }
        }
        else
        {
            if (GetLoadedManaBlobsLoaded(preview) > 0)
            {
                currentPower += might;
                powerOutcome.AddPower(might, preview);
                RefreshValues(delay);
            }
        }

        if (preview)
        {
            this.previewMight += might;
        }
        else
        {
            this.might += might;
        }

        RefreshTooltip();
    }

    public override void RefreshTooltip(int extraMight = -1)
    {
        base.RefreshTooltip(this.might);
    }

    public void OnResetPreview(OnResetPreview onResetPreview)
    {
        previewManaBlobsLoaded = 0;
        currentPreviewPower = 0;
        previewMight = 0;
        RefreshValues(0);
    }
    /*public void OnRequestAddManaToSpellSlot(OnRequestAddManaToSpellSlot onRequestAddManaToSpellSlot)
    {
        if (onRequestAddManaToSpellSlot.row == rowNumber && onRequestAddManaToSpellSlot.column == columnNumber && loaded && !IsFullyLoaded(onRequestAddManaToSpellSlot.preview))
        {
            LoadMana(onRequestAddManaToSpellSlot.mana, false, onRequestAddManaToSpellSlot.preview, onRequestAddManaToSpellSlot.delay + SEND_MANA_DELAY);
            if (!onRequestAddManaToSpellSlot.preview)
            {
                StartCoroutine(SendManaCharge(onRequestAddManaToSpellSlot.sourceSpellSlot.gameObject, this.gameObject, onRequestAddManaToSpellSlot.delay));
            }
        }
    }*/

    public void OnRequestAddManaToSpellSlotOnDirection(OnRequestAddManaToSpellSlotOnDirection onRequestAddManaToSpellSlotOnDirection)
    {
        if (!loaded)
        {
            return;
        }
        if (IsFullyLoaded(onRequestAddManaToSpellSlotOnDirection.preview))
        {
            return;
        }

        bool addMana = BigbootTools.IsInDirection(onRequestAddManaToSpellSlotOnDirection.sourceSpellSlot, this, onRequestAddManaToSpellSlotOnDirection.direction);

        if (addMana)
        {
            LoadMana(onRequestAddManaToSpellSlotOnDirection.mana, false, onRequestAddManaToSpellSlotOnDirection.preview, onRequestAddManaToSpellSlotOnDirection.delay + SEND_MANA_DELAY);
            if (!onRequestAddManaToSpellSlotOnDirection.preview)
            {
                StartCoroutine(SendManaCharge(onRequestAddManaToSpellSlotOnDirection.sourceSpellSlot.gameObject, this.gameObject, onRequestAddManaToSpellSlotOnDirection.delay));
            }

        }
    }

    public void SendManaCharge(GameObject to, float delay)
    {
        StartCoroutine(SendManaCharge(this.gameObject, to, delay));
    }
    private IEnumerator SendManaCharge(GameObject from, GameObject to, float delay)
    {
        yield return new WaitForSeconds(delay);
        ManaChargeVFX manaChargeVFX = Instantiate(prefabDistanceChargeEffect, from.transform).GetComponent<ManaChargeVFX>();
        manaChargeVFX.SetTarget(to);
        manaChargeVFX.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private IEnumerator SendBuffCharge(GameObject from, GameObject to, float delay)
    {
        yield return new WaitForSeconds(delay);
        ManaChargeVFX manaChargeVFX = Instantiate(prefabDistanceBuffEffect, from.transform).GetComponent<ManaChargeVFX>();
        manaChargeVFX.SetTarget(to);
        manaChargeVFX.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void OnTurnEnded(OnTurnEnded onTurnEnded)
    {
        if (currentPower == 0)
        {
            return;
        }
       /* //for (int i = 0; i < spellOwned.spellModel.spellAmount; i++)
        //{
        if (spellOwned.spellModel.effectPrefab != null)
        {
            StartCoroutine(DelayedActivation(spellOwned.spellModel.effectPrefab, 0));//i * spellOwned.spellModel.spellDelay););
        }
        //}
       */
    }

   /* public IEnumerator DelayedActivation(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay + UnityEngine.Random.Range(0, 50f) / 100f);
        GameObject goFX = Instantiate(effect, lane.goEnemyContainer.transform);
    }*/

    private void Start()
    {
        Init();
    }

    public void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        ResetMana();
    }

    private void ResetMana()
    {
        previewManaBlobsLoaded = 0;
        manaBlobsLoaded = 0;
        currentPower = 0;
        minimumAnimatedPower = 0;
        RefreshValues(0);
        foreach (ManaChargeItem manaChargeItem in lManaChargeItems)
        {
            manaChargeItem.ResetMana();
        }
    }

    public int GetHighestLoadedMana()
    {
        if (lManaChargeItems.Count == 0)
        {
            return 0;
        }

        int highestLoadedMana = 0;

        foreach (ManaChargeItem m in lManaChargeItems)
        {
            if (highestLoadedMana < m.GetLoadedMana())
            {
                highestLoadedMana = m.GetLoadedMana();
            }
        }
        return highestLoadedMana;
    }

    public int GetLoadedManaBlobsLoaded(bool includePreview = true)
    {
        if (includePreview)
        {
            return manaBlobsLoaded + previewManaBlobsLoaded;
        }
        else
        {
            return manaBlobsLoaded;
        }
    }

    public bool IsFullyLoaded(bool includePreview = true)
    {
        if (includePreview)
        {
            return spellOwned.spellModel.maxManaBlob <= manaBlobsLoaded + previewManaBlobsLoaded;
        }
        else
        {
            return spellOwned.spellModel.maxManaBlob <= manaBlobsLoaded;
        }
    }

    public void AddManaCharge(int mana, bool preview)
    {
        lManaChargeItems[manaBlobsLoaded + previewManaBlobsLoaded].SetMana(mana, preview);
        if (!preview)
        {
            manaBlobsLoaded++;
            if (IsFullyLoaded())
            {
                Instantiate(prefabFullCharge, transform);
            }
        }
        else
        {
            previewManaBlobsLoaded++;
        }
    }

    public int GetBonusGivenByMight(bool preview)
    {
        int extraPowerByMight = 0;
        if (GetLoadedManaBlobsLoaded(preview) == 0)
        {
            if (preview)
            {
                extraPowerByMight += might + previewMight;
            }
            else
            {
                extraPowerByMight += might;
            }
        }
        return extraPowerByMight;
    }

    public void LoadMana(int mana, bool addByHand, bool preview, float delay)
    {
        if (addByHand && !preview)
        {
            eventBus.Publish(new OnResetPreview());
        }

        int extraPowerByMight = GetBonusGivenByMight(preview);
    
        AddManaCharge(mana, preview);
        if (addByHand)
        {
            eventBus.Publish(new OnAddManaByHand(mana, extraPowerByMight, this, preview));
        }
        eventBus.Publish(new OnAddMana(mana, extraPowerByMight, this, addByHand, preview, delay));
        eventBus.Publish(new OnSpellSlotLoaded(this, preview));
        int previousPower = currentPower;
        if (preview)
        {
            currentPreviewPower += mana + extraPowerByMight;
        }
        else
        {
            currentPower += mana + extraPowerByMight;
        }

        if (spellOwned.spellModel.dealsDamage)
        {
            //Debug.Log("Adding power to Lane " + lane.name + " for spell " + spellOwned.spellName + " with power: " + (currentPower + currentPreviewPower - previousPower) + " preview: " + preview);
            powerOutcome.AddPower(mana + extraPowerByMight, preview);// currentPower + currentPreviewPower - previousPower, preview);
            if (!preview)
            {
                StartCoroutine(SendManaCharge(this.gameObject, powerOutcome.GetComponentInParent<Lane>().GetPlayerEnergyBall().gameObject, SEND_MANA_DELAY));

            }
            RefreshValues(delay);
            lane.RefreshResult(preview);
        }
    }

    private void RefreshValues(float delay)
    {
        if (spellOwned == null || spellOwned.spellName == "" || spellOwned.spellName == null)
        {
            txLoadedPower.gameObject.SetActive(false);
            displayedPower = 0;
        }
        else
        {
            int targetPower = currentPower + currentPreviewPower;

            DOTween.Kill("PowerTween" + rowNumber + columnNumber);       // Stop existing power tweens

            if (currentPreviewPower > 0)
            {
                txLoadedPower.text = targetPower > 0 ? targetPower.ToString() : "";
                displayedPower = targetPower;
            }
            else
            {
                if (targetPower <= 0)
                {
                    txLoadedPower.text = "";
                    displayedPower = 0;
                }
                else if (displayedPower != targetPower)
                {
                    /*if (targetPower > minimumAnimatedPower)
                    {
                        minimumAnimatedPower = targetPower;
                        DOTween.To(() => displayedPower, x =>
                        {
                            displayedPower = x;
                            txLoadedPower.text = displayedPower.ToString();
                        }, targetPower, 0.5f).SetId("PowerTween" + rowNumber + columnNumber).SetDelay(delay);
                    }
                    else
                    {
                        txLoadedPower.text = targetPower.ToString();
                    }*/
                    txLoadedPower.text = targetPower.ToString();
                    //txLoadedPower.transform.localScale = Vector3.one;
                    //txLoadedPower.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f, 10, 1);
                }
                else
                {
                    txLoadedPower.text = targetPower.ToString();
                    displayedPower = targetPower;
                }
            }

            txLoadedPower.gameObject.SetActive(spellOwned.spellModel.dealsDamage && targetPower != 0);
        }
    }

    public PowerOutcome GetPowerOutcome()
    {
        return powerOutcome;
    }
}
