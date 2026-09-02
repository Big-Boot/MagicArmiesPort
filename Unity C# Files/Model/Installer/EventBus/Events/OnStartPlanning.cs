namespace GameLogic.Scripts.EventBus.Events
{
    public record OnStartPlanning
    {
        public bool showEnemy;
        public OnStartPlanning(bool showEnemy)
        {
            this.showEnemy = showEnemy;
        }
    }
}