using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIStationManager : MonoBehaviour
{

    #region Inspector Variables
    
    [Header("Child Panels")]
    public GameObject stationMainMenu;
    public GameObject commodityMarketMenu;

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
        //TODO: When the ESC key is pressed, close the LIFO window in the openWindowList
        //  Keep going until we have no more windows in the collection to close. 
        if (Input.GetKeyUp("escape") && openWindowList.Count>0)
        {
            CloseTopWindow();
        }
    }

    #endregion

    #region Public Methods

    public void OpenCommodityMarket() {
        DisplayCommodityMarket();
    }

    public void OpenShipyard() { }

    public void OpenBar() { }

    public void OpenPersonalHangar() { }

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
        if (!openWindowList.Contains(stationMainMenu))
        {
            openWindowList.Add(stationMainMenu);
        }
        
    }

    private void DisplayCommodityMarket()
    {
        commodityMarketMenu.SetActive(true);
        if (!openWindowList.Contains(commodityMarketMenu))
        {
            openWindowList.Add(commodityMarketMenu);
        }
    }

    private void CloseTopWindow()
    {
        int topWindowIdx = openWindowList.Count - 1;
        GameObject topWindow = openWindowList[topWindowIdx];
        topWindow.SetActive(false);
        openWindowList.Remove(topWindow);
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
