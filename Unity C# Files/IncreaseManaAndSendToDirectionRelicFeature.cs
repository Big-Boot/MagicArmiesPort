using GameLogic.Scripts.EventBus.Events;
using ItemSystem;
using UnityEngine;

public class IncreaseManaAndSendToDirectionRelicFeature : AddManaRelicFeatureBase
{
    public Direction direction;
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        SameRow,
        SameColumn,
        Nearby,
        Anywhere
    }

    public override string AddDescriptionValues(string baseString)
    {
        return baseString
            .Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter)
            .Replace("<direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }

    public override string GetUntranslatedType()
    {
        if (value > 0)
        {
            return base.GetUntranslatedType();
        }
        else
        {
            return "SendToDirectionRelicFeature";
        }
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        int newMana = onAddMana.amount + (int)value;
        eventBus.Publish(new OnRequestAddManaToSpellSlotOnDirection(onAddMana.spellSlot, (int)newMana, onAddMana.delay, direction, onAddMana.preview));
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {

    }
}
