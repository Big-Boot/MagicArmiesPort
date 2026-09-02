namespace GameLogic.Scripts.EventBus.Events
{
    public record OnSpellSlotLoaded
    {
        public SpellSlot SpellSlot;
        public bool preview;

        public OnSpellSlotLoaded(SpellSlot spellSlot, bool preview)
        {
            SpellSlot = spellSlot;
            this.preview = preview;
        }
    }
}