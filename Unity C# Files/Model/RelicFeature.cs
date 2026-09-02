using Assets.SimpleLocalization;
using GameLogic.Scripts.Decoupling;
using GameLogic.Scripts.EventBus;
using ItemSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RelicModel;

public abstract class RelicFeature : MonoBehaviour
{

    protected ServiceLocator serviceLocator;
    protected IEventBus eventBus;
    protected PlayerData playerData;
    protected IModel model;

    public float value;
    //private bool started = false;
    protected bool acquired = false;

    public SpellModel GetSpellModel()
    {
        return (SpellModel)model;
    }

    protected SpellSlot GetSpellSlot()
    {
        foreach (SpellSlot spellSlot in BattleGUI.instance.lSpellSlot)
        {
            if(!spellSlot.loaded)
            {
                continue;
            }
            if(spellSlot.spellOwned.spellName.Equals(""))
            {
                continue;
            }
            if (spellSlot.spellOwned.spellModel == model)
            {
                return spellSlot;
            }
        }
        return null;
    }

    public void StartAcquire()
    {
        if (!acquired)
        {
            acquired = true;
            Acquired();
        }
    }
    public virtual void Acquired()
    {
        acquired = true;
    }

    private void OnDestroy()
    {
        DoEventBusUnsubscriptions();
    }

    public virtual void DoEventBusUnsubscriptions()
    {

    }
    public string GetCurrentlyValue(string currentlyValue)
    {
        return " (" + LocalizationManager.Localize("Currently-Value").Replace("<0>", currentlyValue )+")";
    }

    public void Initialize(IModel model)
    {
        serviceLocator = ServiceLocator.Instance;
        eventBus = serviceLocator.GetService<IEventBus>();
        this.model = model;
        /*Debug.Log("Check if initialize? " + this.name);
        if (!started)
        {
            Debug.Log("Initializing " + this.name);
            InitializeData();
        }
        started = true;*/
        InitializeData();
    }

    public virtual void InitializeData()
    {
        if (playerData != null)
        {
            return;
        }

        playerData = (PlayerData)serviceLocator.GetService<IPlayerData>();
        //DoEventBusSubscriptions();
    }

    public abstract void DoEventBusSubscriptions();

    public virtual string AddDescriptionValues(string baseText)
    {
        return baseText.Replace("<0>", value.ToString());
    }

    public virtual string GetUntranslatedType()
    {
        return this.GetType().ToString();
    }

    protected virtual string AddPreDescriptionValues()
    {
        return "";
    }

    public virtual string GetDescription()
    {
        try
        {
            return AddPreDescriptionValues() + AddDescriptionValues(LocalizationManager.Localize(GetUntranslatedType()));
        }
        catch(Exception ex)
        {
            Debug.Log("Feature not translated " + this.GetType().ToString() + "   " + ex.Message);
        }
        return AddDescriptionValues(this.GetType().ToString());
    }
}
