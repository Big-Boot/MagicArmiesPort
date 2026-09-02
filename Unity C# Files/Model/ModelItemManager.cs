using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RelicModel;

namespace ItemSystem
{
    public abstract class ModelItemManagerBase
    {
        public abstract Type ItemType { get; }

        public abstract List<IModel> GetRandomItems(bool canRepeatInventory, int amount, List<string> lItemsToAvoid);
    }

    public class ModelItemManager<T>: ModelItemManagerBase where T : ItemBase, new()
    {
        public List<T> lItems;
        public Dictionary<ModelRarity, T> dModel;

        public ModelItemManager()
        {
            LoadAllItems();
            dModel = new Dictionary<ModelRarity, T>();
        }

        public override Type ItemType => typeof(T);

        protected void LoadAllItems()
        {
            lItems = new List<T>(ItemSystemUtility.GetAllTypeItems<T>((ItemType)Enum.Parse(typeof(ItemType), typeof(T).Name)));
        }

        protected bool PlayerHasModel(IModel model)
        {
            return PlayerData.instance.HasIModel(model);
        }


        public override List<IModel> GetRandomItems(bool canRepeatInventory, int amount, List<string> lItemsToAvoid)
        {
            List<IModel> lItemsPicked = new List<IModel>();

            List<IModel> lRandomItems = lItems.Cast<IModel>().ToList();
            lRandomItems = lRandomItems.Randomize();

            foreach (IModel model in lRandomItems)
            {
                if (lItemsToAvoid.Contains(model.GetName()))
                    continue;
                if (!canRepeatInventory && PlayerHasModel(model))
                {
                    continue;
                }
                if(model.GetModelRarity() == ModelRarity.Starter)
                {
                    // Skip starter models
                    continue;
                }

                lItemsPicked.Add(model);
                if (lItemsPicked.Count >= amount)
                    break;
            }
            return lItemsPicked;
        }
    }
}