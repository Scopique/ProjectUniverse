using System;
using System.Collections.Generic;

[Serializable]
public class CannonDataObject: _BaseHardwareDataObject
{
    public float BaseToHit;
    public float BaseDamage;
    public string DamageType;
    public float BaseRange;
    public int RateOfFire;
    public int RoundsFired;
    public int AmmoCapacity;

    
    public CannonDataObject()
    {
        NewCannonDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0, 0);
    }

    public CannonDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost)
    {
        NewCannonDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0, 0);
    }

    public CannonDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int RateOfFire, int RoundsFired, int AmmoCapacity)
    {
        NewCannonDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            BaseToHit, BaseDamage, DamageType, BaseRange, RateOfFire, RoundsFired, AmmoCapacity);
    }

    private void NewCannonDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int RateOfFire, int RoundsFired, int AmmoCapacity)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.BaseToHit = BaseToHit;
        this.BaseDamage = BaseDamage;
        this.DamageType = DamageType;
        this.BaseRange = BaseRange;
        this.RateOfFire = RateOfFire;
        this.RoundsFired = RoundsFired;
        this.AmmoCapacity = AmmoCapacity;
    }
}

