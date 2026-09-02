using GameLogic.Scripts.EventBus.Events;

public class IncreaseEvenManaValueRelicFeature : AddManaRelicFeatureBase
{
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        if (onAddMana.amount % 2 == 1)
        {
            return; // Only increase even mana values
        }
        onAddMana.spellSlot.IncreaseCurrentPower((int)value, onAddMana.preview);
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
