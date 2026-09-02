using Assets.SimpleLocalization;
using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public abstract class ActivateRelicFeatureBase: RelicFeature
{
    public int cooldown = 1;
    int currentCooldown = 0;
    bool onCooldown = false;
    protected GameObject fxActivate
    {
        get
        {
            if (_fxActivate == null) {
                SpellSlot spellSlot = GetSpellSlot();
                if (spellSlot != null)
                {
                    _fxActivate = spellSlot.fxReadyActivate;
                }
            }
            return _fxActivate;
        }
    }
    GameObject _fxActivate;

    public int GetBaseCooldown()
    {
        return cooldown;
    }

    public int GetCurrentCooldown()
    {
        return onCooldown ? currentCooldown : 0;
    }

    protected override string AddPreDescriptionValues()
    {
        string description;
        if (BattleGUI.instance.IsInBattle())
        {
            description = TooltipColorManager.SurroundTextAndLeaveNormalTextAfter(TooltipColorManager.HighlightCharacter2 + LocalizationManager.Localize("Cooldown") + ": (" + GetCurrentCooldown())+"): ";
        }
        else
        {
            description = TooltipColorManager.SurroundTextAndLeaveNormalTextAfter(TooltipColorManager.HighlightCharacter2 + LocalizationManager.Localize("Cooldown") + " (" + GetBaseCooldown())+"): ";
        }
        return description;
    }

    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnActivate>(OnActivate);
        eventBus.Subscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
  }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnActivate>(OnActivate);
        eventBus.Unsubscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Unsubscribe<OnTurnStarted>(OnTurnStarted);
    }

    private void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        _fxActivate = null;
        onCooldown = false;
        currentCooldown = 0;
        fxActivate.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        fxActivate.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        fxActivate.gameObject.SetActive(false);
    }

    private void OnActivate(OnActivate onActivate)
    {
        if (onCooldown && !onActivate.ignoreActivateLimit)
        {
            return;
        }
        
        if(GetSpellSlot() != onActivate.spellSlot)
        {
            return;
        }

        if (!onActivate.preview)
        {
            GetSpellSlot().fxActivate.gameObject.SetActive(true);
            onCooldown = true;
            currentCooldown = cooldown;
            fxActivate.gameObject.SetActive(false);
            GetSpellSlot().RefreshTooltip();
        }
        ActivateEffects(onActivate);
    }

    protected abstract void ActivateEffects(OnActivate onActivate);

    private void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        if (currentCooldown > 1 && onCooldown)
        {
            currentCooldown--;
        }
        else
        {
            currentCooldown = cooldown;
            onCooldown = false;
            fxActivate.gameObject.SetActive(true);
        }
        GetSpellSlot().RefreshTooltip();
    }
}