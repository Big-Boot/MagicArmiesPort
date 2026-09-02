using System.Collections.Generic;
using UnityEngine;
public class Relic : MonoBehaviour
{
    public List<RelicFeature> lRelicFeature;
    public SpellData item = null;
    public int index = 0;

    public RelicModel GetRelicModel()
    {
        return GetComponent<RelicModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lRelicFeature = new List<RelicFeature>(GetComponentsInChildren<RelicFeature>());
    }

    public void LoadDataBlob(SpellData item, int defaultIndex)
    {
        this.item = item;
    }
}
