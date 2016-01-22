using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// <para>
/// Defines the data objects that are stored in the
/// DataController. These objects are not changed
/// over the course of the game.
/// </para>
/// <para>
/// The population method is only called ONCE, from the
/// DataController itself, when the initial instance 
/// is defined.
/// </para>
/// </summary>
public class MasterDatabase : MonoBehaviour
{

    #region Private Vars

    public List<SectorDataObject> tempSectorMasterList;
    public List<StationDataObject> tempStationMasterList;
    public List<JumpgateDataObject> tempJumpgateMasterList;
    public List<PlanetDataObject> tempPlanetMasterList;
    
    public List<NPCDataObject> tempNPCMasterList;
     
    public List<CommodityShopDataObject> tempCommodityShopMasterList;
    public List<CommodityDataObject> tempCommodityMasterList;
    public List<CommodityShopInventoryDataObject> tempCommodityShopInventoryList;
    
    public List<HullDataObject> tempHullMasterList;
    public List<CargoDataObject> tempCargoHoldMasterList;
    public List<CannonDataObject> tempCannonMasterList;
    public List<EngineDataObject> tempEngineMasterList;
    public List<FighterBayDataObject> tempFighterBayMasterList;
    public List<MissileLauncherDataObject> tempMissileLauncherMasterList;
    public List<PlatingDataObject> tempPlatingMasterList;
    public List<ScannerDataObject> tempScannerMasterList;
    public List<ShieldDataObject> tempShieldMasterList;
    
    public List<CrewMemberDataObject> tempCrewList;

    #endregion


    public void LoadMasterData()
    {

        tempSectorMasterList = new List<SectorDataObject>();
        tempStationMasterList = new List<StationDataObject>();
        tempJumpgateMasterList = new List<JumpgateDataObject>();
        tempPlanetMasterList = new List<PlanetDataObject>();

        tempNPCMasterList = new List<NPCDataObject>();

        tempCommodityShopMasterList = new List<CommodityShopDataObject>();
        tempCommodityMasterList = new List<CommodityDataObject>();
        tempCommodityShopInventoryList = new List<CommodityShopInventoryDataObject>();

        tempHullMasterList = new List<HullDataObject>();
        tempCargoHoldMasterList = new List<CargoDataObject>();
        tempCannonMasterList = new List<CannonDataObject>();
        tempEngineMasterList = new List<EngineDataObject>();
        tempFighterBayMasterList = new List<FighterBayDataObject>();
        tempMissileLauncherMasterList = new List<MissileLauncherDataObject>();
        tempPlatingMasterList = new List<PlatingDataObject>();
        tempScannerMasterList = new List<ScannerDataObject>();
        tempShieldMasterList = new List<ShieldDataObject>();

        tempCrewList = new List<CrewMemberDataObject>();


        LoadSectors();
        LoadStations();
        LoadJumpgates();
        LoadPlanets();

        LoadNPCs();

        LoadCommodityShops();
        LoadCommodities();
        //LoadCommodityShopInventories();

        LoadHulls();
        LoadCargoModules();
        LoadCannons();
        LoadEngines();
        LoadFighterBays();
        LoadMissileLaunchers();
        LoadPlating();
        LoadScanners();
        LoadShields();

        LoadCrew();

    }

    #region Data Definitions

    /// <summary>
    /// <para>
    /// These private methods define individual data objects and
    /// load them into the tempXMasterList collections
    /// </para>
    /// <para>
    /// Any modification of master data properties should happen here.
    /// </para>
    /// </summary>

    private void LoadSectors()
    {
        ///(SectorID, SectorName)
        tempSectorMasterList.Add(new SectorDataObject(1, "Alcyone"));
        tempSectorMasterList.Add(new SectorDataObject(2, "Aldebaran"));
        tempSectorMasterList.Add(new SectorDataObject(3, "Algol"));
        tempSectorMasterList.Add(new SectorDataObject(4, "Altair"));
        tempSectorMasterList.Add(new SectorDataObject(5, "Antares"));
        tempSectorMasterList.Add(new SectorDataObject(6, "Arcturus"));
        tempSectorMasterList.Add(new SectorDataObject(7, "Atlas"));
        tempSectorMasterList.Add(new SectorDataObject(8, "Auriga"));
        tempSectorMasterList.Add(new SectorDataObject(9, "Barnard's Star"));
        tempSectorMasterList.Add(new SectorDataObject(10, "Bellatrix"));
        tempSectorMasterList.Add(new SectorDataObject(11, "Betelgeus"));
        tempSectorMasterList.Add(new SectorDataObject(12, "Caleano"));
        tempSectorMasterList.Add(new SectorDataObject(13, "Canopus"));
        tempSectorMasterList.Add(new SectorDataObject(14, "Capella"));
        tempSectorMasterList.Add(new SectorDataObject(15, "Castor"));
        tempSectorMasterList.Add(new SectorDataObject(16, "Debenola"));
        tempSectorMasterList.Add(new SectorDataObject(17, "Deneb"));
        tempSectorMasterList.Add(new SectorDataObject(18, "Electra"));
        tempSectorMasterList.Add(new SectorDataObject(19, "Fomalhaut"));
        tempSectorMasterList.Add(new SectorDataObject(20, "Luyten's Star"));
        tempSectorMasterList.Add(new SectorDataObject(21, "Maia"));
        tempSectorMasterList.Add(new SectorDataObject(22, "Merope"));
        tempSectorMasterList.Add(new SectorDataObject(23, "Pleione"));
        tempSectorMasterList.Add(new SectorDataObject(24, "Pollux"));
        tempSectorMasterList.Add(new SectorDataObject(25, "Procyon"));
        tempSectorMasterList.Add(new SectorDataObject(26, "Proxima Centauri"));
        tempSectorMasterList.Add(new SectorDataObject(27, "Rasalhague"));
        tempSectorMasterList.Add(new SectorDataObject(28, "Regulus"));
        tempSectorMasterList.Add(new SectorDataObject(29, "Rigel"));
        tempSectorMasterList.Add(new SectorDataObject(30, "Sirius"));
        tempSectorMasterList.Add(new SectorDataObject(31, "Sol"));
        tempSectorMasterList.Add(new SectorDataObject(32, "Spica"));
        tempSectorMasterList.Add(new SectorDataObject(33, "Sterope"));
        tempSectorMasterList.Add(new SectorDataObject(34, "Taygeta"));
        tempSectorMasterList.Add(new SectorDataObject(35, "Vega"));
        tempSectorMasterList.Add(new SectorDataObject(36, "Wolf 359"));
        tempSectorMasterList.Add(new SectorDataObject(37, "Polaris"));

        //DataController.dataController.sectorMasterList = tempSectorMasterList;

    }

    private void LoadStations()
    {

        ///(StationID, SectorID, StationName)
        tempStationMasterList.Add(new StationDataObject(1, 1, "Alcyone Station"));
        tempStationMasterList.Add(new StationDataObject(2, 2, "Aldebaran Station"));
        tempStationMasterList.Add(new StationDataObject(3, 3, "Algol Station"));
        tempStationMasterList.Add(new StationDataObject(4, 4, "Altair Station"));
        tempStationMasterList.Add(new StationDataObject(5, 5, "Antares Station"));
        tempStationMasterList.Add(new StationDataObject(6, 6, "Arcturus Station"));
        tempStationMasterList.Add(new StationDataObject(7, 7, "Atlas Station"));
        tempStationMasterList.Add(new StationDataObject(8, 8, "Auriga Station"));
        tempStationMasterList.Add(new StationDataObject(9, 9, "Barnard's Star Station"));
        tempStationMasterList.Add(new StationDataObject(10, 10, "Bellatrix Station"));
        tempStationMasterList.Add(new StationDataObject(11, 11, "Betelgeus Station"));
        tempStationMasterList.Add(new StationDataObject(12, 12, "Caleano Station"));
        tempStationMasterList.Add(new StationDataObject(13, 13, "Canopus Station"));
        tempStationMasterList.Add(new StationDataObject(14, 14, "Capella Station"));
        tempStationMasterList.Add(new StationDataObject(15, 15, "Castor Station"));
        tempStationMasterList.Add(new StationDataObject(16, 16, "Debenola Station"));
        tempStationMasterList.Add(new StationDataObject(17, 17, "Deneb Station"));
        tempStationMasterList.Add(new StationDataObject(18, 18, "Electra Station"));
        tempStationMasterList.Add(new StationDataObject(19, 19, "Fomalhaut Station"));
        tempStationMasterList.Add(new StationDataObject(20, 20, "Luyten's Star Station"));
        tempStationMasterList.Add(new StationDataObject(21, 21, "Maia Station"));
        tempStationMasterList.Add(new StationDataObject(22, 22, "Merope Station"));
        tempStationMasterList.Add(new StationDataObject(23, 23, "Pleione Station"));
        tempStationMasterList.Add(new StationDataObject(24, 24, "Pollux Station"));
        tempStationMasterList.Add(new StationDataObject(25, 25, "Procyon Station"));
        tempStationMasterList.Add(new StationDataObject(26, 26, "Proxima Centauri Station"));
        tempStationMasterList.Add(new StationDataObject(27, 27, "Rasalhague Station"));
        tempStationMasterList.Add(new StationDataObject(28, 28, "Regulus Station"));
        tempStationMasterList.Add(new StationDataObject(29, 29, "Rigel Station"));
        tempStationMasterList.Add(new StationDataObject(30, 30, "Sirius Station"));
        tempStationMasterList.Add(new StationDataObject(31, 31, "Earth Station"));
        tempStationMasterList.Add(new StationDataObject(99, 31, "Utopia Planetia"));
        tempStationMasterList.Add(new StationDataObject(32, 32, "Spica Station"));
        tempStationMasterList.Add(new StationDataObject(33, 33, "Sterope Station"));
        tempStationMasterList.Add(new StationDataObject(34, 34, "Taygeta Station"));
        tempStationMasterList.Add(new StationDataObject(35, 35, "Vega Station"));
        tempStationMasterList.Add(new StationDataObject(36, 36, "Wolf 359 Station"));
        tempStationMasterList.Add(new StationDataObject(37, 37, "Polaris Station"));



        //DataController.dataController.stationMasterList = tempStationMasterList;

    }

    private void LoadJumpgates()
    {
        tempJumpgateMasterList.Add(new JumpgateDataObject(1,string.Empty, 1,23, 0,  10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(2, string.Empty, 1, 21, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(3, string.Empty, 1, 22, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(4, string.Empty, 1, 7, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(5, string.Empty, 1, 2, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(6, string.Empty, 2, 1, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(7, string.Empty, 2, 11, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(8, string.Empty, 2, 8, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(9, string.Empty, 2, 14, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(10, string.Empty, 3, 14, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(11, string.Empty, 4, 19, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(12, string.Empty, 4, 35, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(13, string.Empty, 5, 32, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(14, string.Empty, 6, 27, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(15, string.Empty, 6, 17, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(16, string.Empty, 7, 1, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(17, string.Empty, 8, 14, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(18, string.Empty, 8, 2, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(19, string.Empty, 9, 31, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(20, string.Empty, 10, 11, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(21, string.Empty, 10, 29, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(22, string.Empty, 11, 24, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(23, string.Empty, 11, 2, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(24, string.Empty, 11, 10, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(25, string.Empty, 11, 5, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(26, string.Empty, 11, 24, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(27, string.Empty, 11, 25, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(28, string.Empty, 12, 34, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(29, string.Empty, 12, 18, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(30, string.Empty, 13, 35, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(31, string.Empty, 14, 8, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(32, string.Empty, 14, 3, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(33, string.Empty, 15, 28, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(34, string.Empty, 15, 24, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(35, string.Empty, 15, 11, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(36, string.Empty, 16, 6, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(37, string.Empty, 16, 28, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(38, string.Empty, 16, 32, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(39, string.Empty, 17, 35, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(40, string.Empty, 18, 12, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(41, string.Empty, 18, 22, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(42, string.Empty, 19, 4, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(43, string.Empty, 20, 25, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(44, string.Empty, 21, 1, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(45, string.Empty, 21, 33, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(46, string.Empty, 21, 12, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(47, string.Empty, 22, 1, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(48, string.Empty, 22, 18, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(49, string.Empty, 23, 1, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(50, string.Empty, 24, 28, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(51, string.Empty, 24, 15, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(52, string.Empty, 24, 11, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(53, string.Empty, 25, 20, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(54, string.Empty, 25, 11, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(55, string.Empty, 25, 36, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(56, string.Empty, 25, 31, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(57, string.Empty, 25, 30, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(58, string.Empty, 26, 31, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(59, string.Empty, 27, 35, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(60, string.Empty, 27, 6, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(61, string.Empty, 28, 17, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(62, string.Empty, 28, 15, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(63, string.Empty, 28, 24, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(64, string.Empty, 29, 10, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(65, string.Empty, 29, 31, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(66, string.Empty, 30, 31, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(67, string.Empty, 30, 25, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(68, string.Empty, 31, 25, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(69, string.Empty, 31, 29, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(70, string.Empty, 31, 9, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(71, string.Empty, 31, 26, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(72, string.Empty, 31, 30, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(73, string.Empty, 32, 17, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(74, string.Empty, 32, 5, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(75, string.Empty, 33, 21, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(76, string.Empty, 33, 34, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(77, string.Empty, 34, 33, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(78, string.Empty, 34, 12, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(79, string.Empty, 35, 16, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(80, string.Empty, 35, 13, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(81, string.Empty, 35, 27, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(82, string.Empty, 35, 4, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(83, string.Empty, 36, 25, 0, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(84, string.Empty, 37, 30, 0, 10));

        //DataController.dataController.jumpgateMasterList = tempJumpgateMasterList;

    }

    private void LoadPlanets()
    {

    }

    private void LoadCommodityShops()
    {
        tempCommodityShopMasterList.Add(new CommodityShopDataObject(31, "The Leftorium", "A place where you can buy all kinds of gadgets for the lefty in your life", "flanders.png"));
        tempCommodityShopMasterList.Add(new CommodityShopDataObject(25, "CShop", "Buy our crap!", "pileofcrap.png"));

        //DataController.dataController.commodityShopMasterList = tempCommodityShopMasterList;
    }

    /// <summary>
    /// Master commodity list. Serves as the basis for loading shops during new game setup
    /// </summary>
    private void LoadCommodities()
    {
        tempCommodityMasterList.Add(new CommodityDataObject(1, "Carbon", 3, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(2, "Silver", 7, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(3, "Gold", 12, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(4, "Palladium", 50, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(5, "Oxygen", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(6, "Mercury", 3, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        tempCommodityMasterList.Add(new CommodityDataObject(7, "Linens", 7, 0, 0, CommodityDataObject.COMMODITYCLASS.Luxury));
        tempCommodityMasterList.Add(new CommodityDataObject(8, "Livestock", 10, 0, 0, CommodityDataObject.COMMODITYCLASS.Food));
        tempCommodityMasterList.Add(new CommodityDataObject(9, "Space Suits", 20, 0, 0, CommodityDataObject.COMMODITYCLASS.Industrial));
        tempCommodityMasterList.Add(new CommodityDataObject(10, "Toilet Paper", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Common));
        tempCommodityMasterList.Add(new CommodityDataObject(11, "Wine", 8, 0, 0, CommodityDataObject.COMMODITYCLASS.Luxury));
        tempCommodityMasterList.Add(new CommodityDataObject(12, "Antibiotics", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Medical));
        tempCommodityMasterList.Add(new CommodityDataObject(13, "Construction Equipment", 30, 0, 0, CommodityDataObject.COMMODITYCLASS.Industrial));
        tempCommodityMasterList.Add(new CommodityDataObject(14, "Body Armor", 12, 0, 0, CommodityDataObject.COMMODITYCLASS.Military));

        //dbCommodityDataObject db = (dbCommodityDataObject)Resources.Load(@"AssetDatabases/dbCommodityDataItems");
        //DataController.dataController.commodityMasterList = db.database;

    }

    

    private void LoadNPCs()
    {

        //tempNPCMasterList.Add(new NPCDataObject(0, "NAME", "SHIPNAME", "HUMAN"));
        tempNPCMasterList.Add(new NPCDataObject(1, "Douglas Archer", "Singing Trees", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(2, "Jennifer Koswold", "Moonlit Sonata", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(3, "Scott McMahon", "Devilsled", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(4, "Daniel Hebert", "Lazy Sunday", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(5, "Alyssa Comstock", "Eye On You", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(6, "Debora Wilson", "Fitting End", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(7, "Pete Smith", "For Lola", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(8, "Scott Geeding", "Captain Cranky", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(9, "Stephanie Morrow", "Firey Kelowna", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(10, "Jane Smith", "Mabel's Pride", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(11, "Eric Redman", "Creeping Darkness", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(12, "Neil Frankham", "Tinerkbell", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(13, "David Bates", "Tokyo Drifter", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(14, "Brenda Holloway", "Flute Solo", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(15, "Allison Pang", "Brush of Darkness", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(16, "Jonathan Doyle", "Huginator", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(17, "Matt Levine", "Joo Canoe", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(18, "Eric Rhodes", "No Time For Sleep", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(19, "Chris Tremblay", "Big Daddy's Home", "Human"));
        tempNPCMasterList.Add(new NPCDataObject(20, "Emily Joy Bembeneck", "Progressive Fire", "Human"));

        //DataController.dataController.NPCMasterList = tempNPCMasterList;
    }

    private void LoadHulls()
    {
        tempHullMasterList.Add(new HullDataObject(1, "Git-R-Done", "HULL", 1, "HULL_GRD.png", 100000, 500, 9.0f, 1, 1, 1, 1, 1, 1, 1, 1));
        
        //DataController.dataController.hullMasterList = tempHullMasterList;
    }

    private void LoadCargoModules()
    {
        tempCargoHoldMasterList.Add(new CargoDataObject(1, "The Big Bucket","CRGO",1,"cargo_bbkt.png",10000,100));

        //DataController.dataController.cargoHoldMasterList = tempCargoHoldMasterList;
    }

    private void LoadCannons()
    {
        tempCannonMasterList.Add(new CannonDataObject(1, "PeaShot 100", "CNON", 1, "CANON_PSHT.png", 10000, 2.0f, 5.0f, "BLSTC", 500, 500, 2, 500));

        //DataController.dataController.cannonMasterList = tempCannonMasterList;
    }

    private void LoadEngines()
    {
        tempEngineMasterList.Add(new EngineDataObject(1, "Thrustmaster", "ENGN", 1, "ENG.png", 5000, 100.0f, new Maneuverability(75.0f, 75.0f, 50.0f)));

        //DataController.dataController.engineMasterList = tempEngineMasterList;
    }

    private void LoadFighterBays()
    {
        tempFighterBayMasterList.Add(new FighterBayDataObject(1, "Barnstormer Mk I", "FBAY", 1, "FBAY_BSMI.png", 30000, 2.0f, 5.0f, "BLSTC", 500, 500, 500));

        //DataController.dataController.figherBayMasterList = tempFighterBayMasterList;
    }

    private void LoadMissileLaunchers()
    {
        tempMissileLauncherMasterList.Add(new MissileLauncherDataObject(1, "Slingshot", "MSLNC", 1, "MSLNC_SLNG.png", 30000, 2.0f, 5.0f, "BLSTC", 500, 500, 2.0f, 10));

        //DataController.dataController.missileLauncherMasterList = tempMissileLauncherMasterList;
    }

    private void LoadPlating()
    {
        tempPlatingMasterList.Add(new PlatingDataObject(1, "Armadillo", "PLTNG", 1, "PLTNG_ARMA.png", 100000, 5000.0f));

        //DataController.dataController.platingMasterList = tempPlatingMasterList;
    }

    private void LoadScanners()
    {
        tempScannerMasterList.Add(new ScannerDataObject(1, "Eagle-I", "SCNR", 1, "SCNR_EGLI.png", 5000, 1000, 1));

        //DataController.dataController.scannerMasterList = tempScannerMasterList;
    }

    private void LoadShields()
    {
        tempShieldMasterList.Add(new ShieldDataObject(1, "Tortise", "SHLD", 1, "SHLD_TRTS.png", 5000, 500.0f));


        //DataController.dataController.shieldMasterList = tempShieldMasterList;
    }


    private void LoadCrew()
    {
        tempCrewList.Add(new CrewMemberDataObject(1, "Douglas Adams", "M", "crew01", "Crew 01", 500, 1, 0f, 100, 20, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 0f, 5f, 0f, 5f)));
        tempCrewList.Add(new CrewMemberDataObject(2, "Kelly Manson", "F", "crew02", "Crew 02", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(3, "Zelda Hyrule", "F", "crew03", "Crew 03", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 5f, 0f, 5f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(4, "Sammy Sung", "M", "crew04", "Crew 04", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 5f, 0f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(5, "Austin Fione", "M", "crew05", "Crew 05", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 5f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(6, "Howie Didot", "M", "crew06", "Crew 06", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 0f, 0f, 5f, 0f)));

        //DataController.dataController.crewMasterList = tempCrewList;
    }

    #endregion
}

