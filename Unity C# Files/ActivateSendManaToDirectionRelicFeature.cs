using GameLogic.Scripts.EventBus.Events;
using static IncreaseManaAndSendToDirectionRelicFeature;

public class ActivateSendManaToDirectionRelicFeature : ActivateRelicFeatureBase
{
    public Direction direction;

    public override string AddDescriptionValues(string baseString)
    {
        return baseString
            .Replace("<0>", value.ToString())
            .Replace("<direction>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfDirection(direction) + TooltipColorManager.NormalTextCharacter);
    }


    protected override void ActivateEffects(OnActivate onActivate)
    {
        int newMana = (int)value;
        eventBus.Publish(new OnRequestAddManaToSpellSlotOnDirection(onActivate.spellSlot, (int)newMana, onActivate.delay, direction, onActivate.preview));
    }

}