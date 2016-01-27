using UnityEngine;
using System.Linq;
using System.Collections.Generic;


/// <summary>
/// Based on the current sector and the desired destination sector,
/// determine which jumpgates to use to get from point A to point B
/// http://www.redblobgames.com/pathfinding/a-star/introduction.html
/// http://www.raywenderlich.com/4946/introduction-to-a-pathfinding
/// http://heyes-jones.com/astar.php
/// 
/// 
/// </summary>
public class NodePathfindingX : MonoBehaviour 
{
    List<int> Open;
    List<int> Closed;

    List<SectorDataObject> dbSectors;
    List<JumpgateDataObject> dbJumpgates;

    public int StartingSectorID;
    public int EndingSectorID;

    void Start()
    {
        
    }

    void Update()
    {

    }

    

    public void BeginRouting()
    {
        Init();
        GraphIt();
    }

    void Init()
    {
        Open = new List<int>();
        Closed = new List<int>();

        dbSectorDataObject SectorDatabase = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
        dbSectors = SectorDatabase.database;

        dbJumpgateDataObject JumpgateDatabase = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");
        dbJumpgates = JumpgateDatabase.database;

        Closed.Add(StartingSectorID);       
    }

    void GraphIt()
    {
        
    }

    /// <summary>
    /// Based on the sector, find neighboring sectors that aren't in the CLOSED list (already dealt with)
    /// </summary>
    /// <param name="SectorID"></param>
    void FindOpenNeighbors(int SectorID)
    {
        List<JumpgateDataObject> openJGDO = (from db in dbJumpgates where db.sectorID.Equals(SectorID) select db).ToList<JumpgateDataObject>();
        foreach(JumpgateDataObject jgdo in openJGDO)
        {
            if (!Closed.Contains(jgdo.destinationSectorID))
            {
                Open.Add(jgdo.destinationSectorID);
            }
        }

        //We'd normally get the F score here, but we don't have an F score to calculate

    }

}
