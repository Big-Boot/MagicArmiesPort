using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class ActivateDrawManaRelicFeature : ActivateRelicFeatureBase
{
    protected override void ActivateEffects(OnActivate onActivate)
    {
        eventBus.Publish(new OnRequestAddManaToHand(onActivate.preview));
    }
}