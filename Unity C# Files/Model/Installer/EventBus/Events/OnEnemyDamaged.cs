namespace GameLogic.Scripts.EventBus.Events
{
    public record OnEnemyDamaged
    {
        public int damage = 1;
        public bool reflectDamageInstantly;

        public OnEnemyDamaged(int damage, bool reflectDamageInstantly)
        {
            this.damage = damage;
            this.reflectDamageInstantly = reflectDamageInstantly;
        }
    }
}