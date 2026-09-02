using System;
using System.Collections.Generic;
using UnityEngine;
using static RelicModel;

namespace ItemSystem
{
    public class ItemDatabaseDataManager : MonoBehaviour
    {
        public static ItemDatabaseDataManager instance;

        List<ModelItemManagerBase> lItemManager = new List<ModelItemManagerBase>();

        public ItemDatabaseDataManager()
        {
            instance = this;
        }

        public void Start()
        {
            lItemManager.Add(new ModelItemManager<SpellModel>());
            lItemManager.Add(new ModelItemManager<StoryModel>());
        }

        public List<IModel> GetRandomItems(Type type, bool canRepeatInventory, int amount, List<string> lItemsToAvoid)
        {
            foreach (var itemManager in lItemManager)
            {
                if (itemManager.ItemType == type)
                {
                    var items = itemManager.GetRandomItems(canRepeatInventory, amount, lItemsToAvoid);
                    return items;
                }
            }
            return new List<IModel>();
        }
    }
}