using GameLogic.Scripts.EventBus.Events;
using UnityEngine;
using static RelicModel;

public class IncreaseMightWhenManaLoadedIsValueRelicFeature : AddManaRelicFeatureBase
{
    public int mightIncrease = 1;
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter)
            .Replace("<1>", TooltipColorManager.SpriteMight + TooltipColorManager.HighlightCharacter2 + mightIncrease.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    protected override void AddedMana(OnAddMana onAddMana)
    {
        if (onAddMana.amount == value)
        {
            onAddMana.spellSlot.AddMight(mightIncrease, onAddMana.preview, onAddMana.delay);
        }
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
