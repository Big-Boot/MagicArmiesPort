namespace GameLogic.Scripts.EventBus.Events
{
    public record OnEnemyHPBarDiminished
    {
        public bool dead;

        public OnEnemyHPBarDiminished(bool dead = false)
        {
            this.dead = dead;
        }
    }
}