using ItemSystem;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnEnemySpawn
    {
        public EnemyModel enemy;

        public OnEnemySpawn(EnemyModel enemy)
        {
            this.enemy = enemy;
        }
    }
}