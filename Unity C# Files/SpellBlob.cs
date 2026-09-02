using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ItemSystem;
using System.Collections.Generic;

public class SpellBlob : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    SpellData spellOwned;

    public Image imgLoadedSpell;
    public Image imgBlobImage;
    public Image imgPlacementRestriction;

    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    public GameObject goManaChargeContainer;
    public List<ManaChargeItem> lManaChargeItems;

    public GameObject prefabManaCharge;
    public GameObject prefabVFX;

    LayoutElement layoutElement;

    [SerializeField] private SimpleTooltip tooltip;

    private void Awake()
    {
        layoutElement = GetComponent<LayoutElement>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
    }

    public void LoadSpell(SpellData spellOwned)
    {
        this.spellOwned = spellOwned;
        imgLoadedSpell.sprite = spellOwned.spellModel.itemIcon;
        imgBlobImage.color = spellOwned.spellModel.imageAverageColor;
        tooltip.infoLeft = spellOwned.spellModel.GetDescription();
        tooltip.infoRight = spellOwned.spellModel.GetType(true);

        if (spellOwned.spellModel.placementRestriction != RelicModel.PlacementRestriction.Anywhere)
        {
            imgPlacementRestriction.gameObject.SetActive(true);
            imgPlacementRestriction.sprite = SpriteManager.instance.GetSpriteForPlacementRestriction(spellOwned.spellModel.placementRestriction);
        }
        else
        {
            imgPlacementRestriction.gameObject.SetActive(false);
        }

        for (int i = 0; i < spellOwned.spellModel.maxManaBlob; i++)
        {
            lManaChargeItems.Add(Instantiate(prefabManaCharge, goManaChargeContainer.transform).GetComponent<ManaChargeItem>());
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
        transform.position = eventData.position;
        tooltip.HideTooltip();
        tooltip.enabled = false;
        layoutElement.ignoreLayout = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position += (Vector3)eventData.delta;// / canvas.scaleFactor;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        
        layoutElement.ignoreLayout = false;

        // Raycast to find potential targets
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            PlanningSpellSlot slot = result.gameObject.GetComponent<PlanningSpellSlot>();
            if (slot != null)
            {
                if (!slot.loaded)
                {
                    RelicModel.PlacementRestriction placementRestriction = slot.GetPlacementRestriction();
                    if ((spellOwned.spellModel.placementRestriction == RelicModel.PlacementRestriction.Anywhere) || (placementRestriction == spellOwned.spellModel.placementRestriction))
                    {
                        // Load Mana and hide this image
                        slot.LoadSpell(this.spellOwned);
                        GameObject vfx = Instantiate(prefabVFX, slot.transform);
                        Destroy(this.gameObject);
                        return;
                    }
                }
            }
        }

        // Return to original position if no valid target
        transform.position = originalPosition;
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponentInParent<GridLayoutGroup>().GetComponent<RectTransform>());
        tooltip.enabled = true;
    }
}
