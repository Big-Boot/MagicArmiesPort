using DG.Tweening;
using GameLogic.Scripts.EventBus.Events;
using TMPro;
using UnityEngine;

public class PowerOutcome : BasicMonobehaviour
{
    [SerializeField] TMP_Text txPower;
    int currentPower = 0;
    int currentPreviewPower = 0;
    Lane lane;

    int animatedToPower = 0;
    private void Start()
    {
        lane = GetComponentInParent<Lane>();
        ResetPower();
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
        eventBus.Subscribe<OnResetPreview>(OnResetPreview);
    }

    public int GetCurrentPower()
    {
        return currentPower + currentPreviewPower;
    }

    private void OnResetPreview(OnResetPreview onResetPreview)
    {
        currentPreviewPower = 0;
        RefreshText(false);
    }

    public void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        ResetPower();
    }

    public void ResetPower()
    {
        animatedToPower = 0;
        currentPower = 0;
        RefreshText(false);
    }

    public void AddPower(int power, bool preview)
    {
        if (preview)
        {
            currentPreviewPower += power;
        }
        else
        {
            currentPower += power;
        }
        RefreshText(preview);
    }

    public void RefreshText(bool preview)
    {
        if (preview)
        {
            txPower.text = (currentPower + currentPreviewPower + lane.bonusDamage + lane.previewBonusDamage).ToString();
        }
        else
        {
            txPower.text = (currentPower + lane.bonusDamage).ToString();
        }

        if(lane.multiplierDamageEnemyReceivesBonus>1)
        {
            txPower.text += "x" + lane.multiplierDamageEnemyReceivesBonus;
        }
        lane.RefreshResult(preview);

    }
}
