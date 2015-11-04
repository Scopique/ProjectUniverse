using UnityEngine;
using System;


[Serializable]
public class HullDataObject: _BaseHardwareDataObject {

    //Classes - Values indicate maximum gear class that can be used here
    public float baseStrength;
    public float baseDefense;

    public int engineClass;
    public int cargoClass;
    public int shieldClass;
    public int cannonClass;
    public int missileClass;
    public int scannerClass;
    public int fighterClass;
    public int platingClass;

    public HullDataObject()
    {
        NewHullDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0,
            0.0f, 0.0f, 0, 0, 0, 0, 0, 0, 0, 0);
    }

    public HullDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost)
    {
        NewHullDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            0.0f, 0.0f, 0, 0, 0, 0, 0, 0, 0, 0);
    }

    public HullDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseStrength, float BaseDefense, int EngineClass, int CargoClass, int ShieldClass, int CannonClass, int MissileClass, int ScannerClass, int FighterClass, int PlatingClass)
    {
        NewHullDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost,
            BaseStrength, BaseDefense, EngineClass, CargoClass, ShieldClass, CannonClass, MissileClass, ScannerClass, FighterClass, PlatingClass);
    }

    private void NewHullDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost,
        float BaseStrength, float BaseDefense, int EngineClass, int CargoClass, int ShieldClass, int CannonClass, int MissileClass, int ScannerClass, int FighterClass, int PlatingClass)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.baseStrength = BaseStrength;
        this.baseDefense = BaseDefense;
        this.engineClass = EngineClass;
        this.cargoClass = CargoClass;
        this.shieldClass = ShieldClass;
        this.cannonClass = CannonClass;
        this.missileClass = MissileClass;
        this.scannerClass = ScannerClass;
        this.fighterClass = FighterClass;
        this.platingClass = PlatingClass;

    }
}
