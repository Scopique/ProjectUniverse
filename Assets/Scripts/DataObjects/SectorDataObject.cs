using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class SectorDataObject
{
    public int sectorID;
    public string sectorName;
    public Vector2 sectorMapCoordinates;
    //These fluctuate with changes in condition
    public int common = 90;
    public int luxury = 10;
    public int food = 80;
    public int minerals = 60;
    public int medical = 40;
    public int military = 30;
    public int industrial = 50;


    public SectorDataObject()
    {
        NewSectorDataObject(0, string.Empty, Vector2.zero, 0, 0, 0, 0, 0, 0, 0);
    }

    public SectorDataObject(
        int SectorID,
        string SectorName)
    {
        NewSectorDataObject(SectorID, SectorName, Vector2.zero, 0, 0, 0, 0, 0, 0, 0);
    }

    public SectorDataObject(
        int SectorID,
        string SectorName,
        Vector2 SectorMapCoordinates)
    {
        NewSectorDataObject(SectorID, SectorName, SectorMapCoordinates, 0, 0, 0, 0, 0, 0, 0);
    }

    private void NewSectorDataObject(
        int SectorID,
        string SectorName,
        Vector2 SectorMapCoordinates,
        int Common,
        int Luxury,
        int Food,
        int Minerals,
        int Medical,
        int Military,
        int Industrial)
    {
        this.sectorID = SectorID;
        this.sectorName = SectorName;
        this.sectorMapCoordinates = SectorMapCoordinates;
        this.common = Common == 0 ? this.common : Common;
        this.luxury = Luxury == 0 ? this.luxury : Luxury;
        this.food = Food == 0 ? this.food : Food;
        this.minerals = Minerals == 0 ? this.minerals : Minerals;
        this.medical = Medical == 0 ? this.medical : Medical;
        this.military = Military == 0 ? this.military : Military;
        this.industrial = Industrial == 0 ? this.industrial : Industrial;
    }
}
