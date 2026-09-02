
using static IncreaseManaAndSendToDirectionRelicFeature;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnRequestAddMightToSpellSlotOnDirection
    {
        public int might;
        public bool preview;
        public SpellSlot sourceSpellSlot;
        public float delay;
        public Direction direction;
        public OnRequestAddMightToSpellSlotOnDirection(SpellSlot sourceSpellSlot, int might, float delay, Direction direction, bool preview)
        {
            this.sourceSpellSlot = sourceSpellSlot;
            this.might = might;
            this.preview = preview;
            this.delay = delay;
            this.direction = direction;
        }
    }
}