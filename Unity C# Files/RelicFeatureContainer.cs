using UnityEngine;
using static RelicModel;

public class RelicFeatureContainer : MonoBehaviour
{
    protected IModel model;
    public void Initialize(IModel model)
    {
        foreach (RelicFeature relicFeature in GetComponentsInChildren<RelicFeature>())
        {
            relicFeature.Initialize(model);
        }
    }

    public void Acquired()
    {
        foreach (RelicFeature relicFeature in GetComponentsInChildren<RelicFeature>())
        {
            relicFeature.StartAcquire();
        }
    }

    public void Equip()
    {
        Unequip();
        foreach (RelicFeature relicFeature in GetComponentsInChildren<RelicFeature>())
        {
            relicFeature.DoEventBusSubscriptions();
        }
    }

    public void Unequip()
    {
        foreach (RelicFeature relicFeature in GetComponentsInChildren<RelicFeature>())
        {
            relicFeature.DoEventBusUnsubscriptions();
        }
    }

    public virtual string GetDescription(bool showRarity = true)
    {

        string description = "";
        foreach (RelicFeature relicFeature in GetComponentsInChildren<RelicFeature>())
        {
            description += relicFeature.GetDescription() + "\n";
        }

        return description;
    }

}
