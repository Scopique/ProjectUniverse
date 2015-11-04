using System;
using System.Collections.Generic;

[Serializable]
public class MissileLauncherDataObject: _BaseHardwareDataObject
{
    public float baseToHit;
    public float baseDamage;
    public string damageType;
    public float baseRange;
    public int magazine;
    public float reloadSpeed;
    public int roundsOnHand;        //TODO: put this on the HULL or PLAYER object

    
    public MissileLauncherDataObject()
    {
        NewMissileLauncherDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0.0f, 0);
    }

    public MissileLauncherDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost)
    {
        NewMissileLauncherDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            0.0f, 0.0f, string.Empty, 0.0f, 0, 0.0f, 0);
    }

    public MissileLauncherDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int Magazine, float ReloadSpeed, int RoundsOnHand)
    {
        NewMissileLauncherDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            BaseToHit, BaseDamage, DamageType, BaseRange, Magazine, ReloadSpeed, RoundsOnHand);
    }

    private void NewMissileLauncherDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseToHit, float BaseDamage, string DamageType, float BaseRange, int Magazine, float ReloadSpeed, int RoundsOnHand)
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
        this.magazine = Magazine;
        this.reloadSpeed = ReloadSpeed;
        this.roundsOnHand = RoundsOnHand;
    }
}
