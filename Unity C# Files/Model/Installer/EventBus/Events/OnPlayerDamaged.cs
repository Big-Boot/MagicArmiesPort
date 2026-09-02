namespace GameLogic.Scripts.EventBus.Events
{
    public record OnPlayerDamaged
    {
        public int damage = 1;
        public bool reflectDamageInstantly;

        public OnPlayerDamaged(int damage, bool reflectDamageInstantly)
        {
            this.damage = damage;
            this.reflectDamageInstantly = reflectDamageInstantly;
        }
    }
}