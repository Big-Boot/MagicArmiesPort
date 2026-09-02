using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManaBlob : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int mana = 5;
    public TMP_Text txMana;

    private Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    bool manuallySet = false;
    bool preview = false;

    public GameObject prefabVFX;
    LayoutElement layoutElement;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
        layoutElement = GetComponent<LayoutElement>();
    }

    private void Start()
    {
        if (!manuallySet)
        {
            SetManaValue();
        }
    }

    public void SetPreview(bool preview)
    {
        this.preview = preview;
    }
    public void SetManaValueManually(int value)
    {
        manuallySet = true;
        SetManaValue(value);
    }

    public void SetManaValue(int value = -1)
    {
        if (value < 0 && !preview)
        {
            value = GameManager.instance.GetRandomAttackValue().value;//Random.Range(3, 6);
        }
        mana = value;
        if (preview && mana == -1)
        {
            txMana.text = "?";
        }
        else
        {
            txMana.text = mana.ToString();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
        layoutElement.ignoreLayout = true;
        GameManager.instance.SetDraggingMana(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;// / canvas.scaleFactor;

        // Create a ray from the screen pointer position
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            SpellSlot slot = hit.collider.GetComponent<SpellSlot>();
            if (slot != null)
            {
                if (slot.loaded && !slot.IsFullyLoaded(false))
                {
                    // Load Mana Preview
                    if (GameManager.instance.ChangePreview(slot))
                    {
                        slot.LoadMana(mana, true, true,0);
                    }
                    return;
                }
            }
        }

        GameManager.instance.ChangePreview(null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.instance.SetDraggingMana(false);
        canvasGroup.blocksRaycasts = true;

        layoutElement.ignoreLayout = false;
        // Create a ray from the screen pointer position
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            SpellSlot slot = hit.collider.GetComponent<SpellSlot>();
            if (slot != null)
            {
                if (slot.loaded && !slot.IsFullyLoaded(false))
                {
                    GameManager.instance.ChangePreview(null);
                    // Load Mana and hide this image
                    slot.LoadMana(mana, true, false,0);
                    gameObject.SetActive(false);
                    GameObject vfx = Instantiate(prefabVFX, slot.transform);
                    return;
                }
            }
        }

        // Return to original position if no valid target
        transform.position = originalPosition;
    }
}
