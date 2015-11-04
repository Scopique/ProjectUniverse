using System;
using System.Collections.Generic;

[Serializable]
public class CargoDataObject: _BaseHardwareDataObject 
{
    public int capacity;

    public CargoDataObject()
    {
        NewCargoDataObject(
            0,
            string.Empty,
            string.Empty,
            0,
            string.Empty,
            0,
            0);
    }

    public CargoDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, int Capacity)
    {
        NewCargoDataObject(
            ID,
            Name,
            HardwareCode,
            HardwareClass,
            Portrait,
            BaseCost,
            Capacity);
    }

    private void NewCargoDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, int Capacity)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;
        this.capacity = Capacity;
    }
}


