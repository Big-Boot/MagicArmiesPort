using GameLogic.Scripts.EventBus.Events;

public class AllEnemyLanesGainBonusWhenPlayManaWithAmountBiggerThanValueRelicFeature : RelicFeature
{
    public int enemyBonusAdded = 3;
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnAddMana>(OnAddManaByHand);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnAddMana>(OnAddManaByHand);
    }

    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter)
            .Replace("<1>", TooltipColorManager.HighlightCharacter2 + enemyBonusAdded.ToString() + TooltipColorManager.NormalTextCharacter);
    }

    private void OnAddManaByHand(OnAddMana onAddMana)
    {
        if (onAddMana.amount >= value)
        {
            foreach (Lane lane in BattleGUI.instance.GetLanes())
            {
                lane.AddEnemyDamage(enemyBonusAdded, onAddMana.preview);
            }
        }
    }
}
