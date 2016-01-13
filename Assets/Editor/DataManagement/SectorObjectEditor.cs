using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class SectorObjectEditor : EditorWindow
{
    static SectorObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/dbSectorDataItems.asset";

    private dbSectorDataObject db;

    private bool enableEditArea = false;            //Lock it down until NEW or EDIT
    private bool isEdit = false;                    //Is this an edit as opposed to a new record?
    
    private int editID = 1;                         //ID of the item being edited.
    private string editName = string.Empty;         //Name of the item being edited

    private Vector2 scrollPos;

    [MenuItem("Data Management/Sectors")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<SectorObjectEditor>();
        editorWindow.Show();
    }

    void OnEnable()
    {
        if (db == null)
            LoadDatabase();
    }

    void CreateDatabase()
    {
        db = ScriptableObject.CreateInstance<dbSectorDataObject>();
        AssetDatabase.CreateAsset(db, DATABASE_PATH);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void LoadDatabase()
    {
        db = (dbSectorDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(dbSectorDataObject));
        if (db == null)
            CreateDatabase();
    }

    void OnGUI()
    {
        //##################################################################

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        if (GUILayout.Button("Add New Sector", GUILayout.Width(200)))
        {
            NewSector();
            
        }
        EditorGUILayout.EndHorizontal();

        //##################################################################

        EditorGUILayout.Space();

        //##################################################################

        DisplayListAreaHeader();

        //##################################################################

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 120));
        DisplayListArea();
        EditorGUILayout.EndScrollView();

        //##################################################################

        EditorGUILayout.Space();

        //##################################################################

        DisplayEditArea();

        //##################################################################

    }


    private void DisplayListAreaHeader()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        EditorGUILayout.LabelField("ID", GUILayout.Width(75));
        EditorGUILayout.LabelField("NAME", GUILayout.Width(200));

        EditorGUILayout.EndHorizontal();
    }

    private void DisplayListArea()
    {
        
        //for (int cnt = 0; cnt < filterList.Count; cnt++)
        foreach (SectorDataObject cdo in db.database)
        {

            EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            EditorGUILayout.LabelField(cdo.sectorID.ToString(), GUILayout.Width(75));
            EditorGUILayout.LabelField(cdo.sectorName, GUILayout.Width(200));

            if (GUILayout.Button("Edit", GUILayout.Width(100)))
            {
                //Copy this info to the edit form
                enableEditArea = true;

                isEdit = true;
                editID = int.Parse(cdo.sectorID.ToString());
                editName = cdo.sectorName.ToString();

                EditorGUI.FocusTextInControl("EditName");
                
            }
            if (GUILayout.Button("Del", GUILayout.Width(100)))
            {
                //Remove this from the array
                db.Remove(cdo);
                isEdit = false;
                editID = -1;
                editName = string.Empty;
                    
            }

            EditorGUILayout.EndHorizontal();
        }
    }

    private void DisplayEditArea()
    {
        EditorGUI.BeginDisabledGroup(enableEditArea == false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        editID = int.Parse(TextField("ID:", editID.ToString(), GUILayout.Width(75)));

        GUI.SetNextControlName("EditName");
        editName = TextField("Name:", editName, GUILayout.Width(200)).ToString();

        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            //Save this, either as a new item, or an edit
            if (isEdit)
            {
                SectorDataObject sdo = db.GetSector(editID);
                //Can't change the ID once it's set
                sdo.sectorName = editName;
                EditorUtility.SetDirty(db);
            }
            else
            {
                SectorDataObject sdo = new SectorDataObject(editID, editName);
                db.Add(sdo);
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

    private void NewSector()
    {
        enableEditArea = true;

        isEdit = false;
        editID = db.Count > 0 ? (db.Count + 1) : 1;
        editName = "New sector";

        EditorGUI.FocusTextInControl("EditName");
    }

    private void ResetForm()
    {
        enableEditArea = false;

        isEdit = false;
        editID = 0;
        editName = string.Empty;
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

