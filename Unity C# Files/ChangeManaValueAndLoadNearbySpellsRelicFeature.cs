using GameLogic.Scripts.EventBus.Events;

public class ChangeManaValueAndLoadNearbySpellsRelicFeature : AddManaRelicFeatureBase
{
    public IncreaseManaAndSendToDirectionRelicFeature.Direction direction = IncreaseManaAndSendToDirectionRelicFeature.Direction.Nearby;
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter)
            .Replace("<direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        int manaToAdd = (int)value;
        eventBus.Publish(new OnRequestAddManaToSpellSlotOnDirection(onAddMana.spellSlot, manaToAdd, onAddMana.delay, direction, onAddMana.preview));
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
