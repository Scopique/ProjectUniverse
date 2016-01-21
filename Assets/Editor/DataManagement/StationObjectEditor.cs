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

    private StationDataObject sdo;
    private CommodityShopDataObject csdo;

    private Vector2 scrollPos;

    bool foldOutStation = false;
    bool foldOutCShop = false;
    bool okDelete = false;
    
    int currentlySelectedStationID = 0;

    [MenuItem("Data Management/Stations")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<StationObjectEditor>();
        editorWindow.Show();
    }

    void OnEnable()
    {
        LoadResources();
        Initialize();
    }

    void OnGUI()
    {
        EditorGUIUtility.LookLikeInspector();
        
        DisplayActionButtons();
        DisplayFrame();
    }

    /// <summary>
    /// Loads all databases
    /// </summary>
    /// <remarks>
    /// These should already be present since they were loaded in from the 
    /// legacy MasterDatabase structure.
    /// </remarks>
    void LoadResources()
    {
        dbStations = (dbStationDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbStationDataItems.asset", typeof(dbStationDataObject));
        dbCommodityShops = (dbCommodityShopDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCommodityShopDataItems.asset", typeof(dbCommodityShopDataObject));
    }

    private void Initialize()
    {
        //Because we might not have child items for each station, we need to add in blanks to those databases in order to 
        //  prepare for the eventual filling-in of the data
        bool cshopIsDirty = false;
        if (dbCommodityShops.Count < dbStations.Count)
        {
            for (int i = 0; i <= dbStations.Count - 1; i++)
            {
                int stationID = dbStations.database[i].stationID;
                CommodityShopDataObject tempCdso = dbCommodityShops.GetCommodityShopByStation(stationID);
                if (tempCdso.shopName == string.Empty)
                {
                    tempCdso = new CommodityShopDataObject(stationID, "New Commodity Shop", "This space for rent", Texture2D.whiteTexture);
                    dbCommodityShops.Add(tempCdso);

                    cshopIsDirty = true;
                }
            }
        }

        if (cshopIsDirty) { EditorUtility.SetDirty(dbCommodityShops); }

        sdo = new StationDataObject();
        csdo = new CommodityShopDataObject();
    }

    private void DisplayActionButtons()
    {
        EditorGUILayout.Space();
    }

    private void DisplayFrame()
    {

        EditorGUILayout.BeginHorizontal();

        Rect leftSide = EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.25f)); ;

        StationList();

        EditorGUILayout.EndVertical();

        Rect rightSide = EditorGUILayout.BeginVertical(GUILayout.Width((Screen.width * 0.75f) - 30));

        EditorPopulation();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void StationList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width * 0.25f));

        EditorGUILayout.BeginVertical();

        foreach (StationDataObject sdo in dbStations.database)
        { 
            if (GUILayout.Button(sdo.stationID + " - " + sdo.stationName))
            {
                currentlySelectedStationID = sdo.stationID;
                
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void EditorPopulation()
    {
        //###############################################################################################
        // New station button. Also creates a new set of child shops
        if (GUILayout.Button("New Station"))
        {
            //Create empty data buckets. Reset the currentlySelectedStation
            //currentlySelectedStationID = 0;
            CreateRecord();
        }

        //If we HAVE a currently selected station, get its data
        if (currentlySelectedStationID > 0)
        {
             sdo = dbStations.GetStationByID(currentlySelectedStationID);
             csdo = dbCommodityShops.GetCommodityShopByStation(currentlySelectedStationID);
        }
            

        //###############################################################################################
        //Main station input form
        foldOutStation = EditorGUILayout.Foldout(foldOutStation, "Station");
        if (foldOutStation) { 
            //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
            EditorGUI.BeginChangeCheck();
            sdo.stationName = EditorGUILayout.TextField("Name", sdo.stationName, GUILayout.Width(400));
            sdo.sectorID = EditorGUILayout.IntField("Sector", sdo.sectorID, GUILayout.Width(200));
            sdo.stationPosition = EditorGUILayout.Vector3Field("Position", sdo.stationPosition, GUILayout.Width(400));
            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(dbStations); }
        }

        EditorGUILayout.Space();

        foldOutCShop = EditorGUILayout.Foldout(foldOutCShop, "Commodity Shop");
        if (foldOutCShop) { 
                
            //###############################################################################################
            //Commodity market input form

            //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
            EditorGUI.BeginChangeCheck();
            csdo.shopName = EditorGUILayout.TextField("Shop Name", csdo.shopName, GUILayout.Width(350));

            EditorGUILayout.BeginHorizontal();
            csdo.shopDescription = EditorGUILayout.TextArea(csdo.shopDescription, GUILayout.Width(350), GUILayout.Height(64));
            csdo.shopkeeperPortraitTexture = (Texture2D)EditorGUILayout.ObjectField(csdo.shopkeeperPortraitTexture, typeof(Texture2D), false, GUILayout.Height(64), GUILayout.Width(64));
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck()) { 
                //If this doesn't have a cshop record, we need to add one
                EditorUtility.SetDirty(dbCommodityShops); 
            }
        }

        EditorGUILayout.Space();

        //###############################################################################################
        // Delete this station and child shops

        EditorGUILayout.BeginHorizontal();
        okDelete = EditorGUILayout.BeginToggleGroup("Delete the station and everyone in it.", okDelete);
        
        //Sanity check(mark)
        if (GUILayout.Button("Delete this station", GUILayout.Width(175)) && currentlySelectedStationID > 0)
        {
            DeleteRecord();

            okDelete = false;
        }

        EditorGUILayout.EndToggleGroup();    

        EditorGUILayout.EndHorizontal();

    }

    void CreateRecord()
    {
        //Get the next StationID in sequence
        int newStationID = dbStations.database.Max(x => x.stationID) + 1;

        //Create new dummy records
        sdo = new StationDataObject(newStationID, 0, "New station");
        csdo = new CommodityShopDataObject(newStationID, "New commodity shop", "This space for rent", Texture2D.whiteTexture);

        //Add new records to database
        dbStations.Add(sdo);
        dbCommodityShops.Add(csdo);

        //Set the currently selected StationID
        currentlySelectedStationID = newStationID;
    }

    void DeleteRecord()
    {
        int idxStation;
        int idxCommodityShop;

        //Find the object in the database
        StationDataObject tempSDO = dbStations.database.Find(x => x .stationID.Equals(currentlySelectedStationID));
        CommodityShopDataObject tempCSDO = dbCommodityShops.database.Find(x => x.stationID.Equals(currentlySelectedStationID));

        //Get the index
        idxStation = dbStations.database.IndexOf(tempSDO);
        idxCommodityShop = dbCommodityShops.database.IndexOf(tempCSDO);

        //Remove by index
        dbStations.database.RemoveAt(idxStation);
        dbCommodityShops.database.RemoveAt(idxCommodityShop);

        //Update the SOs
        EditorUtility.SetDirty(dbStations);
        EditorUtility.SetDirty(dbCommodityShops);

        //For clearing the form
        sdo = new StationDataObject();
        csdo = new CommodityShopDataObject();

        //Clear the flag
        currentlySelectedStationID = 0;
    }

}


