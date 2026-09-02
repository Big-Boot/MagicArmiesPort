using GameLogic.Scripts.EventBus.Events;
using ItemSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlanningGUI : BasicMonobehaviour
{
    public GameObject goContainer;
    
    public List<PlanningSpellSlot> lSpellGrid;

    public GameObject goEnemyContainer;
    public Button btFinishPlanning;

    public GameObject goSpellContainer;
    public GameObject prefabSpellItem;

    public Image imgEnemy;
    public TMP_Text txEnemyName;
    public TMP_Text txEnemyDescription;

    public SimpleTooltipStyle tooltipStyle;

    public static PlanningGUI instance;
    public PlanningGUI()
    {
        instance = this;
    }

    private void Start()
    {
        eventBus.Subscribe<OnStartPlanning>(OnStartPlanning);


        foreach (AbstractSpellSlot ass in GetComponentsInChildren<AbstractSpellSlot>(true))
        {
            ass.Init();
        }
    }

    public void OnStartPlanning(OnStartPlanning onStartPlanning)
    {
        goContainer.SetActive(true);

        foreach(Transform t in goSpellContainer.transform)
        {
            Destroy(t.gameObject);
        }

        foreach(SpellData spellOwned in PlayerData.instance.lSpells)
        {
            if(spellOwned.equipped)
            {
                continue;
            }
            GameObject goSpellItem = Instantiate(prefabSpellItem, goSpellContainer.transform);
            SpellBlob spellBlob = goSpellItem.GetComponent<SpellBlob>();
            spellBlob.LoadSpell(spellOwned);
        }

        EnemyModel enemy = GameManager.instance.GetEnemy();
        txEnemyDescription.text = STController.instance.ProcessText(enemy.GetDescription(), tooltipStyle);
        txEnemyName.text = enemy.GetLocalizedName();
        imgEnemy.sprite = enemy.itemSprite;

        goEnemyContainer.gameObject.SetActive(onStartPlanning.showEnemy);
        btFinishPlanning.gameObject.SetActive(onStartPlanning.showEnemy);
    }

    public void FinishedPlanning()
    {
        goContainer.SetActive(false);
        eventBus.Publish(new OnFinishedPlanning());
    }

}