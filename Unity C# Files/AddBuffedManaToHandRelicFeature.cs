using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class AddBuffedManaToHandRelicFeature : RelicFeature
{
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnAddMana>(OnAddMana);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnAddMana>(OnAddMana);
    }
    private void OnAddMana(OnAddMana OnAddMana)
    {
        if (OnAddMana.spellSlot.spellOwned.spellModel != model)
        {
            return;
        }
        eventBus.Publish(new OnRequestAddManaToHand(OnAddMana.amount + Mathf.RoundToInt(value), OnAddMana.preview));
    }

    public override string AddDescriptionValues(string baseString)
    {
        return baseString.Replace("<0>", TooltipColorManager.HighlightCharacter2 + value.ToString() + TooltipColorManager.NormalTextCharacter);
    }
}