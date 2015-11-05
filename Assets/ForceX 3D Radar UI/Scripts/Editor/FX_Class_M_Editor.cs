using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FX_Class_Mgr))]
public class FX_Class_M_Editor : Editor {
	FX_Class_Mgr t;
	SerializedObject GetTarget;
	SerializedProperty ThisObjectClassList;
	

	void OnEnable(){
		t = (FX_Class_Mgr)target;
		GetTarget = new SerializedObject(t);
		ThisObjectClassList = GetTarget.FindProperty("ObjectClassList");
	}

	public override void OnInspectorGUI(){
		CheckMinSize();
		CustomClassEditor();
	}

	void CheckMinSize(){
		GetTarget.Update();
		if(ThisObjectClassList.arraySize < 1){
			t.ObjectClassList.Add(new FX_Class_Mgr.objectClassList());
		}
		GetTarget.ApplyModifiedProperties();
	}

	void CustomClassEditor(){
		GetTarget.Update();
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		if(GUILayout.Button("Add New Class",GUILayout.MaxWidth(100),GUILayout.MaxHeight(20))){
			t.ObjectClassList.Add(new FX_Class_Mgr.objectClassList());
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		for(int i = 0; i < ThisObjectClassList.arraySize; i++){
			SerializedProperty MyListRef = ThisObjectClassList.GetArrayElementAtIndex(i);
			SerializedProperty ClassName = MyListRef.FindPropertyRelative("ClassName");
			SerializedProperty SubClassName = MyListRef.FindPropertyRelative("SubClassName");
			SerializedProperty ClassSprite = MyListRef.FindPropertyRelative("ClassSprite");
			SerializedProperty IDOffset = MyListRef.FindPropertyRelative("IDOffset");

			GUI.color = GetColor(i);

			EditorGUILayout.BeginVertical("HelpBox");
			GUI.color = Color.white;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField ("Class Name : ",GUILayout.MaxWidth(85));
			ClassName.stringValue = EditorGUILayout.TextField("",ClassName.stringValue,GUILayout.MaxWidth(130));
			
			if(i > 0 && GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
				t.ObjectClassList.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();

			GUI.color = GetColor(i);

			EditorGUILayout.BeginVertical("HelpBox");
			GUI.color = Color.white;
			EditorGUILayout.Space ();
			
			if(SubClassName.arraySize < 1){
				SubClassName.InsertArrayElementAtIndex(SubClassName.arraySize);
				ClassSprite.InsertArrayElementAtIndex(ClassSprite.arraySize);
				IDOffset.InsertArrayElementAtIndex(IDOffset.arraySize);
				SubClassName.GetArrayElementAtIndex(SubClassName.arraySize - 1).stringValue = "Sub Class " + (SubClassName.arraySize - 1).ToString ();
				ClassSprite.GetArrayElementAtIndex(ClassSprite.arraySize - 1).objectReferenceValue = null;
			}

			if(GUILayout.Button("Add New Sub Class",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
				SubClassName.InsertArrayElementAtIndex(SubClassName.arraySize);
				ClassSprite.InsertArrayElementAtIndex(ClassSprite.arraySize);
				IDOffset.InsertArrayElementAtIndex(IDOffset.arraySize);
				SubClassName.GetArrayElementAtIndex(SubClassName.arraySize - 1).stringValue = "Sub Class " + (SubClassName.arraySize - 1).ToString ();
				ClassSprite.GetArrayElementAtIndex(ClassSprite.arraySize - 1).objectReferenceValue = null;
			}

			EditorGUILayout.Space ();
			for(int a = 0; a < SubClassName.arraySize; a++){
				EditorGUILayout.BeginVertical("HelpBox");
				EditorGUILayout.Space ();
				EditorGUILayout.BeginHorizontal();
				SubClassName.GetArrayElementAtIndex(a).stringValue = EditorGUILayout.TextField("",SubClassName.GetArrayElementAtIndex(a).stringValue,GUILayout.MaxWidth(85));
				ClassSprite.GetArrayElementAtIndex(a).objectReferenceValue = EditorGUILayout.ObjectField("", ClassSprite.GetArrayElementAtIndex(a).objectReferenceValue, typeof(Sprite), true,GUILayout.MaxWidth(125));

				if(a > 0 && GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
					t.ObjectClassList[i].SubClassName.RemoveAt(a);
					t.ObjectClassList[i].ClassSprite.RemoveAt(a);
					t.ObjectClassList[i].IDOffset.RemoveAt(a);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space ();
				IDOffset.GetArrayElementAtIndex(a).vector3Value = EditorGUILayout.Vector3Field("Radar Offset", IDOffset.GetArrayElementAtIndex(a).vector3Value);
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
		}
		
		GetTarget.ApplyModifiedProperties();
	}

	Color GetColor(int i){
		Color NewColor = Color.white;

		if(i%2==0){
			NewColor = new Color(0,.5f,1,1); //Blue
			//GUI.color = new Color(1,.2f,.2f,1); //Red
			//GUI.color = new Color(.5f,1,0,1); //Green
		}else{
			//GUI.color = new Color(0,.5f,1,1); //Blue
			NewColor = new Color(1,.2f,.2f,1); //Red
			//GUI.color = new Color(.5f,1,0,1); //Green
		}
		return NewColor;
	}
}
