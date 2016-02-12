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

    dbSectorDataObject dbSectors;
    dbStationDataObject dbStations;

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

    string[] sectorNames;
    int[] sectorIDs;

    string[] stationNames;
    int[] stationIDs;

    bool isDockedInSector = false;

    Vector2 scrollPos;

    bool isEdit = false;
    int currentlySelectedMerchantID = 0;
    string saveButtonText = "CREATE";
    bool okDelete = false;


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

        Rect leftSide = EditorGUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.40f)); ;

        MerchantList();

        EditorGUILayout.EndVertical();

        Rect rightSide = EditorGUILayout.BeginVertical(GUILayout.Width((Screen.width * 0.60f)));

        MerchantEditor();

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    void MerchantList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width * 0.40f));

        EditorGUILayout.BeginVertical();

        foreach (MerchantNPCDataObject mnpc in dbMerchants.database)
        {
            if (GUILayout.Button(mnpc.MerchantLastName + ", " + mnpc.MerchantFirstName))
            {
                currentlySelectedMerchantID = mnpc.MerchantID;
                isEdit = true;
                saveButtonText = "UPDATE";
            }
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    void MerchantEditor()
    {
        if (currentlySelectedMerchantID > 0 && isEdit == true)
        {
            mnpcdo = dbMerchants.GetMerchantByID(currentlySelectedMerchantID);
        }


        //#################################################################################
        //Demographics -- Identifiers and such
        mnpcdo.MerchantFirstName = EditorGUILayout.TextField("First name", mnpcdo.MerchantFirstName, GUILayout.Width(300));
        mnpcdo.MerchantLastName = EditorGUILayout.TextField("Last name", mnpcdo.MerchantLastName, GUILayout.Width(300));
        EditorGUILayout.LabelField("Faction will go here");

        EditorGUILayout.Space();

        //#################################################################################
        //Ship -- Hull must be selected for the components to be made available.
        //  Components will only display those with proper class for the hull
        mnpcdo.HullID = EditorGUILayout.IntPopup("Ship hull", mnpcdo.HullID, hullNames, hullIDs, GUILayout.Width(350));
        if (mnpcdo.HullID > 0 ) { FilterShipComponentsByHullClasses(mnpcdo.HullID); }

        EditorGUI.BeginDisabledGroup(mnpcdo.HullID == 0);
        mnpcdo.EngineID = EditorGUILayout.IntPopup("Engine", mnpcdo.EngineID, engineNames, engineIDs, GUILayout.Width(350));
        mnpcdo.CargoID = EditorGUILayout.IntPopup("Cargo hold", mnpcdo.CargoID, cargoNames, cargoIDs, GUILayout.Width(350));
        mnpcdo.ShieldID = EditorGUILayout.IntPopup("Shields", mnpcdo.ShieldID, shieldNames, shieldIDs, GUILayout.Width(350));
        mnpcdo.PlatingID = EditorGUILayout.IntPopup("Plating", mnpcdo.PlatingID, platingNames, platingIDs, GUILayout.Width(350));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        //#################################################################################
        //Starting values

        EditorGUI.BeginChangeCheck();
        mnpcdo.CurrentSectorID = EditorGUILayout.IntPopup("Starting sector", mnpcdo.CurrentSectorID, sectorNames, sectorIDs, GUILayout.Width(300));
        if (mnpcdo.CurrentSectorID > 0 ) { FilterStationsBySectorID(mnpcdo.CurrentSectorID); }

        if (mnpcdo.CurrentStationID > 0) { isDockedInSector = true; }
        isDockedInSector = EditorGUILayout.Toggle("Docked?", isDockedInSector);
        EditorGUI.BeginDisabledGroup(!isDockedInSector);
        mnpcdo.CurrentStationID = EditorGUILayout.IntPopup("Docked at", mnpcdo.CurrentStationID, stationNames, stationIDs, GUILayout.Width(300));
        if (!isDockedInSector) { mnpcdo.CurrentStationID = 0; }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        mnpcdo.MerchantTickModifier = EditorGUILayout.FloatField("Sim tick modifier", mnpcdo.MerchantTickModifier, GUILayout.Width(200));

        //#################################################################################
        //Buttons -- Save will change to either CREATE or UPDATE depending on how we started
        //  CREATE just needs an empty form. UPDATE requires a sidebar selection.
        //  CLEAR will empty all memorized values

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button(saveButtonText, GUILayout.Width(125))) {
            GUIUtility.keyboardControl = 0;
            if (isEdit == true)
            {
                UpdateRecord();
            }
            else
            {
                SaveRecord();
            }

            ResetForm();
        }
        if (GUILayout.Button("Cancel", GUILayout.Width(125))) { ResetForm(); }

        EditorGUILayout.EndHorizontal();

        //###############################################################################################
        // Delete this station and child shops

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        okDelete = EditorGUILayout.BeginToggleGroup("Delete the merchant?", okDelete);

        //Sanity check(mark)
        if (GUILayout.Button("Delete", GUILayout.Width(125)) && currentlySelectedMerchantID > 0)
        {
            DeleteRecord();

            okDelete = false;
        }

        EditorGUILayout.EndToggleGroup();

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    #region Database

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

        dbSectors = (dbSectorDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbSectorDataItems.asset", typeof(dbSectorDataObject));
        dbStations = (dbStationDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbStationDataItems.asset", typeof(dbStationDataObject));
    }

    void Initialize()
    {
        mnpcdo = new MerchantNPCDataObject();

        hullNames = dbHulls.database.Select(x => x.name).ToArray();
        hullIDs = dbHulls.database.Select(x => x.iD).ToArray();

        engineNames = new string[] { "None" };
        engineIDs = new int[] { 0 };

        cargoNames = new string[] { "None" };
        cargoIDs = new int[] { 0 };

        shieldNames = new string[] { "None" };
        shieldIDs = new int[] { 0 };

        platingNames = new string[] { "None" };
        platingIDs = new int[] { 0 };

        sectorNames = dbSectors.database.Select(x => x.sectorName).ToArray();
        sectorIDs = dbSectors.database.Select(x => x.sectorID).ToArray();

        stationNames = new string[] { "None" };
        stationIDs = new int[] { 0 };
    }

    void CreateDatabase()
    {
        dbMerchants = ScriptableObject.CreateInstance<dbMerchantNPCDataObject>();
        AssetDatabase.CreateAsset(dbMerchants, DATABASE_PATH + "dbMerchantDataItems.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void CreateRecord()
    {
        currentlySelectedMerchantID = dbMerchants.database.Count > 0 ? dbMerchants.database.Max(x => x.MerchantID) + 1 : 1;
        mnpcdo = new MerchantNPCDataObject();
        mnpcdo.MerchantID = currentlySelectedMerchantID;
        mnpcdo.MerchantFirstName = "New";
        mnpcdo.MerchantLastName = "Merchant";
        mnpcdo.MerchantGender = "Undetermined";
        mnpcdo.MerchantFaction = 0;

        mnpcdo.HullID = 0;
        mnpcdo.EngineID = 0;
        mnpcdo.CargoID = 0;
        mnpcdo.ShieldID = 0;
        mnpcdo.PlatingID = 0;

        mnpcdo.IsDestroyed = false;
    }

    void UpdateRecord()
    {
        EditorUtility.SetDirty(dbMerchants);
    }

    void SaveRecord()
    {
        mnpcdo.MerchantID = dbMerchants.database.Count > 0 ? dbMerchants.database.Max(x => x.MerchantID) + 1 : 1;
        dbMerchants.Add(mnpcdo);
        UpdateRecord();
    }

    void DeleteRecord()
    {
        int idxRecord;

        //Find the object in the database
        MerchantNPCDataObject tempmnpcdo = dbMerchants.database.Find(x => x.MerchantID.Equals(currentlySelectedMerchantID));

        //Get the index
        idxRecord = dbMerchants.database.IndexOf(tempmnpcdo);
        
        //Remove by index
        dbMerchants.database.RemoveAt(idxRecord);

        //Update the SOs
        EditorUtility.SetDirty(dbStations);

        ResetForm();
    }

    void FilterShipComponentsByHullClasses(int HullID)
    {
        //http://www.devcurry.com/2009/05/replicating-in-operator-in-linq.html
        //We need to allow for items that are the same class or less than the 
        //  hull specifies for the class of that slot.
        //Quality ranges from WORST (1) to BEST (5)



        //Use the HullID to get the Hull record, and use the
        //  hull record to get the classes we can use with that hull.
        HullDataObject hdo = dbHulls.database.Find(x => x.iD.Equals(HullID));

        List<EngineDataObject> okEngines = (from db in dbEngines.database where db.hardwareClass <= hdo.engineClass select db).OrderByDescending(x=>x.hardwareClass).ToList<EngineDataObject>();
        List<CargoDataObject> okCargo = (from db in dbCargo.database where db.hardwareClass <= hdo.cargoClass select db).OrderByDescending(x => x.hardwareClass).ToList<CargoDataObject>();
        List<ShieldDataObject> okShields = (from db in dbShields.database where db.hardwareClass <= hdo.shieldClass select db).OrderByDescending(x => x.hardwareClass).ToList<ShieldDataObject>();
        List<PlatingDataObject> okPlating = (from db in dbPlating.database where db.hardwareClass <= hdo.platingClass select db).OrderByDescending(x => x.hardwareClass).ToList<PlatingDataObject>();      

        engineNames = okEngines.Select(x => "["+ x.hardwareClass + "] " + x.name + " (T:"+ x.forwardThrust.ToString() + ")").ToArray();
        engineIDs = okEngines.Select(x => x.iD).ToArray();
        engineNames.Union(emptyName);
        engineIDs.Union(emptyID);

        cargoNames = okCargo.Select(x => "[" + x.hardwareClass + "] " + x.name + " (C:" + x.capacity + ")").ToArray();
        cargoIDs = okCargo.Select(x => x.iD).ToArray();
        cargoNames.Union(emptyName);
        cargoIDs.Union(emptyID);

        shieldNames = okShields.Select(x => "[" + x.hardwareClass + "] " + x.name + " (S:" + x.Strength + ")").ToArray();
        shieldIDs = okShields.Select(x => x.iD).ToArray();
        shieldNames.Union(emptyName);
        shieldIDs.Union(emptyID);

        platingNames = okPlating.Select(x => "[" + x.hardwareClass + "] " + x.name + " (S:" + x.Strength + ")").ToArray();
        platingIDs = okPlating.Select(x => x.iD).ToArray();
        platingNames.Union(emptyName);
        platingIDs.Union(emptyID);
    }

    void FilterStationsBySectorID(int SectorID)
    {
        List<StationDataObject> okStations = (from db in dbStations.database where db.sectorID.Equals(SectorID) select db).ToList<StationDataObject>();
        if (okStations.Count > 0) { 
            stationNames = okStations.Select(x => x.stationName).ToArray();
            stationIDs = okStations.Select(x => x.stationID).ToArray();
        }
        stationNames.Union(emptyName);
        stationIDs.Union(emptyID);
    }

    #endregion

    #region Utility

    void ResetForm()
    {
        isEdit = false;
        currentlySelectedMerchantID = 0;
        saveButtonText = "CREATE";
        isDockedInSector = false;
        mnpcdo = new MerchantNPCDataObject();
    }

    #endregion

}
