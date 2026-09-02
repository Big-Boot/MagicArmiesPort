using GameLogic.Scripts.EventBus.Events;

public class PlayerWeakestLaneReceivesExtraDamageRelicFeature : RelicFeature
{
    public int flatModifier = 0;

    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + flatModifier.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    private void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        foreach (var lane in BattleGUI.instance.GetLanes())
        {
            lane.AddBonusForDamageThePlayerWeakestLaneReceives(flatModifier);
        }
    }
    
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnBattleStarted>(OnBattleStarted);
    }
    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnBattleStarted>(OnBattleStarted);
    }
}
