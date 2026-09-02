using GameLogic.Scripts.EventBus.Events;
using ItemSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ItemSystem.StoryModel;

public class StoryManager : BasicMonobehaviour
{

    StoryModel currentStory;

    public static StoryManager instance;

    public StoryManager()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;

        eventBus.Subscribe<OnAllDialoguesRead>(OnAllDialoguesRead);
    }

    private void OnAllDialoguesRead(OnAllDialoguesRead onAllDialoguesRead)
    {
        if (currentStory != null)
        {
            currentStory.advanceStep();
        }
    }
    private void OnDestroy()
    {
        eventBus.Unsubscribe<OnAllDialoguesRead>(OnAllDialoguesRead);
    }

    public void initiateGame()
    {
        if (!PlayerData.instance.runStarted)
        {
            if (PlayerData.instance.gamesPlayed == 0)
            {
                if (!DebugManager.instance.skipTutorial)
                {
                    StoryManager.instance.loadStory("Tutorial Intro");
                }
                else
                {
                    StoryManager.instance.loadStory("Normal Intro");
                }
            }
            else
            {
                StoryManager.instance.loadStory("Normal Intro");
            }
        }
        else
        {
            EnvironmentManager.instance.ChangeEnvironment(PlayerData.instance.currentEnvironment);
            StoryManager.instance.loadStory(PlayerData.instance.currentStory, PlayerData.instance.currentStoryStep);
        }
    }

    public string getCurrentStoryName()
    {
        
        return currentStory == null ? "Intro" : currentStory.itemName;
    }
    public int getCurrentStep()
    {
        return currentStory.getCurrentStep();
    }

    public MapNodeType getCurrentStoryType()
    {

        return currentStory.storyType;
    }

    public void RemoveCurrentStory()
    {
        currentStory = null;
    }

    public void LoadNewStory(MapNodeType node)
    {
        int level = PlayerData.instance.GetAreaLevel();

        PickStory(level, node);

    }

    public void PickStory(int level, MapNodeType node)
    {
        List<StoryModel> lStories;// = ItemSystemUtility.GetAllTypeItems<StoryModel>(ItemType.StoryModel);

        lStories = ItemSystemUtility.GetAllTypeItems<StoryModel>(ItemType.StoryModel);

        List<StoryModel> lPickableStories = new List<StoryModel>();
        for (int i=0; i<lStories.Count; i++)
        {
            if ((lStories[i].storyType == node) && (lStories[i].level==level) && (!PlayerData.instance.lStoriesReadThisRun.Contains(lStories[i].itemName)))
            {
                lPickableStories.Add(lStories[i]);
            }
        }


        if(lPickableStories.Count ==0)
        {
            for (int i = 0; i < lStories.Count; i++)
            {
                if ((lStories[i].storyType == node) && (lStories[i].level == level))
                {
                    lPickableStories.Add(lStories[i]);
                }
            }
        }

        System.Random rng = new System.Random();
        List<StoryModel> shuffledStories= lPickableStories.OrderBy(a => rng.Next()).ToList();

#if UNITY_EDITOR
        if(!DebugManager.instance.nextForcedStory.Equals(""))
        {
            shuffledStories[0] = ItemSystemUtility.GetItemCopy<StoryModel>(DebugManager.instance.nextForcedStory, ItemType.StoryModel);
        }
#endif
        /*if(DebugManager.instance.isDebugBuild)
        {
            if (!DebugManager.instance.nextForcedStory.Equals("")) {
                shuffledStories[0] = ItemSystemUtility.GetItemCopy<StoryModel>(DebugManager.instance.nextForcedStory, ItemType.StoryModel);
            }
        }
        */
        Debug.Log("Load story named: '" + shuffledStories[0].itemName + "'");

        //PlayerData.instance.rollShopInventory();

        loadStory(shuffledStories[0].itemName);

    }

    public void loadStory(string storyToLoad, int storyStep = 0)
    {
        PlayerData.instance.RegisterStory(storyToLoad);
        //GUIGameManager.instance.hideRestingSprite();
        currentStory = ItemSystemUtility.GetItemCopy<StoryModel>(storyToLoad, ItemType.StoryModel);
        currentStory.setCurrentStep(storyStep);
        currentStory.advanceStep();

    }

    public void dialogueEnded()
    {
        Invoke("advanceStory", 1f);

    }


    public void combatEnded()
    {

        Invoke("advanceStory", 1f);

    }

    public void advanceStory()
    {
        currentStory.advanceStep();

    }

}
