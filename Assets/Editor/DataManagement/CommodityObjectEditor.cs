using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CommodityObjectEditor : EditorWindow {

    static CommodityObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/dbCommodityDataItems.asset";

    private dbCommodityDataObject database;

    private bool enableEditArea = false;     //Lock it down until NEW or EDIT
    private bool isEdit = false;            //Is this an edit as opposed to a new record?
    private int selectedIdx = -1;           //-1 means nothing selected
    private int editID = 0;
    private Texture2D editImage = null;
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
        if (database == null)
            LoadDatabase();
    }

    public void OnGUI()
    {
        //A grid of existing items. Has DEL and EDIT buttons
        //A button to create a NEW item
        //A form that allows for the editing/adding

        if (GUI.Button(new Rect(10, 10, 200, 15), "Add New Commodity"))
        {
            NewCommodity();
        }
        filterClass = CommodityClassPopup(new Rect(220, 10, 175, 15), "Filter by class:", filterClass);

        DisplayListAreaHeader();
        
        scrollPos = GUI.BeginScrollView(new Rect(0, 55, Screen.width, 150), scrollPos, new Rect(5, 0, Screen.width - 10, 800));
        
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
        float rowPos = 1.0f;
        int rowIdx = 1;

        float cellID = Screen.width * 0.1f;
        float cellName = Screen.width * 0.2f;
        float cellBasePrice = Screen.width * 0.1f;
        float cellClass = Screen.width * 0.15f;

        List<CommodityDataObject> filterList = (from db in database.database where db.commodityClass.Equals(filterClass) select db).ToList<CommodityDataObject>();

        //for (int cnt = 0; cnt < filterList.Count; cnt++)
        foreach(CommodityDataObject cdo in filterList)
        {
            Texture2D img = new Texture2D(32, 32);

            if (cdo.commodityImage != null)
            {
                img = AssetPreview.GetAssetPreview(cdo.commodityImage);
            }


            GUILayout.Label(img, GUILayout.Height(32), GUILayout.Width(32));
            //EditorGUI.DrawPreviewTexture(new Rect(10, 32 *  rowPos, 32, 32), cdo.commodityImage);
            EditorGUI.SelectableLabel(new Rect(52, 32 * rowPos, 75, 15), cdo.commodityID.ToString());
            EditorGUI.SelectableLabel(new Rect(137, 32 * rowPos, 200, 15), cdo.commodityName);
            EditorGUI.SelectableLabel(new Rect(347, 32 * rowPos, 125, 15), cdo.commodityBasePrice.ToString());
            EditorGUI.SelectableLabel(new Rect(482, 32 * rowPos, 175, 15), cdo.commodityClass.ToString());

            if (GUI.Button(new Rect(667, 32 * rowPos, 100, 15), "Edit"))
            {
                //Copy this info to the edit form
                enableEditArea = true;

                isEdit = true;
                selectedIdx = rowIdx;
                editImage = cdo.commodityImage;
                editID = int.Parse(cdo.commodityID.ToString());
                editName = cdo.commodityName.ToString();
                editBasePrice = int.Parse(cdo.commodityBasePrice.ToString());
                editClass = cdo.commodityClass;
            }
            if (GUI.Button(new Rect(777, 32 * rowPos, 100, 15), "Del"))
            {
                //Remove this from the array
                database.Remove(cdo);
                isEdit = false;
                selectedIdx = -1;
            }

            rowIdx += 1;
            rowPos += 1.0f;

            
        }
    }

    private void DisplayEditArea()
    {

        EditorGUI.BeginDisabledGroup(enableEditArea == false);

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

        if (GUI.Button(new Rect(809, 225, 100, 15), "Cancel"))
        {
            ResetForm();
        }

        EditorGUI.EndDisabledGroup();
    }




    private void NewCommodity()
    {
        enableEditArea = true;

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
