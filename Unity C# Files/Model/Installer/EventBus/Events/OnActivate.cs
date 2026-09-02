namespace GameLogic.Scripts.EventBus.Events
{
    public record OnActivate
    {
        public bool ignoreActivateLimit;
        public SpellSlot spellSlot;
        public bool preview;
        public float delay;
        public OnActivate(bool ignoreActivateLimit, SpellSlot spellSlot, bool preview, float delay)
        {
            this.ignoreActivateLimit = ignoreActivateLimit;
            this.spellSlot = spellSlot;
            this.preview = preview;
            this.delay = 0;
        }
    }

}