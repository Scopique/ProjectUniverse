using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(FX_Mission_Giver))]
public class FX_Mission_Giver_Editor : Editor {
	FX_Mission_Giver t;
	SerializedObject GetTarget;
	SerializedProperty ThisList;
	
	
	void OnEnable(){
		t = (FX_Mission_Giver)target;
		GetTarget = new SerializedObject(t);
		ThisList = GetTarget.FindProperty("MissionGiverList");
	}
	
	public override void OnInspectorGUI(){
		CustomClassEditor();
	}
	

	void CustomClassEditor(){
		GetTarget.Update();
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		
		if(GUILayout.Button("Add New Mission",GUILayout.MaxWidth(120),GUILayout.MaxHeight(20))){
			t.MissionGiverList.Add(new FX_Mission_Giver.missionGiverList());
		}
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		for(int i = 0; i < ThisList.arraySize; i++){
			SerializedProperty MyListRef = ThisList.GetArrayElementAtIndex(i);;
			SerializedProperty Mission = MyListRef.FindPropertyRelative("Mission");

			if(i%2==0){
				GUI.color = new Color(0,.5f,1,1); //Blue
			}else{
				GUI.color = new Color(1,.2f,.2f,1); //Red
			}

			EditorGUILayout.BeginVertical("HelpBox");
			GUI.color = Color.white;
			if(Mission.objectReferenceValue){
				EditorGUILayout.LabelField(t.MissionGiverList[i].Mission.MissionName.ToString(),EditorStyles.boldLabel);
				t.MissionGiverList[i].Mission.MissionGiver = t.gameObject;
			}else{
				EditorGUILayout.LabelField("No Mission Assigned",EditorStyles.boldLabel);
			}
			EditorGUILayout.BeginHorizontal();



			EditorGUILayout.LabelField ("Mission : ",GUILayout.MaxWidth(85));
			Mission.objectReferenceValue = EditorGUILayout.ObjectField("", Mission.objectReferenceValue, typeof(FX_Mission), true,GUILayout.MaxWidth(125));

			if(GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
				ThisList.DeleteArrayElementAtIndex(i);
				break;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

			EditorGUILayout.Space ();
		}

		GetTarget.ApplyModifiedProperties();
	}
}