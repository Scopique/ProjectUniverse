using UnityEngine;
using UnityEditor;
using System.Collections;

public class CommodityObjectEditor : EditorWindow {

    static CommodityObjectEditor editorWindow;


    private const string DATABASE_PATH = @"Assets/AssetDatabases/dbCommodityDataItems.asset";

    private dbCommodityDataObject database;


    private bool isEdit = false;        //Is this an edit as opposed to a new record?
    private int selectedIdx = -1;       //-1 means nothing selected
    private int editID = 0;
    private string editName = string.Empty;
    private int editBasePrice = 0;
    private int editCurrentPrice = 0;
    private int editQuantity = 0;
    private CommodityDataObject.COMMODITYCLASS editClass = CommodityDataObject.COMMODITYCLASS.Common;

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
        if (database == null)
            LoadDatabase();
    }

    public void OnGUI()
    {
        //A grid of existing items. Has DEL and EDIT buttons
        //A button to create a NEW item
        //A form that allows for the editing/adding

        if (GUILayout.Button("Add New Commodity"))
        {
            NewCommodity();
        }
        EditorGUILayout.Space();

        DisplayListAreaHeaderLayout();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandWidth(true), GUILayout.Height(150));
        DisplayListAreaLayout();
        //DisplayListAreaRect();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();
        DisplayEditAreaLayout();
        //DisplayEditAreaRect();




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

    #region Layout Methods


    private void DisplayListAreaHeaderLayout()
    {
        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.1f;
        float cellClass = Screen.width * 0.15f;

        Rect row = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.SelectableLabel("ID", GUILayout.Width(cellID));
        EditorGUILayout.SelectableLabel("NAME", GUILayout.Width(cellName));
        EditorGUILayout.SelectableLabel("BASE PRICE", GUILayout.Width(cellBasePrice));
        EditorGUILayout.SelectableLabel("CLASS", GUILayout.Width(cellClass));
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayListAreaLayout()
    {


        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.1f;
        float cellClass = Screen.width * 0.15f;

        //Debug.Log(cellID.ToString() + "-" + cellName.ToString() + "-" + cellBasePrice.ToString() + "-" + cellClass.ToString());


        for (int cnt = 0; cnt < database.Count; cnt++)
        {
            Rect row = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));


            EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityID.ToString(), GUILayout.Width(cellID));
            EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityName, GUILayout.Width(cellName));
            EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityBasePrice.ToString(), GUILayout.Width(cellBasePrice));
            EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityClass.ToString(), GUILayout.Width(cellClass));

            //EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityID.ToString());
            //EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityName);
            //EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityBasePrice.ToString());
            //EditorGUILayout.SelectableLabel(database.Commodity(cnt).commodityClass.ToString());

            if (GUILayout.Button("Edit", GUILayout.Width(100)))
            {
                //Copy this info to the edit form
                isEdit = true;
                selectedIdx = cnt;
                editID = int.Parse(database.Commodity(cnt).commodityID.ToString());
                editName = database.Commodity(cnt).commodityName.ToString();
                editBasePrice = int.Parse(database.Commodity(cnt).commodityBasePrice.ToString());
                editClass = database.Commodity(cnt).commodityClass;
            }
            if (GUILayout.Button("Del", GUILayout.Width(100)))
            {
                //Remove this from the array
                database.RemoveAt(cnt);
                isEdit = false;
                selectedIdx = -1;
            }


            EditorGUILayout.EndHorizontal();
        }
    }

    private void DisplayEditAreaLayout()
    {

        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.15f;
        float cellClass = Screen.width * 0.15f;

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        editID = int.Parse(TextField("ID:", editID.ToString(), GUILayout.Width(cellID)));
        editName = TextField("Name:", editName, GUILayout.Width(cellName)).ToString();
        editBasePrice = int.Parse(TextField("Base Price:", editBasePrice.ToString(), GUILayout.Width(cellBasePrice)));
        editClass = CommodityClassPopup("Class:", editClass, GUILayout.Width(cellClass));
        
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            //Save this, either as a new item, or an edit
            if (isEdit)
            {


                CommodityDataObject cib = database.Commodity(selectedIdx);
                cib.commodityName = editName;
                cib.commodityBasePrice = int.Parse(editBasePrice.ToString());
                cib.commodityClass = editClass;
                EditorUtility.SetDirty(database);
            }
            else
            {
                CommodityDataObject cib = new CommodityDataObject(editID, editName, editBasePrice, 0, 0, editClass);
                database.Add(cib);
                EditorUtility.SetDirty(database);
            }

            //Clear and disable the editor section
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    private void NewCommodity()
    {
        isEdit = false;
        editID = database.Count > 0 ? (database.Count) : 0;
        editName = "New commodity";
        editBasePrice = 0;
        editClass = CommodityDataObject.COMMODITYCLASS.Common;

        //Enable the editor section
    }

    #region Helpers
    
    /// <summary>
    /// Create a EditorGUILayout.TextField with no space between label and text field
    /// </summary>
    public static string TextField(string label, string text, params GUILayoutOption[] options )
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.TextField(label, text, options);
    }

    public static CommodityDataObject.COMMODITYCLASS CommodityClassPopup(string label, CommodityDataObject.COMMODITYCLASS commodityClass, params GUILayoutOption[] options)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return (CommodityDataObject.COMMODITYCLASS)EditorGUILayout.EnumPopup(label, commodityClass, options);
    }

    #endregion
}
