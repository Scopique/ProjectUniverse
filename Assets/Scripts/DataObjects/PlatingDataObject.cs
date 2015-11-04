using System;
using System.Collections.Generic;

[Serializable]
public class PlatingDataObject: _BaseHardwareDataObject
{
    public float Strength;

    public PlatingDataObject()
    {
        NewPlatingDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0, 0.0f);
    }

    public PlatingDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float Strength)
    {
        NewPlatingDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost, Strength);
    }

    private void NewPlatingDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float Strength)
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

