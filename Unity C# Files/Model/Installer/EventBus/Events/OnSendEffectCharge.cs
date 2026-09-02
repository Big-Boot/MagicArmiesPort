using UnityEngine;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnSendEffectCharge
    {
        public enum EffectType
        {
            Mana,
            Buff
        }

        public EffectType effectType;
        public GameObject from;
        public GameObject to;
        public float delay;

        public OnSendEffectCharge(EffectType effectType, GameObject from, GameObject to, float delay)
        {
            this.effectType = effectType;
            this.from = from;
            this.to = to;
            this.delay = delay;
        }
    }
}