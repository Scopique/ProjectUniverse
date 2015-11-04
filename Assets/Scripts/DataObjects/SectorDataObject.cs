using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class SectorDataObject
{
    public int sectorID;
    public string sectorName;


    public SectorDataObject()
    {
        NewSectorDataObject(0, string.Empty);
    }

    public SectorDataObject(
        int SectorID,
        string SectorName)
    {
        NewSectorDataObject(SectorID, SectorName);
    }

    private void NewSectorDataObject(
        int SectorID,
        string SectorName)
    {
        this.sectorID = SectorID;
        this.sectorName = SectorName;
    }
}
