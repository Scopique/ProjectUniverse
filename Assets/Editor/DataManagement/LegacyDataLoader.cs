using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class LegacyDataLoader : EditorWindow {

    static LegacyDataLoader editorWindow;

    MasterDatabase md;

    private dbCannonDataObject dbCannons;
    private dbCargoModuleDataObject dbCargo;
    //private dbCommodityDataObject dbCommodity;
    private dbCrewDataObject dbCrew;
    private dbEngineDataObject dbEngines;
    private dbFighterBayDataObject dbFighterBays;
    private dbHullDataObject dbHulls;
    private dbJumpgateDataObject dbJumpgates;
    private dbMissileLauncherDataObject dbMissileLaunchers;
    //private dbNPCDataObject dbNPCs;
    private dbPlatingDataObject dbPlating;
    private dbScannerDataObject dbScanners;
    private dbSectorDataObject dbSectors;
    private dbShieldDataObject dbShields;
    private dbStationDataObject dbStations;


    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/";

    //[MenuItem("Data Management/Legacy Data Load")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<LegacyDataLoader>();
        editorWindow.Show();
    }

    void OnEnable()
    {
        md = new MasterDatabase();
        md.LoadMasterData();

        LoadDatabases();

        PopulateDatabases();
    }


    void LoadDatabases()
    {
        dbCannons = (dbCannonDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCannonDataItems.asset", typeof(dbCannonDataObject));
        dbCargo = (dbCargoModuleDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCargoModuleDataItems.asset", typeof(dbCargoModuleDataObject));
        //dbCommodity = (dbCommodityDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCommodityDataItems.asset", typeof(dbCommodityDataObject));
        dbCrew = (dbCrewDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbCrewDataItems.asset", typeof(dbCrewDataObject));
        dbEngines = (dbEngineDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbEngineDataItems.asset", typeof(dbEngineDataObject));
        dbFighterBays = (dbFighterBayDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbFighterBayDataItems.asset", typeof(dbFighterBayDataObject));
        dbHulls = (dbHullDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbHullDataItems.asset", typeof(dbHullDataObject));
        dbJumpgates = (dbJumpgateDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbJumpgateDataItems.asset", typeof(dbJumpgateDataObject));
        dbMissileLaunchers = (dbMissileLauncherDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbMissileLauncherDataItems.asset", typeof(dbMissileLauncherDataObject));
        //dbNPCs = (dbNPCDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbNPCDataItems.asset", typeof(dbNPCDataObject));
        dbPlating = (dbPlatingDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbPlatingDataItems.asset", typeof(dbPlatingDataObject));
        dbScanners = (dbScannerDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbScannerDataItems.asset", typeof(dbScannerDataObject));
        dbSectors = (dbSectorDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbSectorDataItems.asset", typeof(dbSectorDataObject));
        dbShields = (dbShieldDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbShieldDataItems.asset", typeof(dbShieldDataObject));
        dbStations = (dbStationDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + "dbStationDataItems.asset", typeof(dbStationDataObject));

        if (dbCannons == null)
        {
            dbCannons = ScriptableObject.CreateInstance<dbCannonDataObject>();
            AssetDatabase.CreateAsset(dbCannons, DATABASE_PATH + "dbCannonDataItems.asset");
        }

        if (dbCargo == null)
        {
            dbCargo = ScriptableObject.CreateInstance<dbCargoModuleDataObject>();
            AssetDatabase.CreateAsset(dbCargo, DATABASE_PATH + "dbCargoModuleDataItems.asset");
        }

        //if (dbCommodity == null)
        //{
        //    dbCommodity = ScriptableObject.CreateInstance<dbCommodityDataObject>();
        //    AssetDatabase.CreateAsset(dbCommodity, DATABASE_PATH + "dbCommodityDataItems.asset");
        //}

        if (dbCrew == null)
        {
            dbCrew = ScriptableObject.CreateInstance<dbCrewDataObject>();
            AssetDatabase.CreateAsset(dbCrew, DATABASE_PATH + "dbCrewDataItems.asset");
        }

        if (dbEngines == null)
        {
            dbEngines = ScriptableObject.CreateInstance<dbEngineDataObject>();
            AssetDatabase.CreateAsset(dbEngines, DATABASE_PATH + "dbEngineDataItems.asset");
        }

        if (dbFighterBays == null)
        {
            dbFighterBays = ScriptableObject.CreateInstance<dbFighterBayDataObject>();
            AssetDatabase.CreateAsset(dbFighterBays, DATABASE_PATH + "dbFighterBayDataItems.asset");
        }

        if (dbHulls == null)
        {
            dbHulls = ScriptableObject.CreateInstance<dbHullDataObject>();
            AssetDatabase.CreateAsset(dbHulls, DATABASE_PATH + "dbHullDataItems.asset");
        }

        if (dbJumpgates == null)
        {
            dbJumpgates = ScriptableObject.CreateInstance<dbJumpgateDataObject>();
            AssetDatabase.CreateAsset(dbJumpgates, DATABASE_PATH + "dbJumpgateDataItems.asset");
        }

        if (dbMissileLaunchers == null)
        {
            dbMissileLaunchers = ScriptableObject.CreateInstance<dbMissileLauncherDataObject>();
            AssetDatabase.CreateAsset(dbMissileLaunchers, DATABASE_PATH + "dbMissileLauncherDataItems.asset");
        }

        //if (dbNPCs == null)
        //{
        //    dbNPCs = ScriptableObject.CreateInstance<dbNPCDataObject>();
        //    AssetDatabase.CreateAsset(dbNPCs, DATABASE_PATH + "dbNPCDataItems.asset");
        //}

        if (dbPlating == null)
        {
            dbPlating = ScriptableObject.CreateInstance<dbPlatingDataObject>();
            AssetDatabase.CreateAsset(dbPlating, DATABASE_PATH + "dbPlatingDataItems.asset");
        }

        if (dbScanners == null)
        {
            dbScanners = ScriptableObject.CreateInstance<dbScannerDataObject>();
            AssetDatabase.CreateAsset(dbScanners, DATABASE_PATH + "dbScannerDataItems.asset");
        }

        if (dbSectors == null)
        {
            dbSectors = ScriptableObject.CreateInstance<dbSectorDataObject>();
            AssetDatabase.CreateAsset(dbSectors, DATABASE_PATH + "dbSectorDataItems.asset");
        }

        if (dbShields == null)
        {
            dbShields = ScriptableObject.CreateInstance<dbShieldDataObject>();
            AssetDatabase.CreateAsset(dbShields, DATABASE_PATH + "dbShieldDataItems.asset");
        }

        if (dbStations == null)
        {
            dbStations = ScriptableObject.CreateInstance<dbStationDataObject>();
            AssetDatabase.CreateAsset(dbStations, DATABASE_PATH + "dbStationDataItems.asset");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    void PopulateDatabases()
    {
        
        

        dbCannons.database = md.tempCannonMasterList;
        dbCargo.database = md.tempCargoHoldMasterList;
        //dbCommodity.database = md.tempcommodityMasterList;
        dbCrew.database = md.tempCrewList;
        dbEngines.database = md.tempEngineMasterList;
        dbFighterBays.database = md.tempFighterBayMasterList;
        dbHulls.database = md.tempHullMasterList;
        dbJumpgates.database = md.tempJumpgateMasterList;
        dbMissileLaunchers.database = md.tempMissileLauncherMasterList;
        dbPlating.database = md.tempPlatingMasterList;
        dbScanners.database = md.tempScannerMasterList;
        dbSectors.database = md.tempSectorMasterList;
        dbShields.database = md.tempShieldMasterList;
        dbStations.database = md.tempStationMasterList;

        EditorUtility.SetDirty(dbCannons);
        EditorUtility.SetDirty(dbCargo);
        //EditorUtility.SetDirty(dbCommodity);
        EditorUtility.SetDirty(dbCrew);
        EditorUtility.SetDirty(dbEngines);
        EditorUtility.SetDirty(dbFighterBays);
        EditorUtility.SetDirty(dbHulls);
        EditorUtility.SetDirty(dbJumpgates);
        EditorUtility.SetDirty(dbMissileLaunchers);
        EditorUtility.SetDirty(dbPlating);
        EditorUtility.SetDirty(dbScanners);
        EditorUtility.SetDirty(dbSectors);
        EditorUtility.SetDirty(dbShields);
        EditorUtility.SetDirty(dbStations);
    }



}
