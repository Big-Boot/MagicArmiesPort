using UnityEngine;

public partial class RelicModel
{
    public interface IModel
    {
        public ModelRarity GetModelRarity();
        public string GetName();
        public string GetLocalizedName();
        public GameObject GetGameObject();
    }

}
