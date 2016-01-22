using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class JumpgateObjectEditor : EditorWindow {

    static JumpgateObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/";
    private const string JUMPGATE_DATABASE = @"dbJumpgateDataItems.asset";
    private const string SECTOR_DATABASE = @"dbSectorDataItems.asset";


    private dbJumpgateDataObject db;

    private bool enableEditArea = false;     //Lock it down until NEW or EDIT
    private bool isEdit = false;             //Is this an edit as opposed to a new record?

    private int editID = 0;
    private int editSectorID = 0;
    private int editDestinationJumpgateID = 0;
    private int editFee = 0;

    private Vector2 scrollPos;


    [MenuItem("Data Management/Jumpgates")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<JumpgateObjectEditor>();
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
        if (GUILayout.Button("Add New Jumpgate", GUILayout.Width(200)))
        {
            NewJumpgate();
        }
        
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        DisplayListAreaHeader();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxWidth(Screen.width), GUILayout.Height(Screen.height - 150));
        DisplayListArea();
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        DisplayEditArea();

    }


    void CreateDatabase()
    {
        db = ScriptableObject.CreateInstance<dbJumpgateDataObject>();
        AssetDatabase.CreateAsset(db, DATABASE_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void LoadDatabase()
    {
        db = (dbJumpgateDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + JUMPGATE_DATABASE, typeof(dbJumpgateDataObject));
        if (db == null)
            CreateDatabase();
    }

    private void DisplayListAreaHeader()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        EditorGUILayout.LabelField("ID", GUILayout.Width(75));
        EditorGUILayout.LabelField("SECTOR ID", GUILayout.Width(75));
        EditorGUILayout.LabelField("DESTINATION GATE ID", GUILayout.Width(125));
        EditorGUILayout.LabelField("JUMP FEE", GUILayout.Width(75));
        EditorGUILayout.EndHorizontal();
    }

    private void DisplayListArea()
    {
        int rowIdx = 1;

        //for (int cnt = 0; cnt < filterList.Count; cnt++)
        foreach (JumpgateDataObject jdo in db.database)
        {

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            EditorGUILayout.LabelField(jdo.jumpgateID.ToString(), GUILayout.Width(75));
            EditorGUILayout.LabelField(jdo.sectorID.ToString(), GUILayout.Width(75));
            EditorGUILayout.LabelField(jdo.destinationJumpgateID.ToString(), GUILayout.Width(125));
            EditorGUILayout.LabelField(jdo.fee.ToString(), GUILayout.Width(75));

            if (GUILayout.Button("Edit", GUILayout.Width(100)))
            {
                //Copy this info to the edit form
                enableEditArea = true;

                isEdit = true;
                editID = jdo.jumpgateID;
                editSectorID = jdo.sectorID;              
                editDestinationJumpgateID = jdo.destinationJumpgateID;
                editFee = jdo.fee;
            }
            if (GUILayout.Button("Del", GUILayout.Width(100)))
            {
                //Remove this from the array
                db.Remove(jdo);
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

        editID = IntField("ID", editID, GUILayout.Width(75));
        editSectorID = IntField("Sector", editSectorID, GUILayout.Width(75));
        editDestinationJumpgateID = IntField("Destination Gate ID", editDestinationJumpgateID, GUILayout.Width(200));
        editFee = IntField("Jump Fee:", editFee, GUILayout.Width(100));

        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            //Save this, either as a new item, or an edit
            if (isEdit)
            {
                JumpgateDataObject jdo = db.GetJumpgate(editID);
                jdo.sectorID = editSectorID;
                jdo.destinationJumpgateID = editDestinationJumpgateID;
                jdo.fee = editFee;
                EditorUtility.SetDirty(db);
            }
            else
            {
                JumpgateDataObject jdo = new JumpgateDataObject(0, 0, 0, 0);
                db.Add(jdo);
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

    private void NewJumpgate()
    {
        enableEditArea = true;

        isEdit = false;
        editID = db.Count > 0 ? (db.Count) : 0;
        editSectorID = 0;       
        editDestinationJumpgateID = 0;
        editFee = 0;

        //Enable the editor section
    }

    private void ResetForm()
    {
        enableEditArea = false;

        isEdit = false;
        editID = 0;
        editSectorID = 0;      
        editDestinationJumpgateID = 0;
        editFee = 0;
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

    public static int IntField(string label, int integer, params GUILayoutOption[] args)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return EditorGUILayout.IntField(label, integer, args);
    }

    public static CommodityDataObject.COMMODITYCLASS CommodityClassPopup(string label, CommodityDataObject.COMMODITYCLASS commodityClass, params GUILayoutOption[] args)
    {
        Vector2 textDimensions = GUI.skin.label.CalcSize(new GUIContent(label));
        EditorGUIUtility.labelWidth = textDimensions.x;
        return (CommodityDataObject.COMMODITYCLASS)EditorGUILayout.EnumPopup(label, commodityClass, args);
    }

    #endregion

}
