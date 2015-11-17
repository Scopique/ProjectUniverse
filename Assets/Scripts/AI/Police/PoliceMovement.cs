using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/*########################################################################
 * Police NPC Movement
 * ---------------------
 * Police do not stop except to intercept pirates and players behaving
 * badly.
 * Their route contains all stations and jumpgates in the sector. They
 * bounce between them and will (eventually) attack pirates who venture
 * within their scanner range. 
 * 
 * Furture features include
 * 1. Scanners so they can detect aggressive players and pirates. Maybe
 * scan for contraband
 * 2. Need to work on their movement so they don't "pinball" when they
 * reach their destination node and turn to the next node
 * 3. Start all police at a station. This will help if they are destroyed
 * and a new police needs to be spawned to replace it
 * #####################################################################*/
public class PoliceMovement : MonoBehaviour
{

    #region Inspector Variables

    [Space(10)]
    //How fast the ship moves
    public float shipSpeed = 75.0f;

    [Header("Movement Flags")]
    //Distance to object before action
    public float distanceToAction = 10.0f;
    
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

        //Plan setup and execution
        SetMovementPlan();
        StartMovementPlan();
    }

    /// <summary>
    /// Handles the movement of the NPC
    /// </summary>
    /// <remarks>
    /// <para>
    /// NPC movement is pretty simple: It faces a target and moves forward until it's a
    /// specific distance from it's destination node, at which point we turn the NPC to 
    /// the next node.
    /// </para>
    /// </remarks>
    void Update()
    {

        var rotation = Quaternion.LookRotation(routeDestinations[currentDestinationIndex].Destination - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5.0f);

        transform.Translate(Vector3.forward * shipSpeed * Time.deltaTime);

        RaycastColliderCheck();

        //If the NPC overshoots it's destination, we need
        //  to deactivate it and return it to the pool.
        SelfDestruct();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Create a movement plan for the Police
    /// </summary>
    /// <remarks>
    /// <para>
    /// Unlike Merchants, Police visit ALL population centers like STATIONS and JUMPGATES. 
    /// </para>
    /// <para>
    /// All STATION and JUMPGATES are added to the police route list, and the route list is then
    /// SHUFFLED using an extension method. Because the list is always created in the same order, 
    /// this SHUFFLE method ensures that the police have different routes. 
    /// </para>
    /// <para>
    /// Police do not recalculate their routes unless they're destroyed and respawn.
    /// </para>
    /// </remarks>
    void SetMovementPlan()
    {
        routeDestinations.Clear();

        List<GameObject> destinations = new List<GameObject>();

        GameObject[] stations = GameObject.FindGameObjectsWithTag("Station");
        GameObject[] jumpgates = GameObject.FindGameObjectsWithTag("Jumpgate");
        foreach (GameObject go in stations) { routeDestinations.Add(new RouteDataObject(go.name, go.tag, go.transform.position)); }
        foreach (GameObject go in jumpgates) { routeDestinations.Add(new RouteDataObject(go.name, go.tag, go.transform.position)); }

        routeDestinations.Shuffle();
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
            if (currentDestinationIndex == routeDestinations.Count) { currentDestinationIndex = 0; }
            //transform.LookAt(routeDestinations[currentDestinationIndex].Destination, Vector3.up);
            
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
                    FaceNextNode();
                    break;
                case "BlackHole":
                    break;
                case "Station":
                    FaceNextNode();
                    break;
                case "Jumpgate":
                    FaceNextNode();
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
                    FaceNextNode();
                    break;
                case "BlackHole":
                    break;
                case "Station":
                    FaceNextNode();
                    break;
                case "Jumpgate":
                    FaceNextNode();
                    break;
                default:
                    break;
            }
        }
    }

    #endregion
}
