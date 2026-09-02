using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;
using GameLogic.Scripts.EventBus.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.SimpleLocalization;
using Random = UnityEngine.Random;
using ItemSystem;
using System.Xml;
using UnityEngine.Rendering;
using static RelicModel;

public class PlayerData: IPlayerData
{
    [NonSerialized ]
    ServiceLocator serviceLocator;
    [NonSerialized ]
    IEventBus eventBus;

    public bool runStarted = false;
    public int gold = 0;
    public int legacyPoints = 0;

    public int maxHP = 20;
    public int currentHP = 20;

    public List<ManaPoint> lManaPoint;
    public List<ManaElement> lManaElement;

    public List<SpellData> lSpells;
    public List<SocketData> lSockets;

    public bool finishedTutorial = false;

    public int gamesPlayed = 0;
    public int currentStoryStep = 0;
    public string currentStory = "Start";

    [NonSerialized ]
    public MonoBehaviour owner;

    [NonSerialized ]
    public bool initialized = false;

    public string characterPicked = "Prince Oscar";
    [NonSerialized]
    CharacterModel selectedCharacterModel;

    public List<string> lStoriesOfferedAtCamp = new List<string>();

    public List<string> lStoriesReadThisRun = new List<string>();
    public List<string> lStoriesReadAllGame = new List<string>();

    public bool atCamp = false;

    public string currentEnvironment = "Forest";
    public int areaLevel = 0;

    public static PlayerData instance;
    public void Initialize(MonoBehaviour owner)
    {
        Debug.Log("Data loaded");
        
        ItemSystemUtility.LoadItemDatabase();

        LocalizationManager.Read();

        initialized = false;
        instance = this;
        this.owner = owner;
        serviceLocator = ServiceLocator.Instance;
        eventBus = serviceLocator.GetService<IEventBus>();
        eventBus.Publish(new OnPlayerDataLoaded());

        ContinueGame();
    }

    public int GetAreaLevel()
    {
        return areaLevel;
    }

    public void IncreaseAreaSubLevel()
    {
        areaLevel++;
    }

    public int GetLegacyPointsReward()
    {
        return (areaLevel % 4) + Mathf.FloorToInt(areaLevel / 4)*2+4;
    }

    public CharacterModel GetSelectedCharacterModel()
    {
        if(selectedCharacterModel == null)
        {
            selectedCharacterModel = ItemSystemUtility.GetItemCopy<CharacterModel>(characterPicked, ItemType.CharacterModel);
        }
        return selectedCharacterModel;
    }

    public void RegisterStory(string storyName)
    {
        if (!lStoriesReadThisRun.Contains(storyName))
        {
            lStoriesReadThisRun.Add(storyName);
        }

        if (!lStoriesReadAllGame.Contains(storyName))
        {
            lStoriesReadAllGame.Add(storyName);
        }
    }

    public Vector2 AddCrystalToGrid(RelicModel.PlacementRestriction placementRestriction)
    {
        if(GetAmountOfCrystals(placementRestriction) >= 9)
        {
            Debug.Log("Cannot add more crystals of this type");
            return Vector2.zero;
        }

        List<int> lRowPositionCrystals = lSockets.Where(s => s.placementRestriction == placementRestriction).Select(s => s.row).ToList();
        List<int> lPossibleNumbers = new List<int>();
        for (int i = 0; i < GameSettings.MAX_SPELL_ROWS; i++) { 
            if(!lRowPositionCrystals.Contains(i))
            {
                lPossibleNumbers.Add(i);
            }
        }

        int col = 0;
        switch(placementRestriction)
        {
            case RelicModel.PlacementRestriction.BlueCrystal:
                col = 0;
                break;
            case RelicModel.PlacementRestriction.RedCrystal:
                col = GameSettings.MAX_SPELL_COLUMNS-1;
                break;
        }

        int row = Random.Range(0, lPossibleNumbers.Count);
        lSockets.Add(new SocketData(lPossibleNumbers[row], col, placementRestriction));

        return new Vector2(col, row);
    }

    public List<SocketData> GetCrystalSockets(RelicModel.PlacementRestriction placementRestriction)
    {
        return lSockets.Where(s=>s.placementRestriction == placementRestriction).ToList();
    }

    public int GetAmountOfCrystals(RelicModel.PlacementRestriction placementRestriction)
    {
        return lSockets.Count(socket => socket.placementRestriction == placementRestriction);
    }

    public bool HasIModel(IModel model)
    {
        if(model is SpellModel)
        {
            SpellModel spellModel = model as SpellModel;
            return lSpells.Any(s => s.spellName == spellModel.itemName);
        }
        return false;
    }

    public void AddIModel(IModel model)
    {
        if(model is SpellModel)
        {
            SpellModel spellModel = model as SpellModel;
            lSpells.Add(new SpellData(spellModel.itemName, 0, 0, false));
        }
    }

    public void RestartGame()
    {
        gold = 100;
        gamesPlayed++;
        areaLevel = 0;

        lSockets = new List<SocketData>()
        {
            //new SocketData(0,1,RelicModel.PlacementRestriction.RedCrystal),
            //new SocketData(0,4,RelicModel.PlacementRestriction.RedCrystal),
            //new SocketData(0,7,RelicModel.PlacementRestriction.RedCrystal),
        };

        Vector2 crystal1 = AddCrystalToGrid(RelicModel.PlacementRestriction.BlueCrystal);
        Vector2 crystal2 = AddCrystalToGrid(RelicModel.PlacementRestriction.BlueCrystal);
        AddCrystalToGrid(RelicModel.PlacementRestriction.BlueCrystal);

        AddCrystalToGrid(RelicModel.PlacementRestriction.RedCrystal);
        AddCrystalToGrid(RelicModel.PlacementRestriction.RedCrystal);
        AddCrystalToGrid(RelicModel.PlacementRestriction.RedCrystal);

        lSpells = new List<SpellData>()
        {
            new SpellData("Newbie Mage", 1, 2, true),
            new SpellData("Newbie Mage", 5, 2, true),
            new SpellData("Berserker", 3, 2, true),
            new SpellData("White Mage A", 3, 1, true),
            new SpellData("Miner", (int) crystal1.y, (int) crystal1.x, true),
            new SpellData("Miner",(int) crystal2.y, (int) crystal2.x, true),
            /*new SpellData("Explosion", 0, 4, false),
            new SpellData("Top Charge", 0, 4, false),
            new SpellData("Evenstrike", 0, 7, true),
            new SpellData("Row Charge", 0, 7, false),
            new SpellData("Column Charge", 0, 7, false),
            new SpellData("Redistribution", 0, 7, false),
            new SpellData("Ora Ora Ora", 0, 7, false),
            new SpellData("Fire Orb", 0, 7, false),
            new SpellData("Empower", 0, 7, false),
            new SpellData("Mana Pit", 0, 7, false),
            new SpellData("Side Charge", 0, 7, false),
            new SpellData("Mana Fountain", 0, 7, false),
            new SpellData("Mana Factory", 0, 7, false),
            new SpellData("Mana Spring", 0, 7, false),
            new SpellData("Mana Geyser", 0, 7, false),
            new SpellData("Mana Farming", 0, 7, false),
            new SpellData("Shock", 0, 7, false),
            new SpellData("Zap Field", 0, 7, false),*/
        };

        maxHP = 20;
        currentHP = maxHP;

        atCamp = false;

        lManaPoint = new List<ManaPoint>
        {
            new ManaPoint(1),
            new ManaPoint(2),
            new ManaPoint(3),
            //new ManaPoint(4),
            //new ManaPoint(5),
        };

        lManaElement = new List<ManaElement>
        {
            new ManaElement(ManaElement.ManaType.High),
            new ManaElement(ManaElement.ManaType.Mid),
            new ManaElement(ManaElement.ManaType.Mid),
            new ManaElement(ManaElement.ManaType.Low),
            new ManaElement(ManaElement.ManaType.Low),
            new ManaElement(ManaElement.ManaType.Low),
        };

        owner.StartCoroutine(LoadData());

        runStarted = true;
        initialized = true;
        saveData();
    }

    public void ContinueGame()
    {
        if (!runStarted)
        {
            RestartGame();
        }
        else
        {
            owner.StartCoroutine(LoadData());
            initialized = true;
        }
    }

    public bool IsFirstTutorialBattle()
    {
        return areaLevel == 0 && !finishedTutorial;
    }

    IEnumerator LoadData()
    {

        yield return new WaitForFixedUpdate();

        eventBus.Publish(new OnGameStarted());

        if(atCamp)
        {
            eventBus.Publish(new OnShowCampfire());
        }
        else
        {
            if (IsFirstTutorialBattle())
            {
                eventBus.Publish(new OnFinishedPlanning());
            }
            else
            {
                eventBus.Publish(new OnStartPlanning(true));
            }
        }

        eventBus.Subscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Subscribe<OnBattleEnded>(OnBattleEnded);
        eventBus.Subscribe<OnGoldChanged>(OnGainGold);
        eventBus.Subscribe<OnLegacyPointsChanged>(OnLegacyPointsChanged);
        eventBus.Subscribe<OnHealPlayer>(OnHealPlayer);
        eventBus.Subscribe<OnPlayerDamaged>(OnPlayerDamaged);
        eventBus.Subscribe<OnStartPlanning>(OnStartPlanning);
    }

    public void Dispose()
    {
        eventBus.Unsubscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Unsubscribe<OnBattleEnded>(OnBattleEnded);
        eventBus.Unsubscribe<OnGoldChanged>(OnGainGold);
        eventBus.Unsubscribe<OnLegacyPointsChanged>(OnLegacyPointsChanged);
        eventBus.Unsubscribe<OnHealPlayer>(OnHealPlayer);
        eventBus.Unsubscribe<OnPlayerDamaged>(OnPlayerDamaged);
        eventBus.Unsubscribe<OnStartPlanning>(OnStartPlanning);
    }


    private void OnStartPlanning(OnStartPlanning onStartPlanning)
    {
        atCamp = false;
        if (!finishedTutorial)
        {
            DialogueGUI.instance.LoadDialogueAndAddToQueue("Phase6");
        }
        saveData();
    }

    public void OnPlayerDamaged(OnPlayerDamaged onPlayerDamaged)
    {
        currentHP -= onPlayerDamaged.damage;
        if (onPlayerDamaged.reflectDamageInstantly)
        {
            eventBus.Publish(new OnRefreshPlayerHP());
        }
    }

    public void OnHealPlayer(OnHealPlayer onHealPlayer)
    {
        currentHP += onHealPlayer.damage;
        currentHP = currentHP > maxHP ? maxHP : currentHP; // Ensure HP doesn't go over the MAX
        if (onHealPlayer.reflectDamageInstantly)
        {
            eventBus.Publish(new OnRefreshPlayerHP());
        }
    }


    private void OnLegacyPointsChanged(OnLegacyPointsChanged onLegacyPointsChanged)
    {
        legacyPoints += onLegacyPointsChanged.amount;
    }

    private void OnGainGold(OnGoldChanged onGainGold)
    {
        gold += onGainGold.amount;
    }

    private void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        saveData();
    }


    public void saveData()
    {
        FBPP.SetString("PlayerData", JsonUtility.ToJson(this));
        FBPP.Save();

        /*foreach(SpellData spell in lSpells)
        {
            Debug.Log(spell.spellName + " " + spell.row + "," + spell.column + " " + spell.equipped);
        }*/
    }

    public int GetStartingManaAmount()
    {
        return 2;
    }

    public int GetPerTurnManaAmount()
    {
        return 1;
    }

    public void OnBattleEnded(OnBattleEnded onBattleEnded)
    {
        if (areaLevel >= 1)
        {
            finishedTutorial = true;
        }
        if (!finishedTutorial)
        {
            DialogueGUI.instance.LoadDialogueAndAddToQueue("Phase4");
        }
        eventBus.Publish(new OnLegacyPointsChanged(GetLegacyPointsReward()));
        RollStoriesAtCamp();
        IncreaseAreaSubLevel();
        atCamp = true;
        saveData();
        eventBus.Publish(new OnShowCampfire());
    }

    public void RollStoriesAtCamp()
    {
        lStoriesOfferedAtCamp.Clear();
        List<IModel> models = ItemDatabaseDataManager.instance.GetRandomItems(typeof(StoryModel), false, 5, new List<string>());// lStoriesReadThisRun);

        foreach (IModel model in models)
        {
            lStoriesOfferedAtCamp.Add(model.GetName());
        }
    }
}