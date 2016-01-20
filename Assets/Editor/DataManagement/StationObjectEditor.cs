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

    bool foldStation = false;
    bool foldCShop = false;

    GUISkin editorSkin;
    GUIStyle frame;
    GUIStyle linkButton;

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
        InitializeLists();
    }

    void OnGUI()
    {
        GUI.skin = editorSkin;
        frame = editorSkin.GetStyle("Frame");
        linkButton = editorSkin.GetStyle("LinkeButton");

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

        editorSkin = (GUISkin)AssetDatabase.LoadAssetAtPath("Assets/Editor/DataManagement/_skins/EditorSkin.guiskin", typeof(GUISkin));
    }

    private void InitializeLists()
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
    }

    private void DisplayActionButtons()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(300));
        if (GUILayout.Button("Refresh"))
        {
            LoadResources();
            InitializeLists();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private void DisplayFrame()
    {

        EditorGUILayout.BeginHorizontal();

        Rect leftSide = EditorGUILayout.BeginVertical(frame, GUILayout.Width(Screen.width * 0.25f)); ;

        StationList();

        EditorGUILayout.EndVertical();

        Rect rightSide = EditorGUILayout.BeginVertical(frame,GUILayout.Width((Screen.width * 0.75f) - 30));

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
            if (GUILayout.Button(sdo.stationID + " - " + sdo.stationName, linkButton))
            {
                currentlySelectedStationID = sdo.stationID;
                
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void EditorPopulation()
    {
        if (currentlySelectedStationID > 0)
        {
            StationDataObject sdo = dbStations.GetStationByID(currentlySelectedStationID);
            CommodityShopDataObject csdo = dbCommodityShops.GetCommodityShopByStation(currentlySelectedStationID);

            

            //###############################################################################################
            //Main station input form
            foldStation = EditorGUILayout.Foldout(foldStation, "Station");
            if (foldStation) { 
                //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
                EditorGUI.BeginChangeCheck();
                sdo.stationName = EditorGUILayout.TextField("Name", sdo.stationName, GUILayout.Width(400));
                sdo.sectorID = EditorGUILayout.IntField("Sector", sdo.sectorID, GUILayout.Width(200));
                sdo.stationPosition = EditorGUILayout.Vector3Field("Position", sdo.stationPosition, GUILayout.Width(400));
                if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(dbStations); }
            }

            EditorGUILayout.Space();

            foldCShop = EditorGUILayout.Foldout(foldCShop, "Commodity Shop");
            if (foldCShop) { 
                
                //###############################################################################################
                //Commodity market input form

                //Change check: if anything in this block changes, we need to mark the DB as dirty for update.
                EditorGUI.BeginChangeCheck();
                csdo.shopName = EditorGUILayout.TextField("Shop Name", csdo.shopName, GUILayout.Width(350));
                csdo.shopDescription = EditorGUILayout.TextArea(csdo.shopDescription, GUILayout.Width(350), GUILayout.Height(64));
                csdo.shopkeeperPortraitTexture = (Texture2D)EditorGUILayout.ObjectField(csdo.shopkeeperPortraitTexture, typeof(Texture2D), false, GUILayout.Height(64), GUILayout.Width(64));
                if (EditorGUI.EndChangeCheck()) { 
                    //If this doesn't have a cshop record, we need to add one
                    EditorUtility.SetDirty(dbCommodityShops); 
                }
            }
        }
    }

    void SaveChanges()
    {

    }
}


