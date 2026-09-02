namespace GameLogic.Scripts.EventBus.Events
{
    public record OnAddManaByHand
    {
        public int amount;
        public int mightBonus;
        public SpellSlot spellSlot;
        public bool preview;

        public OnAddManaByHand(int amount, int mightBonus, SpellSlot spellSlot, bool preview)
        {
            this.amount = amount;
            this.mightBonus = mightBonus;
            this.spellSlot = spellSlot;
            this.preview = preview;
        }
    }
}