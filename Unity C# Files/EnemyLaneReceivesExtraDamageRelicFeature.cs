using GameLogic.Scripts.EventBus.Events;
using UnityEngine;
using static Lane;

public class EnemyLaneReceivesExtraDamageRelicFeature : RelicFeature
{
    public enum DamageBuffType
    {
        Flat,
        Mult
    }
    public DamageBuffType buffType;
    public LanePlacement lanePlacement;
    private void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        foreach(var lane in BattleGUI.instance.GetLanes())
        {
            if (lane.GetLanePlacement() == lanePlacement)
            {
                if (buffType == DamageBuffType.Flat)
                {
                    lane.AddBonusForDamageTheEnemyReceives((int)value, 1);
                }
                else if (buffType == DamageBuffType.Mult)
                {
                    lane.AddBonusForDamageTheEnemyReceives(0, (int) value);
                }
            }
        }
    }

    public override string GetUntranslatedType()
    {
        return buffType == DamageBuffType.Flat ? "EnemyLaneReceivesExtraDamageRelicFeatureFlat" : "EnemyLaneReceivesExtraDamageRelicFeatureMult";
    }

    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + ((int)value).ToString() + TooltipColorManager.NormalTextCharacter).Replace("<lane>", TooltipColorManager.HighlightCharacter2 + TooltipColorManager.GetLocalizationOfLane(lanePlacement) + TooltipColorManager.NormalTextCharacter);
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
