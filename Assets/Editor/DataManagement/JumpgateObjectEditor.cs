using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//TODO: Create a stand-alone reciprical updatoer to set destination gates based on the DestinationSector and DestinationGate IDs


public class JumpgateObjectEditor : EditorWindow {

    static JumpgateObjectEditor editorWindow;

    //When loading during runtime, Resources.Load() will need to reference this WITHOUT the .asset extension.
    private const string DATABASE_PATH = @"Assets/Resources/AssetDatabases/";
    private const string JUMPGATE_DATABASE = @"dbJumpgateDataItems.asset";
    private const string SECTOR_DATABASE = @"dbSectorDataItems.asset";


    private dbJumpgateDataObject dbJumpgates;
    private dbSectorDataObject dbSectors;

    private string[] sectorNames;
    private int[] sectorIDs;

    private string[] destGateNames;
    private int[] destGateIDs;

    private bool enableEditArea = false;     //Lock it down until NEW or EDIT
    private bool isEdit = false;             //Is this an edit as opposed to a new record?

    private int editID = 0;
    private string editJumpgateName = string.Empty;
    private int editSectorID = 0;
    private int editDestinationSectorID = 0;
    private int editDestinationJumpgateID = 0;
    private int editFee = 0;


    //Column widths
    int colID = 100;
    int colName = 200;
    int colSectorID;
    int colSectorName = 200;
    int colDestinationSectorName = 225;
    int colDestinationJumpgateID = 175;
    int colDestinationJumpgateName = 200;
    int colFee= 100;

    int colButton1 = 125;
    int colButton2 = 125;
    int colButton3 = 125;
    int colButton4 = 125;

    private Vector2 scrollPos;


    [MenuItem("Data Management/Jumpgates")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<JumpgateObjectEditor>();
        editorWindow.Show();
    }

    void OnEnable()
    {
        LoadDatabases();
    }

    public void OnGUI()
    {
        EditorGUIUtility.LookLikeInspector();

        DisplaySetup();

    }

    void LoadDatabases()
    {
        dbJumpgates = (dbJumpgateDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + JUMPGATE_DATABASE, typeof(dbJumpgateDataObject));
        dbSectors = (dbSectorDataObject)AssetDatabase.LoadAssetAtPath(DATABASE_PATH + SECTOR_DATABASE, typeof(dbSectorDataObject));

        sectorNames = dbSectors.database.Select(x => x.sectorName).ToArray();
        sectorIDs = dbSectors.database.Select(x => x.sectorID).ToArray();
    }

    #region Display

    private void DisplaySetup()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        if (GUILayout.Button("New Jumpgate", GUILayout.Width(300)))
        {
            NewJumpgate();
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //Header
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        EditorGUILayout.LabelField("ID", GUILayout.Width(colID));
        EditorGUILayout.LabelField("NAME", GUILayout.Width(colName));
        EditorGUILayout.LabelField("SECTOR", GUILayout.Width(colSectorName));
        EditorGUILayout.LabelField("DESTINATION SECTOR", GUILayout.Width(colDestinationSectorName));
        EditorGUILayout.LabelField("DESTINATION GATE", GUILayout.Width(colDestinationJumpgateID));
        EditorGUILayout.LabelField("GATE FEE", GUILayout.Width(colFee));

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        //TODO: Set height once we know about the editor requirements
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height - 125));

        DisplayList();

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        DisplayEdit();
        EditorGUILayout.EndHorizontal();

    }

    private void DisplayList()
    {
        foreach(JumpgateDataObject jdo in dbJumpgates.database)
        {
            //List
            EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

            EditorGUILayout.LabelField(jdo.jumpgateID.ToString(), GUILayout.Width(colID));
            EditorGUILayout.LabelField(jdo.jumpgateName.ToString(), GUILayout.Width(colName));
            EditorGUILayout.LabelField(dbSectors.GetSectorByID(jdo.sectorID).sectorName, GUILayout.Width(colSectorName));
            EditorGUILayout.LabelField(dbSectors.GetSectorByID(jdo.destinationSectorID).sectorName, GUILayout.Width(colDestinationSectorName));
            EditorGUILayout.LabelField(dbJumpgates.GetJumpgateByID(jdo.destinationJumpgateID).jumpgateName, GUILayout.Width(colDestinationJumpgateID));
            EditorGUILayout.LabelField(jdo.fee.ToString(), GUILayout.Width(colFee));

            if (GUILayout.Button("Edit", GUILayout.Width(colButton1))) {
                isEdit = true;
                enableEditArea = true;
                editID = jdo.jumpgateID;
                editJumpgateName = jdo.jumpgateName;
                editSectorID = jdo.sectorID;
                editDestinationSectorID = jdo.destinationSectorID;
                editDestinationJumpgateID = jdo.destinationJumpgateID;
                editFee = jdo.fee;
            }
            if (GUILayout.Button("Delete", GUILayout.Width(colButton2))) {
                isEdit = false;
                dbJumpgates.Remove(jdo);
                EditorUtility.SetDirty(dbJumpgates);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DisplayEdit()
    {

        FilterGatesBySector(editDestinationSectorID);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width));

        EditorGUI.BeginDisabledGroup(enableEditArea == false);

        EditorGUILayout.LabelField(editID.ToString(), GUILayout.Width(colID));
        editJumpgateName = EditorGUILayout.TextField(editJumpgateName, GUILayout.Width(colName));
        editSectorID = EditorGUILayout.IntPopup(editSectorID, sectorNames, sectorIDs, GUILayout.Width(colSectorName));

        //Need to update the destination gates to include just those in the proposed destination sector
        EditorGUI.BeginChangeCheck();
        editDestinationSectorID = EditorGUILayout.IntPopup(editDestinationSectorID, sectorNames, sectorIDs, GUILayout.Width(colDestinationSectorName));
        if (EditorGUI.EndChangeCheck())
        {
            FilterGatesBySector(editDestinationSectorID);
        }

        editDestinationJumpgateID = EditorGUILayout.IntPopup(editDestinationJumpgateID, destGateNames, destGateIDs, GUILayout.Width(colDestinationJumpgateID));    //EditorGUILayout.IntField(editDestinationJumpgateID, GUILayout.Width(250));
        editFee = EditorGUILayout.IntField(editFee, GUILayout.Width(colFee));

        if (GUILayout.Button("Save", GUILayout.Width(colButton1))) {
            if (isEdit) { 
                JumpgateDataObject jdo = dbJumpgates.GetJumpgateByID(editID);
                if (jdo != null) { 
                    jdo.sectorID = editSectorID;
                    jdo.jumpgateName = editJumpgateName;
                    jdo.destinationSectorID = editDestinationSectorID;
                    jdo.destinationJumpgateID = editDestinationJumpgateID;
                    jdo.fee = editFee;

                    RecipricateDestinationGate(editSectorID, editID, editDestinationSectorID, editDestinationJumpgateID);
                }
                else
                {
                    Debug.LogError("Jumpgate ID " + editID.ToString() + " was not found. Try restarting the panel.");
                }
            }
            else
            {
                JumpgateDataObject jdo = new JumpgateDataObject(editID, editJumpgateName, editSectorID, editDestinationSectorID, editDestinationJumpgateID, editFee);
                dbJumpgates.Add(jdo);
            }

            ClearJumpgate();

            EditorUtility.SetDirty(dbJumpgates);
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(colButton2))) {
            ClearJumpgate();
        }

        EditorGUI.EndDisabledGroup();

        EditorGUILayout.EndHorizontal();


    }

    private void RecipricateDestinationGate(int EditSectorID, int EditGateID, int DestinationSectorID, int DestinationGateID)
    {
        if (editSectorID != 0 && EditGateID != 0)
        { 
            JumpgateDataObject jgdo = (from db in dbJumpgates.database where db.jumpgateID.Equals(DestinationGateID) && db.sectorID.Equals(DestinationSectorID) select db).FirstOrDefault();
            if (jgdo != null) { 
                jgdo.destinationSectorID = editSectorID;
                jgdo.destinationJumpgateID = EditGateID;
            }
            //int sector = editSectorID;
            //int gate = EditGateID;
        }

    }

    private void NewJumpgate()
    {
        enableEditArea = true;

        editID = dbJumpgates.database.Max(x => x.jumpgateID) + 1;
        editJumpgateName = string.Empty;    
        editSectorID = 1;
        editDestinationSectorID = 1;
        editDestinationJumpgateID = 1;
        editFee = 0;
    }

    private void ClearJumpgate()
    {
        isEdit = false;

        editID = 0;
        editJumpgateName = string.Empty;    
        editSectorID = 0;
        editDestinationSectorID = 0;
        editDestinationJumpgateID = 0;
        editFee = 0;

        enableEditArea = false;
        GUIUtility.keyboardControl = 0;
        GUIUtility.hotControl = 0;
    }
    #endregion


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

    public void FilterGatesBySector(int SectorID)
    {
        List<JumpgateDataObject> jgdo = (from db in dbJumpgates.database where db.sectorID.Equals(SectorID) select db).ToList();

        destGateIDs = jgdo.Select(x => x.jumpgateID).ToArray();
        destGateNames = jgdo.Select(x => x.jumpgateName).ToArray();
    }

    #endregion

}
