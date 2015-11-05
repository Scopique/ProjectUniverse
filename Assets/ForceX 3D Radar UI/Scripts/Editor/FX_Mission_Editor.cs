using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(FX_Mission))]
public class FX_Mission_Editor : Editor {
	FX_Mission t;
	SerializedObject GetTarget;
	SerializedProperty thisList;

	void OnEnable(){
		t = (FX_Mission)target;
		GetTarget = new SerializedObject(t);
		thisList = GetTarget.FindProperty("MissionNAVList");

		if(t.MissionNAVList.Count < 1){
			t.MissionNAVList.Add(new FX_Mission.missionNAVList());
		}
	}

	public override void OnInspectorGUI(){
		//Details();
		NAVList();
	}

	void Details(){

		EditorGUILayout.BeginVertical("HelpBox");
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Name :",GUILayout.MaxWidth(50));
		t.MissionName = EditorGUILayout.TextField("",t.MissionName);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Description Dialog",GUILayout.MaxWidth(200));
		t.DescriptionAudio = (AudioClip)EditorGUILayout.ObjectField("",t.DescriptionAudio, typeof(AudioClip), true);
		t.DescriptionText = EditorGUILayout.TextArea(t.DescriptionText, GUILayout.Height(200));
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Accept Dialog",GUILayout.MaxWidth(200));
		t.AcceptAudio = (AudioClip)EditorGUILayout.ObjectField("",t.AcceptAudio, typeof(AudioClip), true);
		t.AcceptText = EditorGUILayout.TextArea(t.AcceptText, GUILayout.Height(200));
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Decline Dialog",GUILayout.MaxWidth(200));
		t.DeclineAudio = (AudioClip)EditorGUILayout.ObjectField("",t.DeclineAudio, typeof(AudioClip), true);
		t.DeclineText = EditorGUILayout.TextArea(t.DeclineText, GUILayout.Height(200));
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Completion Dialog",GUILayout.MaxWidth(200));
		t.CompleteAudio = (AudioClip)EditorGUILayout.ObjectField("",t.CompleteAudio, typeof(AudioClip), true);
		t.CompleteText = EditorGUILayout.TextArea(t.CompleteText, GUILayout.Height(200));
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.EndVertical();
	}

	void NAVList(){
		GetTarget.Update();

		if(GUILayout.Button("Add New NAV",GUILayout.MaxWidth(100),GUILayout.MaxHeight(20))){
			t.MissionNAVList.Add(new FX_Mission.missionNAVList());
		}

		for(int i = 0; i < thisList.arraySize; i++){
			SerializedProperty MyListRef = thisList.GetArrayElementAtIndex(i);
			SerializedProperty NAVPoint = MyListRef.FindPropertyRelative("NAVPoint");
			SerializedProperty NAVName = MyListRef.FindPropertyRelative("NAVName");
			SerializedProperty NAVArrivalDistance = MyListRef.FindPropertyRelative("NAVArrivalDistance");
			SerializedProperty NextCondition = MyListRef.FindPropertyRelative("NextCondition");
			SerializedProperty Delay = MyListRef.FindPropertyRelative("Delay");
			SerializedProperty ObjectiveType = MyListRef.FindPropertyRelative("ObjectiveType");
			SerializedProperty Objectives = MyListRef.FindPropertyRelative("Objectives");
			SerializedProperty NAVTextIn = MyListRef.FindPropertyRelative("NAVTextIn");
			SerializedProperty NAVTextOut = MyListRef.FindPropertyRelative("NAVTextOut");
			SerializedProperty NAVAudioIn = MyListRef.FindPropertyRelative("NAVAudioIn");
			SerializedProperty NAVAudioOut = MyListRef.FindPropertyRelative("NAVAudioOut");

			if(i%2==0){
				GUI.color = new Color(0,.5f,1,1); //Blue
			}else{
				GUI.color = new Color(1,.2f,.2f,1); //Red
			}

			EditorGUILayout.BeginVertical("HelpBox",GUILayout.MaxWidth(180));

			GUI.color = Color.white;

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("NAV Point[" + i.ToString() + "]",EditorStyles.boldLabel,GUILayout.MaxWidth(120));
			NAVName.stringValue = EditorGUILayout.TextField("",NAVName.stringValue,GUILayout.MaxWidth(120));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			NAVPoint.objectReferenceValue = EditorGUILayout.ObjectField("", NAVPoint.objectReferenceValue, typeof(Transform), true,GUILayout.MaxWidth(120));
			NextCondition.enumValueIndex = (int)(FX_Mission.nextCondition)EditorGUILayout.EnumPopup("", (FX_Mission.nextCondition)NextCondition.enumValueIndex, GUILayout.MaxWidth(120));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Arrival Distance (m)",GUILayout.MaxWidth(120));
			NAVArrivalDistance.floatValue = EditorGUILayout.FloatField("",NAVArrivalDistance.floatValue,GUILayout.MaxWidth(120));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();

			if(t.MissionNAVList[i].NextCondition == FX_Mission.nextCondition.TimeDelay){
				EditorGUILayout.LabelField("Next NAV : Time Delay",EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical("HelpBox");
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Time Delay (sec)",GUILayout.MaxWidth(120));
				Delay.intValue = EditorGUILayout.IntField("", Delay.intValue,GUILayout.MaxWidth(100));
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
			}else if(t.MissionNAVList[i].NextCondition == FX_Mission.nextCondition.CompleteObjective){
				EditorGUILayout.LabelField("Next NAV : Complete Objective",EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical("HelpBox");
				EditorGUILayout.Space();
				if(GUILayout.Button("Add Objective",GUILayout.MaxWidth(100),GUILayout.MaxHeight(20))){
					Objectives.InsertArrayElementAtIndex(Objectives.arraySize);
					ObjectiveType.InsertArrayElementAtIndex(ObjectiveType.arraySize);

				}

				EditorGUILayout.Space();
				for(int a = 0; a < ObjectiveType.arraySize; a++){
					EditorGUILayout.BeginVertical("HelpBox");
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Objective[" + a.ToString() + "]",GUILayout.MaxWidth(80));
					Objectives.GetArrayElementAtIndex(a).objectReferenceValue = EditorGUILayout.ObjectField("", Objectives.GetArrayElementAtIndex(a).objectReferenceValue, typeof(Transform), true,GUILayout.MaxWidth(125));
					EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Condition",GUILayout.MaxWidth(80));
					ObjectiveType.GetArrayElementAtIndex(a).enumValueIndex = (int)(FX_Mission.objectiveType)EditorGUILayout.EnumPopup("", (FX_Mission.objectiveType)ObjectiveType.GetArrayElementAtIndex(a).enumValueIndex, GUILayout.MaxWidth(125));
					if(GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
						t.MissionNAVList[i].Objectives.RemoveAt(a);
						t.MissionNAVList[i].ObjectiveType.RemoveAt(a);
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.Space();
				if(GUILayout.Button("Set Objects As Objective",GUILayout.MaxWidth(260),GUILayout.MaxHeight(20))){
					t.SetObjectiveStatus();
					
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.Space();

			EditorGUILayout.LabelField("NAV Dialog In",EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Dialog Audio",GUILayout.MaxWidth(80));
			NAVAudioIn.objectReferenceValue = EditorGUILayout.ObjectField("", NAVAudioIn.objectReferenceValue, typeof(AudioClip), true,GUILayout.MaxWidth(125));
			EditorGUILayout.EndHorizontal();
			NAVTextIn.stringValue = EditorGUILayout.TextArea(NAVTextIn.stringValue, GUILayout.Height(200));

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("NAV Dialog Out",EditorStyles.boldLabel);
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Dialog Audio",GUILayout.MaxWidth(80));
			NAVAudioOut.objectReferenceValue = EditorGUILayout.ObjectField("", NAVAudioOut.objectReferenceValue, typeof(AudioClip), true,GUILayout.MaxWidth(125));
			EditorGUILayout.EndHorizontal();
			NAVTextOut.stringValue = EditorGUILayout.TextArea(NAVTextOut.stringValue, GUILayout.Height(200));


			EditorGUILayout.Space();
			EditorGUILayout.Space();
			if(i > 0 && GUILayout.Button("Remove NAV",GUILayout.MaxWidth(90),GUILayout.MaxHeight(20))){
				thisList.DeleteArrayElementAtIndex(i);
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
		}

		GetTarget.ApplyModifiedProperties();
	}
}
