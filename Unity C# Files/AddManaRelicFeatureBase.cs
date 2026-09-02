using GameLogic.Scripts.EventBus.Events;

public abstract class AddManaRelicFeatureBase : RelicFeature
{
    public override void DoEventBusSubscriptions()
    {
        eventBus.Subscribe<OnAddMana>(OnAddMana);
        eventBus.Subscribe<OnAddManaByHand>(OnAddManaByHand);
    }

    public override void DoEventBusUnsubscriptions()
    {
        eventBus.Unsubscribe<OnAddMana>(OnAddMana);
        eventBus.Unsubscribe<OnAddManaByHand>(OnAddManaByHand);
    }

    protected abstract void AddedMana(OnAddMana onAddMana);
    protected abstract void AddedManaByHand(OnAddManaByHand onAddMana);

    private void OnAddMana(OnAddMana onAddMana)
    {
        if (IsInvalidSpellSlot(onAddMana.spellSlot, onAddMana.preview))
        {
            return;
        }

        AddedMana(onAddMana);
    }

    private void OnAddManaByHand(OnAddManaByHand onAddMana)
    {
        if(IsInvalidSpellSlot(onAddMana.spellSlot, onAddMana.preview))
        {
            return;
        }
        AddedManaByHand(onAddMana);
    }

    private bool IsInvalidSpellSlot(SpellSlot spellSlot, bool includePreview)
    {
        if (spellSlot.spellOwned.spellModel != model)// || spellSlot.IsFullyLoaded(includePreview))
        {
            return true;
        }
        return false;
    }
}
