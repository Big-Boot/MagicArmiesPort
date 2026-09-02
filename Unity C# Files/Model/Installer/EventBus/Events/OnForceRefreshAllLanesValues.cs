namespace GameLogic.Scripts.EventBus.Events
{
    public record OnForceRefreshAllLanesValues
    {
        public bool preview;

        public OnForceRefreshAllLanesValues(bool preview)
        {
            this.preview = preview;
        }
    }
}