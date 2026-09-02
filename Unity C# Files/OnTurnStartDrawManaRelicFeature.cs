using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class OnTurnStartDrawManaRelicFeature : RelicFeature
{
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnTurnStarted>(OnTurnStarted);
    }
    
    private void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        eventBus.Publish(new OnRequestAddManaToHand(false));
    }
}
