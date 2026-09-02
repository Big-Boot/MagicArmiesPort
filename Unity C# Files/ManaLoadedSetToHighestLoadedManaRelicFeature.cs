using GameLogic.Scripts.EventBus.Events;

public class ManaLoadedSetToHighestLoadedManaRelicFeature : AddManaRelicFeatureBase
{
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        int highestLoadedMana = GetSpellSlot().GetHighestLoadedMana();
        if (highestLoadedMana>onAddMana.amount)
        {
            onAddMana.spellSlot.IncreaseCurrentPower(highestLoadedMana - onAddMana.amount, onAddMana.preview);
        }
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
