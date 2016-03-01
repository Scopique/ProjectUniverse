using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


// SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
//JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");

/// <summary>
/// Finding a path from one sector to another, anywhere in the system
/// </summary>
/// <remarks>
/// <para>
/// Uses a custom A*-like method to plot a route from point A to point B.
/// </para>
/// <para>
/// Usage
/// ---------------------------------------------------------------------------
/// Instantiate a new NodePathfinding object np
/// Call np.FindRoute(Starting_Sector_ID, Ending_Sector_ID)
/// The path is stored in the np.SolutionPath() List(int)
/// </para>
/// <para>
/// FindRoute will attempt to find the easiest route from A to B based
/// on the Jumpgate connections. Each sector has the same weight, so
/// there's no determination of "best" route. We only really care about
/// A) closest next node, and B) if the destination is accessible. 
/// </para>
/// <para>
/// If the route is NOT available, then the system will reverse the attempt
/// by trying to get from point B to point A.
/// </para>
/// <para>
/// Each sector is given a Cartesian location on a theoretical graph. Using
/// the X and Y coordinates, we can calculate the "as the crow flies" distance
/// between two nodes. 
/// </para>
/// <para>
/// Next-node is determined by selecting the node connected to the current node
/// via a jumpgate. For all of the connected nodes, we choose the one that's 
/// closest to the destination via Cartesian coordinate math. We move the pointer
/// to that node, and close out other, less attractive options. This continues
/// until we reach the destination (first pass) or need to reverse the attempt
/// and reach the destination (second pass). If we find no path, then the node is
/// unreachable (should not happen, but...you know)
/// </para>
/// </remarks>
public class NodePathfinding : MonoBehaviour {

    //The final path from A to B
    //Always from A to B, even if reversed.
    public List<int> SolutionPath;

    //Flag for direction
    private bool isReverseLookup = false;

    //Sectors which are under consideration for next-node
    private List<int> openSectors;
    //Sectors which have been ruled out as next-nodes
    private List<int> closedSectors;
    //List of sectors attached to current-node via jumpgates
    private List<int> neighboringSectors;
    
    //References to the Sector and Jumpgate databases
    dbSectorDataObject SectorDatabase;
    dbJumpgateDataObject JumpgateDatabase;
    
    /// <summary>
    /// Constructor
    /// </summary>
    public NodePathfinding()
    {
        
    }

    /// <summary>
    /// Initiate route finding
    /// </summary>
    /// <param name="OriginSectorID">Starting sector. Should be where the NPC/player is.</param>
    /// <param name="DestinationSectorID">Ending sector. Should be where the NPC/Player ends up.</param>
    public void FindRoute(int OriginSectorID, int DestinationSectorID)
    {
        Init();

        //OriginSectorID should be added to the Solution, because we start here
        SolutionPath.Add(OriginSectorID);
        //Origin should ALSO be excluded from consideration going forward
        closedSectors.Add(OriginSectorID);

        //Calculate a route and return TRUE if we have reached our destination
        bool haveRoute = CalculateForward(OriginSectorID, DestinationSectorID);
        if (!haveRoute)
        {
            //We did not find a forward route.
            //Reset and try the route in reverse
            Init();
            haveRoute = CalculateReverse(DestinationSectorID, OriginSectorID);
            if (haveRoute)
            {
                //If the reverse route worked out, reverse the contents
                //of the List(int) so we have the proper direction for use
                SolutionPath.Reverse();
            }
        }

        //TODO: If we have or do not have a route, do something more than just printing as much
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
    
    /// <summary>
    /// Sets up lists and loads data
    /// </summary>
    /// <remarks>
    /// Data comes from the DataController
    /// </remarks>
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

    /// <summary>
    /// Try and find a route from A to B
    /// </summary>
    /// <param name="OriginSectorID">Where we're starting from</param>
    /// <param name="DestinationSectorID">Where we want to end up</param>
    /// <returns>TRUE if we reach the destination</returns>   
    bool CalculateForward(int OriginSectorID, int DestinationSectorID) {

        //Init some data buckets
        bool haveRoute = false;
        int currentSectorID = OriginSectorID;

        //Find the destination (X,Y) by querying the database by SectorID
        Vector2 DestinationSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(DestinationSectorID)).sectorMapCoordinates;

        //Get the neighboring sectors based on the "current" sector
        //Put these into the openSector listing because they're under
        //  consideration
        GetNeighboringSectors(currentSectorID);

        //Loop through sectors under consideration
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
            //Don't come back this way. Have added the current sector to 
            //  the SolutionPath collection in the previous method, so
            //  close this one out as a future option as we move on
            closedSectors.Add(currentSectorID);

            //Get a list of next-node candidates
            GetNeighboringSectors(currentSectorID);

            //Now that we have another batch of next-node candidates,
            //  the openSector.Count value is once again > 0 so
            //  the looping continues.
        }

        //At this point, if we've stored all the steps we've discovered,
        //  and if one of them is the DestinationSectorID, we're done!
        if (SolutionPath.Contains(DestinationSectorID))
        {
            haveRoute = true;
        }

        return haveRoute;
    }

    /// <summary>
    /// Yes, this is EXACTLY the same method as CalculateForward
    /// </summary>
    /// <param name="OriginSectorID">Where we start</param>
    /// <param name="DestinationSectorID">Where we want to end</param>
    /// <returns>TRUE if we have a path</returns>
    /// <remarks>
    /// For some reason, C# or Unity won't respect calling the same method in the way
    /// we need it to be called (swapping origin and destination values). So I have to 
    /// create the same method TWICE, with different names. 
    /// </remarks>
    bool CalculateReverse(int OriginSectorID, int DestinationSectorID) 
    {
        //Init some data buckets
        bool haveRoute = false;
        int currentSectorID = OriginSectorID;

        //Find the destination (X,Y) by querying the database by SectorID
        Vector2 DestinationSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(DestinationSectorID)).sectorMapCoordinates;

        //Get the neighboring sectors based on the "current" sector
        //Put these into the openSector listing because they're under
        //  consideration
        GetNeighboringSectors(currentSectorID);

        //Loop through sectors under consideration
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
            //Don't come back this way. Have added the current sector to 
            //  the SolutionPath collection in the previous method, so
            //  close this one out as a future option as we move on
            closedSectors.Add(currentSectorID);

            //Get a list of next-node candidates
            GetNeighboringSectors(currentSectorID);

            //Now that we have another batch of next-node candidates,
            //  the openSector.Count value is once again > 0 so
            //  the looping continues.
        }

        //At this point, if we've stored all the steps we've discovered,
        //  and if one of them is the DestinationSectorID, we're done!
        if (SolutionPath.Contains(DestinationSectorID))
        {
            haveRoute = true;
        }

        return haveRoute;
    }

    /// <summary>
    /// Find the sector closest to the destination from the pool of sectors connected
    /// to the sector we're currently in.
    /// </summary>
    /// <param name="DestinationSectorCoords">Where we hope to end up</param>
    /// <param name="CurrentSectorID">Used to find all connected sectors</param>
    /// <returns>
    /// Int: Sector closest to the destination that's directly attached
    /// to the current sector.
    /// </returns>
    int GetLowestCostOpenSector(Vector2 DestinationSectorCoords, int CurrentSectorID)
    {
        //Init some data buckets
        //Setting currentLowestSector to CurrentSectorID is a failsafe
        //  in case we can't find any ways out of this sector.
        double currentLowestValue = 10000;
        int currentLowestSector = CurrentSectorID;

        //Use the class-level List(int) which was defined outside of this method
        foreach(int i in openSectors)
        {
            //Keep for looping purposes
            int currentSector = i;

            //Get the coordinates of the current open sector in the loop and calc the distance
            //  between it and the destination sector.
            Vector2 nextOpenSectorCoords = SectorDatabase.database.Find(x => x.sectorID.Equals(currentSector)).sectorMapCoordinates;
            double dX = Math.Pow(nextOpenSectorCoords.x - DestinationSectorCoords.x, 2);
            double dY = Math.Pow(nextOpenSectorCoords.y - DestinationSectorCoords.y, 2);
            double distance = Math.Sqrt(dX + dY);
            if (distance < currentLowestValue)
            {
                //The distance between the current and destination sectors 
                //  are the lowest we've seen this iteration, so this is
                //  the new "champ" next-node
                currentLowestValue = distance;
                currentLowestSector = currentSector;
            }
        }

        //We should have either the closest next-node sector, 
        //  or the current sector if none qualify
        return currentLowestSector;
    }


    /// <summary>
    /// Get a list of next-node candidates into openSectors
    /// </summary>
    /// <param name="CurrentSectorID">Sector we're in, and which should be used to determine the next possible nodes</param>
    void GetNeighboringSectors(int CurrentSectorID)
    {
        //Get all jumpgates from the jumpgate database for the current sector; specifically, the jumpgate's destination sector ID
        neighboringSectors = (JumpgateDatabase.database.FindAll(x => x.sectorID.Equals(CurrentSectorID)).Select(y => y.destinationSectorID)).ToList<int>();
        //Only take sectors that aren't in the closedSectors list
        openSectors = (from n in neighboringSectors
                      where !(from c in closedSectors
                              select c)
                                  .Contains(n)
                      select n).ToList<int>();
    }

    /// <summary>
    /// Close out all potential sectors in openSectors which aren't
    /// the sector that won the proximity contest
    /// </summary>
    /// <param name="WinningSector">The sector that was chosen from
    /// openSectors as being closest to the DestinationSector</param>
    void CloseLosingSectors(int WinningSector)
    {
        //Get a list of open sectors which aren't the one we want to keep
        List<int> losingSectors = (from db in openSectors where !db.Equals(WinningSector) select db).ToList<int>();
        //Add the losing sectors to the closedSectors collection
        closedSectors.AddRange(losingSectors);

        //Add the winning sector as the next-node in the 
        //  solution chain.
        SolutionPath.Add(WinningSector);
    }

    #endregion

    //Print to the debug window
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
