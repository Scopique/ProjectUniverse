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

    private Vector2 scrollPos;

    private List<bool> stationFoldoutOpen;

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
        DisplayActionButtons();
        
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
    }

    private void InitializeLists()
    {
        stationFoldoutOpen = new List<bool>();
       
        for (int i = 0; i <= dbStations.Count - 1; i++ )
        {
            stationFoldoutOpen.Add(false);
        }

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
    }

    private void DisplayActionButtons()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        if (GUILayout.Button("Refresh"))
        {
            LoadDatabases();
            InitializeLists();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void DisplayStations()
    {
        int stationIdx = 0;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width));

        foreach (StationDataObject sdo in dbStations.database)
        {
            //Get data from other databases for this station
            CommodityShopDataObject csdo = dbCommodityShops.GetCommodityShopByStation(sdo.stationID);     

            //Indicator for what's missing from the station
            string missingChildren = string.Empty;
            if (csdo.shopName == string.Empty) { missingChildren += "C"; }

            if (missingChildren != string.Empty) { missingChildren = " [" + missingChildren + "]"; }

            //Foldout for each station
            stationFoldoutOpen[stationIdx] = EditorGUILayout.Foldout(stationFoldoutOpen[stationIdx], sdo.stationID.ToString() + " - " + sdo.stationName + missingChildren);
            if (stationFoldoutOpen[stationIdx])
            {
                //###############################################################################################
                //Main station input form

                //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
                EditorGUI.BeginChangeCheck();
                sdo.stationName = EditorGUILayout.TextField("Name", sdo.stationName, GUILayout.Width(400));
                sdo.sectorID = EditorGUILayout.IntField("Sector", sdo.sectorID, GUILayout.Width(200));
                sdo.stationPosition = EditorGUILayout.Vector3Field("Position", sdo.stationPosition, GUILayout.Width(400));
                if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(dbStations); }

                EditorGUILayout.Space();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
                EditorGUILayout.LabelField("COMMODITY SHOP DETAILS");
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
                EditorGUILayout.LabelField("Shop Name", GUILayout.Width(200));
                EditorGUILayout.LabelField("Description", GUILayout.Width(250));
                EditorGUILayout.LabelField("Portrait", GUILayout.Width(64));
                EditorGUILayout.EndHorizontal();

                //###############################################################################################
                //Commodity market input form

                EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
                //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
                EditorGUI.BeginChangeCheck();
                csdo.shopName = EditorGUILayout.TextField(csdo.shopName, GUILayout.Width(200));
                csdo.shopDescription = EditorGUILayout.TextArea(csdo.shopDescription, GUILayout.Width(250), GUILayout.Height(64));
                csdo.shopkeeperPortraitTexture = (Texture2D)EditorGUILayout.ObjectField(csdo.shopkeeperPortraitTexture, typeof(Texture2D), GUILayout.Width(64), GUILayout.Height(64));
                if (EditorGUI.EndChangeCheck()) { 
                    //If this doesn't have a cshop record, we need to add one
                    EditorUtility.SetDirty(dbCommodityShops); 
                }
                EditorGUILayout.EndHorizontal();
            }

            stationIdx++;
        }

        EditorGUILayout.EndScrollView();


    }
    
    void SetFoldoutOpen(int idx)
    {
        InitializeLists();
        stationFoldoutOpen[idx] = true;
    }

    void SaveChanges()
    {

    }
}


