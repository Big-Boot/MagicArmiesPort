using Assets.SimpleLocalization;
using UnityEngine;
using static RelicModel;

public partial class RelicModel : RelicFeatureContainer, IModel
{

    public const int APROX_GOLD_GAINED_PER_ROUND = 100;
    public const int RELIC_COMMON_PRICE = APROX_GOLD_GAINED_PER_ROUND/3;
    public const int RELIC_RARE_PRICE = APROX_GOLD_GAINED_PER_ROUND;
    public const int RELIC_EPIC_PRICE = APROX_GOLD_GAINED_PER_ROUND * 2;
    public const int RELIC_LEGENDARY_PRICE = APROX_GOLD_GAINED_PER_ROUND * 3;

    //public const int RELIC_COMMON_PRICE = 30;
    //public const int RELIC_RARE_PRICE = 50;
    //public const int RELIC_EPIC_PRICE = 100;
    //public const int RELIC_LEGENDARY_PRICE = 150;
    
    public string relicName;
    public Sprite icon;
    public ModelRarity relicRarity;
    [Range(.01f,3f)]
    public float priceModifier = 1f;
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public int GetPrice()
    {
        int basePrice = 0;
        switch(relicRarity)
        {
            case ModelRarity.Common:
                basePrice += RELIC_COMMON_PRICE;
                break;
            //case ModelRarity.Rare:
            //    basePrice += RELIC_RARE_PRICE;
            //    break;
            case ModelRarity.Epic:
                basePrice += RELIC_EPIC_PRICE;
                break;
            case ModelRarity.Legendary:
                basePrice += RELIC_LEGENDARY_PRICE;
                break;
        }
        return Mathf.CeilToInt(basePrice * priceModifier);
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public ModelRarity GetModelRarity()
    {
        return relicRarity;
    }

    public string GetModelRarityText(bool tooltip)
    {
        string rarity = LocalizationManager.Localize(GetModelRarity().ToString());
        if (tooltip)
        {
            rarity = TooltipColorManager.GetRarityColorCharacter(GetModelRarity()) + rarity+ TooltipColorManager.NormalTextCharacter;
        }
        return rarity;
    }

    public override string GetDescription(bool showRarity = true)
    {

        string description = "";
        if (showRarity)
        {
            description += GetModelRarityText(true) + "\n";
        }

        description += GetLocalizedName() + "\n" + "\n";

        description += base.GetDescription()+"\n";
        

        return description;
    }

    public string GetName()
    {
        return relicName;
    }
    public string GetLocalizedName()
    {
        return LocalizationManager.Localize(relicName.Replace(" ", "-"));
    }

}
