using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class GainManaIfAddedLastHandManaRelicFeature : AddManaRelicFeatureBase
{
    protected override void AddedMana(OnAddMana onAddMana)
    {
        if(ManaManager.instance.GetManaOnHand(onAddMana.preview)>1)
        {
            return;
        }
        eventBus.Publish(new OnRequestAddManaToHand(onAddMana.preview));
    }

    protected override void AddedManaByHand(OnAddManaByHand onAddMana)
    {
    }
}