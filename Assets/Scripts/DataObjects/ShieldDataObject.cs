using System;
using System.Collections.Generic;

[Serializable]
public class ShieldDataObject: _BaseHardwareDataObject
{
    public float Strength;
    
    public ShieldDataObject()
    {
        NewShieldDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0, 0.0f);
    }

    public ShieldDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float Strength)
    {
        NewShieldDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost, Strength);
    }

    private void NewShieldDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float Strength)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.Strength = Strength;
    }
}

