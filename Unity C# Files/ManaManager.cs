using GameLogic.Scripts.EventBus.Events;
using UnityEngine;

public class ManaManager : BasicMonobehaviour
{
    public GameObject goManaContainer;
    public GameObject goPreviewManaContainer;
    public GameObject goManaPrefab;


    bool firstTurn = false;

    public static ManaManager instance;

    public ManaManager()
    {
        instance = this;
    }

    public int GetManaOnHand(bool preview)
    {
        int children = 0;
        foreach (Transform t in goManaContainer.transform)
        {
            if (t.gameObject.activeSelf)
            {
                children++;
            }
        }
        if (preview)
        {
            foreach (Transform t in goPreviewManaContainer.transform)
            {
                if (t.gameObject.activeSelf)
                {
                    children++;
                }
            }
        }
        return children;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        eventBus.Subscribe<OnBattleStarted>(OnBattleStarted);
        eventBus.Subscribe<OnTurnStarted>(OnTurnStarted);
        eventBus.Subscribe<OnRequestAddManaToHand>(OnRequestAddManaToHand);
        eventBus.Subscribe<OnResetPreview>(OnResetPreview);
    }


    private void OnResetPreview(OnResetPreview onResetPreview)
    {
        foreach(Transform t in goPreviewManaContainer.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void OnRequestAddManaToHand(OnRequestAddManaToHand onRequestAddManaToHand)
    {
        CreateManaWithValue(onRequestAddManaToHand.value, onRequestAddManaToHand.preview);
    }

    private void CreateManaWithValue(int manaValue, bool preview)
    {
        CreateMana(preview).GetComponent<ManaBlob>().SetManaValueManually(manaValue);
    }

    private GameObject CreateMana(bool preview)
    {
        if (preview)
        {
            GameObject goTempBlob = Instantiate(goManaPrefab, goPreviewManaContainer.transform);
            goTempBlob.GetComponent<ManaBlob>().SetPreview(true);
            return goTempBlob;
        }
        else
        {
            return Instantiate(goManaPrefab, goManaContainer.transform);
        }
    }

    public void OnBattleStarted(OnBattleStarted onBattleStarted)
    {
        foreach(Transform t in goManaContainer.transform)
        {
            Destroy(t.gameObject);
        }
        foreach (Transform t in goPreviewManaContainer.transform)
        {
            Destroy(t.gameObject);
        }

        firstTurn = true;
    }

    public void OnTurnStarted(OnTurnStarted onTurnStarted)
    {
        int manaToDraw = firstTurn ? PlayerData.instance.GetStartingManaAmount() : PlayerData.instance.GetPerTurnManaAmount();
        for (int i = 0; i < manaToDraw; i++)
        {
            CreateMana(false);
        }
        firstTurn = false;
    }

}
