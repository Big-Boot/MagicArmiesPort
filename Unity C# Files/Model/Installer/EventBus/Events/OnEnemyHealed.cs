namespace GameLogic.Scripts.EventBus.Events
{
    public record OnEnemyHealed
    {
        public int damage = 1;
        public bool reflectDamageInstantly;

        public OnEnemyHealed(int damage, bool reflectDamageInstantly)
        {
            this.damage = damage;
            this.reflectDamageInstantly = reflectDamageInstantly;
        }
    }
}