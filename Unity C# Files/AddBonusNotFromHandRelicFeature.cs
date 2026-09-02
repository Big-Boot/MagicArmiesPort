using System.Collections.Generic;
using GameLogic.Scripts.EventBus.Events;

public class AddBonusNotFromHandRelicFeature : AddManaRelicFeatureBase
{
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        if (!onAddMana.hand)
        {
            onAddMana.spellSlot.IncreaseCurrentPower((int)value, onAddMana.preview);
        }
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
