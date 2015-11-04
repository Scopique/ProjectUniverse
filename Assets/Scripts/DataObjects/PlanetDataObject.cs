using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class PlanetDataObject
{
    public int planetID;
    public int sectorID;
    public string planetName;


    public PlanetDataObject()
    {
        NewPlanetDataObject(0, 0, string.Empty);
    }

    public PlanetDataObject(
        int PlanetID,
        int SectorID,
        string PlanetName)
    {
        NewPlanetDataObject(PlanetID, SectorID, PlanetName);
    }

    private void NewPlanetDataObject(
        int PlanetID,
        int SectorID,
        string PlanetName)
    {
        this.planetID = PlanetID;
        this.sectorID = SectorID;
        this.planetName = PlanetName;
    }
}
