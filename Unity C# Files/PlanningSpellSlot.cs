using GameLogic.Scripts.EventBus.Events;
using ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static RelicModel;

public class PlanningSpellSlot : AbstractSpellSlot, IDragHandler, IBeginDragHandler
{
    public Image imgLoadedPower;
    public GameObject prefabSpellBlob;
    bool initialized = false;

    public Image imgPlacementRestriction;

    private void Start()
    {
        Init();
    }

    protected override void Unload()
    {
        base.Unload();
        imgLoadedPower.gameObject.SetActive(loaded);
    }

    public override void LoadCurrentSpell()
    {
        base.LoadCurrentSpell();
        imgLoadedPower.gameObject.SetActive(loaded);
    }

    public override void LoadSpell(SpellData spellOwned)
    {
        base.LoadSpell(spellOwned);
        imgLoadedPower.sprite = spellOwned.spellModel.itemIcon;
        imgLoadedPower.gameObject.SetActive(loaded);

        PlayerData.instance.saveData();
    }

    public override void Init()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;
        imgLoadedPower.gameObject.SetActive(loaded);
        eventBus.Subscribe<OnGameStarted>(OnGameStarted);
    }

    public PlacementRestriction GetPlacementRestriction()
    {
        foreach (SocketData socketData in PlayerData.instance.lSockets)
        {
            if (socketData.row == rowNumber && socketData.column == columnNumber)
            {
                return socketData.placementRestriction;
            }
        }
        return PlacementRestriction.Anywhere;
    }

    protected override void OnGameStarted(OnGameStarted onGameStarted)
    {
        base.OnGameStarted(onGameStarted);
        RelicModel.PlacementRestriction placementRestriction = GetPlacementRestriction();
        if (placementRestriction != RelicModel.PlacementRestriction.Anywhere)
        {
            imgPlacementRestriction.gameObject.SetActive(true);
            imgPlacementRestriction.sprite = SpriteManager.instance.GetSpriteForPlacementRestriction(placementRestriction);
        }
        else
        {
            imgPlacementRestriction.gameObject.SetActive(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (loaded)
        {
            SpellBlob spellBlob = Instantiate(prefabSpellBlob, PlanningGUI.instance.goSpellContainer.transform).GetComponent<SpellBlob>();
            spellBlob.OnBeginDrag(eventData);
            spellBlob.transform.position = eventData.position;
            spellBlob.LoadSpell(spellOwned);
            eventData.pointerDrag = spellBlob.gameObject;
            Unload();
        }
    }
}
