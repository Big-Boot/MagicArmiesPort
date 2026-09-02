namespace GameLogic.Scripts.EventBus.Events
{
    public record OnShowGameOver
    {
        public string message;

        public OnShowGameOver(string message)
        {
            this.message = message;
        }
    }
}