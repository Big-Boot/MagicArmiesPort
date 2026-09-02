using Assets.SimpleLocalization;
using System;
using UnityEngine;
using static RelicModel;

namespace ItemSystem
{
    [System.Serializable]
    public class SpellModel : ItemBase, IModel
    {
        public SpellType spellType = SpellType.Utility;
        public ModelRarity modelRarity = ModelRarity.Common;
        public PlacementRestriction placementRestriction = PlacementRestriction.Anywhere;
        public Color imageAverageColor = Color.white;
        public int spellAmount = 3;
        public float spellDelay = .6f;
        public int maxManaBlob = 1;
        public bool dealsDamage = true;
        public int might = 0;
        public RelicFeatureContainer componentInstance;

        [Header("Prefabs")]
        public RuntimeAnimatorController customAnimator;
        private RuntimeAnimatorController _runtimeAnimator;
        public RuntimeAnimatorController runtimeAnimator
        {
            get
            {
                if(_runtimeAnimator == null)
                {
                    if (customAnimator != null)
                    {
                        _runtimeAnimator = customAnimator;
                    }
                    else
                    {
                        // Try loading an AnimatorController from Resources
                        string path = $"Characters/{itemName}/Generated/Animator";
                        _runtimeAnimator = Resources.Load<RuntimeAnimatorController>(path);
                    }
                }
                return _runtimeAnimator;
            }
        }
        public GameObject componentsPrefab;
        public GameObject effectPrefab;

        public override void UpdateUniqueProperties(ItemBase itemToChangeTo)
        {
            SpellModel newItem = (SpellModel)itemToChangeTo;

            customAnimator = newItem.customAnimator;
            placementRestriction = newItem.placementRestriction;
            imageAverageColor = newItem.imageAverageColor;
            modelRarity = newItem.modelRarity;
            maxManaBlob = newItem.maxManaBlob;
            componentsPrefab = newItem.componentsPrefab;
            effectPrefab = newItem.effectPrefab;
            spellAmount = newItem.spellAmount;
            spellDelay = newItem.spellDelay;
            dealsDamage = newItem.dealsDamage;
            spellType = newItem.spellType;
            might = newItem.might;

            if (Application.isPlaying)
            {
                componentInstance = GameObject.Instantiate(componentsPrefab, RelicContainer.instance.transform).GetComponent<RelicFeatureContainer>();
                componentInstance.Initialize(this);
            }
        }

        public GameObject GetGameObject()
        {
            return componentInstance.gameObject;
        }

        public ModelRarity GetModelRarity()
        {
            return modelRarity;
        }

        public string GetModelRarityText(bool tooltip)
        {
            string rarity = LocalizationManager.Localize(GetModelRarity().ToString());
            if (tooltip)
            {
                rarity = TooltipColorManager.GetRarityColorCharacter(GetModelRarity()) + rarity + TooltipColorManager.NormalTextCharacter;
            }
            return rarity;
        }

        public string GetType(bool tooltip)
        {
            if (tooltip)
            {
                return TooltipColorManager.GetSpellTypeCharacter(spellType) + LocalizationManager.Localize(spellType.ToString()) + TooltipColorManager.NormalTextCharacter;
            }
            else
            {
                return LocalizationManager.Localize(spellType.ToString());
            }
        }

        public string GetDescription(bool showRarity = true, int overrideMight = -1)
        {

            GameObject componentToCheck = componentInstance == null ? componentsPrefab : componentInstance.gameObject;

            string description = "";
            if (showRarity)
            {
                description += GetLocalizedName() + " | " + GetModelRarityText(true) + "\n" + "\n";
            }
            else
            {
                description += GetLocalizedName() + "\n" + "\n";
            }

            foreach (RelicFeature relicFeature in componentToCheck.GetComponentsInChildren<RelicFeature>())
            {
                description += relicFeature.GetDescription() + "\n";
            }

            if (placementRestriction == PlacementRestriction.BlueCrystal)
            {
                description += "\n" + LocalizationManager.Localize("Requires") + " " + TooltipColorManager.SpriteBlueCrystal;
            }
            if (placementRestriction == PlacementRestriction.RedCrystal)
            {
                description += "\n" + LocalizationManager.Localize("Requires") + " " + TooltipColorManager.SpriteRedCrystal;
            }

            int finalMight = might;

            if (overrideMight >= 0)
            {
                finalMight = overrideMight;
            }

            if (finalMight > 0)
            {
                description += "\n" + TooltipColorManager.SpriteMight + " " + LocalizationManager.Localize("X-Might").Replace("<0>", (finalMight >= 0 ? "+" : "") + finalMight.ToString()) + "\n";
            }


            //now tutorials
            if (finalMight > 0)
            {
                description += "\n------\n\n" + TooltipColorManager.SurroundTextAndLeaveNormalTextAfter(TooltipColorManager.SpriteMight + " " + TooltipColorManager.HighlightCharacter2 + LocalizationManager.Localize("MightTutorial"));
            }

            if(IsActivable())
            {
                int cooldown = GetActivableCooldown();
                if (cooldown == 1)
                {
                    description += "\n------\n\n" + TooltipColorManager.SurroundTextAndLeaveNormalTextAfter(TooltipColorManager.SpriteActivate + " " + TooltipColorManager.HighlightCharacter2 + LocalizationManager.Localize("ActivateTutorial1"));
                }
                else
                {
                    description += "\n------\n\n" + TooltipColorManager.SurroundTextAndLeaveNormalTextAfter(TooltipColorManager.SpriteActivate + " " + TooltipColorManager.HighlightCharacter2 + LocalizationManager.Localize("ActivateTutorial").Replace("<0>", cooldown.ToString()));
                }
            }

            return description;
        }

        public string GetName()
        {
            return itemName;
        }

        public string GetLocalizedName()
        {
            return LocalizationManager.Localize(GetName());
        }

        public int GetActivableCooldown()
        {
            ActivateRelicFeatureBase activable = componentsPrefab.GetComponent<ActivateRelicFeatureBase>();
            if (activable != null)
            {
                return activable.cooldown;
            }
            return 0;
        }

        public bool IsActivable()
        {
            return componentsPrefab.GetComponent<ActivateRelicFeatureBase>() != null;
        }


    }

}