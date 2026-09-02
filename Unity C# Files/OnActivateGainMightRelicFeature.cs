using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class OnActivateGainMightRelicFeature : ActivateRelicFeatureBase
{
    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", value.ToString());
    }


    protected override void ActivateEffects(OnActivate onActivate)
    {
        this.GetSpellSlot().AddMight((int)value, onActivate.preview, 0f);
    }
}