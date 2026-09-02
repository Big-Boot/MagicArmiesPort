using GameLogic.Scripts.EventBus.Events;
using static IncreaseManaAndSendToDirectionRelicFeature;

public class SendManaToDirectionRelicFeature : AddManaRelicFeatureBase
{
    public Direction direction;
   
    public override string AddDescriptionValues(string baseString)
    {
        return baseString
            .Replace("<direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        int newMana = onAddMana.amount;
        eventBus.Publish(new OnRequestAddManaToSpellSlotOnDirection(onAddMana.spellSlot, (int)newMana, onAddMana.delay, direction, onAddMana.preview));
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {

    }
}
