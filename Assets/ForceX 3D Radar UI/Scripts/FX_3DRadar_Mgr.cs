using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FX_3DRadar_Mgr : MonoBehaviour {
	#region Inspector Current Window
	public enum _rSetup {RadarSettings, HUDDisplaySettings, RenderTextureSettings}
	public _rSetup RSetup;
	#endregion

	int ButtonPadding = 2;

	#region Layers
	public int VFBLayer;
	public int[] BlindRadarLayers = new int[1];
	public int HUDCanvasLayer;
	public int RTTLayer;
	#endregion

	#region External Scripts
	FX_Input_Mgr FXIM;
	FX_3DRadar_RID TargetRID;
	public FX_Class_Mgr FXCM;
	public FX_Mission_Mgr FXMM;
	public FX_Faction_Mgr FXFM;
	#endregion

	#region Player Cache
	//Player
	public Transform Player;
	public Vector3 PlayerPos;

	//Player Camera
	public Camera PlayerCameraC;
	public Transform PlayerCameraT;
	public Vector3 PlayerCamPos;

	public RectTransform HUDCanvas;
	public RectTransform RadarUI;

	//Radar Camera
	public Camera RadarCamera;
	#endregion

	#region Radar Settings & Globals
	public bool RadarEnabled = true;
	bool FirstSetup;
	public float WorldScale = 1.0f;  //World Scale is used to define the size of a world unit in meters. The default 1.0f = 1x1x1 meters. Example a value of 10 will make a world unit equal 10x10x10 meters.
	public bool PerspectiveRadar;
	public bool Radar2D;
	
	public enum _radarZoom {Normal = 0, Zoom_In_2X = 1, Zoom_In_X4 = 2, Boost_1_5 = 3, Boost_2 = 4}
	public _radarZoom RadarZoom;
	/*Temp Replace With Input Method*/int CurrentZoom;
	
	public float RadarRange = 100.0f; //The distance in meters the radar can detect an active target from the player.
	public float RadarRangeSQR; //PreCompute Radar Range with scaling. This is used by all potental targets for distance calculations.
	public float RadarLocalScale; //Scales the Targets distance in the Radar view.

	public enum _radarPos {CustomPosition, TopLeft, TopRight, BottomLeft, BottomRight}
	public _radarPos RadarPos;
	#endregion

	#region Blind Radar Settings
	public bool EnableBlindRadar = false;
	public float RadarUpdateTime = 0.5f;
	public float RadarResetTime = 5.0f;
	#endregion

	#region Target/NAV List & Current Target/NAV Info
	public bool EnableTargetList;

	//Target List
	public RectTransform _TargetList;
	public RectTransform _Content;

	public enum _listTargets {DisableList, ListNAV, ListAllTargets, ListNeutralTargets, ListFriendlyTargets, ListHostileTargets, ListOwnedTargets, ListAbandonedTargets, ListObjectiveTargets}
	public _listTargets ListTargets;

	public List<Transform> TargetListAll = new List<Transform>(); //A list contaning all avaliable targets that are in the Players radar range.
	public List<Transform> TargetListNeutral = new List<Transform>(); //A list contaning all Neutral targets that are in the Players radar range.
	public List<Transform> TargetListFriendly = new List<Transform>(); //A list contaning all Friendly targets that are in the Players radar range.
	public List<Transform> TargetListHostile = new List<Transform>(); //A list contaning all Hostile targets that are in the Players radar range.
	public List<Transform> TargetListAband = new List<Transform>(); //A list contaning all Abandoned targets that are in the Players radar range.
	public List<Transform> TargetListOwned = new List<Transform>(); //A list contaning all Owned targets that are in the Players radar range.
	public List<Transform> TargetListObj = new List<Transform>(); //A list contaning all Owned targets that are in the Players radar range.
	public List<Transform> TargetListSubC = new List<Transform>(); //A list contaning the Sub Compoents of a selected target.

	public Transform CurTarget; //The currently selected target.
	Transform CurTargetSub; //The selected Sub Component of the Current Target.

	int CurTargetCnt;
	int CurTargetSubCnt;
	#endregion

	#region MISC Globals
	public int ProjectileVelocity = 100;
	public float VFBounds;
	public float CurTime;
	public int ScreenHeight;
	public int ScreenWidth;
	#endregion

	#region Global Bools

	public bool SelectableHUDID = true; // Target mouse selectable VIA HUD ID icon 
	public bool SelectableRID = true; // Target mouse selectable VIA radar RID icon
	public bool SelectableObj = true; // Target mouse selectable VIA target Object
	public bool SelectableSub = true; // Target Sub Component mouse selectabe VIA sub component Object

	public bool RenderNAVRadar; // Display the NAV in the radar
	public bool DisableTSIOS = true;  // Disable main target selection indicator when target is off screen

	public bool RenderHUDID;
	public bool RenderHUDDIA;
	public bool RenderBounds;
	public bool AdvancedBounds = true;
	public bool RenderTLI = true;
	public bool RenderTSIID = true; // Display the Target Selection Indicator target ID icon 
	public bool RenderVDI = true; // Display the radars Vertical Directional Indicator line. Will also disable the RID Base icon
	public bool RenderRIDB = true; // Display the radars RID Base icon.

	public bool HUDEnabled;
	public bool TSIEnabled;
	public bool TSI2Enabled;
	public bool DIAEnabled;
	bool TLIEnabled;
	bool TSISCEnabled;

	public bool G_DisplayBounds = true;
	public bool G_DisplayHUDID = true;

	public bool HUDIAsIcon;
	public bool FadeInvert;
	public bool FadeBounds;
	public bool FadeHUDID;
	public bool FadeTSI;
	public bool FadeRID;
	public bool FadeTLI;
	public bool UseObjectiveColor;

	public bool FilterHostile;
	public bool TargetOffScreen;
	#endregion

	#region HUD Elements
	public RectTransform TSI; //The Target Selection Indicator.
	public RectTransform TSISC; //The Sub Component Target Selection Indicator
	public RectTransform TLI; //The Target Lead Indicator.
	public RectTransform TSIID; //The Target Selection Indicator Target ID Icon.
	public RectTransform TSI2;
	public RectTransform RTSI; //The Radar Target Selection Box.
	public RectTransform RadarNAV;
	RectTransform DIA;

	public Image Image_TSI;
	public Image Image_TSIID;
	public Image Image_TSISC;
	public Image Image_TLI;
	public Image Image_TSI2;
	public Image Image_RTSI;
	public Image Image_DIA;

	public Sprite Sprite_TLI;
	public Sprite Sprite_SCTSI;
	public Sprite Sprite_TSI2;
	public Sprite Sprite_HUDID;
	public Sprite Sprite_RadarTSI;
	public Sprite Sprite_RIDBase;
	public Sprite Sprite_RadarNAV;
	public Sprite Sprite_DIA;
	public Sprite Sprite_NAV;
	public Sprite Sprite_BoundSquare;
	public Sprite Sprite_SolidColor;
	#endregion

	#region HUD Screen Padding
	public int HUDMainOffset = 32;
	public int HUDIDPadding = 16;
	#endregion

	#region IFF Colors
	//Color Key : 0 = Color Neutral, 1 = Color Friendly, 2 = Color Hostile, 3 = Color Unknown, 4 = Color Abandoned, 5 = Color Player Owned, 6 = Color Objective 7 = Color NAV
	public Color32[] IFFColor = new Color32[8]{
		new Color32(255,255,255,255), 
		new Color(0,255,0,255),
		new Color32(255,0,0,255), 
		new Color32(128,128,128,255), 
		new Color32(255,0,255,255),
		new Color32(0,255,255,255), 
		new Color32(255, 255, 26, 255),
		new Color32(255, 128, 0, 255)};

	public byte HUDAlpha = 255;
	public byte RadarRIDAlpha = 255;
	public byte RadarVDIAlpha = 50;
	public byte MinFadeAmount = 25;
	#endregion

	#region Bounding Indicator Settings
	public bool LBS;
	public int MBS = 64;
	public int BPadding = 0;
	#endregion

	#region Directional Indicator Arrow Settings
	public bool RenderDIA; // Directional Indicator Arrow
	public int HUDDIARad = 120; // The radius of the Directional Indicator Arrow
	public int DIAOffset = 25;
	#endregion

	#region Target Lead Settings
	//Player Information
	public bool UsePlayerPhysics;
	Rigidbody PlayerRigidbody;
	Vector3 PlayerPreviousPos;
	
	//Target Information
	public bool UseTargetPhysics;
	Rigidbody TargetRigidbody;
	Vector3 TargetPreviousPos;
	#endregion

	public Vector3 VDIOffset = new Vector3(1,1,0);
	public Vector3 RIDBOffset = new Vector3(0,0,0);

	public bool RenderToTexture;
	public Color32 BGColor = new Color32(0,0,0,0);
	public RenderTexture RT;
	public Material RenderMaterial;
	public GameObject RenderTarget;

	void Start(){
		//Cache Globals
		FXIM = GetComponent<FX_Input_Mgr>();
		FXFM = GetComponent<FX_Faction_Mgr>();
		FXCM = GetComponent<FX_Class_Mgr>();
		FXMM = GetComponent<FX_Mission_Mgr>();

		new GameObject ("HUDIDs").AddComponent<RectTransform> ().SetParent(RadarUI);
		new GameObject ("BoundCorners").AddComponent<RectTransform> ().SetParent(RadarUI);
		new GameObject ("RadarTargets").AddComponent<RectTransform> ().SetParent(RadarUI);

		if(UsePlayerPhysics){
			PlayerRigidbody = Player.GetComponent<Rigidbody>();
		}

		PlayerCameraT = PlayerCameraC.transform;
		ScreenHeight = (int)HUDCanvas.sizeDelta.y;
		ScreenWidth = (int)HUDCanvas.sizeDelta.x;

		TSI.sizeDelta = TSI.FindChild("TSI").GetComponent<RectTransform>().sizeDelta;
		Image_TSI = TSI.FindChild("TSI").GetComponent<Image>();
		TSIID = Image_TSIID.GetComponent<RectTransform>();
		HUDCanvas.gameObject.layer = HUDCanvasLayer;

		VFBLayer = LayerMask.NameToLayer("VFB");
		MakeVFB("VFB", transform);


		//GroupTargetElements = GameObject.Find ()
		GameObject.Find ("Radar").layer = LayerMask.NameToLayer("Radar");
		RadarCamera.cullingMask = (1 << LayerMask.NameToLayer("Radar"));

		//Initalize The Radar
		SetRadarScaleZoom();
		RadarCameraSetup();

		//Create The HUD Elements
		if (RadarEnabled) {
			CreateHUD ();


			if(RenderToTexture){
				Canvas C = new GameObject ("RadarRTTC").AddComponent<Canvas>();
				C.renderMode = RenderMode.ScreenSpaceCamera;
				C.worldCamera = RadarCamera;
				C.gameObject.layer = LayerMask.NameToLayer("Radar");
				C.planeDistance = 0.5f;
				C.pixelPerfect = true;
				C.GetComponent<RectTransform> ().SetParent (GameObject.Find("_GameMgr").GetComponent<Transform>());


				RectTransform RTC = GameObject.Find ("RadarTargets").GetComponent<RectTransform> ();
				RTC.SetParent(C.GetComponent<RectTransform>());
				RTC.anchorMax = Vector2.zero;
				RTC.anchorMin = Vector2.zero;
				RTC.anchoredPosition = Vector3.zero;

				RadarCamera.targetTexture = RT;
				RadarCamera.clearFlags = CameraClearFlags.SolidColor;
				RadarCamera.backgroundColor = BGColor;
				RadarCamera.rect = new Rect (0,0,1,1);
				RenderTarget.GetComponent<Renderer> ().material = RenderMaterial;
			}

			if (!EnableTargetList) {
				_TargetList.gameObject.SetActive (false);
			} else {
				UpdateDisplayedList ();
			}
			FirstSetup = true;
		}
	}

	/*************************************************************************************************************************************/
	//	--------------------------------------------------------------Late Update------------------------------------------------------------------------------------------------
	/*************************************************************************************************************************************/
	void LateUpdate(){

		CurTime = Time.deltaTime;
		PlayerPos = Player.position;
		PlayerCamPos = PlayerCameraT.position;

		if(RadarEnabled){
			if(!FirstSetup){
				Start ();
			}
			if(ScreenHeight != HUDCanvas.sizeDelta.y){
				ResetScale();
			}

			//!!!!---Change This Method From State Check To Button Call
			// Change The Radar Zoom / Boost Amount
			if(CurrentZoom != (int)RadarZoom){
				SetRadarScaleZoom();
			}

			InputMonitor();
	        MouseSelectObject();

			if(CurTarget){
				Radar();
				if(RenderHUDDIA){
					DrawDIA(CurTarget, DIA);
				}
			}else if(HUDEnabled){
				DisableTargetIndicators();
				DisableTLI();
				if(TSI2Enabled){
					Image_TSI2.enabled = false;
					TSI2Enabled = false;
				}
			}
		}
	}

	public void SelectRadarStyle(){
		PerspectiveRadar = !PerspectiveRadar;
		if(PerspectiveRadar){
			RadarCamera.orthographic = false;
		}else{
			RadarCamera.orthographic = true;
		}
	}

	public void Radar3D_2D(){// Switch between 2D / 3D Camera views
		Radar2D = !Radar2D;
		if(Radar2D){
			RadarCamera.orthographic = true;
			RadarCamera.transform.eulerAngles = new Vector3(90,0,0);
			RadarCamera.transform.position = new Vector3(0,0.85f, 0);
		}else{
			RadarCamera.transform.eulerAngles = new Vector3(42,0,0);
			RadarCamera.transform.position = new Vector3(0,0.85f, -0.93f);
			if(PerspectiveRadar){
				RadarCamera.orthographic = false;
			}else{
				RadarCamera.orthographic = true;
			}
		}
	}

	/*************************************************************************************************************************************/
	//																						Radar Main Loop
	/*************************************************************************************************************************************/
	public void Radar(){
		Vector3 TargetPos = CurTarget.position;
		Vector3 TargetSubPos = new Vector3(0,0,0);
		if(CurTargetSub){
			TargetSubPos = CurTargetSub.position;
		}

		TSI.anchoredPosition = GetScreenPos(TargetPos, true, HUDMainOffset);

		if (CurTargetSub) {
			TSISC.anchoredPosition = GetScreenPos (TargetSubPos, false, 0);
			FindTargetLead (TargetSubPos);
		} else {
			FindTargetLead (TargetPos);
		}
	}

	void FindTargetLead(Vector3 TargetPos){
		float SmoothDT = Time.smoothDeltaTime;
		
		//Get Player Velocity
		Vector3 PlayerVel = Vector3.zero;
		Vector3 TargetVel = Vector3.zero;
		
		if (UsePlayerPhysics) {
			PlayerVel = PlayerRigidbody.velocity;
		} else {
			PlayerVel = (PlayerPos - PlayerPreviousPos) / SmoothDT;
			PlayerPreviousPos = PlayerPos;
		}
		
		//Get Target Velocity
		if(UseTargetPhysics){
			TargetVel = TargetRigidbody.velocity;
		}else{
			TargetVel = (TargetPos - TargetPreviousPos) / SmoothDT;
			TargetPreviousPos = TargetPos;
		}

		if(RenderTLI){
			TLI.anchoredPosition = TLIScreenPos(TargetLead.FirstOrderIntercept(PlayerPos, PlayerVel, TargetPos, TargetVel, ProjectileVelocity));
		}
	}

	Vector3 TLIScreenPos(Vector3 Target){
		Vector3 ScreenPos = PlayerCameraC.WorldToScreenPoint(Target);
		bool IsFront = Vector3.Dot (PlayerCameraT.TransformDirection(Vector3.forward), Target - PlayerCameraT.position) > 0;
		bool OffScreen = (ScreenPos.x > ScreenWidth || ScreenPos.x < 0 || ScreenPos.y > ScreenHeight || ScreenPos.y < 0);

		if(IsFront && !OffScreen){
			if(!TLIEnabled){
				EnableTLI();
			}
		}else if(!IsFront || OffScreen){
			if(TLIEnabled){
				DisableTLI();
			}
		}
		return ScreenPos;
	}

	Vector3 GetScreenPos(Vector3 Target, bool IsTSI, int Offset){
		Vector3 ScreenPos = PlayerCameraC.WorldToScreenPoint(Target);

		if(TargetRID.OffScreen){
			if(IsTSI && DisableTSIOS){
				if(TSIEnabled){
					DisableTSI();
					Image_TSISC.enabled = false;
					TSISCEnabled = false;
				}
				TargetOffScreen = true;
			}else{
				RaycastHit Hit;
				Vector3 TRelPos;
				TRelPos = PlayerCameraT.InverseTransformPoint(Target);

				if(IsTSI){
					if(!TSIEnabled){
						EnableTargetIndicators();
					}
					//Change TSI Texture
				}
				
				//Disable The Sub Component Target Selection Box
				if(IsTSI && TSISCEnabled){
					Image_TSISC.enabled = false;
					TSISCEnabled = false;
				}
				
				if(ScreenPos.x ==0.5f && ScreenPos.y ==0.5f){
					TRelPos.y = 1;
				}
				
				Physics.Raycast (Vector3.zero, new Vector3(TRelPos.x, TRelPos.y, 0),out Hit, 2, 1<<VFBLayer);
				ScreenPos = new Vector3((((VFBounds * 0.5f) + Hit.point.x) / VFBounds) * ScreenWidth, (0.5f + Hit.point.y) * ScreenHeight, 0 );
			}
		}else{
			if(IsTSI){
				if(!TSIEnabled || !HUDEnabled){
					EnableTargetIndicators();
				}
				TargetOffScreen = false;
				//Change TSI Texture
			}

			//Enable The Sub Component Target Selection Box If Active Sub Component Is Selected
			if(!IsTSI && CurTargetSub && !TSISCEnabled && TSIEnabled){
				Image_TSISC.enabled = true;
				TSISCEnabled = true;
			}
		}

		// Screen Edge Offset
		if(ScreenPos.x < Offset){
			ScreenPos.x = Offset;
		}
		
		if(ScreenPos.x > ScreenWidth - Offset){
			ScreenPos.x = ScreenWidth - Offset;
		}

		if(ScreenPos.y < Offset){
			ScreenPos.y = Offset;
		}
		
		if(ScreenPos.y > ScreenHeight - Offset){
			ScreenPos.y = ScreenHeight - Offset;
		}
		
		return ScreenPos;
	}

	void DrawDIA(Transform ThisObject, Transform ThisIndicator){
		Vector3 ThisScreenPos = GetScreenPos(ThisObject.position, false, HUDMainOffset);
		Vector3 DIAPos = DIA.position;
		Vector3 dir = ThisScreenPos - DIAPos;
		float angle = (Mathf.Atan2(dir.y, dir.x)) * Mathf.Rad2Deg;

		DIA.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

		int newrot = HUDDIARad + DIAOffset;

		if(dir.sqrMagnitude < (newrot * newrot)){
			if(DIAEnabled){
				DIA.gameObject.SetActive(false);
				DIAEnabled = false;
			}
		}else if(!DIAEnabled){
			DIA.gameObject.SetActive(true);
			DIAEnabled = true;
		}
	}


	void SetRadarScaleZoom(){
		float RadarZoomAmount = 0.0f;
		
		switch ((int)RadarZoom){
		case 0:
			RadarZoomAmount = WorldScale;
			break;
			
		case 1:
			RadarZoomAmount = (WorldScale * 2.0f);
			break;
			
		case 2:
			RadarZoomAmount = (WorldScale * 4.0f);
			break;
			
		case 3:
			RadarZoomAmount = (WorldScale * 0.5f);
			break;
			
		case 4:
			RadarZoomAmount = (WorldScale * 0.25f);
			break;
		}
		RadarRangeSQR = (RadarRange * RadarRange) / (RadarZoomAmount * RadarZoomAmount);
		RadarLocalScale = (1 / ((RadarRange / RadarZoomAmount) * 2));
		CurrentZoom = (int)RadarZoom;
	}
	
	/*************************************************************************************************************************************/
	//																						Monitor User Input
	/*************************************************************************************************************************************/
	void InputMonitor(){
		// Find the closest target to the player
		if((int)FXIM.TargetClosestKM == 0 && Input.GetKeyDown(FXIM.TargetClosest)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			ClosestTarget();
		}else if((int)FXIM.TargetClosestKM == 1 && Input.GetKeyDown(FXIM.TargetClosest) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			ClosestTarget();	
		}else if((int)FXIM.TargetClosestKM == 2 && Input.GetKeyDown(FXIM.TargetClosest) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			ClosestTarget();	
		}else if((int)FXIM.TargetClosestKM == 3 && Input.GetKeyDown(FXIM.TargetClosest) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			ClosestTarget();
		}
		
		// Find the next target in the array
		if((int)FXIM.TargetNextKM == 0 && Input.GetKeyDown(FXIM.TargetNext)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			NextTarget();
		}else if((int)FXIM.TargetNextKM == 1 && Input.GetKeyDown(FXIM.TargetNext) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			NextTarget();
		}else if((int)FXIM.TargetNextKM == 2 && Input.GetKeyDown(FXIM.TargetNext) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			NextTarget();
		}else if((int)FXIM.TargetNextKM == 3 && Input.GetKeyDown(FXIM.TargetNext) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			NextTarget();
		}
		
		// Find the previous target in the array
		if((int)FXIM.TargetPrevKM == 0 && Input.GetKeyDown(FXIM.TargetPrev)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			PreviousTarget();
		}else if((int)FXIM.TargetPrevKM == 1 && Input.GetKeyDown(FXIM.TargetPrev) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			PreviousTarget();
		}else if((int)FXIM.TargetPrevKM == 2 && Input.GetKeyDown(FXIM.TargetPrev) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			PreviousTarget();
		}else if((int)FXIM.TargetPrevKM == 3 && Input.GetKeyDown(FXIM.TargetPrev) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}
			PreviousTarget();
		}
		
		// Find the next Sub-component on the selected target	
		if((int)FXIM.TargetNextSKM == 0 && Input.GetKeyDown(FXIM.TargetNextS)){
			NextSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetNextSKM == 1 && Input.GetKeyDown(FXIM.TargetNextS) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			NextSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetNextSKM == 2 && Input.GetKeyDown(FXIM.TargetNextS) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			NextSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetNextSKM == 3 && Input.GetKeyDown(FXIM.TargetNextS) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			NextSubComp();
			//PlaySelectSCSound();
		}
		
		// Find the previous Sub-component on the selected target
		if((int)FXIM.TargetPrevSKM == 0 && Input.GetKeyDown(FXIM.TargetPrevS)){
			PreviousSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetPrevSKM == 1 && Input.GetKeyDown(FXIM.TargetPrevS) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			PreviousSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetPrevSKM == 2 && Input.GetKeyDown(FXIM.TargetPrevS) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			PreviousSubComp();
			//PlaySelectSCSound();
		}else if((int)FXIM.TargetPrevSKM == 3 && Input.GetKeyDown(FXIM.TargetPrevS) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			PreviousSubComp();
			//PlaySelectSCSound();
		}
		
		// Clear selected Sub-component
		if((int)FXIM.ClearSubCKM == 0 && Input.GetKeyDown(FXIM.ClearSubC)){
			//PlayClearSCSound();
			ClearSubC();
		}else if((int)FXIM.ClearSubCKM == 1 && Input.GetKeyDown(FXIM.ClearSubC) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			//PlayClearSCSound();
			ClearSubC();
		}else if((int)FXIM.ClearSubCKM == 2 && Input.GetKeyDown(FXIM.ClearSubC) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			//PlayClearSCSound();
			ClearSubC();
		}else if((int)FXIM.ClearSubCKM == 3 && Input.GetKeyDown(FXIM.ClearSubC) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			//PlayClearSCSound();
			ClearSubC();
		}
		
		// Clear selected Target
		if((int)FXIM.ClearTargetKM == 0 && Input.GetKeyDown(FXIM.ClearTarget)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			ClearTarget();
		}else if((int)FXIM.ClearTargetKM == 1 && Input.GetKeyDown(FXIM.ClearTarget) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			ClearTarget();
		}else if((int)FXIM.ClearTargetKM == 2 && Input.GetKeyDown(FXIM.ClearTarget) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			ClearTarget();
		}else if((int)FXIM.ClearTargetKM == 3 && Input.GetKeyDown(FXIM.ClearTarget) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			ClearTarget();
		}
		
		// Find the closest Hostile to the player
		if((int)FXIM.TargetClosestHKM == 0 && Input.GetKeyDown(FXIM.TargetClosestH)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//ClosestHostile();
		}else if((int)FXIM.TargetClosestHKM == 1 && Input.GetKeyDown(FXIM.TargetClosestH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//ClosestHostile();
		}else if((int)FXIM.TargetClosestHKM == 2 && Input.GetKeyDown(FXIM.TargetClosestH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//ClosestHostile();
		}else if((int)FXIM.TargetClosestHKM == 3 && Input.GetKeyDown(FXIM.TargetClosestH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//ClosestHostile();
		}
		
		// Find the next Hostile in the array
		if((int)FXIM.TargetNextHKM == 0 && Input.GetKeyDown(FXIM.TargetNextH)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//NextHostile();
		}else if((int)FXIM.TargetNextHKM == 1 && Input.GetKeyDown(FXIM.TargetNextH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//NextHostile();
		}else if((int)FXIM.TargetNextHKM == 2 && Input.GetKeyDown(FXIM.TargetNextH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//NextHostile();
		}else if((int)FXIM.TargetNextHKM == 3 && Input.GetKeyDown(FXIM.TargetNextH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//NextHostile();
		}
		
		// Find the previous Hostile in the array
		if((int)FXIM.TargetPrevHKM == 0 && Input.GetKeyDown(FXIM.TargetPrevH)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//PreviousHostile();
		}else if((int)FXIM.TargetPrevHKM == 1 && Input.GetKeyDown(FXIM.TargetPrevH) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//PreviousHostile();
		}else if((int)FXIM.TargetPrevHKM == 2 && Input.GetKeyDown(FXIM.TargetPrevH) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//PreviousHostile();
		}else if((int)FXIM.TargetPrevHKM == 3 && Input.GetKeyDown(FXIM.TargetPrevH) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			if(CurTarget){
				TargetRID.SetInactiveTarget();
			}	
			//PreviousHostile();
		}

		// Display / Hide Target List
		if(FXIM.TargetListKM == 0 && Input.GetKeyDown(FXIM.TargetList)){
			EnableTargetList = !EnableTargetList;

			if(EnableTargetList){
				_TargetList.gameObject.SetActive(true);
				DisplayTargetListAll();
			}else{
				_TargetList.gameObject.SetActive(false);
				ListTargets = _listTargets.DisableList;
				ClearList();
			}

		}else if((int)FXIM.TargetListKM == 1 && Input.GetKeyDown(FXIM.TargetList) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			EnableTargetList = !EnableTargetList;
		}else if((int)FXIM.TargetListKM == 2 && Input.GetKeyDown(FXIM.TargetList) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			EnableTargetList = !EnableTargetList;
		}else if((int)FXIM.TargetListKM == 3 && Input.GetKeyDown(FXIM.TargetList) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			EnableTargetList = !EnableTargetList;
		}

		/*
		// Filter Hostile
		if(FXIM.FilterHostileKM == 0 && Input.GetKeyDown(FXIM.FilterHostile)){
			FilterHostile = !FilterHostile;
		}else if(FXIM.FilterHostileKM == 1 && Input.GetKeyDown(FXIM.FilterHostile) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			FilterHostile = !FilterHostile;
		}else if(FXIM.FilterHostileKM == 2 && Input.GetKeyDown(FXIM.FilterHostile) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			FilterHostile = !FilterHostile;
		}else if(FXIM.FilterHostileKM == 3 && Input.GetKeyDown(FXIM.FilterHostile) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			FilterHostile = !FilterHostile;
		}
		*/
		/*
		// Switch 3D 2D Radar
		if(FXIM.Switch3D2DKM == 0 && Input.GetKeyDown(FXIM.Switch3D2D)){
			SetStatus[7] = !SetStatus[7];
		}else if(FXIM.Switch3D2DKM == 1 && Input.GetKeyDown(FXIM.Switch3D2D) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			SetStatus[7] = !SetStatus[7];
		}else if(FXIM.Switch3D2DKM == 2 && Input.GetKeyDown(FXIM.Switch3D2D) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			SetStatus[7] = !SetStatus[7];
		}else if(FXIM.Switch3D2DKM == 3 && Input.GetKeyDown(FXIM.Switch3D2D) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			SetStatus[7] = !SetStatus[7];
		}
		
		// Toggle Indicators
		if(FXIM.ToggleIndicatorsKM == 0 && Input.GetKeyDown(FXIM.ToggleIndicators)){
			SetStatus[13] = !SetStatus[13];
		}else if(FXIM.ToggleIndicatorsKM == 1 && Input.GetKeyDown(FXIM.ToggleIndicators) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)){
			SetStatus[13] = !SetStatus[13];
		}else if(FXIM.ToggleIndicatorsKM == 2 && Input.GetKeyDown(FXIM.ToggleIndicators) && Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)){
			SetStatus[13] = !SetStatus[13];
		}else if(FXIM.ToggleIndicatorsKM == 3 && Input.GetKeyDown(FXIM.ToggleIndicators) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)){
			SetStatus[13] = !SetStatus[13];
		}
		*/
	}

	public void ClearTarget(){//Targeting Input Command
		/***********************************************************************************/
		//Clear the current target
		/***********************************************************************************/
		if(CurTarget){
			TargetRID.SetInactiveTarget();
			TargetRigidbody = null;
			PlayClearSound();
			CurTarget = null;
		}

		ClearSubC();
	}

	public void SetTarget(Transform t){//Targeting Function
		/***********************************************************************************/
		//Gather the current target components & set target state
		/***********************************************************************************/
		ClearTarget();

		CurTarget = t;

		if(TargetRID){
			TargetRID.SetInactiveTarget();
		}

		if(CurTarget){
			PlaySelectSound();
			ClearSubC();
			TargetRID = CurTarget.GetComponent<FX_3DRadar_RID>();

			RTSI.SetParent(TargetRID.RIDI);
			RTSI.anchoredPosition = Vector2.zero;

			TSI2.SetParent(TargetRID.ThisHID);
			TSI2.anchoredPosition = Vector2.zero;

			TargetRID.SetAsActiveTarget();
			TargetRID.RenderToRadar(false);
			EnableTargetIndicators();

			if(UseTargetPhysics){
				TargetRigidbody = CurTarget.GetComponent<Rigidbody>();
			}

			//Set The HUD Target Selection Box Target Icon
			if(RenderTSIID){
				Image_TSIID.sprite = TargetRID.Image_RIDI.sprite;
				Image_TSIID.SetNativeSize();
			}
			FindSubComp();
		}
	}

	void ClosestTarget(){//Targeting Input Command
		float closestDistance = Mathf.Infinity;
		foreach (Transform t in TargetListAll){
			float curDistance = (t.position - PlayerPos).sqrMagnitude;	
			if(curDistance <= closestDistance){
				SetTarget(t);
				closestDistance = curDistance;
			}
		}
	}

	void NextTarget(){//Targeting Input Command
		if(TargetListAll.Count > 0){
			CurTargetCnt = (CurTargetCnt + 1) % TargetListAll.Count;
			SetTarget(TargetListAll[CurTargetCnt]);
		}
	}

	void PreviousTarget(){//Targeting Input Command
		if(TargetListAll.Count > 0){
			if (CurTargetCnt == 0){
				CurTargetCnt = TargetListAll.Count;
			}
			if(CurTargetCnt > 0){
				CurTargetCnt = CurTargetCnt -1;
			}
		}
		SetTarget(TargetListAll[CurTargetCnt]);
	}

	void FindSubComp(){//Targeting Function
		/***********************************************************************************/
		//Create an array of all subcomponents on the selected target
		/***********************************************************************************/
		if(CurTarget){
			TargetListSubC.Clear();
			foreach(Transform s in CurTarget){
				if(s.tag == "Sub_Component"){
					TargetListSubC.Add(s);
				}
			}
		}
	}
	
	void NextSubComp(){//Targeting Input Command
		if(CurTarget && TargetListSubC.Count > 0){
			CurTargetSubCnt = (CurTargetSubCnt + 1) % TargetListSubC.Count;
			CurTargetSub = TargetListSubC[CurTargetSubCnt];
		}
	}
	
	void PreviousSubComp(){//Targeting Input Command
		if(CurTarget && TargetListSubC.Count > 0){
			if (CurTargetSubCnt == 0){
				CurTargetSubCnt = TargetListSubC.Count;
			}
			if(CurTargetSubCnt > 0){
				CurTargetSubCnt = CurTargetSubCnt -1;
			}
			CurTargetSub = TargetListSubC[CurTargetSubCnt];
		}
	}
	
	void ClearSubC(){//Targeting Input Command
		Image_TSISC.enabled = false;
		TSISCEnabled = false;
		CurTargetSub = null;
		CurTargetSubCnt = 0;
	}

    void MouseSelectObject(){
        if (SelectableObj && Input.GetMouseButtonDown(0)){
            LayerMask mask0 = (1 << 0);
            Ray ray0 = PlayerCameraC.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit0;
            if (Physics.Raycast(ray0, out hit0, Mathf.Infinity, mask0)){

				if(hit0.transform.tag != "Sub_Component" && hit0.transform.GetComponent<FX_3DRadar_RID>().IsNAV == true){
					Debug.Log ("return");
					return;
				}

                ClearTarget();

                if (!SelectableSub || SelectableSub && hit0.transform.tag != "Sub_Component"){
                    if (hit0.transform.GetComponent<FX_3DRadar_RID>()){
						SetTarget(hit0.transform);
                    }
                    else if (hit0.transform.parent && hit0.transform.parent.GetComponent<FX_3DRadar_RID>()){
						SetTarget(hit0.transform.parent);
                    }
                }else{
					SetTarget(hit0.transform.parent);
                    CurTargetSub = hit0.transform;
                }
            }
		}
	}

	/*************************************************************************************************************************************/
	//																			Initalize The Players HUD Elements
	/*************************************************************************************************************************************/

	void RadarCameraSetup(){//Setup Radar Settings & Camera Viewport
		SetRadarScaleZoom();
		
		switch((int)RadarPos){
		case 1: // top left
			RadarCamera.targetTexture = null;
			RadarCamera.rect = new Rect( 1 - (RadarCamera.rect.x + RadarCamera.rect.width), 1 - (RadarCamera.rect.y + RadarCamera.rect.height), RadarCamera.rect.width, RadarCamera.rect.height);
			break;
			
		case 2: // top right
			RadarCamera.targetTexture = null;
			RadarCamera.rect = new Rect(RadarCamera.rect.x, 1 - (RadarCamera.rect.y + RadarCamera.rect.height), RadarCamera.rect.width, RadarCamera.rect.height);
			break;
			
		case 3: // bottom left
			RadarCamera.targetTexture = null;
			RadarCamera.rect = new Rect( 1 - (RadarCamera.rect.x +  RadarCamera.rect.width), RadarCamera.rect.y, RadarCamera.rect.width, RadarCamera.rect.height);
			break;
			
		case 4: // bottom right
			RadarCamera.targetTexture = null;
			RadarCamera.rect =new Rect(RadarCamera.rect.x, RadarCamera.rect.y, RadarCamera.rect.width, RadarCamera.rect.height);
			break;
		}
	}

	void ResetScale(){
		ScreenHeight = (int)Screen.height;
		ScreenWidth = (int)Screen.width;

		//Rebuild The View Fustrum
		Destroy(GameObject.Find("VFB"));
		MakeVFB("VFB", transform);
	}

	void CreateHUD(){

		//Make The HUD Sub Component Target Selection Box
		TSISC = MakeImage("HUD_TSISC", 0.5f);
		TSISC.gameObject.layer = HUDCanvasLayer;
		TSISC.SetParent(RadarUI.transform);
		Image_TSISC = TSISC.GetComponent<Image>();
		Image_TSISC.sprite = Sprite_SCTSI;
		Image_TSISC.SetNativeSize();
		Image_TSISC.enabled = false;

		if(RenderTLI){
			//Make The HUD Target Lead Indicator
			TLI = MakeImage("HUD_TLI",0.5f);
			TLI.gameObject.layer = HUDCanvasLayer;
			TLI.SetParent(RadarUI.transform);
			Image_TLI = TLI.GetComponent<Image>();
			Image_TLI.sprite = Sprite_TLI;
			Image_TLI.SetNativeSize();
		}

		//Make The Target Selection Box Screen Edge Indicator
		TSI2 = MakeImage("HUD_TSI2",0.5f);
		TSI2.gameObject.layer = HUDCanvasLayer;
		TSI2.SetParent(GameObject.Find("HUDIDs").transform);
		Image_TSI2 = TSI2.GetComponent<Image>();
		Image_TSI2.sprite = Sprite_TSI2;
		Image_TSI2.SetNativeSize();

		//Make The Radar Target Selection Box
		RTSI = MakeImage("Radar_TSI",0.5f);
		RTSI.SetParent(GameObject.Find("RadarTargets").transform);
		Image_RTSI = RTSI.GetComponent<Image>();
		Image_RTSI.sprite = Sprite_RadarTSI;
		Image_RTSI.SetNativeSize();

		if(RenderToTexture){
			RTSI.gameObject.layer = RTTLayer;
		}

		//Make The Directional Indicator Arrow
		if(RenderHUDDIA){
			DIA = MakeImage("HUD_DIA_Root", 0.0f);
			DIA.SetParent(RadarUI.transform);
			Destroy (DIA.GetComponent<Image>());
			RectTransform newDIA = MakeImage("HUD_DIA_Root", 0.5f);
			newDIA.SetParent(GameObject.Find("HUD_DIA_Root").transform);
			Image_DIA = newDIA.GetComponent<Image>();
			Image_DIA.sprite = Sprite_DIA;
			Image_DIA.SetNativeSize();
			
			DIA.anchoredPosition = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
			newDIA.anchoredPosition = new Vector3(0,HUDDIARad,0);
		}

		if(!CurTarget){
			DisableTargetIndicators();
			DisableTLI();
		}
	}

	public RectTransform MakeImage(string name, float Anchor){
		GameObject newImage = new GameObject(name);
		newImage.layer = 5;
		newImage.AddComponent<Image>();
		newImage.GetComponent<RectTransform>().anchorMin = new Vector2(Anchor,Anchor);
		newImage.GetComponent<RectTransform>().anchorMax = new Vector2(Anchor,Anchor);
		return newImage.GetComponent<RectTransform>();
	}

	void MakeVFB(string Name, Transform ThisParent){

		Transform ThisMesh  = new GameObject(Name).transform;
		Mesh mesh = ThisMesh.gameObject.AddComponent <MeshFilter>().mesh;
		Vector3[] vertices = new Vector3[24];

		float scale = (1.0f * ((ScreenWidth  * 1.0f) / ScreenHeight)) * 0.5f;

		//Face Down - TOP
		vertices[0] = new Vector3( -scale,0.5f,0.5f); //bottom left
		vertices[1] =  new Vector3( -scale,0.5f, -0.5f); // Top Left
		vertices[2] =  new Vector3(scale,0.5f,0.5f); // bottom right
		vertices[3] =  new Vector3(scale,0.5f, -0.5f); // Top right
		
		//Face Up - Down
		vertices[4] = new Vector3( -scale, -0.5f, -0.5f); //bottom left
		vertices[5] =  new Vector3( -scale, -0.5f,0.5f); // Top Left
		vertices[6] =  new Vector3(scale, -0.5f, -0.5f); // bottom right
		vertices[7] =  new Vector3(scale, -0.5f,0.5f); // Top right
		
		//Face Right - Left
		vertices[8] = new Vector3( -scale, -0.5f,0.5f); //bottom left
		vertices[9] =  new Vector3( -scale, -0.5f, -0.5f); // Top Left
		vertices[10] =  new Vector3(-scale,0.5f,0.5f); // bottom right
		vertices[11] =  new Vector3(-scale,0.5f, -0.5f); // Top right
		
		//Face Left - Right
		vertices[12] = new Vector3(scale, -0.5f, -0.5f); //bottom left
		vertices[13] =  new Vector3(scale, -0.5f,0.5f); // Top Left
		vertices[14] =  new Vector3(scale,0.5f, -0.5f); // bottom right
		vertices[15] =  new Vector3(scale,0.5f,0.5f); // Top right
		
		//Face Back
		vertices[16] = new Vector3( -scale,0.5f, -0.5f); //bottom left
		vertices[17] =  new Vector3( -scale, -0.5f, -0.5f); // Top Left
		vertices[18] =  new Vector3(scale,0.5f, -0.5f); // bottom right
		vertices[19] =  new Vector3(scale, -0.5f, -0.5f); // Top right
		
		//Face Front
		vertices[20] = new Vector3( -scale, -0.5f,0.5f); //bottom left
		vertices[21] =  new Vector3( -scale,0.5f,0.5f); // Top Left
		vertices[22] =  new Vector3(scale, -0.5f,0.5f); // bottom right
		vertices[23] =  new Vector3(scale,0.5f,0.5f); // Top right
		
		Vector2[] uv = new Vector2[16];
		uv[0] =  new Vector2(0, 0);
		uv[1] =  new Vector2(0, 1);
		uv[2] =  new Vector2(1, 0);
		uv[3] =  new Vector2(1, 1);
		
		uv[4] =  new Vector2(0, 0);
		uv[5] =  new Vector2(0, 1);
		uv[6] =  new Vector2(1, 0);
		uv[7] =  new Vector2(1, 1);
		
		uv[8] =  new Vector2(0, 0);
		uv[9] =  new Vector2(0, 1);
		uv[10] =  new Vector2(1, 0);
		uv[11] =  new Vector2(1, 1);
		
		uv[12] =  new Vector2(0, 0);
		uv[13] =  new Vector2(0, 1);
		uv[14] =  new Vector2(1, 0);
		uv[15] =  new Vector2(1, 1);
		
		int[] triangles = new int[24];
		
		//Top
		triangles[0] = 0;
		triangles[1] = 1; 
		triangles[2] = 2;
		triangles[3] = 2; 
		triangles[4] = 1; 
		triangles[5] = 3;
		
		//Bottom
		triangles[6] = 4;
		triangles[7] = 5; 
		triangles[8] = 6;
		triangles[9] = 6; 
		triangles[10] = 5; 
		triangles[11] = 7;
		
		//Left
		triangles[12] = 8;
		triangles[13] = 9; 
		triangles[14] = 10;
		triangles[15] = 10; 
		triangles[16] = 9; 
		triangles[17] = 11;
		
		//Right
		triangles[18] = 12;
		triangles[19] = 13; 
		triangles[20] = 14;
		triangles[21] = 14; 
		triangles[22] = 13; 
		triangles[23] = 15;
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		ThisMesh.parent = ThisParent;
		ThisMesh.localPosition = Vector3.zero;

		ThisMesh.gameObject.AddComponent<MeshCollider>();
		ThisMesh.gameObject.layer = VFBLayer;

		VFBounds = scale * 2;
	}


	/*************************************************************************************************************************************/
	//																			Set HUD Display Enable / Disable
	/*************************************************************************************************************************************/
	public void EnableTargetIndicators(){
		TSI.gameObject.SetActive(true);

		RTSI.gameObject.SetActive(true);
		HUDEnabled = true;
		TSIEnabled = true;
	}

	void DisableTargetIndicators(){
		DisableTSI();
		Image_TSI2.enabled = false;
		RTSI.gameObject.SetActive(false);
		HUDEnabled = false;
		TSIEnabled = false;
		TSI2Enabled = false;

		if(RenderHUDDIA){
			DIA.gameObject.SetActive(false);
			DIAEnabled = false;
		}
	}

	void DisableTSI(){
		TSI.gameObject.SetActive(false);
		TSIEnabled = false;
	}

	void EnableTLI(){
		if (RenderTLI) {
			TLI.gameObject.SetActive (true);
			TLIEnabled = true;
		}
	}

	void DisableTLI(){
		if(RenderTLI){
			TLI.gameObject.SetActive (false);
			TLIEnabled = false;
		}
	}

	/*************************************************************************************************************************************/
	//																						Sound FX
	/*************************************************************************************************************************************/	
	void PlaySelectSound(){

	}

	void PlayClearSound(){

	}

	/*************************************************************************************************************************************/
	//																						Display Target List
	/*************************************************************************************************************************************/	

	public void DisplayTargetListAll(){
		ClearList();
		BuildList(TargetListAll);
		ListTargets = _listTargets.ListAllTargets;
	}

	public void DisplayTargetListHostile(){
		ClearList();
		BuildList(TargetListHostile);
		ListTargets = _listTargets.ListHostileTargets;

	}

	public void DisplayTargetListFriendly(){
		ClearList();
		BuildList(TargetListFriendly);
		ListTargets = _listTargets.ListFriendlyTargets;

	}

	public void DisplayTargetListOwned(){
		ClearList();
		BuildList(TargetListOwned);
		ListTargets = _listTargets.ListOwnedTargets;

	}

	public void UpdateDisplayedList(){
		if(EnableTargetList){
			switch((int)ListTargets){
			case 1:
				
				break;
				
			case 2:
				DisplayTargetListAll();
				break;
				
			case 3:
				
				break;
				
			case 4:
				DisplayTargetListFriendly();
				break;
				
			case 5:
				DisplayTargetListHostile();
				break;
				
			case 6:
				DisplayTargetListOwned();
				break;
			}
		}
	}

	public void ClearList(){
		foreach(GameObject button in GameObject.FindGameObjectsWithTag("TargetButton")){
			if(button.name == "TargetButton"){
				button.SetActive(false);
			}
		}
	}

	void BuildList(List<Transform> ThisList){
		ClearList();
			if(ThisList.Count > 0){

			for(int i = 0; i < ThisList.Count; i++){
				RectTransform button = ThisList[i].GetComponent<FX_3DRadar_RID>().ThisButton;
				button.anchoredPosition = new Vector3(0,(button.sizeDelta.y + ButtonPadding)* -i, 0);
				button.gameObject.SetActive(true);
			}

			Vector2 size = _Content.sizeDelta;
			size.y = ThisList.Count * ThisList[0].GetComponent<FX_3DRadar_RID>().ThisButton.sizeDelta.y;
			_Content.sizeDelta = size;
		}
	}

	public void EnableRadar(){
		RadarCamera.enabled = true;
		RadarUI.gameObject.SetActive (true);
		RadarEnabled = true;
	}

	public void DisableRadar(){
		RadarCamera.enabled = false;
		RadarUI.gameObject.SetActive (false);
		RadarEnabled = false;
	}
}