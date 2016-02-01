using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


// SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
//JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");

public class NodePathfinding : MonoBehaviour {

    public List<int> SolutionPath;

    private bool isReverseLookup = false;

    private List<int> openSectors;
    private List<int> closedSectors;

    private List<int> neighboringSectors;
    

    dbSectorDataObject SectorDatabase;
    dbJumpgateDataObject JumpgateDatabase;
    
    public NodePathfinding()
    {
        
    }


    public void FindRoute(int OriginSectorID, int DestinationSectorID)
    {
        Init();

        SolutionPath.Add(OriginSectorID);
        closedSectors.Add(OriginSectorID);        //Don't come back here.

        bool haveRoute = CalculateForward(OriginSectorID, DestinationSectorID);
        if (!haveRoute)
        {
            Init();
            haveRoute = CalculateReverse(DestinationSectorID, OriginSectorID);
            if (haveRoute)
            {
                SolutionPath.Reverse();
            }
        }

        if (haveRoute)
        {
            PrintSolution();
        }
        else
        {
            Debug.LogError("There's no route from " + OriginSectorID.ToString() + " to " + DestinationSectorID.ToString());
        }

    }


    #region Private Methods
    
    void Init()
    {
        SolutionPath = new List<int>();
        
        openSectors = new List<int>();
        closedSectors = new List<int>();
        neighboringSectors = new List<int>();

        //TODO: Get this from DataController
        if (SectorDatabase == null && JumpgateDatabase == null) { 
            SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
            JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");
        }
    }

    bool CalculateForward(int OriginSectorID, int DestinationSectorID) {

        bool haveRoute = false;
        int currentSectorID = OriginSectorID;

        Vector2 DestinationSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(DestinationSectorID)).sectorMapCoordinates;

        //Get the neighboring sectors based on the starting sector
        //Put these into the openSector listing
        GetNeighboringSectors(currentSectorID);

        while (openSectors.Count > 0)
        {
            if (openSectors.Contains(DestinationSectorID))
            {
                //We're done!
                SolutionPath.Add(DestinationSectorID);
                break;
            }

            //Need to check each one to see who's got the shortest route from
            //  the currentSectorID and set the NEW currentSectorID
            currentSectorID = GetLowestCostOpenSector(DestinationSectorCoords, currentSectorID);

            //Remove other sectors who aren't the currentSectorID;
            CloseLosingSectors(currentSectorID);
            //Don't come back this way.
            closedSectors.Add(currentSectorID);     

            GetNeighboringSectors(currentSectorID);
        }

        if (SolutionPath.Contains(DestinationSectorID))
        {
            haveRoute = true;
        }

        return haveRoute;
    }

    bool CalculateReverse(int OriginSectorID, int DestinationSectorID) 
    {
        bool haveRoute = false;
        int currentSectorID = OriginSectorID;

        Vector2 DestinationSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(DestinationSectorID)).sectorMapCoordinates;

        //Get the neighboring sectors based on the starting sector
        //Put these into the openSector listing
        GetNeighboringSectors(currentSectorID);

        while (openSectors.Count > 0)
        {
            if (openSectors.Contains(DestinationSectorID))
            {
                //We're done!
                SolutionPath.Add(DestinationSectorID);
                break;
            }

            //Need to check each one to see who's got the shortest route from
            //  the currentSectorID and set the NEW currentSectorID
            currentSectorID = GetLowestCostOpenSector(DestinationSectorCoords, currentSectorID);

            //Remove other sectors who aren't the currentSectorID;
            CloseLosingSectors(currentSectorID);
            //Don't come back this way.
            closedSectors.Add(currentSectorID);

            GetNeighboringSectors(currentSectorID);
        }

        if (SolutionPath.Contains(DestinationSectorID))
        {
            haveRoute = true;
        }

        return haveRoute;
    }


    int GetLowestCostOpenSector(Vector2 DestinationSectorCoords, int CurrentSectorID)
    {
        double currentLowestValue = 10000;
        int currentLowestSector = CurrentSectorID;

        foreach(int i in openSectors)
        {
            int currentSector = i;

            Vector2 nextOpenSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(currentSector)).sectorMapCoordinates;
            double dX = Math.Pow(nextOpenSectorCoords.x - DestinationSectorCoords.x, 2);
            double dY = Math.Pow(nextOpenSectorCoords.y - DestinationSectorCoords.y, 2);
            double distance = Math.Sqrt(dX + dY);
            if (distance < currentLowestValue)
            {
                //It's a good match. Keep it in Open and
                //  set the new champ values
                currentLowestValue = distance;
                currentLowestSector = currentSector;
            }
        }

        return currentLowestSector;
    }

    //double GetSectorCost(int CurrentSectorID, int SectorIDToCheck)
    //{
    //    Vector2 currentSectorCoord = SectorDatabase.database.Find(x => x.sectorID.Equals(CurrentSectorID)).sectorMapCoordinates;
    //    Vector2 nextOpenSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(SectorIDToCheck)).sectorMapCoordinates;
    //    double dX =Math.Pow(nextOpenSectorCoords.x - currentSectorCoord.x, 2);
    //    double dY = Math.Pow(nextOpenSectorCoords.y - currentSectorCoord.y, 2);
    //    double distance = Math.Sqrt(dX + dY);
    //    return distance;
    //}

    void GetNeighboringSectors(int CurrentSectorID)
    {
        neighboringSectors = (JumpgateDatabase.database.FindAll(x => x.sectorID.Equals(CurrentSectorID)).Select(y => y.destinationSectorID)).ToList<int>();
        openSectors = (from n in neighboringSectors
                      where !(from c in closedSectors
                              select c)
                                  .Contains(n)
                      select n).ToList<int>();
        int i = 0;

    }

    void CloseLosingSectors(int WinningSector)
    {
        List<int> losingSectors = (from db in openSectors where !db.Equals(WinningSector) select db).ToList<int>();
        closedSectors.AddRange(losingSectors);

        SolutionPath.Add(WinningSector);
    }

    bool HaveDestinationInSolution(int DestinationSectorID)
    {
        bool haveDestination = false;
        if (SolutionPath[SolutionPath.Count - 1] == DestinationSectorID)
        {
            haveDestination = true;
        }


        return haveDestination;
    }

    void ReverseRoute()
    {

    }

    #endregion


    void PrintSolution()
    {
        string output = string.Empty;
        foreach(int i in SolutionPath)
        {
            output += i + " - ";
        }
        output += " [Complete]";

        Debug.Log(output);
    }
}
