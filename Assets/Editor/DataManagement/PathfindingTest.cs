using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PathfindingTest : EditorWindow
{
    static PathfindingTest editorWindow;

    NodePathfinding np = new NodePathfinding();

    int StartingSectorID = 1;
    int EndingSectorID = 25;

    [MenuItem("Data Management/Pathing Test")]
    public static void Init()
    {
        editorWindow = EditorWindow.GetWindow<PathfindingTest>();
        editorWindow.Show();
    }

    void OnGUI()
    {
        StartingSectorID = EditorGUILayout.IntField("Starting ID", StartingSectorID);
        EndingSectorID = EditorGUILayout.IntField("Ending ID", EndingSectorID);
        if (GUILayout.Button("Find Path"))
        {
            np.FindRoute(StartingSectorID, EndingSectorID);
        }
    }

}
