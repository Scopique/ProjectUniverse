using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FX_3DRadar_RID))]

public class FX_3DRadar_R_Editor : Editor {

	FX_3DRadar_RID t;
	FX_Class_Mgr FXCM;
	bool setup;
	string[] Factions;


	public override void OnInspectorGUI(){

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		ClassSelecetion();
		EditorGUILayout.Space ();
		FactionSelection();
		EditorGUILayout.Space ();
		StateCondition();
		EditorGUILayout.Space ();
	}

	void OnEnable(){
		t = (FX_3DRadar_RID)target;
        if(PrefabUtility.GetPrefabType(t) == PrefabType.PrefabInstance){
            PrefabUtility.DisconnectPrefabInstance(t.gameObject);
        }

		if(!setup){
			FXCM = GameObject.Find("_GameMgr").GetComponent<FX_Class_Mgr>();
			Factions = GameObject.Find("_GameMgr").GetComponent<FX_Faction_Mgr>().Factions;
			if(t.ThisFaction[0] > Factions.Length - 1){
				t.ThisFaction[0] = 0;
			}
			setup = true;
		}
	}

	void FactionSelection(){
		EditorGUILayout.LabelField("Faction Selection", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();
		
		t.ThisFaction[0] = EditorGUILayout.IntSlider(Factions[t.ThisFaction[0]],t.ThisFaction[0], 0, Factions.Length - 1);
		
		EditorGUILayout.LabelField("Faction ID :	" + t.ThisFactionID.ToString());
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void ClassSelecetion(){
		EditorGUILayout.LabelField("Class Selection", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		if(t.MainClass[0] > FXCM.ObjectClassList.Count - 1){
			t.MainClass[0] = 0;
		}

		t.MainClass[0] = EditorGUILayout.IntSlider(FXCM.ObjectClassList[t.MainClass[0]].ClassName, t.MainClass[0], 0, (FXCM.ObjectClassList.Count - 1));

		if(t.SubClass[0] > FXCM.ObjectClassList[t.MainClass[0]].SubClassName.Count - 1){
			t.SubClass[0] = 0;
		}

		t.SubClass[0] = EditorGUILayout.IntSlider(FXCM.ObjectClassList[t.MainClass[0]].SubClassName[t.SubClass[0]], t.SubClass[0], 0, (FXCM.ObjectClassList[t.MainClass[0]].SubClassName.Count - 1));

		EditorGUILayout.Space ();

		DrawOnGUISprite (FXCM.ObjectClassList[t.MainClass[0]].ClassSprite[t.SubClass[0]]);

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void DrawOnGUISprite(Sprite aSprite){
		EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(10));
		Rect c = aSprite.rect;
		Rect rect = GUILayoutUtility.GetRect(10, 10);

		if (Event.current.type == EventType.Repaint){
			Texture tex = aSprite.texture;
			c.xMin /= tex.width;
			c.xMax /= tex.width;
			c.yMin /= tex.height;
			c.yMax /= tex.height;

			GUI.DrawTextureWithTexCoords(rect, tex, c);
		}
		EditorGUILayout.EndHorizontal();
	}

	void StateCondition(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("State", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("IFF Status : " + t.IFFStatus.ToString());

		if(t.IsObjective[0]){
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.MaxWidth(18),GUILayout.MaxHeight(18))){
				t.ResetObjectiveStatus();
			}
			EditorGUILayout.LabelField("Is Objective : Yes  (" + t.ObjectiveType.ToString() + ")",GUILayout.MaxWidth(220));
			EditorGUILayout.EndHorizontal();
		}else{
			EditorGUILayout.LabelField("Is Objective : No");
		}


		if(t.IsPlayerTarget){
			EditorGUILayout.LabelField("Player Target : Yes");
		}else{
			EditorGUILayout.LabelField("Player Target : No");
		}
		EditorGUILayout.Space ();

		t.IsPOI = EditorGUILayout.Toggle("Is POI", t.IsPOI);
		t.IsNAV = EditorGUILayout.Toggle("Is NAV", t.IsNAV);
		if(t.IsNAV){
			t.IsActiveNAV[0] = EditorGUILayout.Toggle("Is Active NAV", t.IsActiveNAV[0]);
		}
		t.IsPlayer = EditorGUILayout.Toggle("Is Player", t.IsPlayer);
		EditorGUILayout.Space ();

		t.IsPlayerOwned[0] = EditorGUILayout.Toggle("Is Player Owned", t.IsPlayerOwned[0]);
		t.IsAbandoned[0] = EditorGUILayout.Toggle("Is Abandoned", t.IsAbandoned[0]);

		EditorGUILayout.Space ();

		GUI.color = new Color(.75f,0,0,1);
		if(t.RIDI && GUILayout.Button("Destroy",GUILayout.MaxWidth(80),GUILayout.MaxHeight(18))){
			t.DestroyThis();
		}
		GUI.color = Color.white;

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();

		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Targeting & Detection Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		t.IsTargetable = EditorGUILayout.Toggle("Is Targetable", t.IsTargetable);
		t.IsDetectable = EditorGUILayout.Toggle("Is Detectable", t.IsDetectable);
		t.IsDiscovered = EditorGUILayout.Toggle("Is Discovered", t.IsDiscovered);
		t.DetectionReset = EditorGUILayout.Toggle("Detection Reset", t.DetectionReset);

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();

		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("This HUD ID Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		t.DisplayHUDID = EditorGUILayout.Toggle("* Display This ID", t.DisplayHUDID);
		if(t.DisplayHUDID){
			t.HUDIDOnScreen = EditorGUILayout.Toggle("Show On Screen", t.HUDIDOnScreen);
			EditorGUILayout.Space ();
			t.HUDIDShow = (FX_3DRadar_RID._boundsShow)EditorGUILayout.EnumPopup("Display HUD ID :",t.HUDIDShow);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();

		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("This Bounds Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();
		t.DisplayBounds = EditorGUILayout.Toggle("Display This Bounds", t.DisplayBounds);
		EditorGUILayout.Space ();
		if(t.DisplayBounds){
			t.BoundsShow = (FX_3DRadar_RID._boundsShow)EditorGUILayout.EnumPopup("Display Bounds :",t.BoundsShow);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();

		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Blind Radar Settings", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		EditorGUILayout.Space ();

		t.BlindRadarOverride = EditorGUILayout.Toggle("Radar Override", t.BlindRadarOverride);

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
	}
}
