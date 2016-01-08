using UnityEngine;
using UnityEditor;
using System.Collections;

public class CommodityObjectEditor : EditorWindow {

    static CommodityObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/dbCommodityDataItems.asset";

    private dbCommodityDataObject database;


    private bool isEdit = false;        //Is this an edit as opposed to a new record?
    private int selectedIdx = -1;       //-1 means nothing selected
    private int editID = 0;
    private Texture2D editImage = null;
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

        if (GUI.Button(new Rect(10,10,Screen.width-20, 25), "Add New Commodity"))
        {
            NewCommodity();
        }

        DisplayListAreaHeader();

        scrollPos = GUI.BeginScrollView(new Rect(0, 55, Screen.width, 150), scrollPos, new Rect(5, 0, Screen.width - 10, 400));
        DisplayListArea();
        //DisplayListAreaRect();
        GUI.EndScrollView();
        EditorGUILayout.Space();
        DisplayEditArea();
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


    private void DisplayListAreaHeader()
    {
        EditorGUI.SelectableLabel(new Rect(5,   45, 32, 32),"IMG");
        EditorGUI.SelectableLabel(new Rect(47,  45, 75, 15), "ID");
        EditorGUI.SelectableLabel(new Rect(132, 45, 200, 15), "NAME");
        EditorGUI.SelectableLabel(new Rect(342, 45, 125, 15), "BASE PRICE");
        EditorGUI.SelectableLabel(new Rect(479, 45, 175, 15), "CLASS");
    }

    private void DisplayListArea()
    {

        
        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.1f;
        float cellClass = Screen.width * 0.15f;

        for (int cnt = 0; cnt < database.Count; cnt++)
        {
            Rect row = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            EditorGUI.DrawPreviewTexture(new Rect(10, 32 * cnt, 32, 32), database.Commodity(cnt).commodityImage);
            EditorGUI.SelectableLabel(new Rect(52, 32 * cnt, 75, 15), database.Commodity(cnt).commodityID.ToString());
            EditorGUI.SelectableLabel(new Rect(137, 32 * cnt, 200, 15), database.Commodity(cnt).commodityName);
            EditorGUI.SelectableLabel(new Rect(347, 32 * cnt, 125, 15), database.Commodity(cnt).commodityBasePrice.ToString());
            EditorGUI.SelectableLabel(new Rect(482, 32 * cnt, 175, 15), database.Commodity(cnt).commodityClass.ToString());

            if (GUI.Button(new Rect(667, 32 * cnt, 100, 15), "Edit"))
            {
                //Copy this info to the edit form
                isEdit = true;
                selectedIdx = cnt;
                editImage = database.Commodity(cnt).commodityImage;
                editID = int.Parse(database.Commodity(cnt).commodityID.ToString());
                editName = database.Commodity(cnt).commodityName.ToString();
                editBasePrice = int.Parse(database.Commodity(cnt).commodityBasePrice.ToString());
                editClass = database.Commodity(cnt).commodityClass;
            }
            if (GUI.Button(new Rect(777, 32 * cnt, 100, 15), "Del"))
            {
                //Remove this from the array
                database.RemoveAt(cnt);
                isEdit = false;
                selectedIdx = -1;
            }


            EditorGUILayout.EndHorizontal();
        }
    }

    private void DisplayEditArea()
    {
        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.15f;
        float cellClass = Screen.width * 0.15f;

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

        editImage = (Texture2D)EditorGUI.ObjectField(new Rect(10, 225, 64, 64), editImage, typeof(Texture2D), false);
        editID = int.Parse(TextField(new Rect(84, 225, 75, 15), "ID:", editID.ToString()));
        editName = TextField(new Rect(169, 225, 200, 15), "Name:", editName).ToString();
        editBasePrice = int.Parse(TextField(new Rect(379, 225, 125, 15),  "Base Price:", editBasePrice.ToString()));
        editClass = CommodityClassPopup(new Rect(514, 225, 175, 15), "Class:", editClass);
        
        if (GUI.Button(new Rect(699, 225, 100, 15), "Save"))
        {
            //Save this, either as a new item, or an edit
            if (isEdit)
            {
                CommodityDataObject cib = database.Commodity(selectedIdx);
                cib.commodityImage = editImage;
                cib.commodityName = editName;
                cib.commodityBasePrice = int.Parse(editBasePrice.ToString());
                cib.commodityClass = editClass;
                EditorUtility.SetDirty(database);
            }
            else
            {
                CommodityDataObject cib = new CommodityDataObject(editID, editName, editBasePrice, 0, 0, editImage, editClass);
                database.Add(cib);
                EditorUtility.SetDirty(database);
            }

            //Clear and disable the editor section
            ResetForm();
        }

        EditorGUILayout.EndHorizontal();
    }

    #endregion

    private void NewCommodity()
    {
        isEdit = false;
        editImage = null;
        editID = database.Count > 0 ? (database.Count) : 0;
        editName = "New commodity";
        editBasePrice = 0;
        editClass = CommodityDataObject.COMMODITYCLASS.Common;

        //Enable the editor section
    }

    private void ResetForm()
    {
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
    public static string TextField(Rect rect, string label, string text)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUI.TextField(rect, label, text);
    }

    public static CommodityDataObject.COMMODITYCLASS CommodityClassPopup(Rect rect, string label, CommodityDataObject.COMMODITYCLASS commodityClass)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return (CommodityDataObject.COMMODITYCLASS)EditorGUI.EnumPopup(rect, label, commodityClass);
    }

    #endregion
}
