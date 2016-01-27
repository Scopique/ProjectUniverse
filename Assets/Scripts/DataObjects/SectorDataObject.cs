using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class SectorDataObject
{
    public int sectorID;
    public string sectorName;
    public Vector2 sectorMapCoordinates;


    public SectorDataObject()
    {
        NewSectorDataObject(0, string.Empty, Vector2.zero);
    }

    public SectorDataObject(
        int SectorID,
        string SectorName)
    {
        NewSectorDataObject(SectorID, SectorName, Vector2.zero);
    }

    public SectorDataObject(
        int SectorID,
        string SectorName,
        Vector2 SectorMapCoordinates)
    {
        NewSectorDataObject(SectorID, SectorName, SectorMapCoordinates);
    }

    private void NewSectorDataObject(
        int SectorID,
        string SectorName,
        Vector2 SectorMapCoordinates)
    {
        this.sectorID = SectorID;
        this.sectorName = SectorName;
        this.sectorMapCoordinates = SectorMapCoordinates;
    }
}
