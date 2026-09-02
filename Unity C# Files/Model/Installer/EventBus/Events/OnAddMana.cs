namespace GameLogic.Scripts.EventBus.Events
{
    public record OnAddMana
    {
        public int amount;
        public int mightBonus;
        public SpellSlot spellSlot;
        public bool preview;
        public bool hand;
        public float delay;
        public OnAddMana(int amount, int mightBonus, SpellSlot spellSlot, bool hand, bool preview, float delay)
        {
            this.amount = amount;
            this.mightBonus = mightBonus;
            this.spellSlot = spellSlot;
            this.preview = preview;
            this.hand = hand;
            this.delay = 0;
        }
    }
}