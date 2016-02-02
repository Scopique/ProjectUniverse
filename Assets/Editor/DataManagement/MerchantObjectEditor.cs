using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class MerchantObjectEditor : EditorWindow {

    static MerchantObjectEditor editorWindow;
   
    //Unlike other editors, this value does not contain the .asset filename
    //  because we're going to try and work with several at once.
    const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/";

    dbMerchantNPCDataObject dbMerchants;
    dbHullDataObject dbHulls;
    dbEngineDataObject dbEngines;
    dbCargoModuleDataObject dbCargo;
    dbShieldDataObject dbShields;
    dbPlatingDataObject dbPlating;

    MerchantNPCDataObject mnpcdo;

    string[] genderOptions = new string[] { "Male", "Female", "Undetermined" };
    int genderIndex = 0;

    string[] emptyName = new string[] { "None" };
    int[] emptyID = new int[] { 0 };

    string[] hullNames;
    int[] hullIDs;

    string[] engineNames;
    int[] engineIDs;

    string[] cargoNames;
    int[] cargoIDs;

    string[] shieldNames;
    int[] shieldIDs;

    string[] platingNames;
    int[] platingIDs;

    Vector2 scrollPos;

    bool isEdit = true;
    int currentlySelectedMerchantID = 0;


    [MenuItem("Data Management/Merchants NPCs")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<MerchantObjectEditor>();
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

    #region Form Elements

    void DisplayActionButtons()
    {
        EditorGUILayout.Space();
    }

    void DisplayFrame()
    {

        EditorGUILayout.BeginHorizontal();

        Rect leftSide = EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.25f)); ;

        MerchantList();

        EditorGUILayout.EndVertical();

        Rect rightSide = EditorGUILayout.BeginVertical(GUILayout.Width((Screen.width * 0.75f) - 30));

        MerchantEditor();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    void MerchantList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width * 0.25f));

        EditorGUILayout.BeginVertical();

        foreach (MerchantNPCDataObject mnpc in dbMerchants.database)
        {
            if (GUILayout.Button(mnpc.MerchantLastName + ", " + mnpc.MerchantFirstName)) ;
            {
                currentlySelectedMerchantID = mnpc.MerchantID;

            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    void MerchantEditor()
    {
        if (GUILayout.Button("New Merchant NPC"))
        {

        }

        if (currentlySelectedMerchantID > 0 )
        {
            mnpcdo = dbMerchants.GetMerchantByID(currentlySelectedMerchantID);
        }

        EditorGUI.BeginDisabledGroup(isEdit == true || currentlySelectedMerchantID > 0);

        //#################################################################################
        //Demographics
        mnpcdo.MerchantFirstName = EditorGUILayout.TextField("First name", mnpcdo.MerchantFirstName, GUILayout.Width(300));
        mnpcdo.MerchantLastName = EditorGUILayout.TextField("Last name", mnpcdo.MerchantLastName, GUILayout.Width(300));
        if (mnpcdo.MerchantGender != null) { genderIndex = (from go in genderOptions select go.IndexOf(mnpcdo.MerchantGender)).First(); }
        genderIndex = EditorGUILayout.Popup("Gender", genderIndex, genderOptions, GUILayout.Width(300));
        EditorGUILayout.LabelField("Faction will go here");

        EditorGUILayout.Space();

        //Here we'll have the ship builder. Everything is based on the values of the 
        //  selected hull, so the Hull drop down needs to be active when the main form
        //  is active, and other drop downs are inactive until a valid hull is selected.
        mnpcdo.HullID = EditorGUILayout.IntPopup("Ship hull", mnpcdo.HullID, hullNames, hullIDs, GUILayout.Width(300));

        EditorGUI.BeginDisabledGroup(mnpcdo.HullID > 0);

        EditorGUI.BeginChangeCheck();
        mnpcdo.EngineID = EditorGUILayout.IntPopup("Engine", mnpcdo.HullID, hullNames, hullIDs, GUILayout.Width(300));
        if (EditorGUI.EndChangeCheck())
        {
            FilterShipComponentsByHullClasses(mnpcdo.HullID);
        }
        //Arrays are null. Fill them with something to start out with
        mnpcdo.CargoID = EditorGUILayout.IntPopup("Cargo hold", mnpcdo.CargoID, cargoNames, cargoIDs, GUILayout.Width(300));
        mnpcdo.ShieldID = EditorGUILayout.IntPopup("Shields", mnpcdo.ShieldID, shieldNames, shieldIDs, GUILayout.Width(300));
        mnpcdo.PlatingID = EditorGUILayout.IntPopup("Plating", mnpcdo.PlatingID, platingNames, platingIDs, GUILayout.Width(300));

        EditorGUI.EndDisabledGroup();


        EditorGUI.EndDisabledGroup();
    }

    #endregion

    #region Loading

    void LoadResources()
    {
        //dbMerchant won't be available to start
        dbMerchants = (dbMerchantNPCDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbMerchantDataItems.asset", typeof(dbMerchantNPCDataObject));
        if (dbMerchants == null) { CreateDatabase(); }
        dbHulls = (dbHullDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbHullDataItems.asset", typeof(dbHullDataObject));
        dbEngines = (dbEngineDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbEngineDataItems.asset", typeof(dbEngineDataObject));
        dbCargo = (dbCargoModuleDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCargoModuleDataItems.asset", typeof(dbCargoModuleDataObject));
        dbShields = (dbShieldDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbShieldDataItems.asset", typeof(dbShieldDataObject));
        dbPlating = (dbPlatingDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbPlatingDataItems.asset", typeof(dbPlatingDataObject));
    }

    void Initialize()
    {
        mnpcdo = new MerchantNPCDataObject();

        hullNames = dbHulls.database.Select(x => x.name).ToArray();
        hullIDs = dbHulls.database.Select(x => x.iD).ToArray();
    }

    void CreateDatabase()
    {
        dbMerchants = ScriptableObject.CreateInstance<dbMerchantNPCDataObject>();
        AssetDatabase.CreateAsset(dbMerchants, DATABASE_PATH + "dbMerchantDataItems.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void FilterShipComponentsByHullClasses(int HullID)
    {
        //Use the HullID to get the Hull record, and use the
        //  hull record to get the classes we can use with that hull.
        HullDataObject hdo = dbHulls.database.Find(x => x.iD.Equals(HullID));

        List<EngineDataObject> okEngines = (from db in dbEngines.database where db.hardwareClass.Equals(hdo.engineClass) select db).ToList<EngineDataObject>();
        List<CargoDataObject> okCargo = (from db in dbCargo.database where db.hardwareClass.Equals(hdo.cargoClass) select db).ToList<CargoDataObject>();
        List<ShieldDataObject> okShields = (from db in dbShields.database where db.hardwareClass.Equals(hdo.shieldClass) select db).ToList<ShieldDataObject>();
        List<PlatingDataObject> okPlating = (from db in dbPlating.database where db.hardwareClass.Equals(hdo.platingClass) select db).ToList<PlatingDataObject>();      

        engineNames = okEngines.Select(x => x.name).ToArray();
        engineIDs = okEngines.Select(x => x.iD).ToArray();
        engineNames.Union(emptyName);
        engineIDs.Union(emptyID);

        cargoNames = okCargo.Select(x => x.name).ToArray();
        cargoIDs = okCargo.Select(x => x.iD).ToArray();
        cargoNames.Union(emptyName);
        cargoIDs.Union(emptyID);

        shieldNames = okShields.Select(x => x.name).ToArray();
        shieldIDs = okShields.Select(x => x.iD).ToArray();
        shieldNames.Union(emptyName);
        shieldIDs.Union(emptyID);

        platingNames = okPlating.Select(x => x.name).ToArray();
        platingIDs = okPlating.Select(x => x.iD).ToArray();
        platingNames.Union(emptyName);
        platingIDs.Union(emptyID);
    }

    #endregion

}
