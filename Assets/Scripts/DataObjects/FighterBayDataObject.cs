using System;
using System.Collections.Generic;

[Serializable]
public class FighterBayDataObject: _BaseHardwareDataObject
{
    public float baseToHit;
    public float baseDamage;
    public string damageType;
    public float baseRange;
    public int totalFighters;
    public int currentFighters;        //TODO: put this on the HULL or PLAYER object

    
    public FighterBayDataObject()
    {
        NewFighterBayDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0);
    }

    public FighterBayDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost)
    {
        NewFighterBayDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0);
    }

    public FighterBayDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int TotalFighters, int CurrentFighters)
    {
        NewFighterBayDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            BaseToHit, BaseDamage, DamageType, BaseRange, TotalFighters,  CurrentFighters);
    }

    private void NewFighterBayDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int TotalFighters, int CurrentFighters)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.baseToHit = BaseToHit;
        this.baseDamage = BaseDamage;
        this.damageType = DamageType;
        this.baseRange = BaseRange;
        this.totalFighters = TotalFighters;
        this.currentFighters = CurrentFighters;
    }
}

