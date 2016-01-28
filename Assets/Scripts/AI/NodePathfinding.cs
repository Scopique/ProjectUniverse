using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


// SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
//JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");

public class NodePathfinding : MonoBehaviour {

    public int StartingSectorID;
    public int EndingSectorID;
    public List<int> SolutionPath;

    private int currentSectorID;
    private List<int> openSectors;
    private List<int> closedSectors;

    private List<int> neighboringSectors;

    dbSectorDataObject SectorDatabase;
    dbJumpgateDataObject JumpgateDatabase;
    
    public NodePathfinding()
    {
        
        Init();
    }


    public void FindRoute(int StartingSectorID, int EndingSectorID)
    {

        currentSectorID = StartingSectorID;
        SolutionPath.Add(StartingSectorID);
        closedSectors.Add(StartingSectorID);        //Don't come back here.

        //Get the neighboring sectors based on the starting sector
        //Put these into the openSector listing
        GetNeighboringSectors();

        while (openSectors.Count > 0)
        {
            if (openSectors.Contains(EndingSectorID))
            {
                //We're done!
                SolutionPath.Add(EndingSectorID);
                PrintSolution();
                break;
            }

            //Need to check each one to see who's got the shortest route from
            //  the currentSectorID
            currentSectorID = GetLowestCostOpenSector();

            //Remove other sectors who aren't the currentSectorID;
            CloseLosingSectors(currentSectorID);

            GetNeighboringSectors();
            
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
        SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
        JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");
    }

    int GetLowestCostOpenSector()
    {
        Vector2 endSectorCoord = SectorDatabase.database.Find(x => x.sectorID.Equals(EndingSectorID)).sectorMapCoordinates;
        double currentLowestValue = 10000;
        int currentLowestSector = currentSectorID;

        foreach(int i in openSectors)
        {
            int currentSector = i;

            Vector2 nextOpenSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(currentSector)).sectorMapCoordinates;
            double dX =Math.Pow(nextOpenSectorCoords.x - endSectorCoord.x, 2);
            double dY = Math.Pow(nextOpenSectorCoords.y - endSectorCoord.y, 2);
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

    double GetSectorCost(int SectorIDToCheck)
    {
        Vector2 currentSectorCoord = SectorDatabase.database.Find(x => x.sectorID.Equals(StartingSectorID)).sectorMapCoordinates;
        Vector2 nextOpenSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(SectorIDToCheck)).sectorMapCoordinates;
        double dX =Math.Pow(nextOpenSectorCoords.x - currentSectorCoord.x, 2);
        double dY = Math.Pow(nextOpenSectorCoords.y - currentSectorCoord.y, 2);
        double distance = Math.Sqrt(dX + dY);
        return distance;
    }

    void GetNeighboringSectors()
    {
        neighboringSectors = (JumpgateDatabase.database.FindAll(x => x.sectorID.Equals(currentSectorID)).Select(y => y.destinationSectorID)).ToList<int>();
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

        closedSectors.Add(currentSectorID);     //Don't come back this way.
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
