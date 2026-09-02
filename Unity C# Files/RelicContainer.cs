using UnityEngine;

public class RelicContainer : MonoBehaviour
{
    public static RelicContainer instance;

    public RelicContainer()
    {
        instance = this;
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }
}
