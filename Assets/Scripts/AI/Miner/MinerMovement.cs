using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*########################################################################
 * Miner NPC Movement
 * ---------------------
 * Miners have two stops on their route:
 * 1. Their home asteroid
 * 2. A home station
 * 
 * Miners will sit at their asteroid for a random period of time, after
 * which they'll head to their home station to unload their cargo. They'll
 * then return to their home asteroid to continue mining. 
 * 
 * Future improvements include
 * 1. Actual mining: Collect whatever is in the rock they're attacking
 * and put it in their cargo. Only return to the station when they are full. 
 * 2. Adding their cargo to the market: Alter market quantity based on what
 * and when the miners drop off their goods. 
 * 3. Evasion techniques when they're under attack. They aren't defensive
 * units, so they should immediately break off an head for the station.
 * 
 * Being able to deliver cargo to the station makes the miners a desirable
 * target for player defense.
 * #####################################################################*/
public class MinerMovement : MonoBehaviour
{

    #region Inspector Variables

    [Space(10)]
    public float shipSpeed = 10.0f;
    public float minerStayMining = 5.0f;

    [Header("Movement Flags")]
    //Are we currently evading attack?
    public bool isEvading;
    //Are we docked at a station?
    public bool isDocked;
    //Is the miner at an asteroid?
    public bool isMining;
    //Time since docked
    public float dockTime = 0.0f;

    #endregion

    #region Local Variables

    NPC self;
    SectorController sc;
    NPCController nc;

    //The NPC route collection of GO nodes -- stations, jumpgates, etc.
    List<RouteDataObject> routeDestinations;
    //The current destination index of the routeDestinations list
    int currentDestinationIndex = 0;

    //The time stamp of when the miner last returned to their station
    public float lastMinerReturnToStationCheck = 0.0f;
    
    #endregion

    #region Unity Methods

    /// <summary>
    /// Set up references and initialize lists. Start the creation 
    /// of the initial movement plan, and then start the movement.
    /// </summary>
    void Start()
    {
        //Init
        routeDestinations = new List<RouteDataObject>();

        //References
        self = gameObject.GetComponent<NPC>();
        GameObject sectorController = GameObject.Find("SectorController");
        if (sectorController != null)
        {
            sc = sectorController.GetComponent<SectorController>();
            nc = sectorController.GetComponent<NPCController>();
        }
        else
        {
            throw new System.Exception("SectorController was not found. Is it present and named 'SectorController'?");
        }

        //Plan setup
        SetMovementPlan();
    }

    /// <summary>
    /// Handles the movement of the NPC
    /// </summary>
    /// <remarks>
    /// <para>
    /// NPC movement is pretty simple: It faces a target and moves forward until reach a 
    /// certain distance from the destination location, at which point we turn the NPC 
    /// to the next node. 
    /// </para>
    /// <para>
    /// For miners, the Update method also checks whether or not it's time to 
    /// return to the station, based on a timer.
    /// </para>
    /// </remarks>
    void Update()
    {
        if (!isDocked && !isMining)
        {
            transform.Translate(Vector3.forward * shipSpeed * Time.deltaTime);

            DistanceCheck();
        }else if (isMining)
        {
            ReturnToStationCheck();
        }

        //Stop rogue NPCs from blowing past their destinations
        //  and traveling into deep space.
        SelfDestruct();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Set up a time delayed route between an asteroid option and a single station
    /// </summary>
    /// <remarks>
    /// <para>
    /// Miners move between an asteroid and a station. This has a large delay, as we 
    /// want to ensure that the miner spends time at the asteroid working, and very
    /// little time at the station (a fixed amount)
    /// </para>
    /// <para>
    /// The movement plan starts at an asteroid position, which is calculated as an array
    /// around the asteroid so that no two NPCs spawn or move on top of one another. This
    /// is saved as Index 0. Index 1 is the station that the miner reports to.
    /// </para>
    /// </remarks>
    void SetMovementPlan()
    {
        routeDestinations.Clear();

        routeDestinations.Add(new RouteDataObject("Mining Location", "Asteroid",transform.position));
        routeDestinations.Add(sc.GetRandomRouteNode("Station"));
    }

    /// <summary>
    /// Checks to see if it's time (literally) to return to the station
    /// </summary>
    /// remarks>
    /// <para>
    /// The time the miner spends at the asteroid is calculated based on the last time
    /// we checked for return plus the minimum time they have to spend "work" plus
    /// a random amount between 0s and 500s.
    /// </para>
    /// <para>
    /// When it's time for them to move, their IsMining flag is set to false, their 
    /// currentDestination is set to the Station index (1), and they're turned to face
    /// the station. The Update method will handle their movement forward.
    /// </para>
    /// </remarks>
    void ReturnToStationCheck()
    {
        float currentTime = Time.time;

        if (currentTime > (lastMinerReturnToStationCheck + minerStayMining + sc.GetRandomInt(10, 500)))
        {
            this.isMining = false;
            currentDestinationIndex = 1;
            FaceSpecificNode(currentDestinationIndex);
        }
    }

    /// <summary>
    /// Turns the NPC to face the next node in their destination list
    /// </summary>
    /// <remarks>
    /// <para>
    /// We ASSUME that facing the next node requires that we increment the 
    /// current node index by 1.
    /// </para>
    /// </remarks>
    public void FaceNextNode(bool UseTrigger = false)
    {
        currentDestinationIndex += 1;
        if (currentDestinationIndex <= routeDestinations.Count)
        {
            transform.LookAt(routeDestinations[currentDestinationIndex].Destination, Vector3.up);
        }
        else
        {
            //Note2Dev: If we get to dest > routeCount, something is really wrong
            SelfDestruct();
        }
    }

    /// <summary>
    /// Turns the NPC to look at a specific node in their route.
    /// </summary>
    /// <param name="Index">Int: The Index of the node to look at</param>
    public void FaceSpecificNode(int Index)
    {
        transform.LookAt(routeDestinations[Index].Destination, Vector3.up);
    }

    /// <summary>
    /// Used to reset the NPC's movement and ready them for eventual redeployment
    /// </summary>
    /// <remarks>
    /// <para>
    /// This shouldn't be used for miners. If they blow past a destination, then
    /// the system isn't set up to respawn them. They're forever lost to the void.
    /// </para>
    /// </remarks>
    public void ResetNPC()
    {
        this.transform.position = new Vector3(0, 0, 0);
        currentDestinationIndex = 0;
        isEvading = false;
        isDocked = false;
        isMining = false;

        //A new plan
        SetMovementPlan();
    }

    /// <summary>
    /// Return to the pool if the NPC gets too far from 
    /// the sector's center at [0,0,0]
    /// </summary>
    /// <remarks>
    /// <para>
    /// Useful for Merchants; not so much for everyone else.
    /// </para>
    /// </remarks>
    void SelfDestruct()
    {
        if (Vector3.Distance(new Vector3(0, 0, 0), gameObject.transform.position) > 5000)
        {
            gameObject.SetActive(false);
            ResetNPC();
        }
    }

    /// <summary>
    /// "Docks" an NPC at a station
    /// </summary>
    /// <remarks>
    /// <para>
    /// Docking sets the IsDocking flag, and deactivates the object.
    /// In the object pool, these two factors are enough for us to
    /// be able to randomly pull a docked NPC from the pool to 
    /// reactivate it for it's next step. It also sets the time
    /// of docking, so we can check how much time has elapsed.
    /// </para>
    /// <para>
    /// This also pre-aligns the NPC with it's next node so when
    /// it wakes up it'll be pointing in the proper direction.
    /// </para>
    /// </remarks>
    IEnumerator Dock()
    {
        if (isDocked == false)
        {
            isDocked = true;
            dockTime = Time.time;

            currentDestinationIndex = 0;
            FaceSpecificNode(currentDestinationIndex);

            gameObject.SetActive(false);
        }

        yield return null;
    }

    /// <summary>
    /// Avoid an object type that requires avoidance (Planet, black hole, etc)
    /// </summary>
    /// <remarks>
    /// <para>
    /// Currently unfinished and unrefined. Doesn't really work like it should. 
    /// </para>
    /// </remarks>
    public void Avoid()
    {
        if (isEvading == false)
        {
            //Turn 20 degrees in a random direction (+ or -)
            int turn = sc.GetRandomInt(0, 50) > 25 ? 20 : -20;
            transform.rotation = Quaternion.AngleAxis(turn, Vector3.up);

            //Need to set this so we can 
            //  know when to reset the NPC
            //  to continue to destination
            isEvading = true;
        }

    }

    /// <summary>
    /// Sets the NPC to "mine" a local asteroid
    /// </summary>
    /// <remarks>
    /// <para>
    /// Curently, mining involves setting a flag and setting the timer
    /// on the last time we checked whether or not to return to the station.
    /// </para>
    /// <para>
    /// Eventually, we'll need to start some animation at this point.
    /// </para>
    /// </remarks>
    public void Mine()
    {
        isMining = true;
        lastMinerReturnToStationCheck = Time.time;
        //TODO: Trigger mining animation
    }

    /// <summary>
    /// The NEW action trigger system
    /// </summary>
    /// <remarks>
    /// <para>
    /// The routeDestinations collection contains RouteDataObject objects which list
    /// vector positions of the nodes the NPC visits. This method checks the destination
    /// that the NPC is heading to, and calculates the distance between the NPC and the
    /// destination. If the NPC is <= 2 units, then the NPC is considered to be within 
    /// the objects sphere of influence and should take the appropriate action. 
    /// </para>
    /// <para>
    /// I switched from the trigger method because entering the trigger was firing at
    /// undesirable times, like when an NPC had to move across a trigger after having
    /// spawned on the opposite side of the trigger, and also reduces the number of 
    /// boolean flags we have to use to keep track of the NPC's state for various triggers. 
    /// </para>
    /// </remarks>
    void DistanceCheck()
    {
        float dist = Vector3.Distance(transform.position, routeDestinations[currentDestinationIndex].Destination);
        if (dist <= 2){

            string tag = routeDestinations[currentDestinationIndex].DestinationTag;
            switch (tag)
            {
                case "Planet":
                    break;
                case "NPC":
                    break;
                case "Asteroid":
                    Mine();
                    break;
                case "BlackHole":
                    break;
                case "Station":
                    StartCoroutine(Dock());
                    break;
                case "Jumpgate":
                    break;
                default:
                    break;
            }        
        }
    }

    #endregion
}
