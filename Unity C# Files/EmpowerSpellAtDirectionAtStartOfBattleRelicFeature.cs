using GameLogic.Scripts.EventBus.Events;
using UnityEngine;
using static IncreaseManaAndSendToDirectionRelicFeature;

public class EmpowerSpellAtDirectionAtStartOfBattleRelicFeature : RelicFeature
{
    public Direction direction;
    
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnRequestAddBuffsOnBattleStart>(OnRequestAddBuffsOnBattleStart);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnRequestAddBuffsOnBattleStart>(OnRequestAddBuffsOnBattleStart);
    }

    public override string AddDescriptionValues(string baseText)
    {
        return baseText
            .Replace("<0>", value.ToString())
            .Replace("<direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }

    private void OnRequestAddBuffsOnBattleStart(OnRequestAddBuffsOnBattleStart onRequestAddBuffsOnBattleStart)
    {
        SpellSlot spellSlot = GetSpellSlot();
        if(spellSlot == null)
        {
            Debug.LogWarning("Spell slot not found for the spell model: " + gameObject.name);
            return;
        }
        eventBus.Publish(new OnRequestAddMightToSpellSlotOnDirection(
            spellSlot,
            (int)value,
            0,
            direction,
            false)
        );
    }
}
