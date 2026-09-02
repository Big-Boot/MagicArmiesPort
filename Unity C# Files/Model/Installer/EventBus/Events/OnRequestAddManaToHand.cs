namespace GameLogic.Scripts.EventBus.Events
{
    public record OnRequestAddManaToHand
    {
        public int value;
        public bool preview;
        public OnRequestAddManaToHand(int value, bool preview)
        {
            this.value = value;
            this.preview = preview;
        }
        public OnRequestAddManaToHand(bool preview)
        {
            this.value = -1;
            this.preview = preview;
        }
    }
}