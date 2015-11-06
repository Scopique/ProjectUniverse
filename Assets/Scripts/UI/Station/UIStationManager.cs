using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIStationManager : MonoBehaviour
{

    #region Inspector Variables
    
    [Header("Child Panels")]
    public GameObject stationMainMenu;

    #endregion

    #region Public Variables
    
    #endregion

    #region Private Variables

    List<GameObject> openWindowList;

    #endregion

    #region Unity Methods

    // Use this for initialization
	void Start () {
	    
        //Register listeners. Used for initial docking, and leaving the station
        Messenger.AddListener("OpenPlayerDockingMenu", DisplayMainMenu);
        Messenger.AddListener("CloseStructureMenu", CloseStructureMenu);

        openWindowList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    #endregion

    #region Public Methods
    
    public void CloseStationMenus()
    {
        CloseStructureMenu();
    }

    #endregion

    #region Private Methods
    
    private void DisplayMainMenu()
    {
        //Open the 
        stationMainMenu.SetActive(true);
        openWindowList.Add(stationMainMenu);
    }

    private void CloseStructureMenu()
    {
        //Close all open menus, regardless of which ones they are.
        for (int i = openWindowList.Count - 1; i>=0; i--)
        {
            openWindowList[i].SetActive(false);
        }
        openWindowList.Clear();
    }

    #endregion
}
