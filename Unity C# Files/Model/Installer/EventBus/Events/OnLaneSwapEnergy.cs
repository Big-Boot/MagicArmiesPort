using static Lane;

namespace GameLogic.Scripts.EventBus.Events
{
    public record OnLaneSwapEnergy
    {
        public LanePlacement laneSource;
        public LanePlacement laneTarget;

        public OnLaneSwapEnergy(LanePlacement laneSource, LanePlacement laneTarget)
        {
            this.laneSource = laneSource;
            this.laneTarget = laneTarget;
        }
    }
}