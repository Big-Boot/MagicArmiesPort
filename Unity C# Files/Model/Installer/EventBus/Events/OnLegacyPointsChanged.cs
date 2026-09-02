namespace GameLogic.Scripts.EventBus.Events
{
    public record OnLegacyPointsChanged
    {
        public int amount = 0;
        public OnLegacyPointsChanged(int amount)
        {
            this.amount = amount;
        }
    }
}