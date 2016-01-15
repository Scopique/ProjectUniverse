using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class StationObjectEditor : EditorWindow {

    static StationObjectEditor editorWindow;

    //Unlike other editors, this value does not contain the .asset filename
    //  because we're going to try and work with several at once.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/";

    private dbStationDataObject dbStations;
    private dbCommodityShopDataObject dbCommodityShops;
    private dbCommodityDataObject dbCommodities;

    private string[] sectorNames;

    private Vector2 scrollPos;

    private List<bool> stationFoldoutOpen;
    private List<bool> commodityShopFolderOpen;
    private List<StationEditorValues> stationEditorValues;

    [MenuItem("Data Management/Stations")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<StationObjectEditor>();
        editorWindow.Show();
    }

    void OnEnable()
    {
        LoadDatabases();
        InitializeLists();
    }

    void OnGUI()
    {
        DisplayStations();
    }

    /// <summary>
    /// Loads all databases
    /// </summary>
    /// <remarks>
    /// These should already be present since they were loaded in from the 
    /// legacy MasterDatabase structure.
    /// </remarks>
    void LoadDatabases()
    {
        dbStations = (dbStationDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbStationDataItems.asset", typeof(dbStationDataObject));
        dbCommodityShops = (dbCommodityShopDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCommodityShopDataItems.asset", typeof(dbCommodityShopDataObject));
        dbCommodities = (dbCommodityDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCommodityDataItems.asset", typeof(dbCommodityDataObject));
    }

   

    /// <summary>
    /// Display the list of stations in foldout format. Includes scrolling ability
    /// </summary>
    void DisplayStations()
    {
        int itemIdx = 0;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height-150));

        //In here will be foldouts for each Station we have defined. 
        foreach (StationDataObject dbso in dbStations.database)
        {
            stationFoldoutOpen[itemIdx] = EditorGUILayout.Foldout((bool)stationFoldoutOpen[itemIdx], dbso.stationName);
            if ((bool)stationFoldoutOpen[itemIdx])
            {
                stationEditorValues[dbso.stationID].stationID = dbso.stationID;
                stationEditorValues[dbso.stationID].stationName = EditorGUILayout.TextField("Station Name", stationEditorValues[dbso.stationID].stationName, GUILayout.Width(300));
                stationEditorValues[dbso.stationID].sectorID = int.Parse(EditorGUILayout.TextField("Sector ID", stationEditorValues[dbso.stationID].sectorID.ToString(), GUILayout.Width(200)));
                stationEditorValues[dbso.stationID].stationPosition = EditorGUILayout.Vector3Field("Position in Sector", stationEditorValues[dbso.stationID].stationPosition, GUILayout.Width(300));

                DisplayCommodityShop(dbso.stationID, itemIdx);
            }

            itemIdx++;
        }
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
    }

    //TODO: Needs more spacing :(
    void DisplayCommodityShop(int StationID, int Idx)
    {
        CommodityShopDataObject cdo = GetCommodityShopByStationID(StationID);
        commodityShopFolderOpen[Idx] = EditorGUILayout.Foldout(commodityShopFolderOpen[Idx], "Commodity Shop - " + cdo.shopName);
        if (commodityShopFolderOpen[Idx])
        {

        }

    }

    /// <summary>
    /// Get the commodity shop for the provided station by station ID
    /// </summary>
    /// <returns>CommodityShopDataObject</returns>
    CommodityShopDataObject GetCommodityShopByStationID(int StationID)
    {
        List<CommodityShopDataObject> shop = new List<CommodityShopDataObject>();
        CommodityShopDataObject cdo = shop.Find(x => x.stationID.Equals(StationID));
        if (cdo == null) { cdo = new CommodityShopDataObject(StationID, "Unassigned", "This commodity shop has not been opened.", "Empty.jpg"); }
        return cdo;
    }

    
    void InitializeLists()
    {
        stationEditorValues = new List<StationEditorValues>();
        stationFoldoutOpen = new List<bool>();
        commodityShopFolderOpen = new List<bool>();

        stationEditorValues.Add(new StationEditorValues(0, 0, Vector3.zero, string.Empty));

        for (int i = 0; i <= dbStations.Count - 1; i++ )
        {
            stationFoldoutOpen.Add(false);
            commodityShopFolderOpen.Add(false);
        }

        for (int i = 1; i <= dbStations.Count-1; i++)
        {
            stationEditorValues.Add(new StationEditorValues(dbStations.database[i].stationID, dbStations.database[i].sectorID, dbStations.database[i].stationPosition, dbStations.database[i].stationName));
        }
    }

    void SetFoldoutOpen(int idx)
    {
        InitializeLists();
        stationFoldoutOpen[idx] = true;
    }

    #region Editor Value Tracking Objects

    private class StationEditorValues
    {
        public int stationID;
        public int sectorID;
        public Vector3 stationPosition;
        public string stationName;

        public StationEditorValues(
            int StationID,
            int SectorID,
            Vector3 StationPosition,
            string StationName)
            {
                this.stationID = StationID;
                this.sectorID = SectorID;
                this.stationPosition = StationPosition;
                this.stationName = StationName;
            }
    }

    #endregion  

}


