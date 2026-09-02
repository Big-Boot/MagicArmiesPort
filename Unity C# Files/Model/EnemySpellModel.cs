using Assets.SimpleLocalization;
using UnityEngine;
using static RelicModel;

public class EnemySpellModel : IModel
{
    public GameObject gameObject;
    public string itemName;
    public ModelRarity modelRarity;

    public EnemySpellModel(GameObject gameObject, string itemName, ModelRarity modelRarity)
    {
        this.gameObject = gameObject;
        this.itemName = itemName;
        this.modelRarity = modelRarity;
    }   

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public string GetLocalizedName()
    {
        return LocalizationManager.Localize(itemName.Replace(" ", "-"));
    }

    public ModelRarity GetModelRarity()
    {
        return modelRarity;
    }

    public string GetName()
    {
        return itemName;
    }
}
