using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*########################################################################
 * Merchant NPC Movement
 * ---------------------
 * Merchants have a very straightforward pattern:
 * 1. Jump into the system at a random jumpgate
 * 2. Move to a station a dock
 * 3. Undock from the station and jump out through a jumpgate
 * 
 * Currently, merchants can only pick a single station to dock at.
 * Their choice has nothing to do with cargo, and is entirely random.
 * 
 * Future improvements include:
 * 1. Picking a station based on current cargo carried
 * 2. Actually buying and carrying cargo in inventory
 * 3. Named NPCs that persist at a Game Control object pool level
 * 4. NPCs that make use of jumpgate connections to travel, and don't
 *      just deactivate when they jump
 * 5. Evasive and defensive activities when under attack
 * #####################################################################*/
public class MerchantMovement : MonoBehaviour
{

    #region Inspector Variables

    [Space(10)]
    public float shipSpeed = 10.0f;

    [Header("Movement Flags")]
    //Are we currently evading attack?
    public bool isEvading;
    //Are we docked at a station?
    public bool isDocked;
    //Are we considered to be jumping?
    public bool isJumping;
    //Time since docked
    public float dockTime = 0.0f;
    //Distance to object before action
    public float distanceToAction = 20.0f;

    #endregion

    #region Local Variables

    NPC self;
    SectorController sc;
    NPCController nc;

    //The NPC route collection of GO nodes -- stations, jumpgates, etc.
    List<RouteDataObject> routeDestinations;
    //The current destination index of the routeDestinations list
    int currentDestinationIndex = 0;

    #endregion

    #region Unity Methods

    /// <summary>
    /// Set up references and initialize lists. Start the creation 
    /// of the initial movement plan, and then start the movement.
    /// </summary>
	void Start () {

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
        
        //Plan setup and execution
        SetMovementPlan();
        StartMovementPlan();
	}

    /// <summary>
    /// Deal with the enabling of the object after its "jumped" or when 
    /// it first spawns in so it can start a new route.
    /// </summary>
    /// <remarks>
    /// <para>
    /// When an NPC jumps, it's deactivated and it's IsJumping flag is set. 
    /// For sector-specific NPCs who are pooled, when they're pulled out of
    /// the pool for another tour, they need to have their IsJumping flag
    /// reset and a new route plan set. 
    /// </para>
    /// <para>
    /// This randomizes the routes the NPC will take between pool calls, so 
    /// this check is currently invalid for looping NPCs.
    /// </para>
    /// </remarks>
    void OnEnable()
    {
        if (isJumping)
        {
            isJumping = false;

            SetMovementPlan();

            StartMovementPlan();
        }
    }

    /// <summary>
    /// Handles the movement of the NPC on tick
    /// </summary>
    /// <remarks>
    /// <para>
    /// NPC movement is pretty simple: It faces a target and moves forward until we get
    /// within a specific distance from the current target, at which point we turn the 
    /// NPC to the next node. At the last stop, the NPC is pulled from active duty.
    /// </para>
    /// </remarks>
	void Update () {
        if (!isDocked && !isJumping) { 
            transform.Translate(Vector3.forward * shipSpeed * Time.deltaTime);
            
            RaycastColliderCheck();
        }

        //This method is to ensure that a rogue NPC
        //  doesn't overshoot it's target and head off
        //  into space for eternity.
        SelfDestruct();
    }

    #endregion
    
    #region Private Methods
    
    /// <summary>
    /// Create a movement plan for Merchants
    /// </summary>
    /// <remarks>
    /// <para>
    /// First, we clear any previous route we might have. This is essential for creating
    /// a new route when pulling an NPC from the pool, as they might have a previous 
    /// route still in memory. 
    /// </para>
    /// <para>
    /// We then select one STATION and one JUMPGATE of each from the sector list. We add 
    /// the STATION to the route list, and then the JUMPGATE.
    /// </para>
    /// <para>
    /// This means that all merchants will jump in, dock, and jump out. There is no current
    /// method to allow for multiple station visits, and it's OK if they leave the sector
    /// from the same gate they used to enter through.
    /// </para>
    /// </remarks>
    void SetMovementPlan()
    {
        routeDestinations.Clear();

        routeDestinations.Add(sc.GetRandomRouteNode("Station")); 
        routeDestinations.Add(sc.GetRandomRouteNode("Jumpgate"));
    }

    /// <summary>
    /// Starts the movement plan by initializing the 
    /// current  destination node to -1 and calling the method
    /// to face the "next" node. That method increments the 
    /// current destination index to current +1, which is why
    /// we set it to -1 here. 
    /// </summary>
    void StartMovementPlan()
    {
        currentDestinationIndex = -1;
        FaceNextNode();
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
        try
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
        catch (System.Exception ex)
        {
            throw ex;
        }

    }

    /// <summary>
    /// Used to reset the NPC's movement and ready them for eventual redeployment
    /// </summary>
    /// <remarks>
    /// <para>
    /// We set all flags to FALSE and move the NPC to [0,0,0] just for the
    /// heck of it. We then set up another random movement plan for the 
    /// next deployment. 
    /// </para>
    /// </remarks>
    public void ResetNPC()
    {
        this.transform.position = new Vector3(0, 0, 0);
        currentDestinationIndex = 0;
        isEvading = false;
        isDocked = false;

        //A new plan
        SetMovementPlan();
    }

    /// <summary>
    /// Return to the pool if the NPC gets too far from 
    /// the sector's center at [0,0,0]
    /// </summary>
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

            FaceNextNode();

            gameObject.SetActive(false);
        }
        yield return null;
    }

    /// <summary>
    /// "Jumps" the NPC to another sector
    /// </summary>
    /// <remarks>
    /// <para>
    /// All it does is set
    /// the IsJumping flag to true, resets the NPC's movement
    /// path so it can be re-randomized when its next pulled
    /// from the pool, and its Active flag is set to false to 
    /// shut it down for the time being.
    /// </para>
    /// <para>
    /// In the future, this will update the LastSector flag
    /// with the CURRENT sector ID, set the CurrentSector
    /// flag with the NEXT sector ID, and will set the 
    /// object Inactive and ineligible for reconstitution in 
    /// this sector for a while.
    /// </para>
    /// </remarks>
    public void Jump()
    {
        isJumping = true;
        ResetNPC();
        gameObject.SetActive(false);   
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
        if (dist <= 2)
        {
            string tag = routeDestinations[currentDestinationIndex].DestinationTag;
            switch (tag)
            {
                case "Planet":
                    break;
                case "NPC":
                    break;
                case "Asteroid":
                    break;
                case "BlackHole":
                    break;
                case "Station":
                    StartCoroutine(Dock());
                    break;
                case "Jumpgate":
                    Jump();
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Using raycasting, keep an eye out for what's in front of us.
    /// If it's something we need to take an action on, then determine
    /// what we hit, and what action to take.
    /// </summary>
    void RaycastColliderCheck()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit, distanceToAction))
        {
            string tag = hit.collider.gameObject.tag;
            switch (tag)
            {
                case "Planet":
                    break;
                case "NPC":
                    break;
                case "Asteroid":
                    break;
                case "BlackHole":
                    break;
                case "Station":
                    StartCoroutine(Dock());
                    break;
                case "Jumpgate":
                    Jump();
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
}
