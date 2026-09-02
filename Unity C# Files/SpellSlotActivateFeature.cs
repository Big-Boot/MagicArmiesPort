using UnityEngine;
using UnityEngine.EventSystems;

public class SpellSlotActivateFeature: MonoBehaviour
{
    public EventSystem eventSystem;
    SpellSlot spellSlot;
    private bool isMouseOver = false;

    private void Start()
    {
        spellSlot = GetComponent<SpellSlot>();
        eventSystem = EventSystem.current;
    }
    void Update()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        // Raycast to find potential targets
        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        bool isNowOver = false;

        foreach (var result in raycastResults)
        {
            if (result.gameObject == this.gameObject)
            {
                isNowOver = true;
                break;
            }
        }

        // Detect enter
        if (isNowOver && !isMouseOver)
        {
            isMouseOver = true;
            OnMouseEnter();
        }
        // Detect exit
        else if (!isNowOver && isMouseOver)
        {
            isMouseOver = false;
            OnMouseExit();
        }

        if (isMouseOver && Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }

    void OnMouseDown()
    {
        if (GameManager.instance.GetDraggingMana())
        {
            return;
        }

        GameManager.instance.ChangePreview(null);
        spellSlot.Activate(false);
    }

    void OnMouseEnter()
    {
        if (GameManager.instance.IsPreviewing() || GameManager.instance.GetDraggingMana())
        {
            return;
        }

        if (GameManager.instance.ChangePreview(spellSlot))
        {
            spellSlot.Activate(true);
        }
    }

    void OnMouseExit()
    {
        if (!GameManager.instance.IsPreviewing() || GameManager.instance.GetDraggingMana())
        {
            return;
        }

        if(GameManager.instance.GetPreviewingSlot()!=spellSlot)
        {
            return;
        }

        GameManager.instance.ChangePreview(null);
    }

}