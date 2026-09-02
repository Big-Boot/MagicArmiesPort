namespace GameLogic.Scripts.EventBus.Events
{
    public record OnHealPlayer
    {
        public int damage = 1;
        public bool reflectDamageInstantly;

        public OnHealPlayer(int damage, bool reflectDamageInstantly)
        {
            this.damage = damage;
            this.reflectDamageInstantly = reflectDamageInstantly;
        }
    }
}