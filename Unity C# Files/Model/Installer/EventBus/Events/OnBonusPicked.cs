using static RelicModel;

namespace GameLogic.Scripts.EventBus.Events
{

    public record OnBonusPicked
    {
        public IModel model;
        public OnBonusPicked(IModel model)
        {
            this.model = model;
        }
    }

}