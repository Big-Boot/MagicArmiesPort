using GameLogic.Scripts.EventBus.Events;
using UnityEngine;
using static IncreaseManaAndSendToDirectionRelicFeature;

public class WhenOtherUnitsLoadManaGainPowerRelicFeature : RelicFeature
{
    public Direction direction = Direction.Nearby;

    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnAddMana>(OnAddMana);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnAddMana>(OnAddMana);
    }

    public override string AddDescriptionValues(string baseString)
    {
        return baseString
            .Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter)
            .Replace("<in-direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfInDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }

    private void OnAddMana(OnAddMana onAddMana)
    {
        if (!IsValidSpellSlot(onAddMana.spellSlot, onAddMana.preview))
        {
            return;
        }
        GainPower(onAddMana.preview);
    }

    private bool IsValidSpellSlot(SpellSlot spellSlot, bool includePreview)
    {
        if (spellSlot.spellOwned.spellModel != model && BigbootTools.IsInDirection(spellSlot, GetSpellSlot(), direction))
        {
            return true;
        }
        return false;
    }

    private void GainPower(bool preview)
    {
        GetSpellSlot().GetLane().AddDamage((int) value, preview);
        if (!preview)
        {
            GetSpellSlot().SendManaCharge(GetSpellSlot().GetLane().GetPlayerEnergyBall().gameObject, 0f);
        }
    }
}
