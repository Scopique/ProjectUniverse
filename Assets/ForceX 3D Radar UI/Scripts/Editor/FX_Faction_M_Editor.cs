using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FX_Faction_Mgr))]
public class FX_Faction_M_Editor : Editor {

	FX_Faction_Mgr t;
	int FactionCNT;
	int NumberOfFactions;
	int UniqueRelations;

	public override void OnInspectorGUI(){
		Checker();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		MenuSelection();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		CheckFactionSize();

		switch((int)t.FMSetup){
		case 0:
			FactionRelationValues();
			EditorGUILayout.Space ();
			FactionSetup ();
			break;

		case 1:
			SetPlayerFaction();
			EditorGUILayout.Space ();
			SetPlayerRelations();
			break;

		case 2:
			SetFactionRelations();
			break;
		}
	}

	void OnEnable(){
		t = (FX_Faction_Mgr)target;
		FactionCNT = t.Factions.Length;
	}

	void Checker(){
		if(t.PlayerFaction > t.Factions.Length){
			t.PlayerFaction = 0;
		}
	}

	void MenuSelection(){
		EditorGUILayout.LabelField("Menu Selection", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();	
		t.FMSetup = (FX_Faction_Mgr._fmSetup)EditorGUILayout.EnumPopup("", t.FMSetup);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void CheckFactionSize(){
		if(FactionCNT != t.Factions.Length){
			System.Array.Resize(ref t.Factions, FactionCNT);
			System.Array.Resize(ref t.FactionID, FactionCNT);
			for(int n = 0; n < t.Factions.Length; n++){
				if(t.Factions[n] == null){
					t.Factions[n] = "";
				}
			}
		}

		if (t.PlayerRelations.Length != t.Factions.Length){//resize the array
			System.Array.Resize(ref t.PlayerRelations, t.Factions.Length);
		}
		
		NumberOfFactions = t.Factions.Length;
		UniqueRelations = (int)((NumberOfFactions - 1)  * (NumberOfFactions * 0.5f));

		if(UniqueRelations != t.StartRelations.Length){
			System.Array.Resize(ref t.StartRelations, UniqueRelations);
		}
	}

	void FactionSetup(){
		EditorGUILayout.LabelField("Number Of Factions", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();	
		FactionCNT = EditorGUILayout.IntSlider( "Factions: ", FactionCNT, 2, 32);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("Faction Names", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		for (int i = 0; i < t.Factions.Length; i++){//display  all the elements of the array
			EditorGUILayout.Space ();
			t.Factions[i] = EditorGUILayout.TextField ("Faction (" + (i).ToString() + ")", t.Factions[i]);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void FactionRelationValues(){
		EditorGUILayout.LabelField("Relation Ranges :", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();	
		EditorGUILayout.LabelField ("Ranges -1000 --> 1000");
		EditorGUILayout.Space ();
		t.HFS[0] = EditorGUILayout.IntSlider("Hostile Value < ", t.HFS[0], -1000, 0);
		t.HFS[1] = EditorGUILayout.IntSlider("Friendly Value > ", t.HFS[1],0, 1000);
		
		float h = t.HFS[0];
		float f = t.HFS[1];
		
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		
		GUILayout.Label("<-------- Hostile <-------- Neutral --------> Friendly -------->");
		EditorGUILayout.MinMaxSlider(ref h,ref f, -1000.0f, 1000.0f);
		
		t.HFS[0] = Mathf.RoundToInt(h);
		t.HFS[1] = Mathf.RoundToInt(f);
		
		EditorGUILayout.Space ();	
		EditorGUILayout.EndVertical ();
	}

	void SetPlayerFaction(){
		EditorGUILayout.LabelField("Player Faction", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		t.PlayerFaction = (int)EditorGUILayout.Slider(t.Factions[t.PlayerFaction] ,t.PlayerFaction, 0, t.Factions.Length - 1);
		EditorGUILayout.Space ();	
		EditorGUILayout.EndVertical ();
	}

	void SetPlayerRelations(){
		EditorGUILayout.LabelField("Player Relations Matrix", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 0;
		
		for (int i = 0; i < t.PlayerRelations.Length; i++){//display  all the elements of the array
			EditorGUILayout.Space ();
			t.PlayerRelations[i] = EditorGUILayout.Slider("Player  <---->  " + t.Factions[i].ToString(),t.PlayerRelations[i], -1000, 1000);
		}

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void SetFactionRelations(){
		EditorGUILayout.LabelField("Faction Relations Matrix", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUI.indentLevel = 0;

		int[] cnt = new int[3];

		for(int n = 0; n < UniqueRelations; n++){
			if(n < NumberOfFactions - 1){
				EditorGUILayout.LabelField("Unique Relations (" + t.Factions[n].ToString() + ")", EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical ("HelpBox");
			}

			for(int x = 0; x < ((NumberOfFactions - 1) - cnt[2]); x++){
				EditorGUILayout.Space ();
				EditorGUILayout.LabelField(t.Factions[cnt[2]].ToString() + "  <---->  " + t.Factions[((cnt[2] + cnt[1]) + 1)]);
				t.StartRelations[cnt[0]] = EditorGUILayout.Slider( "Faction Relation: ",t.StartRelations[cnt[0]], -1000, 1000);

				if(cnt[0] < UniqueRelations){
					cnt[0]++;
				}

				cnt[1]++;
			}

			if(n < NumberOfFactions - 1){
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical ();
			}

			if(cnt[1] > 0){
				EditorGUILayout.Space ();
				EditorGUILayout.Space ();
			}

			cnt[1] = 0;
			cnt[2]++;
		}

		EditorGUILayout.EndVertical ();
	}
}
