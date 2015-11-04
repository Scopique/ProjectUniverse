using UnityEngine;
using System.Linq;
using System.Collections;

public class StationController : MonoBehaviour {

    #region Inspector variables

    [Header("Demographics")]
    public int stationID;
    public string stationName;

    [Header("Spawn Control")]
    public GameObject egressPoint;

    #endregion

    #region Local Variables

    private 

    GameObject dockingObject;
    SectorController sc;
    NPCController nc;

    #endregion

    #region Unity Methods

    void Awake()
    {
        MeshRenderer mr = egressPoint.GetComponent<MeshRenderer>();
        if (mr != null) { mr.enabled = false; }
    }

    void Start()
    {
        GameObject sectorController = GameObject.Find("SectorController");
        if (sectorController != null)
        {
            sc = sectorController.GetComponent<SectorController>();
            nc = sectorController.GetComponent<NPCController>();
        }
        else
        {
            throw new System.Exception("Cannot find SectorController. Is it present and named 'SectorController'?");
        }
    }

    #endregion

    #region Private Methods

    private void Init()
    {
        StationDataObject sdo = sc.GetStationData(this.stationID);
        if (sdo != null)
        {
            stationName = sdo.stationName;
            sdo.stationPosition = transform.position;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        dockingObject = other.gameObject;

        //This should only concern itself with the player.
        //Set the PlayerController.CurrentStation value to this ID
        //Display the Docking Prompt (Messenger 'ShowDockingPrompt')
    }

    void OnTriggerExit(Collider other)
    {
        dockingObject = null;

        //This should only concern itself with the player
        //Reset PlayerController.CurrentStation to 0
        //Clear Docking Prompt (Messenger 'HideDockingPrompt')
    }

    #endregion 

}
