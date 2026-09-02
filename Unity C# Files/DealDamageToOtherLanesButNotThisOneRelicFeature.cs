using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class DealDamageToOtherLanesButNotThisOneRelicFeature : AddManaRelicFeatureBase
{

    protected override void AddedMana(OnAddMana onAddMana)
    {
        foreach (Lane lane in BattleGUI.instance.GetLanes())
        {
            if (onAddMana.spellSlot.GetLane() != lane)
            {
                lane.AddDamage(onAddMana.mightBonus + onAddMana.amount, onAddMana.preview);
            }
        }
        foreach (PowerOutcome powerOutcome in BattleGUI.instance.GetPowerOutcomes())
        {
            if (onAddMana.spellSlot.GetPowerOutcome() != powerOutcome)
            {
                //powerOutcome.AddPower(GetSpellSlot().GetBonusGivenByMight(onAddMana.preview) + onAddMana.amount, onAddMana.preview);
                if (!onAddMana.preview)
                {
                    this.GetSpellSlot().SendManaCharge(powerOutcome.GetComponentInParent<Lane>().GetPlayerEnergyBall().gameObject, onAddMana.delay);
                }
            }
        }
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}
