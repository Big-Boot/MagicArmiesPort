
using static IncreaseManaAndSendToDirectionRelicFeature;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnRequestAddManaToSpellSlotOnDirection
    {
        public int mana;
        public bool preview;
        public SpellSlot sourceSpellSlot;
        public float delay;
        public Direction direction;
        public OnRequestAddManaToSpellSlotOnDirection(SpellSlot sourceSpellSlot, int mana, float delay, Direction direction, bool preview)
        {
            this.sourceSpellSlot = sourceSpellSlot;
            this.mana = mana;
            this.preview = preview;
            this.delay = delay;
            this.direction = direction;
        }
    }
}