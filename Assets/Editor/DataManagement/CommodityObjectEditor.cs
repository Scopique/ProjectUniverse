using UnityEngine;
using UnityEditor;
using System.Collections;

public class CommodityObjectEditor : EditorWindow {

    private const string DATABASE_PATH = @"Assets/AssetDatabases";

    private dbCommodityDataObject database;

    private int selectedIdx = -1;       //-1 means nothing selected
    private int editID;
    private string editName;
    private int editBasePrice;
    private int editCurrentPrice;
    private int editQuantity;
    private CommodityDataObject.COMMODITYCLASS editClass;

    private Vector2 scrollPos;


    [MenuItem("Data Management/Commodity Items")]
    public static void Init()
    {
        CommodityObjectEditor editor = EditorWindow.GetWindow<CommodityObjectEditor>();
        //editor.minSize = new Vector2(800, 400);
        editor.Show();
    }

    void OnEnable()
    {
        if (database == null)
            LoadDatabase();
    }

    void OnGUI()
    {

    }


    void CreateDatabase()
    {
        database = ScriptableObject.CreateInstance<dbCommodityDataObject>();
        AssetDatabase.CreateAsset(database, DATABASE_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void LoadDatabase()
    { 
        database = (dbCommodityDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(dbCommodityDataObject));
        if (database == null)
            CreateDatabase();
    }
}
