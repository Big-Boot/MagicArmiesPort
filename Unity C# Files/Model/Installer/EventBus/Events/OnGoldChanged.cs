namespace GameLogic.Scripts.EventBus.Events
{
    public record OnGoldChanged
    {
        public int amount;
        public OnGoldChanged(int amount)
        {
            this.amount = amount;
        }
    }
}