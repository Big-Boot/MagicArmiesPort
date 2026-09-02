using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class EnemyRegeneratesHPOnTurnStartIfNoTiesRelicFeature : RelicFeature
{
    private void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        bool canHeal = true;
        foreach (Lane lane in BattleGUI.instance.GetLanes())
        {
            if (lane.GetLaneOutcome() == Lane.LaneOutcome.Draw)
            {
                canHeal = false;
            }
        }
        if (canHeal)
        {
            eventBus.Publish(new OnEnemyHealed((int) value, true));
        }
    }

    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
    }
    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnTurnStarted>(OnTurnStarted);
    }
}
