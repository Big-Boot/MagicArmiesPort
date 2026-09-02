using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemSystem
{
    [System.Serializable]
    public class EncounterModel : ItemBase
    {
        public enum EnemyTier
        {
            Easy, Medium, Hard, Boss
        }
        public enum CampaignTier
        {
            Chapter1, Chapter2, Chapter3
        }

        public EnemyTier enemyTier= EnemyTier.Easy;
        public CampaignTier campaignTier = CampaignTier.Chapter1;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            EncounterModel newItem = (EncounterModel)itemToChangeTo;
            enemyTier = newItem.enemyTier;
            campaignTier = newItem.campaignTier;
        }
    }
}