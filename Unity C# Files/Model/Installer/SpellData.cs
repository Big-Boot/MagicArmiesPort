using ItemSystem;
using System;
using UnityEngine;

[Serializable]
public class SpellData
{
    public string spellName;
    public int row;
    public int column;
    public bool equipped;
    public SpellModel spellModel
    {
        get
        {
            InitializeSpellModel();
            return _spellModel;
        }
    }
    [NonSerialized]
    SpellModel _spellModel;

    public RelicFeatureContainer component;

    public SpellData(string spellName, int row, int column, bool equipped)
    {
        this.spellName = spellName;
        this.row = row;
        this.column = column;
        this.equipped = equipped;
        _spellModel = null;
        InitializeSpellModel();
    }

    private void InitializeSpellModel()
    {
        if (_spellModel == null)
        {
            if(spellName==null)
            {
                return;
            }
            if(spellName.Equals(""))
            {
                return;
            }
            _spellModel = ItemSystemUtility.GetItemCopy<SpellModel>(spellName, ItemType.SpellModel);
            component = _spellModel.componentInstance;
        }
    }
}
