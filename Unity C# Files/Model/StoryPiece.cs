using System.Collections.Generic;
using static ItemSystem.StoryModel;

//public class CraftableItem : ItemSystem.ItemBase {
namespace ItemSystem
{
    [System.Serializable]
    public class StoryPiece
    {
        public StoryEventType lStoryEvent;
        public string lStoryEventDetail;
    }
}