using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemSystem
{
    [System.Serializable]
    public class CharacterModel : ItemBase
    {

        public Sprite dialoguePortrait;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            CharacterModel newItem = (CharacterModel)itemToChangeTo;
            dialoguePortrait = newItem.dialoguePortrait;
        }

        public string GetLocalizedName()
        {
            return itemName;
        }

        public string GetName()
        {
            return itemName;
        }
    }
}