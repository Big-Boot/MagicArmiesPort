using DG.Tweening;
using GameLogic.Scripts.EventBus.Events;
using TMPro;
using UnityEngine;

public class ManaChargeItem : BasicMonobehaviour
{
    [SerializeField] TMP_Text txMana;
    int loadedMana = 0;
    bool preview = false;
    private void Start()
    {
        eventBus.Subscribe<OnResetPreview>(OnResetPreview);
    }

    private void OnResetPreview(OnResetPreview onResetPreview)
    {
        if (preview)
        {
            ResetMana();
        }
    }

    public void ResetMana()
    {
        txMana.text = "";
        loadedMana = 0;
    }

    public int GetLoadedMana()
    {
        return loadedMana;
    }

    public void SetMana(int mana, bool preview)
    {
        this.preview = preview;
        if (preview)
        {
            loadedMana = mana;
            txMana.text = mana.ToString();
        }
        else
        {
            txMana.DOKill(); // Stop any previous tweens on this object

            loadedMana = mana;
            int displayedValue = 0;
            DOTween.To(() => displayedValue, x => {
                displayedValue = x;
                txMana.text = displayedValue.ToString();
            }, mana, 0.5f);
        }
    }
}
