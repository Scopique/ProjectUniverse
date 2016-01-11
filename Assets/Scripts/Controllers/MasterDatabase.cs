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

    #region Inspector Vars

    public float perItemMax = 1500.0f;

    #endregion

    #region Private Vars

    List<SectorDataObject> tempSectorMasterList;
    List<StationDataObject> tempStationMasterList;
    List<JumpgateDataObject> tempJumpgateMasterList;
    List<PlanetDataObject> tempPlanetMasterList;

    List<NPCDataObject> tempNPCMasterList;

    List<CommodityShopDataObject> tempCommodityShopMasterList;
    List<CommodityDataObject> tempCommodityMasterList;
    List<CommodityShopInventoryDataObject> tempCommodityShopInventoryList;

    List<HullDataObject> tempHullMasterList;
    List<CargoDataObject> tempCargoHoldMasterList;
    List<CannonDataObject> tempCannonMasterList;
    List<EngineDataObject> tempEngineMasterList;
    List<FighterBayDataObject> tempFighterBayMasterList;
    List<MissileLauncherDataObject> tempMissileLauncherMasterList;
    List<PlatingDataObject> tempPlatingMasterList;
    List<ScannerDataObject> tempScannerMasterList;
    List<ShieldDataObject> tempShieldMasterList;

    List<CrewMemberDataObject> tempCrewList;

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
        LoadCommodityShopInventories();

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

        DataController.dataController.sectorMasterList = tempSectorMasterList;

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
        tempStationMasterList.Add(new StationDataObject(31, 99, "Utopia Planetia"));
        tempStationMasterList.Add(new StationDataObject(32, 32, "Spica Station"));
        tempStationMasterList.Add(new StationDataObject(33, 33, "Sterope Station"));
        tempStationMasterList.Add(new StationDataObject(34, 34, "Taygeta Station"));
        tempStationMasterList.Add(new StationDataObject(35, 35, "Vega Station"));
        tempStationMasterList.Add(new StationDataObject(36, 36, "Wolf 359 Station"));
        tempStationMasterList.Add(new StationDataObject(37, 37, "Polaris Station"));



        DataController.dataController.stationMasterList = tempStationMasterList;

    }

    private void LoadJumpgates()
    {
        tempJumpgateMasterList.Add(new JumpgateDataObject(1, 1, 23, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(2, 1, 21, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(3, 1, 22, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(4, 1, 7, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(5, 1, 2, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(6, 2, 1, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(7, 2, 11, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(8, 2, 8, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(9, 2, 14, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(10, 3, 14, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(11, 4, 19, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(12, 4, 35, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(13, 5, 32, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(14, 6, 27, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(15, 6, 17, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(16, 7, 1, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(17, 8, 14, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(18, 8, 2, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(19, 9, 31, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(20, 10, 11, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(21, 10, 29, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(22, 11, 24, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(23, 11, 2, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(24, 11, 10, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(25, 11, 5, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(26, 11, 24, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(27, 11, 25, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(28, 12, 34, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(29, 12, 18, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(30, 13, 35, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(31, 14, 8, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(32, 14, 3, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(33, 15, 28, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(34, 15, 24, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(35, 15, 11, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(36, 16, 6, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(37, 16, 28, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(38, 16, 32, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(39, 17, 35, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(40, 18, 12, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(41, 18, 22, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(42, 19, 4, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(43, 20, 25, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(44, 21, 1, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(45, 21, 33, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(46, 21, 12, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(47, 22, 1, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(48, 22, 18, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(49, 23, 1, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(50, 24, 28, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(51, 24, 15, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(52, 24, 11, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(53, 25, 20, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(54, 25, 11, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(55, 25, 36, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(56, 25, 31, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(57, 25, 30, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(58, 26, 31, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(59, 27, 35, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(60, 27, 6, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(61, 28, 17, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(62, 28, 15, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(63, 28, 24, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(64, 29, 10, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(65, 29, 31, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(66, 30, 31, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(67, 30, 25, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(68, 31, 25, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(69, 31, 29, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(70, 31, 9, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(71, 31, 26, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(72, 31, 30, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(73, 32, 17, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(74, 32, 5, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(75, 33, 21, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(76, 33, 34, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(77, 34, 33, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(78, 34, 12, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(79, 35, 16, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(80, 35, 13, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(81, 35, 27, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(82, 35, 4, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(83, 36, 25, 10));
        tempJumpgateMasterList.Add(new JumpgateDataObject(84, 37, 30, 10));

        DataController.dataController.jumpgateMasterList = tempJumpgateMasterList;

    }

    private void LoadPlanets()
    {

    }

    private void LoadCommodityShops()
    {
        tempCommodityShopMasterList.Add(new CommodityShopDataObject(31, "The Leftorium", "A place where you can buy all kinds of gadgets for the lefty in your life", "flanders.png"));
        tempCommodityShopMasterList.Add(new CommodityShopDataObject(25, "CShop", "Buy our crap!", "pileofcrap.png"));

        DataController.dataController.commodityShopMasterList = tempCommodityShopMasterList;
    }

    /// <summary>
    /// Master commodity list. Serves as the basis for loading shops during new game setup
    /// </summary>
    private void LoadCommodities()
    {
        //tempCommodityMasterList.Add(new CommodityDataObject(1, "Carbon", 3, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(2, "Silver", 7, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(3, "Gold", 12, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(4, "Palladium", 50, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(5, "Oxygen", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(6, "Mercury", 3, 0, 0, CommodityDataObject.COMMODITYCLASS.Minerals));
        //tempCommodityMasterList.Add(new CommodityDataObject(7, "Linens", 7, 0, 0, CommodityDataObject.COMMODITYCLASS.Luxury));
        //tempCommodityMasterList.Add(new CommodityDataObject(8, "Livestock", 10, 0, 0, CommodityDataObject.COMMODITYCLASS.Food));
        //tempCommodityMasterList.Add(new CommodityDataObject(9, "Space Suits", 20, 0, 0, CommodityDataObject.COMMODITYCLASS.Industrial));
        //tempCommodityMasterList.Add(new CommodityDataObject(10, "Toilet Paper", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Common));
        //tempCommodityMasterList.Add(new CommodityDataObject(11, "Wine", 8, 0, 0, CommodityDataObject.COMMODITYCLASS.Luxury));
        //tempCommodityMasterList.Add(new CommodityDataObject(12, "Antibiotics", 2, 0, 0, CommodityDataObject.COMMODITYCLASS.Medical));
        //tempCommodityMasterList.Add(new CommodityDataObject(13, "Construction Equipment", 30, 0, 0, CommodityDataObject.COMMODITYCLASS.Industrial));
        //tempCommodityMasterList.Add(new CommodityDataObject(14, "Body Armor", 12, 0, 0, CommodityDataObject.COMMODITYCLASS.Military));

        dbCommodityDataObject db = (dbCommodityDataObject)Resources.Load(@"AssetDatabases/dbCommodityDataItems");
        DataController.dataController.commodityMasterList = db.database;

    }

    /// <summary>
    /// Will be loaded during initial game setup; Here only for testing purposes
    /// </summary>
    private void LoadCommodityShopInventories()
    {
        //Only happens on a NEW GAME
        //loop through all stores
        foreach (CommodityShopDataObject csd in DataController.dataController.commodityShopMasterList)
        {

            List<CommodityShopInventoryDataObject> tempShopInventory = new List<CommodityShopInventoryDataObject>();

            //create an inventory item for each master item record we have
            //  and assign it a quantity
            foreach (CommodityDataObject cd in DataController.dataController.commodityMasterList)
            {
                int even = (int)perItemMax / DataController.dataController.commodityMasterList.Count;
                int share = even - (DataController.dataController.GetRandomInt(0, 100) / 10);
                string shopBuysOrSells = DataController.dataController.GetRandomInt(0, 50) > 25 ? "B" : "S";

                CommodityShopInventoryDataObject cid = new CommodityShopInventoryDataObject(csd.stationID, cd.commodityID, share, 0, shopBuysOrSells);

                tempShopInventory.Add(cid);

            }

            //move some of the inventory quantity around so it's not so uniform.
            //TODO: Check perItemMin and keep it under perItemMax
            foreach (CommodityShopInventoryDataObject cid in tempShopInventory)
            {
                int rndAmt = DataController.dataController.GetRandomInt(1, cid.commodityQuantity);
                int rndItem = DataController.dataController.GetRandomInt(0, tempShopInventory.Count - 1);

                tempShopInventory.ElementAt(rndItem).commodityQuantity += rndAmt;
                cid.commodityQuantity -= rndAmt;
            }

            DataController.dataController.commodityShopInventoryList.AddRange(tempShopInventory);
        }
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

        DataController.dataController.NPCMasterList = tempNPCMasterList;
    }

    private void LoadHulls()
    {
        tempHullMasterList.Add(new HullDataObject(1, "Git-R-Done", "HULL", 1, "HULL_GRD.png", 100000, 500, 9.0f, 1, 1, 1, 1, 1, 1, 1, 1));
        
        DataController.dataController.hullMasterList = tempHullMasterList;
    }

    private void LoadCargoModules()
    {
        tempCargoHoldMasterList.Add(new CargoDataObject(1, "The Big Bucket","CRGO",1,"cargo_bbkt.png",10000,100));

        DataController.dataController.cargoHoldMasterList = tempCargoHoldMasterList;
    }

    private void LoadCannons()
    {
        tempCannonMasterList.Add(new CannonDataObject(1, "PeaShot 100", "CNON", 1, "CANON_PSHT.png", 10000, 2.0f, 5.0f, "BLSTC", 500, 500, 2, 500));

        DataController.dataController.cannonMasterList = tempCannonMasterList;
    }

    private void LoadEngines()
    {
        tempEngineMasterList.Add(new EngineDataObject(1, "Thrustmaster", "ENGN", 1, "ENG.png", 5000, 100.0f, new Maneuverability(75.0f, 75.0f, 50.0f)));

        DataController.dataController.engineMasterList = tempEngineMasterList;
    }

    private void LoadFighterBays()
    {
        tempFighterBayMasterList.Add(new FighterBayDataObject(1, "Barnstormer Mk I", "FBAY", 1, "FBAY_BSMI.png", 30000, 2.0f, 5.0f, "BLSTC", 500, 500, 500));

        DataController.dataController.figherBayMasterList = tempFighterBayMasterList;
    }

    private void LoadMissileLaunchers()
    {
        tempMissileLauncherMasterList.Add(new MissileLauncherDataObject(1, "Slingshot", "MSLNC", 1, "MSLNC_SLNG.png", 30000, 2.0f, 5.0f, "BLSTC", 500, 500, 2.0f, 10));

        DataController.dataController.missileLauncherMasterList = tempMissileLauncherMasterList;
    }

    private void LoadPlating()
    {
        tempPlatingMasterList.Add(new PlatingDataObject(1, "Armadillo", "PLTNG", 1, "PLTNG_ARMA.png", 100000, 5000.0f));

        DataController.dataController.platingMasterList = tempPlatingMasterList;
    }

    private void LoadScanners()
    {
        tempScannerMasterList.Add(new ScannerDataObject(1, "Eagle-I", "SCNR", 1, "SCNR_EGLI.png", 5000, 1000, 1));

        DataController.dataController.scannerMasterList = tempScannerMasterList;
    }

    private void LoadShields()
    {
        tempShieldMasterList.Add(new ShieldDataObject(1, "Tortise", "SHLD", 1, "SHLD_TRTS.png", 5000, 500.0f));


        DataController.dataController.shieldMasterList = tempShieldMasterList;
    }


    private void LoadCrew()
    {
        tempCrewList.Add(new CrewMemberDataObject(1, "Douglas Adams", "M", "crew01", "Crew 01", 500, 1, 0f, 100, 20, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 0f, 5f, 0f, 5f)));
        tempCrewList.Add(new CrewMemberDataObject(2, "Kelly Manson", "F", "crew02", "Crew 02", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(5f, 0f, 0f, 0f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(3, "Zelda Hyrule", "F", "crew03", "Crew 03", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 5f, 0f, 5f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(4, "Sammy Sung", "M", "crew04", "Crew 04", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 5f, 0f, 0f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(5, "Austin Fione", "M", "crew05", "Crew 05", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 5f, 0f, 0f, 0f)));
        tempCrewList.Add(new CrewMemberDataObject(6, "Howie Didot", "M", "crew06", "Crew 06", 500, 1, 0f, 100, 100, 31, new CrewMemberSkillsDataObject(0f, 0f, 0f, 0f, 0f, 0f, 5f, 0f)));

        DataController.dataController.crewMasterList = tempCrewList;
    }

    #endregion
}

