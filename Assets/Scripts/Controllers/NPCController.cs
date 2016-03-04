using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


/* ============================================================================================
 * Scene NPC Controller
 * -------------------
 * The NPC Controller oversees all handling of NPCs that includes instantiation,
 * tracking, and pooling.
 * 
 * The script requires that the developer provide a list of prefabs for each NPC type so that
 * the script can randomly select a model to use when creating a new instance of that type. 
 * 
 * It also tracks the current instances (object pool) overall, as well as the NPC objects that
 * are considered to be "docked" at a station (doesn't matter which station for our purposes).
 * 
 * Each NPC type has it's own section of the script for spawning purposes, although each spawn
 * follows the same general pattern:
 * 1. Set up the pool: Instantiate objects from prefabs and add them to the object pool
 * 2. Check for permission to spawn: In order to ensure spawns seem random, we delay 
 *      the use of items from the pool by different amounts.
 * 3. Spawning: The actual selection of an item from the pool, and it's placement in the world.
 * 
 * There's also some helper methods that deal with NPC management including undocking NPCs from 
 * the station collection and shortcuts to enumerate the NPCs that are active in the game.
 * =============================================================================================
 * Updates:
 *  8/7/15: Consolidation and refactoring. Some of the trigger functionality moved out of this
 *      and into the NPC component so it's not spread out and taking up resources.
 * =============================================================================================
 */
public class NPCController : MonoBehaviour
{
    #region Inspector Variables

    /*
     * Prefab list for spawn assignments
     */
    [Header("Prefabs")]
    public List<GameObject> merchantPrefabs;
    public List<GameObject> policePrefabs;
    public List<GameObject> minerPrefabs;

    /*
     * Max pool counts for NPCs in the scene. 
     */
    [Header("Max NPC Counts")]
    public int maxMerchantCount = 20;
    public int maxPoliceCount = 3;
    public int maxMinerCount = 1;       //Should not exceed asteroid's MaxSpawnPoint value

    /*
     * Controls spawn and undock delay
     */
    [Header("Timers")]
    public float spawnInterval = 3.0f;
    public float undockInterval = 10.0f;
    public float simulationStepIntervalMinutes = 3.0f;

    #endregion

    #region Local Variables

    //Info from the sector controller
    SectorController sc;

    /*
     * Per-NPC-Type variables
     */
    List<GameObject> merchantNPCList;
    float lastMerchantSpawnCheck = 0.0f;

    //These two spawn on Start and do not
    //  need a timer to check eligibility
    List<GameObject> policeNPCList;
    List<GameObject> minerNPCList;


    #endregion

    #region Unity Methods

    /// <summary>
    /// Setup and init
    /// </summary>
    /// <remarks>
    /// <para>
    /// Initialize the lists. Setup the NPC pools. Starts the POLICE  and MINER
    /// spawning process so ther are in place as the player jumps in.
    /// </para>
    /// </remarks>
    void Start()
    {
        sc = GetComponent<SectorController>();

        merchantNPCList = new List<GameObject>();
        policeNPCList = new List<GameObject>();
        minerNPCList = new List<GameObject>();
        
        SetupMerchantPool();
        SetupPolicePool();
        SetupMinerPool();

        //Spawn police and miners
        //TODO: Spawning will move into a check based on sim interval step and player presence
        CheckPoliceSpawnPermission();
        CheckMinerSpawnPermission();

    }

    /// <summary>
    /// Check spawn permissions and spawn as needed
    /// </summary>
    /// <remarks>
    /// <para>
    /// During the update, we check to see if a new merchant has jumped into the 
    /// system (spawning an object from the merchant pool). 
    /// We also check to see if we can undock any docked NPCs.
    /// </para>
    /// </remarks>
    void Update()
    {
        CheckMerchantSpawnPermission();

        UndockNPC();
    }

    #endregion

    #region Merchant Spawning

    /// <summary>
    /// Create Merchant NPCs and assign them to the sector pool
    /// </summary>
    /// <remarks>
    /// <para>
    /// The setup takes random items from the merchant prefab collection and assigns
    /// data to the GO's NPC component. 
    /// </para>
    /// <para>
    /// Right now this data only ID's the object as a generic NPC. Later, we might move the 
    /// pooling up to the static GameController level, generating NPC identities on game creation,
    /// and allow for the persistance of NPCs throughout the universe. 
    /// </para>
    /// </remarks>
    void SetupMerchantPool()
    {
        if (merchantPrefabs.Count == 0)
        {
            print("No merchant prefabs were defined on the NPCController of SectorController object.");
        }
        else
        {
            for (int i = 0; i < maxMerchantCount; i++)
            {
                int prefabIDX = sc.GetRandomInt(0, merchantPrefabs.Count);
                GameObject newNPC = (GameObject)Instantiate(merchantPrefabs[prefabIDX], new Vector3(0, 0, 0), Quaternion.identity);
                NPC npc = newNPC.GetComponent<NPC>();

                npc.NPCID = i;      //TODO: Need to set Merchant NPCID using something more robust
                npc.NPCName = "Merchant Freighter";
                npc.NPCShipName = "Merchant Freighter";
                npc.NPCFaction = "Merchant";
                npc.NPCType = NPC.NPCTYPE.Merchant;
                npc.currentSector = sc.sectorID;
                if (npc.lastSector == 0) { npc.lastSector = sc.sectorID; }

                newNPC.SetActive(false);

                merchantNPCList.Add(newNPC);
            }
        }
    }

    /// <summary>
    /// Check to see if we can spawn a new NPC
    /// </summary>
    /// <remarks>
    /// <para>
    /// Every time this method is called, we compare the currentTime to the 
    /// last time we spawned an NPC, plus a spawn interval in seconds.  
    /// </para>
    /// <para>
    /// If that last spawn plus interval exceeds the current time, we test a
    /// coin flip. If the value out of 10 is less than 5, we pull an NPC 
    /// from the inactive pool and create it. 
    /// </para>
    /// </remarks>
    void CheckMerchantSpawnPermission()
    {
        float currentTime = Time.time;

        if (currentTime > lastMerchantSpawnCheck + spawnInterval)
        {
            int coinFlip = sc.GetRandomInt(0, 10);
            if (coinFlip < 5)
            {
                StartCoroutine(SpawnMerchantNPC());
                
            }

            lastMerchantSpawnCheck = currentTime;
        }
    }

    /// <summary>
    /// Get an NPC from the pool, place it, and activate it.
    /// </summary>
    /// <returns>IEnumerable: Null, for the coroutine</returns>
    /// <remarks>
    /// <para>
    /// Once it's determined that we need to spawn an NPC, we pull one from 
    /// the inactive NPC pool to use. 
    /// </para>
    /// <para>
    /// Each merchant NPC spawns in at a jumpgate's EGRESS point, which is an
    /// invisible GO orbiting the jumpgate outside of the jumpgate's trigger. 
    /// This is also where players will spawn in when they enter the scene. 
    /// </para>
    /// <para>
    /// Finally, we assign some data to the NPC's NPC component for tracking
    /// purposes, and set the Active flag to TRUE.
    /// </para>
    /// </remarks>
    IEnumerator SpawnMerchantNPC()
    {
        GameObject newMerchant = GetMerchantFromPool();

        if (newMerchant != null)
        {
            //Get demographic component
            NPC npcComponent = newMerchant.GetComponent<NPC>();

            //Assign placement point
            GameObject startGate = sc.GetRandomJumpgate();
            JumpgateController jgc = startGate.GetComponent<JumpgateController>();
            GameObject egressPoint = jgc.egressPoint;

            //Update demographic, place object, activate
            npcComponent.currentSector = sc.sectorID;
            newMerchant.transform.position = egressPoint.transform.position;
            newMerchant.SetActive(true);
        }

        yield return null;
    }

    /// <summary>
    /// Pull a random Merchant NPC from the Merchant pool
    /// </summary>
    /// <returns>GameObject: The NPC</returns>
    /// <remarks>
    /// <para>
    /// We can only consider items in the pool that are INACTIVE and NOT DOCKED.
    /// Also, have an NPCTYPE of Merchant. Not sure why we need that qualifier if
    /// we're looking in the MERCHANT pool, but it doesn't hurt.
    /// </para>
    /// <para>
    /// This would be where we'd need to focus when selecting a merchant from the
    /// Game Controller. It would need to include only those merchants who are 
    /// listed as being in this sector when the player jumps in, or who end up
    /// in this sector while the player is here. 
    /// </para>
    /// </remarks>
    GameObject GetMerchantFromPool()
    {
        //Get a random item from the local Merchant pool that's not active, 
        try
        {
            var inactiveNPCS = (from np in merchantNPCList
                               where np.GetComponent<MerchantMovement>().isDocked.Equals(false)
                               && np.activeInHierarchy.Equals(false)
                               && np.GetComponent<NPC>().NPCType.Equals(NPC.NPCTYPE.Merchant)
                               select np).ToList<GameObject>();

            if (inactiveNPCS.Count() > 0)
            {
                //From our results, select and return a random merchant.
                int randomMerchantIDX = sc.GetRandomInt(0, inactiveNPCS.Count() - 1);
                GameObject randomMerchant = inactiveNPCS.ElementAt(randomMerchantIDX);

                return randomMerchant;
            }
            else
            {
                return null;
            }

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    #endregion

    #region Police Spawning

    /// <summary>
    /// Populates the POLICE specific object pool
    /// </summary>
    /// <remarks>
    /// <para>
    /// Like Merchants, the police object pool is populated by randomly choosing
    /// a model from the prefab list, and then assigning default info to the NPC
    /// component on the object. 
    /// </para>
    /// <para>
    /// Unlike the merchants, however, police are sector specific and do not need
    /// to persist between sectors. 
    /// </para>
    /// </remarks>
    void SetupPolicePool()
    {
        if (policePrefabs.Count == 0)
        {
            print("No police prefabs were defined for instantiation in NPC Controller on Sector Controller.");
        }
        else 
        { 
            for (int i = 0; i < maxPoliceCount; i++)
            {
                int randomPolice = sc.GetRandomInt(0, policeNPCList.Count);
                GameObject newPolice = (GameObject)Instantiate(policePrefabs[randomPolice], new Vector3(sc.GetRandomInt(0, 100), sc.GetRandomInt(0, 100), sc.GetRandomInt(0, 100)), Quaternion.identity);
                NPC npc = newPolice.GetComponent<NPC>();

                npc.NPCID = i;      //TODO: Need to set Merchant NPCID using something more robust
                npc.NPCName = "Sector Security";
                npc.NPCShipName = "Sector Security";
                npc.NPCFaction = "Police";
                npc.NPCType = NPC.NPCTYPE.Police;
                npc.currentSector = sc.sectorID;
                if (npc.lastSector == 0) { npc.lastSector = sc.sectorID; }

                newPolice.SetActive(false);

                policeNPCList.Add(newPolice);
            }
        }
    }

    /// <summary>
    /// Iterates through police, placing them with a slight interval
    /// </summary>
    /// <remarks>
    /// <para>
    /// Police are placed all at once because it's assumed that they're
    /// always patrolling, even when the player isn't present to spawn them. 
    /// </para>
    /// <para>
    /// In the future, police should start at stations, and not just randomly.
    /// </para>
    /// </remarks>
    void CheckPoliceSpawnPermission()
    {
        int inactivePolice = (from pl in policeNPCList
                              where pl.activeInHierarchy.Equals(false)
                              select pl).Count();

        for (int i = 0; i < inactivePolice; i++)
        {
            StartCoroutine(SpawnPoliceNPC());
        }
    }

    /// <summary>
    /// Spawns ALL Police that aren't currently active at random locations around
    /// points of interest in the system
    /// </summary>
    /// <returns>IEnumerator for coroutine</returns>
    /// <remarks>
    /// <para>
    /// Unlike Merchants, Police have a looping route. They'll start at a random
    /// location (station, jumpgate, etc) and will then move between all other 
    /// known objects in their route list (handled in NPCMovement). 
    /// </para>
    /// <para>
    /// This method chooses randomly between a station or jumpgate, and assigns the 
    /// police to head there. 
    /// </para>
    /// </remarks>
    IEnumerator SpawnPoliceNPC()
    {
        if (policeNPCList.Count > 0) { 
            GameObject newNPC = GetPoliceFromPool();
            NPC npcComponent = newNPC.GetComponent<NPC>();

            GameObject startingObject = null;
            GameObject egressPoint = null;
            int rndStart = sc.GetRandomInt(1, 2);
            switch (rndStart)
            {
                case 1:     //Jumpgate
                    startingObject = sc.GetRandomJumpgate();
                    egressPoint = startingObject.GetComponent<JumpgateController>().egressPoint;
                    break;
                case 2:     //Station
                    startingObject = sc.GetRandomStation();
                    egressPoint = startingObject.GetComponent<StationController>().egressPoint;
                    break;
                default:    //Others, like asteroids, mining platforms, etc
                    break;
            }

            if (startingObject != null)
            {
                //Spawn this object at the designated starting point's EgressPoint
                Vector3 spawnPos = egressPoint.transform.position; 
                npcComponent.currentSector = sc.sectorID;
                newNPC.transform.position = spawnPos;
                newNPC.SetActive(true);
            }
        }
        yield return null;
    }

    /// <summary>
    /// Selects a random, inactive NPC from the object pool
    /// </summary>
    GameObject GetPoliceFromPool()
    {
        //Get a random item from the Merchant pool that's not active, 
        var inactiveNPCS = from np in policeNPCList
                           where np.activeInHierarchy.Equals(false)
                           && np.GetComponent<NPC>().NPCType.Equals(NPC.NPCTYPE.Police)
                           select np;
        if (inactiveNPCS.Count() > 0)
        {
            int randomIDX = sc.GetRandomInt(0, inactiveNPCS.Count() - 1);
            GameObject randomNPC = inactiveNPCS.ElementAt(randomIDX);

            return randomNPC;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Miner Spawning

    /// <summary>
    /// Add prefab instances to the Miner pool
    /// </summary>
    /// <remarks>
    /// <para>
    /// Nothing new here. Follows the same protocols as the Merchants and Police
    /// </para>
    /// </remarks>
    void SetupMinerPool()
    {
        if (minerPrefabs.Count == 0)
        {
            print("No miner prefabs were defined for instantiation in NPC Controller on Sector Controller.");
        }
        else 
        { 
            for (int i = 0; i < maxMinerCount; i++)
            {
                int randomMinder = sc.GetRandomInt(0, minerPrefabs.Count);
                GameObject newMiner = (GameObject)Instantiate(minerPrefabs[randomMinder], new Vector3(0, 0, 0), Quaternion.identity);
                NPC npc = newMiner.GetComponent<NPC>();

                npc.NPCID = i;      //TODO: Need to set Merchant NPCID using something more robust
                npc.NPCName = "Deep Space Miner";
                npc.NPCShipName = "Rock Jockey";
                npc.NPCFaction = "Miner";
                npc.NPCType = NPC.NPCTYPE.Miner;
                npc.currentSector = sc.sectorID;
                if (npc.lastSector == 0) { npc.lastSector = sc.sectorID; }

                newMiner.SetActive(false);

                minerNPCList.Add(newMiner);
            }
        }
    }

    /// <summary>
    /// Determines eligibility for Miner spawning
    /// </summary>
    /// <remarks>
    /// <para>
    /// Like police, miners should start in the sector when the sector loads
    /// </para>
    /// </remarks>
    void CheckMinerSpawnPermission()
    {
        var inactiveMiner = (from m in minerNPCList
                             where m.activeInHierarchy.Equals(false)
                             select m).Count();

        int minerCount = 0;

        for (int i = 0; i < inactiveMiner; i++)
        {
            StartCoroutine(SpawnMinerNPC(minerCount));
            minerCount++;
        }
    }

    /// <summary>
    /// Spawns a miner and places them in the world
    /// </summary>
    /// <param name="minerCount">The current iterator from the merchant pool</param>
    /// <remarks>
    /// <para>
    /// Miner spawning is the more complex of the NPCs. 
    /// </para>
    /// <para>
    /// For each Miner we're spawning, we have to pick a random asteroid. Asteroids
    /// have an AsteroidController which creates a series of eligible spawn points around the
    /// center point of the game object. Using the minerCount in conjunction with these 
    /// spawn points, we place miners at unique spawn points around the asteroid game object.
    /// </para>
    /// <para>
    /// We do this because miners need to not spawn on top of one another, and to the 
    /// point where the miners spawn, they must return when they leave the station. 
    /// </para>
    /// <para>
    /// Myabe in the future we can spawn NPCs at asteroids which have the most lucritive 
    /// yield, although that would take up all slots on the asteroid.
    /// </para>
    /// <para>
    /// Note2Dev: If we have 6 spawn points per asteroid, and all spots are taken up, 
    /// then the minerCount value will exceed the ability to place new miners because we use that
    /// value to determine the numbered spawn point. Maybe we can randomly select a 
    /// spawn point rather than rely on the number scheme. 
    /// </para>
    /// </remarks>
    IEnumerator SpawnMinerNPC(int minerCount)
    {
        if (minerNPCList.Count > 0) { 
            GameObject newNPC = GetMinerFromPool();

            NPC npcComponent = newNPC.GetComponent<NPC>();
            MinerMovement npcMove = newNPC.GetComponent<MinerMovement>();

            //Get a random asteroid in the scene.
            GameObject startingObject = sc.GetRandomAsteroid();
            AsteroidController ac = startingObject.GetComponent<AsteroidController>();

            if (startingObject != null)
            {
                //Each asteroid should have some children named "ast_egressPoint[X]", where X is an integer.
                if (startingObject.transform.childCount > 0) { 
                    //Find an egressPoint which matches the minerCount. 
                    GameObject child = startingObject.transform.FindChild("ast_egressPoint" + minerCount).gameObject;
                    //Check the asteroid's assigned points list. If this point isn't in there, use it
                    if (child != null && !ac.egressAssigned.Contains(child.name))
                    {
                        //Mark as used
                        ac.egressAssigned.Add(child.name);

                        //Spawn the NPC at this point
                        Vector3 spawnPos = child.transform.position;
                        newNPC.transform.position = spawnPos;

                        //Set demographics, turn, and activate
                        npcComponent.currentSector = sc.sectorID;
                        npcMove.isMining = true;
                        transform.LookAt(startingObject.transform, Vector3.up);
                        newNPC.SetActive(true);

                    }
                }               
            }
        }
        yield return null;

    }

    /// <summary>
    /// Get an eligible miner NPC from the minerNPCList
    /// </summary>
    GameObject GetMinerFromPool()
    {
        //Get a random item from the Merchant pool that's not active, 
        var inactiveNPCS = from np in minerNPCList
                           where np.activeInHierarchy.Equals(false)
                           && np.GetComponent<NPC>().NPCType.Equals(NPC.NPCTYPE.Miner)
                           select np;
        if (inactiveNPCS.Count() > 0)
        {
            int randomIDX = sc.GetRandomInt(0, inactiveNPCS.Count() - 1);
            GameObject randomNPC = inactiveNPCS.ElementAt(randomIDX);

            return randomNPC;
        }
        else
        {
            return null;
        }
    }

    #endregion

    #region Shared

    /// <summary>
    /// Determines if an NPC should undock
    /// </summary>
    /// <remarks>
    /// <para>
    /// All eligble NPCs can undock at the same time. An NPC is eligible if they have been 
    /// docked greater than or equal to their dockTime value plus a small random value.
    /// </para>
    /// <para>
    /// This method handles both merchants and miners in the same query.
    /// </para>
    /// <para>
    /// The NPC should have already had their facing set to their next destination
    /// in the NPCMovement script, so when they reactivate, they'll be facing in 
    /// the proper direction already.
    /// </para>
    /// </remarks>
    void UndockNPC()
    {
        float currentTime = Time.time;

        List<GameObject> dockedNPCs = (from m in merchantNPCList
                                       where m.GetComponent<MerchantMovement>().isDocked.Equals(true)
                                       && (m.GetComponent<MerchantMovement>().dockTime + sc.GetRandomInt(5, 30) + 5.0f) < currentTime
                                       select m)
                                       .Union
                                       (from m in minerNPCList
                                        where m.GetComponent<MinerMovement>().isDocked.Equals(true)
                                        && (m.GetComponent<MinerMovement>().dockTime + sc.GetRandomInt(5, 30) + 5.0f) < currentTime
                                        select m
                                       ).ToList();

        //All eligible NPCs. Clear their IsDocked flags and reset their dockTime
        //Individual movement scripts will handle the forward movement at that point
        foreach (GameObject go in dockedNPCs)
        {
            NPC thisNPC = go.GetComponent<NPC>();
            switch (thisNPC.NPCType)
            {
                case NPC.NPCTYPE.Merchant:
                    MerchantMovement merchantMovement = go.GetComponent<MerchantMovement>();
                    merchantMovement.isDocked = false;
                    merchantMovement.dockTime = 0.0f;
                    break;
                case NPC.NPCTYPE.Pirate:
                    break;
                case NPC.NPCTYPE.Police:
                    break;
                case NPC.NPCTYPE.Miner:
                    MinerMovement minerMovement = go.GetComponent<MinerMovement>();
                    minerMovement.isDocked = false;
                    minerMovement.dockTime = 0.0f;
                    break;
                default:
                    break;
            }

            //Activate the docked object
            go.SetActive(true);
        }

    }

    /// <summary>
    /// Set up a spawning circle around an object so objects that
    /// spawn around the same object aren't right on top of one another.
    /// </summary>
    /// <param name="centerX">Float: Center of the hub along the X axis</param>
    /// <param name="centerZ">Float: Center of the hub along the Z axis</param>
    /// <param name="radius">Float: How far from the center to draw</param>
    /// <param name="totalPoints">Float: How many items to we expect to place</param>
    /// <param name="currentPoint">Float: Which point are we placing now?</param>
    /// <returns>Vector3: Next free position around the circle</returns>
    private Vector3 SpawnCircle(float centerX, float centerY, float centerZ, float radius, float totalPoints, float currentPoint)
    {
        float ptRatio = currentPoint / totalPoints;
        float pointX = centerX + (Mathf.Cos(ptRatio * 2 * Mathf.PI)) * radius;
        float pointZ = centerZ + (Mathf.Sin(ptRatio * 2 * Mathf.PI)) * radius;
        float pointY = centerY + (Mathf.Tan(ptRatio * 2 * Mathf.PI)) * radius;

        Vector3 spawnPoint = new Vector3(pointX, pointY, pointZ);

        return spawnPoint;
    }

    #endregion

    #region Merchant Simulation

    /*#############################################################################################
     * Merchant NPCs should be loaded into GDC from MDS and augmented from the state file if a 
     * save game has been loaded. 
     * ---
     * The simulation runs on a timer, and that timer actually acts based on an interval value
     * that has a base setting defined above, and a random value for each NPC that is assigned
     * when the NPC is added to the data pool. This allows us to stagger the activity of NPCs so 
     * they don't all act at the exact same time.
     * 
     * When a tick happens, NPCs will:
     * * Check to see if the next action will take place in the player's current sector or not. 
     *      This will determine if we only simulate or have to instantiate
     * * Simulation:
     *      If at a station, they will find a station that buys their goods for less than they
     *      paid for them, calc the route, and jump to the first sector.
     *      If in a sector on their route, they'll jump to the next sector in the route
     *      If the next sector in the route is the destination sector, they'll dock immediately
     *          at the station, sell their goods, and buy the lowest cost good that they can
     *          afford/fit into their cargo hold
     *  * Instantiation
     *  If at any time any of the sim steps coincide with the player's current sector
     *      If at a station, we instantiate the prefab at the spawn point outside of that station
     *          and assign any data we need from the data object to the component (at least the
     *          ID of the data object and the name of the pilot). We also generate the route as
     *          per the sim verion and the NPC will start their trip to the necessary gate. 
     *      If in a sector, we instantiate the prefab at the gate that leads to the previous 
     *          sector. The NPC will move to the gate that leads to the next sector, and the
     *          gate mechanics will take over at that point.
     *      If in the destination sector, the NPC will spawn at the inbound gate and will move
     *          to the destination station where the docking process will take over. 
     * ---
     * The tick might happen before or after the player arrives in the system. In this case, the tick
     *      will be SIMULATED and will not invoke the instantiation, so the NPC will "ghost in" while
     *      the player isn't looking. 
     * Certain instantiated activities will force a tick and reset the tick timer for that NPC:
     *  * Jumping out of a system - gate mechanics transfer the NPC and resets the timer
     *  * Docking at a station - triggers the buying and selling, and resets the timer
    #############################################################################################*/
    /*
     * 
     */

    public void LoadNewMerchants()
    {
        //Takes the records from the DataController and puts them into the 
        //  working collection for use in the game. 
        List<MerchantNPCDataObject> Merchs = DataController.DataAccess.merchantMasterList;

        //But we need to set them up with initial locations and purposes. 
        
        foreach(MerchantNPCDataObject merch in Merchs)
        {
            //Each merchant will start at a station (currently 37)
            StationDataObject sdo = (from s in DataController.DataAccess.stationMasterList orderby System.Guid.NewGuid() select s).First();

            merch.CurrentSectorID = sdo.sectorID;
            merch.CurrentStationID = sdo.stationID;

            //Now it needs to BUY stuff from that station
            //  Go for the LOWEST PRICED ITEM
            List<CommodityShopInventoryDataObject> shopInv = DataController.DataAccess.GetShopInventory(sdo.stationID);
            CommodityShopInventoryDataObject csidoBuy = (from si in shopInv where si.currentPrice.Equals(shopInv.Min(s => s.currentPrice)) && si.shopBuysOrSells.Equals("S") select si).First();

            int shopQty = csidoBuy.commodityQuantity;

            //How much can we hold?
            int cargoCap = (from cgo in DataController.DataAccess.cargoHoldMasterList where cgo.iD.Equals(merch.CargoID) select cgo).FirstOrDefault().capacity;
            //Do we have anything in there already?
            int cargoNow = merch.Inventory.inventoryQuantity;

            if (cargoNow == 0) { 
                //They can only carry one item, and since this is the only way for them to get
                //  items into their cargo hold, we assume they have something or they don't
                if (cargoNow < cargoCap)
                {
                    int canBuy = cargoCap - cargoNow;
                    if (shopQty > canBuy && (csidoBuy.currentPrice * canBuy) < merch.Wallet)
                    {
                        //Buy the canBuy amount.
                        merch.Inventory.inventoryQuantity = canBuy;
                        merch.Inventory.inventoryObjectType = PlayerInventoryDataObject.INVENTORY_TYPE.Commodity;
                        merch.Inventory.inventoryObjectID = csidoBuy.commodityID;
                        merch.Inventory.inventoryObjectClass = PlayerInventoryDataObject.INVENTORY_CLASS.Commodity;
                        merch.InventoryPurchaseTotal = (canBuy * csidoBuy.currentPrice);

                        merch.Wallet -= merch.InventoryPurchaseTotal;

                    }else{
                        //Need to find another low price item to buy.
                        //Need to excise the item finding part into it's own method. 
                        //Also need to track the ID's of the items we've already tried.
                    }
                }
            }

            //So we have cargo (theoretically). Figure out where in the universe we can sell this crap
            CommodityShopInventoryDataObject csidoSell = (from si in DataController.DataAccess.CommodityShopInventoryList where 
                                                              si.commodityID.Equals(csidoBuy.commodityID) && 
                                                              si.stationID != sdo.stationID && 
                                                              si.currentPrice > csidoBuy.currentPrice 
                                                          select si).FirstOrDefault();

            //Ideally we have values. Set them
            merch.DestinationSectorID = 0; //Crap. Need to get this from the record we have in csidoSell  :(

            
        }

        

    }
   
    #endregion
}
