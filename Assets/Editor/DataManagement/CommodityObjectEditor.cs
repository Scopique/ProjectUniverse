using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CommodityObjectEditor : EditorWindow {

    static CommodityObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/dbCommodityDataItems.asset";

    private dbCommodityDataObject db;

    private bool enableEditArea = false;     //Lock it down until NEW or EDIT
    private bool isEdit = false;            //Is this an edit as opposed to a new record?

    private int editID = 0;
    private Texture2D editImage = new Texture2D(64,64);
    private string editName = string.Empty;
    private int editBasePrice = 0;
    private int editCurrentPrice = 0;
    private int editQuantity = 0;
    private CommodityDataObject.COMMODITYCLASS editClass = CommodityDataObject.COMMODITYCLASS.Common;

    private CommodityDataObject.COMMODITYCLASS filterClass = CommodityDataObject.COMMODITYCLASS.Common;

    private Vector2 scrollPos;


    [MenuItem("Data Management/Commodity Items")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<CommodityObjectEditor>();
        //editor.minSize = new Vector2(800, 400);
        editorWindow.Show();
    }

    void OnEnable()
    {
        if (db == null)
            LoadDatabase();
    }

    public void OnGUI()
    {
        //A grid of existing items. Has DEL and EDIT buttons
        //A button to create a NEW item
        //A form that allows for the editing/adding

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        if (GUILayout.Button("Add New Commodity", GUILayout.Width(200)))
        {
            NewCommodity();
        }
        filterClass = CommodityClassPopup("Filter by class:", filterClass, GUILayout.Width(175));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        DisplayListAreaHeader();
        
        scrollPos = EditorGUILayout.BeginScrollView( scrollPos,GUILayout.MaxWidth(Screen.width), GUILayout.Height(Screen.height - 150));
        DisplayListArea();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        DisplayEditArea();
        
    }


    void CreateDatabase()
    {
        db = ScriptableObject.CreateInstance<dbCommodityDataObject>();
        AssetDatabase.CreateAsset(db, DATABASE_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void LoadDatabase()
    { 
        db = (dbCommodityDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(dbCommodityDataObject));
        if (db == null)
            CreateDatabase();
    }


    private void DisplayListAreaHeader()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        EditorGUILayout.LabelField("IMG", GUILayout.Width(32));
        EditorGUILayout.LabelField("ID", GUILayout.Width(75));
        EditorGUILayout.LabelField("NAME", GUILayout.Width(200));
        EditorGUILayout.LabelField("BASE PRICE", GUILayout.Width(125));
        EditorGUILayout.LabelField("CLASS", GUILayout.Width(175));
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayListArea()
    {
        int rowIdx = 1;

        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.1f;
        float cellClass = Screen.width * 0.15f;

        List<CommodityDataObject> filterList = (from assetDB in db.database where assetDB.commodityClass.Equals(filterClass) select assetDB).ToList<CommodityDataObject>();

        //for (int cnt = 0; cnt < filterList.Count; cnt++)
        foreach(CommodityDataObject cdo in filterList)
        {
            Texture2D img = new Texture2D(32, 32);

            if (cdo.commodityImage != null)
            {
                img = AssetPreview.GetAssetPreview(cdo.commodityImage);
            }

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            GUILayout.Label(img, GUILayout.Height(32), GUILayout.Width(32));
            //EditorGUI.DrawPreviewTexture(new Rect(10, 32 *  rowPos, 32, 32), cdo.commodityImage);
            EditorGUILayout.LabelField(cdo.commodityID.ToString(), GUILayout.Width(75));
            EditorGUILayout.LabelField(cdo.commodityName, GUILayout.Width(200));
            EditorGUILayout.LabelField(cdo.commodityBasePrice.ToString(), GUILayout.Width(125));
            EditorGUILayout.LabelField(cdo.commodityClass.ToString(), GUILayout.Width(175));

            if (GUILayout.Button("Edit", GUILayout.Width(100)))
            {
                //Copy this info to the edit form
                enableEditArea = true;

                isEdit = true;
                editImage = cdo.commodityImage;
                editID = int.Parse(cdo.commodityID.ToString());
                editName = cdo.commodityName.ToString();
                editBasePrice = int.Parse(cdo.commodityBasePrice.ToString());
                editClass = cdo.commodityClass;
            }
            if (GUILayout.Button("Del", GUILayout.Width(100)))
            {
                //Remove this from the array
                db.Remove(cdo);
                isEdit = false;
            }

            EditorGUILayout.EndHorizontal();

            rowIdx += 1;
            
        }
    }

    private void DisplayEditArea()
    {


        EditorGUI.BeginDisabledGroup(enableEditArea == false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        editImage = (Texture2D)EditorGUILayout.ObjectField(editImage, typeof(Texture2D), false, GUILayout.Height(64), GUILayout.Width(64));
        editID = int.Parse(TextField("ID:", editID.ToString(), GUILayout.Width(75)));
        editName = TextField("Name:", editName, GUILayout.Width(200)).ToString();
        editBasePrice = int.Parse(TextField("Base Price:", editBasePrice.ToString(), GUILayout.Width(125)));
        editClass = CommodityClassPopup("Class:", editClass, GUILayout.Width(175));

        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            //Save this, either as a new item, or an edit
            if (isEdit)
            {
                CommodityDataObject cib = db.GetCommodity(editID);
                cib.commodityImage = editImage;
                cib.commodityName = editName;
                cib.commodityBasePrice = int.Parse(editBasePrice.ToString());
                cib.commodityClass = editClass;
                EditorUtility.SetDirty(db);
            }
            else
            {
                CommodityDataObject cib = new CommodityDataObject(editID, editName, editBasePrice, 0, 0, editImage, editClass);
                db.Add(cib);
                EditorUtility.SetDirty(db);
            }

            //Clear and disable the editor section
            ResetForm();
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(100)))
        {
            ResetForm();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUI.EndDisabledGroup();
    }



    private void NewCommodity()
    {
        enableEditArea = true;

        isEdit = false;
        editImage = null;
        editID = db.Count > 0 ? (db.Count) : 0;
        editName = "New commodity";
        editBasePrice = 0;
        editClass = CommodityDataObject.COMMODITYCLASS.Common;

        //Enable the editor section
    }

    private void ResetForm()
    {
        enableEditArea = false; 

        isEdit = false;
        editImage = null;
        editID = 0;
        editName = string.Empty;
        editBasePrice = 0;
        editClass = CommodityDataObject.COMMODITYCLASS.Common;
    }

    #region Helpers
    
    /// <summary>
    /// Create a EditorGUILayout.TextField with no space between label and text field
    /// </summary>
    public static string TextField(string label, string text, params GUILayoutOption[] args)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.TextField(label, text, args);
    }

    public static CommodityDataObject.COMMODITYCLASS CommodityClassPopup(string label, CommodityDataObject.COMMODITYCLASS commodityClass, params GUILayoutOption[] args)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return (CommodityDataObject.COMMODITYCLASS)EditorGUILayout.EnumPopup(label, commodityClass, args);
    }

    #endregion
}
