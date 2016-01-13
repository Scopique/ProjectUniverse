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

    private Vector2 scrollPos;

    private List<bool> foldoutOpen;
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

    List<CommodityShopDataObject> GetCommodityShopByStationID(int StationID)
    {
        List<CommodityShopDataObject> shop = new List<CommodityShopDataObject>();



        return shop;
    }

    void DisplayStations()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height-150));

        //In here will be foldouts for each Station we have defined. 
        foreach (StationDataObject dbso in dbStations.database)
        {
            foldoutOpen[dbso.stationID] = EditorGUILayout.Foldout((bool)foldoutOpen[dbso.stationID], dbso.stationName);
            if ((bool)foldoutOpen[dbso.stationID])
            {
                EditorGUILayout.TextField("Sector ID", "");
            }
        }
        

        EditorGUILayout.EndScrollView();
    }

    void DisplayCommodityShop()
    {

    }

    
    void InitializeLists()
    {
        stationEditorValues = new List<StationEditorValues>();
        foldoutOpen = new List<bool>();

        stationEditorValues.Add(new StationEditorValues(0, 0, Vector3.zero, string.Empty));
        foldoutOpen.Add(false);         //Since we're working by Station ID, reserve Idx 0

        for (int i = 1; i <= dbStations.Count - 1; i++)
        {
            foldoutOpen.Add(false);

            stationEditorValues.Add(new StationEditorValues(dbStations.database[i].stationID, dbStations.database[i].sectorID, dbStations.database[i].stationPosition, dbStations.database[i].stationName));
        }
    }

    void SetFoldoutOpen(int idx)
    {
        InitializeLists();
        foldoutOpen[idx] = true;
    }


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

}


