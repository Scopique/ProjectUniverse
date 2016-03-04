using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SaveGameDataController))]
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

    public static DataController DataAccess;        //For use with this singleton

    //Master Lists -- These do not change over the course of the game
    public List<SectorDataObject> sectorMasterList;
    public List<StationDataObject> stationMasterList;
    public List<JumpgateDataObject> jumpgateMasterList;
    public List<PlanetDataObject> planetMasterList;
    public List<MerchantNPCDataObject> merchantMasterList;

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
    //Actual properties that access the values stored in SAVEGAMEDATACONTROLLER
    public List<CommodityShopInventoryDataObject> CommodityShopInventoryList { 
        get { return SaveGameDataController.SaveGameAccess.WorkingCommodityShopInventoryList; }
        set { SaveGameDataController.SaveGameAccess.WorkingCommodityShopInventoryList = value; }
    }

    public List<SectorDataObject> SectorList
    {
        get { return SaveGameDataController.SaveGameAccess.WorkingSectorList; }
        set { SaveGameDataController.SaveGameAccess.WorkingSectorList = value; }
    }

    public List<MerchantNPCDataObject> MerchantNPCList
    {
        get { return SaveGameDataController.SaveGameAccess.WorkingMerchantNPCList; }
        set { SaveGameDataController.SaveGameAccess.WorkingMerchantNPCList = value; }
    }

    //Poor NPCs...  :(
    public List<GameObject> NPCPool;


    #endregion

    #region Private Variables

    private static System.Random rnd;               //Use with the globally accessible Random Number
    

    public int currentSectorID;                     // Which sector are we in? Always non-zero
    public int currentStationID;                    // Which station are we docked at? 0 if not docked


    //public enum COMMODITYCLASS
    //{
    //    Common,
    //    Luxury,
    //    Food,
    //    Minerals,
    //    Medical,
    //    Military,
    //    Industrial
    //}

    #endregion

    #region Constructors and Unity Methods
    
    /// <summary>
    /// Ensure that this Controller is the only one we have
    /// </summary>
    /// <remarks>
    /// 
    /// <para>
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

        if (DataAccess == null)
        {

            DontDestroyOnLoad(gameObject);
            DataAccess = this;

            //Set up the global random number generator
            rnd = new System.Random();

            //Note2Dev: Might need to check this to ensure we aren't always loading...
            LoadMasterData();

            NewGame();

            int i = 0;

        }
        else if (DataAccess != null)
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Master Data Loading

    /// <summary>
    /// Loads data from the Resource store into the public lists. Only need to do this once (on new or load game)
    /// </summary>
    private void LoadMasterData()
    {
        dbCannonDataObject dbCannons = (dbCannonDataObject)Resources.Load(@"AssetDatabases/dbCannonDataItems");
        cannonMasterList = dbCannons.database;

        dbCargoModuleDataObject dbCargo = (dbCargoModuleDataObject)Resources.Load(@"AssetDatabases/dbCargoModuleDataItems");
        cargoHoldMasterList = dbCargo.database;

        dbCommodityDataObject dbCommodities = (dbCommodityDataObject)Resources.Load(@"AssetDatabases/dbCommodityDataItems");
        commodityMasterList = dbCommodities.database;

        dbCommodityShopDataObject dbCommodityShops = (dbCommodityShopDataObject)Resources.Load(@"AssetDatabases/dbCommodityShopDataItems");
        commodityShopMasterList = dbCommodityShops.database;

        dbCrewDataObject dbCrew = (dbCrewDataObject)Resources.Load(@"AssetDatabases/dbCrewDataItems");
        crewMasterList = dbCrew.database;

        dbEngineDataObject dbEngines = (dbEngineDataObject)Resources.Load(@"AssetDatabases/dbEngineDataItems");
        engineMasterList = dbEngines.database;

        dbFighterBayDataObject dbFighterBays = (dbFighterBayDataObject)Resources.Load(@"AssetDatabases/dbFighterBayDataItems");
        figherBayMasterList = dbFighterBays.database;

        dbHullDataObject dbHulls = (dbHullDataObject)Resources.Load(@"AssetDatabases/dbHullDataItems");
        hullMasterList = dbHulls.database;

        dbJumpgateDataObject dbJumpgates = (dbJumpgateDataObject)Resources.Load(@"AssetDatabases/dbJumpgateDataItems");
        jumpgateMasterList = dbJumpgates.database;

        dbMissileLauncherDataObject dbMissileLaunchers = (dbMissileLauncherDataObject)Resources.Load(@"AssetDatabases/dbMissileLauncherDataItems");
        missileLauncherMasterList = dbMissileLaunchers.database;

        dbMerchantNPCDataObject dbMerchants = (dbMerchantNPCDataObject)Resources.Load(@"AssetDatabases/dbMerchantDataItems");
        merchantMasterList = dbMerchants.database;

        dbPlatingDataObject dbPlating = (dbPlatingDataObject)Resources.Load(@"AssetDatabases/dbPlatingDataItems");
        platingMasterList = dbPlating.database;

        dbScannerDataObject dbScanners = (dbScannerDataObject)Resources.Load(@"AssetDatabases/dbScannerDataItems");
        scannerMasterList = dbScanners.database;

        dbSectorDataObject dbSectors = (dbSectorDataObject)Resources.Load(@"AssetDatabases/dbSectorDataItems");
        sectorMasterList = dbSectors.database;

        dbShieldDataObject dbShields = (dbShieldDataObject)Resources.Load(@"AssetDatabases/dbShieldDataItems");
        shieldMasterList = dbShields.database;

        dbStationDataObject dbStations = (dbStationDataObject)Resources.Load(@"AssetDatabases/dbStationDataItems");
        stationMasterList = dbStations.database;
    }

    #endregion

    #region Game Loading
    
    private void NewGame()
    {
        MarketController mc = new MarketController();
        mc.LoadCommodityShopInventories();

        NPCController nc = new NPCController();
        nc.LoadNewMerchants();
    }

    private void LoadGame(string SaveName)
    {

    }

    #endregion

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
        List<CommodityShopInventoryDataObject> items = DataAccess.CommodityShopInventoryList
            .Where(c => c.stationID.Equals(StationID))
            .ToList<CommodityShopInventoryDataObject>();

        return items;
    }

    #endregion
    
    #region Globally Accessible Methods
    
    public int GetRandomInt(int min, int max)
    {
        //Random.seed = int.Parse(Time.time.ToString());
        return Random.Range(min, max);
    }

    #endregion
}
