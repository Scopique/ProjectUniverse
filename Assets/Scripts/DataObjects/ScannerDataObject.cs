using System;
using System.Collections.Generic;

[Serializable]
public class ScannerDataObject: _BaseHardwareDataObject
{
    public float baseRange;
    public int signatureThreshold;

    public ScannerDataObject()
    {
        NewScannerDataObject(0, string.Empty, string.Empty, 0, string.Empty, 0, 0.0f, 0);
    }

    public ScannerDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float BaseRange, int SignatureThreshold)
    {
        NewScannerDataObject(ID, Name, HardwareCode, HardwareClass, Portrait, BaseCost, BaseRange, SignatureThreshold);
    }


    private void NewScannerDataObject(int ID, string Name, string HardwareCode, int HardwareClass, string Portrait, long BaseCost, float BaseRange, int SignatureThreshold)
    {
        this.iD = ID;
        this.name = Name;
        this.hardwareCode = HardwareCode;
        this.hardwareClass = HardwareClass;
        this.portrait = Portrait;
        this.baseCost = BaseCost;

        this.baseRange = BaseRange;
        this.signatureThreshold = SignatureThreshold;
    }
}

