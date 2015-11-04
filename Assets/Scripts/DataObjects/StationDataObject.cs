using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class StationDataObject
{
    public int stationID;
    public int sectorID;
    public Vector3 stationPosition;
    public string stationName;


    public StationDataObject()
    {
        NewStationDataObject(0, 0, Vector3.zero, string.Empty);
    }

    public StationDataObject(
        int StationID,
        int SectorID,
        string StationName)
    {
        NewStationDataObject(StationID, SectorID, Vector3.zero, StationName);
    }

    public StationDataObject(
        int StationID,
        int SectorID,
        Vector3 StationPosition,
        string StationName)
    {
        NewStationDataObject(StationID, SectorID, StationPosition, StationName);
    }

    private void NewStationDataObject(
        int StationID,
        int SectorID,
        Vector3 StationPosition,
        string StationName)
    {
        this.stationID = StationID;
        this.sectorID = SectorID;
        this.stationPosition = StationPosition;
        this.stationName = StationName;
    }
}
