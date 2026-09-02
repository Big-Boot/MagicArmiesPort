using Assets.SimpleLocalization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace ItemSystem
{
    [System.Serializable]
    public class EnemyModel : ItemBase
    {

        public int hp = 10;
        
        [HideInInspector]
        public int currentHP = 10;

        public GameObject bossModel;


        public RuntimeAnimatorController leaderAnimator;

        public List<string> lDialoguesPerTurn;
        public List<EnemyManaPoint> lDeckNumber;
        public List<ManaElement> lDeckElement;

        [HideInInspector]
        public List<EnemyManaPoint> lDrawnDeckNumbers = new List<EnemyManaPoint>();

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            EnemyModel newItem = (EnemyModel)itemToChangeTo;
            hp = newItem.hp;
            bossModel = newItem.bossModel;
            currentHP = newItem.hp;
            lDeckElement = newItem.lDeckElement;
            lDeckNumber = newItem.lDeckNumber;
            leaderAnimator = newItem.leaderAnimator;
            lDrawnDeckNumbers = new List<EnemyManaPoint>();
            lDialoguesPerTurn = newItem.lDialoguesPerTurn;
        }

        public EnemyManaPoint GetRandomAttackValue()
        {
            if(lDeckNumber.Count == lDrawnDeckNumbers.Count)
            {
                lDrawnDeckNumbers.Clear();
            }
            EnemyManaPoint manaPoint = lDeckNumber.Randomize().Where(m => !lDrawnDeckNumbers.Contains(m)).Take(1).ToList()[0];

            EnemyManaPoint copiedManaPoint = new EnemyManaPoint(manaPoint.value, manaPoint.goUnit, manaPoint.unitAmount, manaPoint.spriteCellSize);

            lDrawnDeckNumbers.Add(manaPoint);

            //copiedManaPoint.value = copiedManaPoint.value > GameManager.instance.enemy.currentHP ? GameManager.instance.enemy.currentHP : copiedManaPoint.value;

            return copiedManaPoint;
        }

        public string GetDescription()
        {
            if (bossModel == null)
            {
                return LocalizationManager.Localize(itemName.Replace(" ", "-") + "-Description");
            }
            else
            {
                return bossModel.GetComponent<RelicFeatureContainer>().GetDescription();
            }
        }

        public string GetLocalizedName()
        {
            return LocalizationManager.Localize(itemName.Replace(" ", "-"));
        }

    }
}