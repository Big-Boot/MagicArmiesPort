using UnityEngine;
using Assets.SimpleLocalization;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using System;
using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;
using GameLogic.Scripts.EventBus.Events;
using static RelicModel;

//public class CraftableItem : ItemSystem.ItemBase {
namespace ItemSystem
{

    [System.Serializable]
	public class StoryModel : ItemBase, IModel
	{
		
		public string displayName = "";
		public enum StoryEventType { ChangeMusic, Conversation, ShowAndPickRelic, ShowAndPickSoldier, ShowCharactersFromCombat, ChangeEnvironment, TutorialEnded, EndDemo, AreaEnd, Shop, Menu, ChangeStory, GameOver, Nothing, GainGold, ShowAndPickAllSoldiers, Heal }

		public enum MapNodeType { Unknown, Shop, Soldier, Relic };

        public ModelRarity rarity = ModelRarity.Common;

        public MapNodeType storyType = MapNodeType.Unknown;

		public int level = 0;

        public List<StoryPiece> lStoryPieces;

		private int currentStep = 0;

		private ServiceLocator serviceLocator;
        private IEventBus eventBus;
        
        /// <summary>
        /// Updates any unique properties of the item
        /// </summary>
        /// <param name="itemToChangeTo"></param>
        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
		{

			StoryModel newStory = (StoryModel)itemToChangeTo;

			level = newStory.level;
			lStoryPieces = new List<StoryPiece>();
			foreach (StoryPiece storyPiece in newStory.lStoryPieces)
			{
				lStoryPieces.Add(storyPiece);
			}
			storyType = newStory.storyType;

			displayName = newStory.displayName;

            rarity = newStory.rarity;

            serviceLocator = ServiceLocator.Instance;
            eventBus = serviceLocator.GetService<IEventBus>();

        }

        public int getCurrentStep()
        {
			return currentStep;
        }

		public void setCurrentStep(int step)
        {
			currentStep = step;

		}
		public void advanceStep()
        {
			bool instantlyAdvanceStep = false;
			PlayerData.instance.currentStoryStep = currentStep;
			PlayerData.instance.currentStory = itemName;

			instantlyAdvanceStep = executeStoryStep(lStoryPieces[currentStep].lStoryEvent, lStoryPieces[currentStep].lStoryEventDetail, eventBus);
			currentStep++;

			if (instantlyAdvanceStep)
            {
				advanceStep();
            }

		}

		public static bool executeStoryStep(StoryEventType storyEventType, string detail, IEventBus eventBus)
		{
            bool instantlyAdvanceStep = false;

            switch (storyEventType)
            {
                case StoryEventType.Heal:
                    eventBus.Publish(new OnHealPlayer(int.Parse(detail), true));
                    instantlyAdvanceStep = true;
                    break;
                case StoryEventType.TutorialEnded:
                    /*foreach (WearableModel wearable in PlayerData.instance.lInventory)
					{
						PlayerData.instance.equipWearable(wearable);
					}
					PlayerData.instance.deck.lDeck = PlayerData.instance.deck.getStartingDeck(true);
					PlayerData.instance.saveData();
					ToastMessagesGUI.instance.showToastMessage(LocalizationManager.Localize("Starting-Items-Added"));*/
                    instantlyAdvanceStep = true;
                    break;
                case StoryEventType.ChangeMusic:
                    MasterAudio.ChangePlaylistByName(detail);
                    instantlyAdvanceStep = true;
                    break;
                case StoryEventType.EndDemo:
                    //DebugManager.instance.uploadRunData("Beat Demo");
                    //GUIGameManager.instance.showDemoEndScreen();
                    break;
                case StoryEventType.AreaEnd:
                    //PlayerData.instance.areaLevel = level + 1;
                    //PlayerData.instance.healPlayerOutsideCombat(Mathf.FloorToInt(PlayerData.instance.hero.hpBonus() * FBPP.GetInt("Uneasy Rest Value", 100) / 100f));
                    instantlyAdvanceStep = true;
                    //PlayerData.instance.saveData();
                    break;
                case StoryEventType.Shop:
                case StoryEventType.Menu:
                    StoryManager.instance.RemoveCurrentStory();
                    PlayerData.instance.runStarted = true;
                    PlayerData.instance.saveData();
                    eventBus.Publish(new OnShowCampfire());
                    //SettingsManager.instance.doBackup();
                    break;
                case StoryEventType.ShowAndPickRelic:
                    //RelicMenuGUI.instance.prepareGUI();
                    break;
                case StoryEventType.GainGold:
                    eventBus.Publish(new OnGoldChanged(int.Parse(detail)));
                    break;
                case StoryEventType.ShowAndPickSoldier:
                    eventBus.Publish(new OnRequestPickSoldier(int.Parse(detail), false, "PickSoldier2-Description", "Skip"));
                    //RelicMenuGUI.instance.prepareGUI();
                    break;
                case StoryEventType.ShowAndPickAllSoldiers:
                    eventBus.Publish(new OnRequestPickSoldier(int.Parse(detail), true, "Receive3Soldiers-Description", "Accept"));
                    //RelicMenuGUI.instance.prepareGUI();
                    break;
                case StoryEventType.Conversation:
                    DialogueGUI.instance.LoadDialogueAndAddToQueue(detail);

                    break;
                case StoryEventType.ShowCharactersFromCombat:
                    //EnvironmentManager.instance.SpawnNPCFromCombat(lStoryEventDetail[currentStep]);
                    instantlyAdvanceStep = true;
                    break;
                case StoryEventType.ChangeEnvironment:
                    PlayerData.instance.currentEnvironment = detail;
                    EnvironmentManager.instance.ChangeEnvironment(detail);
                    instantlyAdvanceStep = true;
                    break;
                case StoryEventType.ChangeStory:
                    StoryManager.instance.loadStory(detail);
                    break;
                case StoryEventType.GameOver:
                    //GUIGameManager.instance.showGameoverScreen();
                    break;
                case StoryEventType.Nothing:
                    instantlyAdvanceStep = false;
                    break;
            }

            return instantlyAdvanceStep;
        }

		public string getDisplayName()
        {
			return displayName;
        }

        public ModelRarity GetModelRarity()
        {
            return rarity;
        }

        public string GetName()
        {
            return itemName;
        }

        public string GetLocalizedName()
        {
            return LocalizationManager.Localize(itemName.Replace(" ", "-"));
        }

        public string GetDescription()
        {
            return LocalizationManager.Localize(itemName.Replace(" ", "-")+"-Description");
        }

        public GameObject GetGameObject()
        {
            return null;
        }

        public int GetPrice()
        {
            switch(rarity)
            {
                case ModelRarity.Starter:
                    return 0;
                case ModelRarity.Common:
                    return 2;
                case ModelRarity.Epic:
                    return 4;
                case ModelRarity.Legendary:
                    return 6;
            }
            return 2;
        }
    }
}