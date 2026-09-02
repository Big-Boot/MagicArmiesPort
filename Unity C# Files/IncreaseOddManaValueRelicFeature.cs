using GameLogic.Scripts.EventBus.Events;

public class IncreaseOddManaValueRelicFeature : AddManaRelicFeatureBase
{
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        if (onAddMana.spellSlot.spellOwned.spellModel != model)
        {
            return;
        }
        if (onAddMana.amount % 2 == 0)
        {
            return; // Only increase odd mana values
        }
        onAddMana.spellSlot.IncreaseCurrentPower((int)value, onAddMana.preview);
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
