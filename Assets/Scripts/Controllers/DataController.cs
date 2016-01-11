using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(MasterDatabase))]
[RequireComponent(typeof(PlayerController))]
public class DataController : MonoBehaviour
{

    #region Public Properties
    /// <summary>
    /// <para>
    /// These properties will hold the MASTER DATA LISTS of 
    /// objects. When we want to pull from them, the 
    /// handlers specific to the data need will need to get 
    /// their own data from the collections and store it
    /// locally for reference. 
    /// </para>
    /// <para>
    /// Master data is READ ONLY. It's never modified during
    /// play, and is never returned to the list.
    /// </para>
    /// </summary>

    //Master Lists -- Do not change over the course of the game
    public List<SectorDataObject> sectorMasterList;
    public List<StationDataObject> stationMasterList;
    public List<JumpgateDataObject> jumpgateMasterList;
    public List<PlanetDataObject> planetMasterList;
    public List<NPCDataObject> NPCMasterList;

    public List<CommodityShopDataObject> commodityShopMasterList;
    public List<CommodityDataObject> commodityMasterList;

    public List<HullDataObject> hullMasterList;
    public List<CargoDataObject> cargoHoldMasterList;
    public List<CannonDataObject> cannonMasterList;
    public List<EngineDataObject> engineMasterList;
    public List<FighterBayDataObject> figherBayMasterList;
    public List<MissileLauncherDataObject> missileLauncherMasterList;
    public List<PlatingDataObject> platingMasterList;
    public List<ScannerDataObject> scannerMasterList;
    public List<ShieldDataObject> shieldMasterList;

    public List<CrewMemberDataObject> crewMasterList;

    //Working lists - Modified over the course of the game
    public List<CommodityShopInventoryDataObject> commodityShopInventoryList;


    public List<GameObject> NPCPool;


    #endregion

    #region Variables

    private static System.Random rnd;
    public static DataController dataController;

    public int currentSectorID;         // Which sector are we in? Always non-zero
    public int currentStationID;        // Which station are we docked at? 0 if not docked


    public enum COMMODITYCLASS
    {
        Common,
        Luxury,
        Food,
        Minerals,
        Medical,
        Military,
        Industrial
    }

    #endregion



    /// <summary>
    /// Ensure that this Controller is the only one we have
    /// </summary>
    /// <remarks>
    /// 
    /// <para>
    /// 
    /// If we don't have an instance yet, then ensure that 
    /// the Game Object that this is attached to will not
    /// be destroyed between loads, and set the instance 
    /// of this object to itself.
    /// </para>
    /// <para>
    /// If we DO have an instance of this object, then destroy
    /// the new object we're attempting to create, in favor
    /// of the original one already in use.
    /// </para>
    /// </remarks>
    void Awake()
    {

        if (dataController == null)
        {

            DontDestroyOnLoad(gameObject);
            dataController = this;

            rnd = new System.Random();

            sectorMasterList = new List<SectorDataObject>();
            stationMasterList = new List<StationDataObject>();
            jumpgateMasterList = new List<JumpgateDataObject>();
            planetMasterList = new List<PlanetDataObject>();

            commodityShopMasterList = new List<CommodityShopDataObject>();      //Loaded from dbCommodityDataItems.asset
            commodityMasterList = new List<CommodityDataObject>();
            commodityShopInventoryList = new List<CommodityShopInventoryDataObject>();

            hullMasterList = new List<HullDataObject>();
            cargoHoldMasterList = new List<CargoDataObject>();
            cannonMasterList = new List<CannonDataObject>();
            engineMasterList = new List<EngineDataObject>();
            figherBayMasterList = new List<FighterBayDataObject>();
            missileLauncherMasterList = new List<MissileLauncherDataObject>();
            platingMasterList = new List<PlatingDataObject>();
            scannerMasterList = new List<ScannerDataObject>();
            shieldMasterList = new List<ShieldDataObject>();

            crewMasterList = new List<CrewMemberDataObject>();


            NPCPool = new List<GameObject>();

            MasterDatabase md = gameObject.GetComponent<MasterDatabase>();
            md.LoadMasterData();


            //Loading or generating session data is handled elsewhere.

        }
        else if (dataController != null)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Specialized Data Access Methods


    public GameObject GetRandomJumpgate()
    {
        List<GameObject> jumpgates = GameObject.FindGameObjectsWithTag("Jumpgate").ToList();
        if (jumpgates.Count > 0)
        {
            GameObject rndObject = (jumpgates.OrderBy(x => rnd.Next()).Take(1)).FirstOrDefault();
            return rndObject;
        }
        else
        {
            return null;
        }

    }

    public GameObject GetRandomStation()
    {
        List<GameObject> stations = GameObject.FindGameObjectsWithTag("Station").ToList();
        if (stations.Count > 0)
        {
            GameObject rndObject = (stations.OrderBy(x => rnd.Next()).Take(1)).FirstOrDefault();
            return rndObject;
        }
        else
        {
            return null;
        }
    }

    public GameObject GetRandomAsteroid()
    {
        List<GameObject> asteroids = GameObject.FindGameObjectsWithTag("Asteroid").ToList();
        if (asteroids.Count > 0)
        {
            GameObject rndObject = (asteroids.OrderBy(x => rnd.Next()).Take(1)).FirstOrDefault();
            return rndObject;
        }
        else
        {
            return null;
        }
    }

    public List<CommodityShopInventoryDataObject> GetShopInventory(int StationID)
    {
        List<CommodityShopInventoryDataObject> items = dataController.commodityShopInventoryList
            .Where(c => c.stationID.Equals(StationID))
            .ToList<CommodityShopInventoryDataObject>();

        return items;
    }

    #endregion



    public int GetRandomInt(int min, int max)
    {
        //Random.seed = int.Parse(Time.time.ToString());
        return Random.Range(min, max);
    }
}
