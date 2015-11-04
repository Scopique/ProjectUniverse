using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// <para>
/// Sector controller deals with any non-specific object
/// data or activities that the game needs to deal with 
/// when the player enters, acts within, or leaves the
/// sector. 
/// </para>
/// <para>
/// This object is not static; it's prefab needs to be 
/// added manually to each scene, and it's properties 
/// set accordingly.
/// </para>
/// </summary>
[RequireComponent(typeof(NPCController))]
[RequireComponent(typeof(MarketController))]
[RequireComponent(typeof(EventController))]
public class SectorController : MonoBehaviour
{

    #region Properties
    //The current sector, from the database
    public int sectorID;

    [Header("Demand Percents")]
    public int common = 90;
    public int luxury = 10;
    public int food = 80;
    public int minerals = 60;
    public int medical = 40;
    public int military = 30;
    public int industrial = 50;

    #endregion

    #region Variables

    DataController dc;
    
    #endregion

    #region Unity Methods


    void Awake()
    {

    }

    void Start()
    {
        GameObject dataController = GameObject.Find("DataController");
        if (dataController != null)
        {
            dc = dataController.GetComponent<DataController>();
        }
        else
        {
            throw new System.Exception("DataController was not found. Is it present and named 'DataController'?");
        }
    }

    void Update()
    {
        
    }

    void OnGUI()
    {
        if (DataController.dataController.sectorMasterList.Count > 0)
        {
            SectorDataObject sdo = DataController.dataController.sectorMasterList.FirstOrDefault(x => x.sectorID.Equals(this.sectorID));
            string sectorName = "Not Found (ID " + this.sectorID.ToString() + ")";
            if (sdo != null)
            {
                DataController.dataController.currentSectorID = this.sectorID;
                sectorName = sdo.sectorName;
            }
            GUI.Label(new Rect(0, 0, 250, 30), "Sector: " + sectorName);
        }
    }

    #endregion

    #region DataControl Access
    /*############################################
     * These methods are masked for specific purpose
     * and access DataControl so individual elements
     * in the scene don't have to.
    #############################################*/

    public int GetRandomInt(int min, int max)
    {
        return DataController.dataController.GetRandomInt(min, max);
    }

    public GameObject GetRandomStation()
    {
        return dc.GetRandomStation();
    }

    public GameObject GetRandomJumpgate()
    {
        return dc.GetRandomJumpgate();
    }

    public GameObject GetRandomAsteroid()
    {
        return DataController.dataController.GetRandomAsteroid();
    }

    public StationDataObject GetStationData(int StationID)
    {
        StationDataObject sdo = DataController
            .dataController
            .stationMasterList
            .FirstOrDefault(x => x.stationID.Equals(StationID));
        return sdo;
    }

    public JumpgateDataObject GetJumpgateData(int JumpgateID)
    {
        JumpgateDataObject jdo = DataController
            .dataController
            .jumpgateMasterList
            .FirstOrDefault(x => x.jumpgateID.Equals(JumpgateID));
        return jdo;
    }

    public GameObject GetClosestGameObjectByTag(string Tag, Vector3 CurrentPosition)
    {
        GameObject closest = null;
        GameObject[] gos = GameObject.FindGameObjectsWithTag(Tag);
        if (gos.Length > 0)
        {
            float lastDistance = 100.0f;
            foreach(GameObject go in gos)
            {
                float dist = Vector3.Distance(CurrentPosition, go.transform.position);
                if (dist < lastDistance)
                {
                    lastDistance = dist;
                    closest = go;
                }
            }
        }

        return closest;
    }

    public RouteDataObject GetRandomRouteNode(string NodeType)
    {
        RouteDataObject newRoute = new RouteDataObject() ;
        
        switch (NodeType.ToUpper())
        {
            case "STATION":
                GameObject rndStation = dc.GetRandomStation();
                newRoute = new RouteDataObject(rndStation.name, rndStation.tag, rndStation.transform.position);
                break;
            case "JUMPGATE":
                GameObject rndJumpgate = dc.GetRandomJumpgate();
                newRoute = new RouteDataObject(rndJumpgate.name, rndJumpgate.tag, rndJumpgate.transform.position);
                break;
            case "ASTEROID":
                GameObject rndAsteroid = dc.GetRandomAsteroid();
                newRoute = new RouteDataObject(rndAsteroid.name, rndAsteroid.tag, rndAsteroid.transform.position);
                break;
            default:
                break;
        }
        
        return newRoute;
    }

    #endregion

}
