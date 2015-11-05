using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CustomEditor(typeof(FX_3DRadar_Mgr))]

public class FX_3DRadar_M_Editor : Editor {
	FX_3DRadar_Mgr t;


	bool Help_RRTT;
	bool Help_RRTT1;
	bool Help_RRTT1_txt;

	void OnEnable(){
		t = (FX_3DRadar_Mgr)target;
        if(PrefabUtility.GetPrefabType(t) == PrefabType.PrefabInstance){
            PrefabUtility.DisconnectPrefabInstance(t.gameObject);
        }
	}

	public override void OnInspectorGUI(){

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		GUI.color = new Color(.5f,1f,.0f,1);
		EditorGUILayout.LabelField("Menu Selection", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		t.RSetup = (FX_3DRadar_Mgr._rSetup)EditorGUILayout.EnumPopup("", t.RSetup);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();

		if (t.RadarEnabled) {
			if (GUILayout.Button ("Disable Radar", GUILayout.MaxWidth (110), GUILayout.MaxHeight (20))) {
				t.DisableRadar ();
			}
		} else {
			if (GUILayout.Button ("Enable Radar", GUILayout.MaxWidth (110), GUILayout.MaxHeight (20))) {
				t.EnableRadar();
			}
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		switch((int)t.RSetup){

			case 0:
				WorldScale();
				Player();
				RadarListFilters();
				RadarConfig();
				BlindRadar();
				RenderOptions();
			break;

			case 1:
			IFFColors();
			EditorGUILayout.Space ();
			HUDDisplaySettings();
			EditorGUILayout.Space ();
			Padding();
			EditorGUILayout.Space ();
			MouseSelection();
			EditorGUILayout.Space ();
			NAVSettings();
			EditorGUILayout.Space ();
			DIASettings();
			EditorGUILayout.Space ();
			TargetSelectionIndicator();
			EditorGUILayout.Space ();
			TLISettings();
			EditorGUILayout.Space ();
			HUDIDSettings();
			EditorGUILayout.Space ();
			BoundIndicators();
			EditorGUILayout.Space ();
			RIDSettings();
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			break;

			case 2:
				RTTSettings ();
			break;

			case 3:

			break;
		}
	}

	void RTTSettings(){
		EditorGUILayout.BeginVertical("HelpBox");
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render To Texture");
		t.RenderToTexture = EditorGUILayout.Toggle("", t.RenderToTexture);
		EditorGUILayout.EndHorizontal();

		if(t.RenderToTexture){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Background Color");
			t.BGColor = EditorGUILayout.ColorField ("",t.BGColor);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Render Texture", GUILayout.MaxWidth(100));
			t.RT = (RenderTexture)EditorGUILayout.ObjectField("",t.RT, typeof(RenderTexture),true,GUILayout.MaxWidth(125));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Render Material", GUILayout.MaxWidth(100));
			t.RenderMaterial = (Material)EditorGUILayout.ObjectField("",t.RenderMaterial, typeof(Material),true,GUILayout.MaxWidth(125));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Render Target", GUILayout.MaxWidth(100));
			t.RenderTarget = (GameObject)EditorGUILayout.ObjectField("",t.RenderTarget, typeof(GameObject),true,GUILayout.MaxWidth(125));
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndVertical();
	}

	void Player(){
		EditorGUILayout.LabelField("Player Properties", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Player :", GUILayout.MaxWidth(100));
		if(!t.Player){
			GUI.color = Color.red;
		}
		t.Player = (Transform)EditorGUILayout.ObjectField("",t.Player, typeof(Transform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Player Camera:", GUILayout.MaxWidth(100));
		if(!t.PlayerCameraC){
			GUI.color = Color.red;
		}
		t.PlayerCameraC = (Camera)EditorGUILayout.ObjectField("",t.PlayerCameraC, typeof(Camera),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radar Camera :", GUILayout.MaxWidth(100));
		if(!t.RadarCamera){
			GUI.color = Color.red;
		}
		t.RadarCamera = (Camera)EditorGUILayout.ObjectField("",t.RadarCamera, typeof(Camera),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("HUD Canvas", GUILayout.MaxWidth(100));
		if(!t.HUDCanvas){
			GUI.color = Color.red;
		}
		t.HUDCanvas = (RectTransform)EditorGUILayout.ObjectField("",t.HUDCanvas, typeof(RectTransform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		t.HUDCanvasLayer = EditorGUILayout.LayerField("HUD Canvas", t.HUDCanvasLayer);

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Radar UI", GUILayout.MaxWidth(100));
		if(!t.RadarUI){
			GUI.color = Color.red;
		}
		t.RadarUI = (RectTransform)EditorGUILayout.ObjectField("",t.RadarUI, typeof(RectTransform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
	}

	void NAVSettings(){
		EditorGUILayout.LabelField("NAV Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("NAV HUD Dispaly", GUILayout.MaxWidth(210));
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
		if(!t.Sprite_NAV){
			GUI.color = Color.red;
		}
		t.Sprite_NAV = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_NAV, typeof(Sprite), true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("NAV Radar Display", GUILayout.MaxWidth(210));

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Render NAV In Radar");
		t.RenderNAVRadar = EditorGUILayout.Toggle("", t.RenderNAVRadar);
		EditorGUILayout.EndHorizontal();

		if(t.RenderNAVRadar){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
			if(!t.Sprite_RadarNAV){
				GUI.color = Color.red;
			}
			t.Sprite_RadarNAV = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_RadarNAV, typeof(Sprite), true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.Space ();

		EditorGUILayout.EndVertical();
	}

	void RIDSettings(){
		EditorGUILayout.LabelField("Global RID Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("Radar Selection Indicator", GUILayout.MaxWidth(210));
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
		if(!t.Sprite_RadarTSI){
			GUI.color = Color.red;
		}
		t.Sprite_RadarTSI = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_RadarTSI, typeof(Sprite), true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render RID VDI");
		t.RenderVDI = EditorGUILayout.Toggle("", t.RenderVDI);
		EditorGUILayout.EndHorizontal();
		
		if(t.RenderVDI){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Render RID Base");
			t.RenderRIDB = EditorGUILayout.Toggle("", t.RenderRIDB);
			EditorGUILayout.EndHorizontal();

			t.VDIOffset = EditorGUILayout.Vector3Field("VDI Offset", t.VDIOffset);

			if(t.RenderRIDB){
				t.RIDBOffset = EditorGUILayout.Vector3Field("RID Base Offset", t.RIDBOffset);
				EditorGUILayout.Space ();
				EditorGUILayout.LabelField("RID Base Spite", GUILayout.MaxWidth(210));
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
				if(!t.Sprite_RIDBase){
					GUI.color = Color.red;
				}
				t.Sprite_RIDBase = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_RIDBase, typeof(Sprite), true,GUILayout.MaxWidth(125));
				GUI.color = Color.white;
				EditorGUILayout.EndHorizontal();
			}
		}else{
			t.RenderRIDB = false;
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
	}

	void TargetSelectionIndicator(){
		EditorGUILayout.LabelField("Target Selection Indicator Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Disable Off Screen");
		t.DisableTSIOS = EditorGUILayout.Toggle("", t.DisableTSIOS);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Indicator", GUILayout.MaxWidth(100));
		if(!t.TSI){
			GUI.color = Color.red;
		}
		t.TSI = (RectTransform)EditorGUILayout.ObjectField("",t.TSI, typeof(RectTransform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Render Target ID", GUILayout.MaxWidth(194));
		t.RenderTSIID = EditorGUILayout.Toggle("", t.RenderTSIID,GUILayout.MaxWidth(20));
		EditorGUILayout.EndHorizontal();
		
		if(t.RenderTSIID){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Target ID", GUILayout.MaxWidth(100));
			if(!t.Image_TSIID){
				GUI.color = Color.red;
			}
			t.Image_TSIID = (Image)EditorGUILayout.ObjectField("",t.Image_TSIID, typeof(Image),true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("SubComponent Selection Indicator", GUILayout.MaxWidth(210));
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
		if(!t.Sprite_SCTSI){
			GUI.color = Color.red;
		}
		t.Sprite_SCTSI = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_SCTSI, typeof(Sprite), true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical();
	}

	void HUDDisplaySettings(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("HUD Opacity Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		t.HUDAlpha = (byte)EditorGUILayout.IntSlider("HUD Opacity", t.HUDAlpha, 0, 255);
		t.RadarRIDAlpha = (byte)EditorGUILayout.IntSlider("RID Opacity", t.RadarRIDAlpha, 0, 255);
		t.RadarVDIAlpha = (byte)EditorGUILayout.IntSlider("VDI Opacity", t.RadarVDIAlpha, 0, 255);

		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Invert Fade");
		t.FadeInvert = EditorGUILayout.Toggle("", t.FadeInvert);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fade TSI");
		t.FadeTSI = EditorGUILayout.Toggle("", t.FadeTSI);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fade TLI");
		t.FadeTLI = EditorGUILayout.Toggle("", t.FadeTLI);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fade HUDID");
		t.FadeHUDID = EditorGUILayout.Toggle("", t.FadeHUDID);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fade Bounds");
		t.FadeBounds = EditorGUILayout.Toggle("", t.FadeBounds);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fade RID's");
		t.FadeRID = EditorGUILayout.Toggle("", t.FadeRID);
		EditorGUILayout.EndHorizontal();

		t.MinFadeAmount = (byte)EditorGUILayout.IntSlider("Min Fade Amount", t.MinFadeAmount, 0, 255);

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void MouseSelection(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Mouse Selectable", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Target HUD ID");
		t.SelectableHUDID = EditorGUILayout.Toggle("", t.SelectableHUDID);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Target RID");
		t.SelectableRID = EditorGUILayout.Toggle("", t.SelectableRID);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target Object");
		t.SelectableObj = EditorGUILayout.Toggle("", t.SelectableObj);
		EditorGUILayout.EndHorizontal();

		if(t.SelectableObj){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Target Sub Component");
			t.SelectableSub = EditorGUILayout.Toggle("", t.SelectableSub);
			EditorGUILayout.EndHorizontal();
		}else{
			t.SelectableSub = false;
		}

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void RadarListFilters(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Filters & List", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target List :", GUILayout.MaxWidth(100));
		if(!t._TargetList){
			GUI.color = Color.red;
		}
		t._TargetList = (RectTransform)EditorGUILayout.ObjectField("",t._TargetList, typeof(RectTransform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Target List Content:", GUILayout.MaxWidth(100));
		if(!t._Content){
			GUI.color = Color.red;
		}
		t._Content = (RectTransform)EditorGUILayout.ObjectField("",t._Content, typeof(RectTransform),true,GUILayout.MaxWidth(125));
		GUI.color = Color.white;
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space ();
		if(!t.EnableTargetList){
			if(GUILayout.Button("Open Target List",GUILayout.MaxWidth(110),GUILayout.MaxHeight(20))){
				t.EnableTargetList = true;
				t._TargetList.gameObject.SetActive(true);
				t.DisplayTargetListAll();
			}
		}else if(GUILayout.Button("Close Target List",GUILayout.MaxWidth(110),GUILayout.MaxHeight(20))){
			t.EnableTargetList = false;
			t._TargetList.gameObject.SetActive(false);
			t.ListTargets = FX_3DRadar_Mgr._listTargets.DisableList;
			t.ClearList();
		}
		EditorGUILayout.LabelField("    " + t.ListTargets.ToString ());
		//t.ListTargets = (FX_3DRadar_Mgr._listTargets)EditorGUILayout.EnumPopup("", t.ListTargets,GUILayout.MaxWidth(110),GUILayout.MaxHeight(20));
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Filter Hostile Only");
		t.FilterHostile = EditorGUILayout.Toggle("", t.FilterHostile);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void BlindRadar(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Blind Radar Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Enable Blind Radar");
		t.EnableBlindRadar = EditorGUILayout.Toggle("", t.EnableBlindRadar);
		EditorGUILayout.EndHorizontal();

			if(t.EnableBlindRadar){
				EditorGUILayout.Space ();
				t.RadarUpdateTime = EditorGUILayout.FloatField("Radar Refresh (sec)", t.RadarUpdateTime);
				t.RadarResetTime = EditorGUILayout.FloatField("Auto Reaquire Time (sec)", t.RadarResetTime);
				
			int o = t.BlindRadarLayers.Length;

			if(o < 1){
				System.Array.Resize(ref t.BlindRadarLayers, 1);
			}
				
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("Occlusion Layers", EditorStyles.boldLabel);
			if(GUILayout.Button("Add Occlusion Layer",GUILayout.MaxWidth(130),GUILayout.MaxHeight(20))){
				System.Array.Resize(ref t.BlindRadarLayers, o + 1);
			}
			EditorGUILayout.Space ();
				for(int i = 0; i < o; i++){
					EditorGUILayout.BeginHorizontal();
					t.BlindRadarLayers[i] = EditorGUILayout.LayerField("Occlusion Layer (" + i.ToString() + ")", t.BlindRadarLayers[i], GUILayout.MaxWidth(220));
					if(o > 1 && GUILayout.Button("-",GUILayout.MaxWidth(15),GUILayout.MaxHeight(15))){
						ResizeArray(i);
						break;
					}
					EditorGUILayout.EndHorizontal();
				}
			}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void ResizeArray(int remove){
		int[] tempArray = new int[t.BlindRadarLayers.Length];
		int cnt = 0;
		for(int i = 0; i < tempArray.Length; i++){
			if(i != remove){
				tempArray[cnt] = t.BlindRadarLayers[i];
				cnt++;
			}
		}
		System.Array.Resize(ref tempArray, tempArray.Length - 1);
		t.BlindRadarLayers = tempArray;
	}

	void BoundIndicators(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Global Bounds Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render Bounds");
		t.RenderBounds = EditorGUILayout.Toggle("", t.RenderBounds);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space ();

		if(t.RenderBounds){

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Advanced Bounds");
			t.AdvancedBounds = EditorGUILayout.Toggle("", t.AdvancedBounds);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Sprite ",GUILayout.MaxWidth(85));
			if(!t.Sprite_BoundSquare){
				GUI.color = Color.red;
			}
			t.Sprite_BoundSquare = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_BoundSquare, typeof(Sprite), true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Display Bounds");
			t.G_DisplayBounds = EditorGUILayout.Toggle("", t.G_DisplayBounds);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Limit Screen Size");
			t.LBS = EditorGUILayout.Toggle("", t.LBS);
			EditorGUILayout.EndHorizontal();

			t.MBS = EditorGUILayout.IntField("Max Size (Pixels)", t.MBS);
			t.BPadding = EditorGUILayout.IntField("Padding", t.BPadding);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void WorldScale(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("World Scale", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("World Scale : Default 1 unit = 1m", EditorStyles.boldLabel);
		t.WorldScale = EditorGUILayout.FloatField("* Scale :", t.WorldScale);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void RadarConfig(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Radar Detection Range", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		t.RadarRange = EditorGUILayout.FloatField("* Radar Range :", t.RadarRange);
		EditorGUILayout.Space ();
		t.RadarZoom = (FX_3DRadar_Mgr._radarZoom)EditorGUILayout.EnumPopup("Radar Zoom Level :", t.RadarZoom);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void IFFColors(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("IFF Color Assignments", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		t.IFFColor[0] = EditorGUILayout.ColorField("Color Neutral", t.IFFColor[0]);
		t.IFFColor[1] = EditorGUILayout.ColorField("Color Friendly", t.IFFColor[1]);
		t.IFFColor[2] = EditorGUILayout.ColorField("Color Hostile", t.IFFColor[2]);
		t.IFFColor[3] = EditorGUILayout.ColorField("Color Unknown", t.IFFColor[3]);
		t.IFFColor[4] = EditorGUILayout.ColorField("Color Abandoned", t.IFFColor[4]);
		t.IFFColor[5] = EditorGUILayout.ColorField("Color Player Owned", t.IFFColor[5]);
		t.IFFColor[6] = EditorGUILayout.ColorField("Color Objective", t.IFFColor[6]);
		t.IFFColor[7] = EditorGUILayout.ColorField("Color NAV", t.IFFColor[7]);

		t.UseObjectiveColor = EditorGUILayout.Toggle("Objective Coolor", t.UseObjectiveColor);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void Padding(){
		EditorGUILayout.LabelField("Screen Edge Padding (Pixels)", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();
		t.HUDMainOffset = EditorGUILayout.IntField("Target Selection : ",t.HUDMainOffset);
		t.HUDIDPadding = EditorGUILayout.IntField("Target Indicators : ",t.HUDIDPadding);
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void HUDIDSettings(){
		EditorGUILayout.LabelField("Global HUD Identifier Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render HUD ID's");
		t.RenderHUDID = EditorGUILayout.Toggle("", t.RenderHUDID);
		EditorGUILayout.EndHorizontal();

		if(t.RenderHUDID){
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Display As ID");
			t.HUDIAsIcon = EditorGUILayout.Toggle("", t.HUDIAsIcon);
			EditorGUILayout.EndHorizontal();

			if(!t.HUDIAsIcon){
				EditorGUILayout.Space ();
				EditorGUILayout.LabelField("Default HUD ID", GUILayout.MaxWidth(210));
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
				t.Sprite_HUDID = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_HUDID, typeof(Sprite), true,GUILayout.MaxWidth(125));
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("HUD ID Selection Indicator", GUILayout.MaxWidth(210));
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Sprite",GUILayout.MaxWidth(85));
			if(!t.Sprite_TSI2){
				GUI.color = Color.red;
			}
			t.Sprite_TSI2 = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_TSI2, typeof(Sprite), true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Display HUD ID's");
			t.G_DisplayHUDID = EditorGUILayout.Toggle("", t.G_DisplayHUDID);
			EditorGUILayout.EndHorizontal();
		}
			EditorGUILayout.Space ();
			EditorGUILayout.EndVertical ();

	}

	void TLISettings(){
		EditorGUILayout.LabelField("Target Lead Indicator Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render TLI");
		t.RenderTLI = EditorGUILayout.Toggle("", t.RenderTLI);
		EditorGUILayout.EndHorizontal();

		if(t.RenderTLI){

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Sprite ",GUILayout.MaxWidth(85));
			if(!t.Sprite_TLI){
				GUI.color = Color.red;
			}
			t.Sprite_TLI = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_TLI, typeof(Sprite), true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Use Player Physics");
			t.UsePlayerPhysics = EditorGUILayout.Toggle("", t.UsePlayerPhysics);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Use Target Physics");
			t.UseTargetPhysics = EditorGUILayout.Toggle("", t.UseTargetPhysics);
			EditorGUILayout.EndHorizontal();

			t.ProjectileVelocity = EditorGUILayout.IntField("Projectile Velocity : ",t.ProjectileVelocity);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}

	void DIASettings(){
		EditorGUILayout.LabelField("Directional Indicator Arrow Settings", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("* Render DIA");
		t.RenderHUDDIA = EditorGUILayout.Toggle("", t.RenderHUDDIA);
		EditorGUILayout.EndHorizontal();

		if(t.RenderHUDDIA){
			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("* Sprite : ID",GUILayout.MaxWidth(85));
			if(!t.Sprite_DIA){
				GUI.color = Color.red;
			}
			t.Sprite_DIA = (Sprite)EditorGUILayout.ObjectField("", t.Sprite_DIA, typeof(Sprite), true,GUILayout.MaxWidth(125));
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space ();
			t.HUDDIARad = EditorGUILayout.IntField("Radius : ",t.HUDDIARad);

			EditorGUILayout.Space ();
			t.DIAOffset = EditorGUILayout.IntField("Offset : ",t.DIAOffset);
		}
		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}


	void RenderOptions(){
		EditorGUI.indentLevel = 0;
		EditorGUILayout.Space ();
		EditorGUILayout.LabelField("Radar Display Setup", EditorStyles.boldLabel);
		GUI.color = new Color(.7f,.7f,.7f,1);
		EditorGUILayout.BeginVertical ("HelpBox");
		GUI.color = Color.white;
		EditorGUILayout.Space ();

		EditorGUILayout.LabelField("Select Radar Display Method");

		if(t.Radar2D){
			if(GUILayout.Button("Switch To 3D",GUILayout.MaxWidth(150),GUILayout.MaxHeight(20))){
				t.Radar3D_2D();
			}
		}else{
			if(GUILayout.Button("Switch To 2D",GUILayout.MaxWidth(150),GUILayout.MaxHeight(20))){
				t.Radar3D_2D();
			}
		}

		if(!t.Radar2D){
			if(t.PerspectiveRadar){
				if(GUILayout.Button("Switch To Orthographic",GUILayout.MaxWidth(150),GUILayout.MaxHeight(20))){
					t.SelectRadarStyle();
				}
			}else{
				if(GUILayout.Button("Switch To Perspective",GUILayout.MaxWidth(150),GUILayout.MaxHeight(20))){
					t.SelectRadarStyle();
				}
			}
		}

		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		t.RadarPos = (FX_3DRadar_Mgr._radarPos)EditorGUILayout.EnumPopup("Radar Position :", t.RadarPos);

		EditorGUILayout.Space ();
		EditorGUILayout.EndVertical ();
	}
}