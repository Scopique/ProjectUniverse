using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FX_3DRadar_RID : MonoBehaviour {

	FX_3DRadar_Mgr FX3DRM; // Local Cache To The FX_3DRadar_Mgr Script

	public enum iffStatus{Neutral, Friendly, Hostile, Unknown, Abandoned, Owned, Objective, NAV}
	public iffStatus IFFStatus;

	public FX_Mission.objectiveType ObjectiveType;

	public int[] MainClass = new int[2]; //0 = Current Main Class, 1 = Check Last Main Class
	public int[] SubClass = new int[2]; //0 = Current Sub Class, 1 = Check Last Sub Class
	public int[] ThisFaction = new int[2]; //0 = Current Faction, 1 = Checl Last Faction,
	public int ThisFactionID;

	Transform ThisTransform; // Local Cache To This Transform
	Vector3 ThisPos; // This Transforms Position
	Vector3 RelPos; // This Transforms Relative Position To The Player
	
	public RectTransform ThisButton;
	public RectTransform ThisHID;
	public RectTransform RIDI; // Radar ID Icon
	RectTransform RIDV; // Radar ID Vertical Distance Indicator
	RectTransform RIDB; // Radar ID Base Icon
	RectTransform BoundCorner; // Bounding Corner Indicator

	public Image Image_RIDI; // Radar ID Icon
	Image Image_RIDV; // Radar ID Vertical Distance Indicator
	Image Image_RIDB; // Radar ID Base Icon
	Image Image_BC;
	Image Image_HID;

	private Color32 ThisColor;

	//Current State
	bool FirstSetup = false;
	bool Enabled = false;
	public bool IsPOI;
	public bool IsNAV;
	public bool[] IsActiveNAV = new bool[2];
	public bool IsPlayer;
	public bool IsPlayerTarget;
	public bool[] IsPlayerOwned = new bool[2];
	public bool[] IsAbandoned = new bool[2];
	public bool[] IsObjective = new bool[2];
	public bool BlindRadarOverride;
	bool VDIDown;

	//Detection & Targeting States
	public bool IsTargetable = true; // Determines if this object can be targeted while displayed on the players radar.
	public bool IsDetectable = true; // Determines if this object can be detected.
	public bool IsDiscovered; // Stores if this object has been detected by the players radar at any point.
	public bool DetectionReset; // Reset this objects Is Discovered if it enters an undetectable state. 

	//HUD Display Settings
	bool HUDIDEnabled; //The Current State Of The HUD ID
	public bool DisplayHUDID = true; //This Local HUD ID Enable - Disable Override
	public bool HUDIDOnScreen;
	public bool OffScreen;
	public bool VDIBaseEnabled;

	//Bounds
	public enum _boundsShow {OnlyInRadarRange, AlwaysAfterContact, Always}
	public _boundsShow BoundsShow;
	public _boundsShow HUDIDShow;
	public bool WasPlayerTarget;
	public bool DisplayBounds = true; //This Local Bounds Enable - Disable Override
	bool BoundsEnabled = true; //The Current State Of The Bounds
	bool LOS;

	//Timers
	float StatusTimer;
	float RadarUpdateTimer;
	float RadarResetTimer;


	void Start () {
		FX3DRM = GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>();
		ThisTransform = transform;

		if(FX3DRM.RadarEnabled){
			if(ThisTransform.FindChild("TargetButton")){
				ThisButton = ThisTransform.FindChild("TargetButton").GetComponent<RectTransform>();
				ThisButton.GetComponent<FX_MouseSelect>().ThisParent = ThisTransform;
				ThisButton.SetParent(FX3DRM._Content);
				ThisButton.eulerAngles = Vector3.zero;
				ThisButton.anchoredPosition = new Vector3(0,0,0);
			}

			CreateRID();
			UpdateIFFTexture();
			SetIFFColor();
			DisableRID();
			DisableHUDID ();
			DisableBounds();
			RemoveFromTargetList();
			FirstSetup = true;
		}
	}

	void LateUpdate() {
		if(!FX3DRM.RadarEnabled){
			if(Enabled){
				DisableThis();
			}
			return;
		}

		if(FX3DRM.RadarEnabled && !Enabled){
			if(!FirstSetup){
				Start ();
			}
			EnableThis();
		}

		if(IsNAV){
			if(IsActiveNAV[0] && !IsActiveNAV[1]){
				SetNAVActive();
			}
			if(!IsActiveNAV[0] && IsActiveNAV[1]){
				SetNAVInactive();
			}
			if(IsActiveNAV[0]){
				UpdateRadar();
			}
			return;
		}

		//Check For Status Changes
		StatusCheck();
		
		if(!IsPlayer){
			//Update Main Radar ID Loop
			UpdateRadar();
		}else if(Enabled){
			DisableThis();
			DisableHUDID();
			DisableBounds();
		}
	}

	//*************************************************************************************************************************************
	//															Main Loop
	//*************************************************************************************************************************************
	void UpdateRadar(){
		//Store this transform's current position
		ThisPos = ThisTransform.position;
		RelPos = FX3DRM.Player.InverseTransformPoint(ThisPos) * FX3DRM.RadarLocalScale;

		//Determine if this object is in the players radar range
		bool InRadarRange = (Vector3.SqrMagnitude(ThisPos - FX3DRM.PlayerPos) < FX3DRM.RadarRangeSQR);

		if(IsNAV){
			ThisHID.anchoredPosition = GetScreenPos(FX3DRM.HUDIDPadding);
		}

		//Render this object to the radar as a NAV or POI
		if(IsNAV && FX3DRM.RenderNAVRadar|| IsPOI){
			if(InRadarRange){
				RenderToRadar(false);
				return;
			}else{
				RenderToRadar(true);
				return;
			}
		}

		if(FX3DRM.FilterHostile && IFFStatus != iffStatus.Hostile){
			if(Enabled){
				DisableThis();
				DisableHUDID();
				DisableBounds();
			}
			return;
		}

		SetColorAlphaDistance();
		//Render this object to the radar normal
		if(InRadarRange && IsDetectable || FX3DRM.FilterHostile && IFFStatus != iffStatus.Hostile){
			if(FX3DRM.EnableBlindRadar){
				UpdateBlindRadar();
			}else{
				RenderToRadar(false);
			}
		}else if(Enabled){
			DisableThis();
		}

		//Check if this objects Is Discovered will be reset
		if(DetectionReset && IsDiscovered && !IsDetectable){
			IsDiscovered = false;
		}

		//Render the HUDID and Bounding Indicators to the screen
		RenderHUD();
	}

	void UpdateBlindRadar(){
		RaycastHit hit;

		if(FX3DRM.CurTime > RadarUpdateTimer){
			RadarUpdateTimer = FX3DRM.CurTime + FX3DRM.RadarUpdateTime;
			
			if(Physics.Linecast(FX3DRM.PlayerPos,ThisPos, out hit, ~(1<<FX3DRM.VFBLayer)) && hit.transform == ThisTransform){
				if(!LOS && IsTargetable){
					EnableThis();
					EnableHUDID();
					EnableBounds();
					LOS = true;

					if(WasPlayerTarget){
						WasPlayerTarget = false;
						FX3DRM.CurTarget = ThisTransform;
					}
				}
			}else if(LOS){
				DisableRID();
				DisableHUDID();
				DisableBounds();
				RemoveFromTargetList();
				LOS = false;

				if(IsPlayerTarget){
					RadarResetTimer = FX3DRM.CurTime + FX3DRM.RadarResetTime;
					WasPlayerTarget = true;
					FX3DRM.ClearTarget();
				}
			}
		}

		if(!LOS && WasPlayerTarget && FX3DRM.CurTime > RadarResetTimer){
			SetInactiveTarget();
			FX3DRM.ClearTarget();
		} 

		if(LOS){
			RenderToRadar(false);
		}
	}
		
	//*************************************************************************************************************************************
	//														Render RID To Players Radar
	//*************************************************************************************************************************************

	public void RenderToRadar(bool RenderOnEdge){

		if(!Enabled){
			EnableThis();
			IsDiscovered = true;
			if(ObjectiveType == FX_Mission.objectiveType.Destroy){
				UpdateMissionObjective();
			}
		}

		//Convert Radar Contact From World Space To Local Radar Space / Scale
		Vector3 newPosA = new Vector3(0,0,0);
		Vector3 newPosB = new Vector3(0,0,0);

		if(RenderOnEdge){
			newPosA = FX3DRM.RadarCamera.WorldToScreenPoint(RelPos.normalized * .5f);
		}else{
			newPosA = FX3DRM.RadarCamera.WorldToScreenPoint(RelPos);
			newPosB = FX3DRM.RadarCamera.WorldToScreenPoint(new Vector3(RelPos.x, 0 ,RelPos.z));
		}

		newPosA.x = Mathf.Round(newPosA.x); 
		newPosA.y = Mathf.Round(newPosA.y);
		newPosB.x = Mathf.Round(newPosB.x); 
		newPosB.y = Mathf.Round(newPosB.y);

		//Hide The VDI & Base Icon If RID Is Too Close
		bool HideVDI = (Mathf.Abs(newPosB.y - newPosA.y) <= 3);

		if(FX3DRM.Radar2D || HideVDI || RenderOnEdge){
			if(VDIBaseEnabled){
				DisableRIDVDIBase();
			}
		}else if(!VDIBaseEnabled){
			EnableRIDVDIBase();
		}

		//Place This RID Icon On The Canvas
		RIDI.anchoredPosition = newPosA + FX3DRM.FXCM.ObjectClassList[MainClass[0]].IDOffset[SubClass[0]];

		//Place This RID Base Icon On The Canvas
		if(!HideVDI || !RenderOnEdge){
			if(FX3DRM.RenderRIDB){
				RIDB.anchoredPosition = newPosB; //Radar ID Base Icon
			}

			Vector3 NegOffset = new Vector3(0,0,0);

			if (newPosA.y < newPosB.y) {
				NegOffset.x = 0.5f;

				if (FX3DRM.RenderToTexture && !VDIDown) {
					Vector3 tempEuler = RIDV.eulerAngles;
					tempEuler.z -= 180;
					RIDV.eulerAngles = tempEuler;
					VDIDown = true;
				}
			} else {
				if (FX3DRM.RenderToTexture && VDIDown) {
					Vector3 tempEuler = RIDV.eulerAngles;
					tempEuler.z = 0;
					RIDV.eulerAngles = tempEuler;
					VDIDown = false;
				}
			}

			if(FX3DRM.RenderVDI){
				RIDV.anchoredPosition = newPosB + FX3DRM.VDIOffset + NegOffset;
				float NewHeight = Mathf.Abs (newPosB.y - newPosA.y);

				RIDV.sizeDelta = new Vector2(1,NewHeight);

				if (!FX3DRM.RenderToTexture) {
					Vector3 dir = newPosA - newPosB;
					float angle = (Mathf.Atan2 (dir.y, dir.x)) * Mathf.Rad2Deg;

					RIDV.rotation = Quaternion.AngleAxis ((angle - 90), Vector3.forward);
				}
			}
		}
	}

	//*************************************************************************************************************************************
	//														Status & Relations Check
	//*************************************************************************************************************************************
	//Check For IFF & Class Status Changes Once A Second
	void StatusCheck(){
		if(FX3DRM.CurTime >= StatusTimer){

			//Check For Main Class & Sub Class Changes
			if(MainClass[0] != MainClass[1] || SubClass[0] != SubClass[1]){
				UpdateIFFTexture();
			}

			//Check For Faction Change
			if(ThisFaction[1] != ThisFaction[0] || IsAbandoned[0] != IsAbandoned[1] || IsPlayerOwned[0] != IsPlayerOwned[1] || IsObjective[0] != IsObjective[1]){
				SetIFFColor();
			}

			StatusTimer = FX3DRM.CurTime + 1;
		}
	}

	//Check This Objects Relationship With The Player
	void CheckPlayerRelation(){
		ThisFaction[1] = ThisFaction[0];
		IsObjective[1] = IsObjective[0];

		if(IsNAV){
			IFFStatus = iffStatus.NAV;
			return;
		}

		if(IsAbandoned[0]){
			IFFStatus = iffStatus.Abandoned;
			IsAbandoned[1] = true;
			RemoveFromTargetList();
			AddToTargetList();
			return;
		}else{
			IsAbandoned[1] = false;
		}

		if(IsPlayerOwned[0]){
			IFFStatus = iffStatus.Owned;
			ThisFactionID = FX3DRM.FXFM.PlayerFactionID;
			ThisFaction[0] = FX3DRM.FXFM.PlayerFaction;
			ThisFaction[1] = ThisFaction[0];
			IsPlayerOwned[1] = true;
			RemoveFromTargetList();
			AddToTargetList();
			return;
		}else{
			IsPlayerOwned[1] = false;
		}

		ThisFactionID = FX3DRM.FXFM.FactionID[ThisFaction[0]];

		if(ThisFactionID != FX3DRM.FXFM.PlayerFactionID){
			float ThisRelation = FX3DRM.FXFM.FactionRelations[(FX3DRM.FXFM.PlayerFactionID + ThisFactionID)];

			if(ThisRelation < FX3DRM.FXFM.HFS[0]){ // Player Is Hostile
				IFFStatus = iffStatus.Hostile;
			}else if(ThisRelation > FX3DRM.FXFM.HFS[1]){ // Player Is Friendly
				IFFStatus = iffStatus.Friendly;
			}else{ // Player Is Netural
				IFFStatus = iffStatus.Neutral;
			}
		}else{// Player Is Same Faction
			IFFStatus = iffStatus.Friendly;
		}

		RemoveFromTargetList();
		AddToTargetList();
	}

	//*************************************************************************************************************************************
	//														Render HUD Elements To The Screen
	//*************************************************************************************************************************************
	void RenderHUD(){
		if(IsPlayerTarget){
			if(BoundsEnabled){
				DisableBounds();
			}

			//Check Which Method To Call For HUD ID As Player Target
			if(FX3DRM.RenderHUDID && FX3DRM.G_DisplayHUDID && FX3DRM.DisableTSIOS && FX3DRM.TargetOffScreen){
				DrawHUDIDMethod();
			}else if(HUDIDEnabled){
				DisableHUDID();
			}
		}else{
			//Check Which Method To Call For Bounds
			if(FX3DRM.RenderBounds){
				DrawBoundsMethod();
				if(BoundsEnabled){
					DrawBounds();
				}
			}
			//Check Which Method To Call For HUD ID
			if(FX3DRM.RenderHUDID && FX3DRM.G_DisplayHUDID){
				DrawHUDIDMethod();
			}else if(HUDIDEnabled){
				DisableHUDID();
			}
		}

		if(DisplayHUDID){
			ThisHID.anchoredPosition = GetScreenPos(FX3DRM.HUDIDPadding);
		}
	}

	//*************************************************************************************************************************************
	//														HUD ID Functions
	//*************************************************************************************************************************************
	void DrawHUDIDMethod(){

		switch((int)HUDIDShow){
		case 0:
			DrawHUDIDInRadar();
			break;
		case 1:
			DrawHUDIDAfterContact();
			break;
		case 2:
			DrawHUDIDAlways();
			break;
		}
	}

	void DrawHUDIDInRadar(){
		if(!FX3DRM.G_DisplayHUDID && HUDIDEnabled || !DisplayHUDID && HUDIDEnabled || !HUDIDOnScreen && !OffScreen && HUDIDEnabled ||!Enabled && HUDIDEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableHUDID();
		}else if(FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && OffScreen && Enabled || FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && HUDIDOnScreen && Enabled || !HUDIDEnabled && IsPlayerTarget && FX3DRM.DisableTSIOS && FX3DRM.TargetOffScreen){
			EnableHUDID();
		}
	}
	
	void DrawHUDIDAfterContact(){
		if(!FX3DRM.G_DisplayHUDID && HUDIDEnabled || !DisplayHUDID && HUDIDEnabled || !IsDiscovered && HUDIDEnabled || !IsDetectable && HUDIDEnabled || !HUDIDOnScreen && !OffScreen && HUDIDEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableHUDID();
		}else if(FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && OffScreen && IsDiscovered && IsDetectable || FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && HUDIDOnScreen && IsDiscovered && IsDetectable || !HUDIDEnabled && IsPlayerTarget && FX3DRM.DisableTSIOS && FX3DRM.TargetOffScreen){
			EnableHUDID();
		}
	}
	
	void DrawHUDIDAlways(){
		if(!FX3DRM.G_DisplayHUDID && HUDIDEnabled || !DisplayHUDID && HUDIDEnabled || !IsDetectable && HUDIDEnabled || !HUDIDOnScreen && !OffScreen && HUDIDEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableHUDID();
		}else if(FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && OffScreen && IsDetectable || FX3DRM.G_DisplayHUDID && !HUDIDEnabled && DisplayHUDID && IsDetectable && HUDIDOnScreen || !HUDIDEnabled && IsPlayerTarget && FX3DRM.DisableTSIOS && FX3DRM.TargetOffScreen){
			EnableHUDID();
		}
	}

	Vector3 GetScreenPos(int padding){
		Vector3 ScreenPos = FX3DRM.PlayerCameraC.WorldToScreenPoint(ThisPos);
		OffScreen = (ScreenPos.x > FX3DRM.ScreenWidth || ScreenPos.x < 0 || ScreenPos.y > FX3DRM.ScreenHeight || ScreenPos.y < 0 || ScreenPos.z <= 0.01);

		if(OffScreen){

			if(IsPlayerTarget && FX3DRM.DisableTSIOS && !FX3DRM.TSI2Enabled){
				FX3DRM.Image_TSI2.enabled = true;
				FX3DRM.TSI2Enabled = true;
			} 
			
			RaycastHit Hit;
			Vector3 TRelPos;
			
			TRelPos = FX3DRM.PlayerCameraT.InverseTransformPoint(ThisPos);

			if(ScreenPos.x ==0.5f && ScreenPos.y ==0.5f){
				TRelPos.y = 1;
			}
			
			Physics.Raycast (Vector3.zero, new Vector3(TRelPos.x, TRelPos.y, 0),out Hit, 2, 1<<FX3DRM.VFBLayer);
			ScreenPos = new Vector3((((FX3DRM.VFBounds * 0.5f) + Hit.point.x) / FX3DRM.VFBounds) * FX3DRM.ScreenWidth, (0.5f + Hit.point.y) * FX3DRM.ScreenHeight, 0 );

		}else if(IsPlayerTarget && FX3DRM.TSI2Enabled && FX3DRM.TSIEnabled){
			FX3DRM.Image_TSI2.enabled = false;
			FX3DRM.TSI2Enabled = false;
		}

		// Screen Offset Normal

		float swp = FX3DRM.ScreenWidth - padding;
		float shp = FX3DRM.ScreenHeight - padding;

		if(ScreenPos.x < padding){
			ScreenPos.x = padding;
		}
		
		if(ScreenPos.x > swp){
			ScreenPos.x = swp;
		}
				
		if(ScreenPos.y < padding){
			ScreenPos.y = padding;
		}
		
		if(ScreenPos.y > shp){
			ScreenPos.y = shp;
		}
		return ScreenPos;
	}

	//*************************************************************************************************************************************
	//														Bounds Functions
	//*************************************************************************************************************************************
	void DrawBoundsMethod(){
		bool InFront = (Vector3.Dot(FX3DRM.PlayerCameraT.forward, ThisPos - FX3DRM.PlayerCamPos) > 0);

		switch((int)BoundsShow){
		case 0:
			DrawBoundsInRadar(InFront);
			break;
		case 1:
			DrawBoundsAfterContact(InFront);
			break;
		case 2:
			DrawBoundsAlways(InFront);
			break;
		}
	}
	
	void DrawBoundsInRadar(bool InFront){
		if(!FX3DRM.G_DisplayBounds && BoundsEnabled || !InFront && BoundsEnabled || !DisplayBounds && BoundsEnabled || !Enabled && BoundsEnabled || OffScreen && BoundsEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableBounds();
		}else if(FX3DRM.G_DisplayBounds && !BoundsEnabled && DisplayBounds && !OffScreen && InFront && Enabled){
			EnableBounds();
		}
	}
	
	void DrawBoundsAfterContact(bool InFront){
		if(!FX3DRM.G_DisplayBounds && BoundsEnabled || !InFront && BoundsEnabled || !DisplayBounds && BoundsEnabled || !IsDiscovered && BoundsEnabled || !IsDetectable && BoundsEnabled || OffScreen && BoundsEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableBounds();
		}else if(FX3DRM.G_DisplayBounds && !BoundsEnabled && DisplayBounds && IsDiscovered && IsDetectable && !OffScreen && InFront){
			EnableBounds();
		}
	}
	
	void DrawBoundsAlways(bool InFront){
		if(!FX3DRM.G_DisplayBounds && BoundsEnabled || !InFront && BoundsEnabled || !DisplayBounds && BoundsEnabled || !IsDetectable && BoundsEnabled || OffScreen && BoundsEnabled || FX3DRM.EnableBlindRadar && !LOS){
			DisableBounds();
		}else if(FX3DRM.G_DisplayBounds && !BoundsEnabled && DisplayBounds && IsDetectable && !OffScreen && InFront){
			EnableBounds();
		}
	}
	
	void DrawBounds(){

		Bounds ThisBounds = ThisTransform.GetComponent<Renderer>().bounds;
		Vector3 Center = ThisBounds.center;

		if(FX3DRM.AdvancedBounds){
			Vector3 Ext = ThisBounds.extents;

			Vector2[] ExtPoints = new Vector2[8]{
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(-Ext.x, Ext.y, Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(Ext.x, Ext.y, Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(-Ext.x, -Ext.y, Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(Ext.x, -Ext.y, Ext.z) + Center),
				
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(-Ext.x, Ext.y, -Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(Ext.x, Ext.y, -Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(-Ext.x, -Ext.y, -Ext.z) + Center),
				FX3DRM.PlayerCameraC.WorldToScreenPoint(new Vector3(Ext.x, -Ext.y, -Ext.z) + Center)
			};
			
			Vector2 min = ExtPoints[0];
			Vector2 max = ExtPoints[0];

			foreach(Vector2 v2 in ExtPoints){
				min = Vector2.Min(min, v2);
				max = Vector2.Max(max, v2);
			}

			Vector2 newCenter = new Vector2(min.x + ((max.x - min.x) * 0.5f), min.y + ((max.y - min.y) * 0.5f));

			if(FX3DRM.LBS){
				if(max.x - min.x > FX3DRM.MBS){
					min.x = newCenter.x - (FX3DRM.MBS * 0.5f);
					max.x = newCenter.x + (FX3DRM.MBS * 0.5f);
				}
				
				if(max.y - min.y > FX3DRM.MBS){
					min.y = newCenter.y - (FX3DRM.MBS * 0.5f);
					max.y = newCenter.y + (FX3DRM.MBS * 0.5f);
				}
			}
			
			min.x = Mathf.Round(min.x) - FX3DRM.BPadding;
			min.y = Mathf.Round(min.y) - FX3DRM.BPadding;
			
			max.x = Mathf.Round(max.x) + FX3DRM.BPadding;
			max.y = Mathf.Round(max.y) + FX3DRM.BPadding;

			BoundCorner.anchoredPosition = new Vector3(newCenter.x, newCenter.y, 0);
			BoundCorner.sizeDelta = new Vector2((max.x - min.x) + 15, (max.y - min.y) + 15);
		}else{
			Vector3 minX = FX3DRM.PlayerCameraC.WorldToScreenPoint(Center + new Vector3 (ThisBounds.min.x,0,0));
			Vector3 maxX = FX3DRM.PlayerCameraC.WorldToScreenPoint(Center + new Vector3 (ThisBounds.max.x,0,0));
			Vector3 minY = FX3DRM.PlayerCameraC.WorldToScreenPoint(Center + new Vector3 (0,ThisBounds.min.y,0));
			Vector3 maxY = FX3DRM.PlayerCameraC.WorldToScreenPoint(Center + new Vector3 (0,ThisBounds.max.y,0));
			Center = FX3DRM.PlayerCameraC.WorldToScreenPoint(Center);

			if(FX3DRM.LBS){
				if(maxX.x - minX.x > FX3DRM.MBS){
					minX.x = Center.x - (FX3DRM.MBS * 0.5f);
					maxX.x = Center.x + (FX3DRM.MBS * 0.5f);
				}
				
				if(maxY.y - minY.y > FX3DRM.MBS){
					minY.y = Center.y - (FX3DRM.MBS * 0.5f);
					maxY.y = Center.y + (FX3DRM.MBS * 0.5f);
				}
			}
			
			minX.x = Mathf.Round(minX.x) - FX3DRM.BPadding - 2;
			minY.y = Mathf.Round(minY.y) - FX3DRM.BPadding - 2;
			
			maxX.x = Mathf.Round(maxX.x) + FX3DRM.BPadding + 2;
			maxY.y = Mathf.Round(maxY.y) + FX3DRM.BPadding + 2;
			
			BoundCorner.anchoredPosition = new Vector3(Center.x, Center.y, 0);
			BoundCorner.sizeDelta = new Vector2((maxX.x - minX.x) + 15, (maxY.y - minY.y) + 15);
		}
	}

	//*************************************************************************************************************************************
	//														Create This Objects RID & HUD Elements
	//*************************************************************************************************************************************

	void CreateRID(){
		//Make This Objects Radar ID Elements
		RIDI = FX3DRM.MakeImage("RID_Icons", 0.5f);
		RIDI.pivot = new Vector2(0f, 0f);
		Image_RIDI = RIDI.GetComponent<Image>();
		RIDI.SetParent(GameObject.Find("RadarTargets").transform);

		if(FX3DRM.RenderRIDB){
			RIDB = FX3DRM.MakeImage("RID_Base",0.5f);
			RIDB.SetParent(GameObject.Find("RadarTargets").transform);
			RIDB.pivot = new Vector2(0f, 0f);
			Image_RIDB = RIDB.GetComponent<Image>();
			Image_RIDB.sprite = FX3DRM.Sprite_RIDBase;
			Image_RIDB.SetNativeSize();
		}

		if(FX3DRM.RenderVDI){
			RIDV = FX3DRM.MakeImage("RID_VDI",0.5f);
			RIDV.SetParent(GameObject.Find("RadarTargets").transform);
			RIDV.pivot = new Vector2(0f, 0f);
			Image_RIDV = RIDV.GetComponent<Image>();
		}

		if(!IsNAV && FX3DRM.SelectableRID){
			RIDI.gameObject.AddComponent<Button>();
			RIDI.gameObject.GetComponent<Button>().onClick.AddListener(() => { ThisHID.GetComponent<FX_MouseSelect>().SetTarget();});
			RIDI.gameObject.AddComponent<FX_MouseSelect>().ThisParent = ThisTransform;
		}
		DisableThis();

		//Create This Objects HUD Target Indicator
		if(FX3DRM.RenderHUDID){
			ThisHID = FX3DRM.MakeImage("HUD_ID",0.5f);
			ThisHID.SetParent(GameObject.Find("HUDIDs").transform);
			ThisHID.gameObject.layer = FX3DRM.HUDCanvasLayer;
			Image_HID = ThisHID.GetComponent<Image>();
			
			if(IsNAV){
				Image_HID.sprite = FX3DRM.Sprite_NAV;
			}else if(FX3DRM.HUDIAsIcon){
				Image_HID.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
			}else{
				Image_HID.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
			}
			
			ThisHID.GetComponent<Image>().SetNativeSize();
			
			if(!IsNAV && FX3DRM.SelectableHUDID){
				ThisHID.gameObject.AddComponent<Button>();
				ThisHID.gameObject.GetComponent<Button>().onClick.AddListener(() => { ThisHID.GetComponent<FX_MouseSelect>().SetTarget();});
				ThisHID.gameObject.AddComponent<FX_MouseSelect>().ThisParent = ThisTransform;
			}

			if(FX3DRM.RenderToTexture){
				RIDI.gameObject.layer = LayerMask.NameToLayer("Radar");
				RIDB.gameObject.layer = LayerMask.NameToLayer("Radar");
				RIDV.gameObject.layer = LayerMask.NameToLayer("Radar");
			}

			DisableHUDID();
		}

		//Create This Objects Bounding Indicators
		if(!IsNAV){
			if(FX3DRM.RenderBounds){
				BoundCorner = FX3DRM.MakeImage("BC_L",0.5f);
				BoundCorner.SetParent(GameObject.Find("BoundCorners").transform);
				BoundCorner.gameObject.layer = FX3DRM.HUDCanvasLayer;
				Image_BC = BoundCorner.GetComponent<Image>();
				Image_BC.type = Image.Type.Sliced;
				Image_BC.sprite = FX3DRM.Sprite_BoundSquare;
				Image_BC.SetNativeSize();

				DisableBounds();
			}
		}
	}

	//Change This Objects Displayed Radar ID Icon
	public void UpdateIFFTexture(){
		if(IsNAV){
			Image_RIDI.sprite = FX3DRM.Sprite_RadarNAV;
			Image_RIDI.SetNativeSize();
		}else{
			Image_RIDI.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
			Image_RIDI.SetNativeSize();

			if(IsPlayerTarget && FX3DRM.RenderTSIID){
				FX3DRM.Image_TSIID.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
				FX3DRM.Image_TSIID.SetNativeSize();
			}

			if(FX3DRM.RenderHUDID){
				if(FX3DRM.HUDIAsIcon){
					Image_HID.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
				}else{
					Image_HID.sprite = FX3DRM.Sprite_HUDID;
				}
				Image_HID.SetNativeSize();
			}
		}

		if(ThisButton){
			SetButtonSprite();
		}

		MainClass[1] = MainClass[0];
		SubClass[1] = SubClass[0];
	}

	void SetButtonSprite(){
		Image ButtonImage = ThisButton.FindChild("Image").GetComponent<Image>();
		ButtonImage.sprite = FX3DRM.FXCM.ObjectClassList[MainClass[0]].ClassSprite[SubClass[0]];
		ButtonImage.SetNativeSize();
		
		RectTransform ImageRect = ButtonImage.GetComponent<RectTransform>();
		float ImageHeight = ImageRect.sizeDelta.y;
		Vector3 ImagePos = ImageRect.anchoredPosition;
		ImagePos.y = Mathf.Round(-ImageHeight * 0.5f);
		ImageRect.anchoredPosition = ImagePos;
		ThisButton.FindChild("Text").GetComponent<Text>().text = this.name.ToString ();
	}

	//Change This Objects IFF Color
	void SetIFFColor(){
		CheckPlayerRelation();

		if(IsObjective[0] && FX3DRM.UseObjectiveColor){
			ThisColor = FX3DRM.IFFColor[(int)iffStatus.Objective];
		}else if(IsNAV){
			ThisColor = FX3DRM.IFFColor[(int)iffStatus.NAV];
		}else{
			ThisColor = FX3DRM.IFFColor[(int)IFFStatus];
		}

		SetButtonColor();

		ThisColor.a = FX3DRM.RadarRIDAlpha;
		Image_RIDI.color = ThisColor;

		if(FX3DRM.RenderRIDB){
			Image_RIDB.color = ThisColor;
		}
		if(FX3DRM.RenderVDI){
			ThisColor.a = FX3DRM.RadarVDIAlpha;
			Image_RIDV.color = ThisColor;
		}
		
		if(FX3DRM.RenderHUDID){
			ThisColor.a = FX3DRM.HUDAlpha;
			Image_HID.color = ThisColor;
		}
		
		if(FX3DRM.RenderBounds && !IsNAV){
			ThisColor.a = FX3DRM.HUDAlpha;
			Image_BC.color = ThisColor;
		}

		if(IsPlayerTarget){
			SetHUDColor(ThisColor);
			SetAsActiveTarget();
		}
		SetColorAlphaDistance();
	}

	void SetHUDColor(Color32 ThisColor){
		if(FX3DRM.RenderTLI){
			FX3DRM.Image_TLI.color = ThisColor;
		}
		FX3DRM.Image_TSI.color = ThisColor;
		FX3DRM.Image_TSISC.color = ThisColor;
		FX3DRM.Image_RTSI.color = ThisColor;

		if(FX3DRM.RenderHUDID){
			FX3DRM.Image_TSI2.color = ThisColor;
		}
		if(FX3DRM.RenderHUDDIA){
			FX3DRM.Image_DIA.color = ThisColor;
		}
		if(FX3DRM.RenderTSIID){
			FX3DRM.Image_TSIID.color = ThisColor;
		}
	}

	void SetColorAlphaDistance(){
		if(Enabled && IsDetectable){
			if(FX3DRM.FadeBounds && BoundsEnabled || FX3DRM.FadeHUDID && HUDIDEnabled || FX3DRM.FadeTSI){
				float Distance = RelPos.sqrMagnitude * 2;
				byte Alpha;
				
				if(Distance > 0.5f){
					Alpha = FX3DRM.MinFadeAmount;
				}else{
					Alpha = (byte)(-((Distance * 2)) * 255  % 256);
				}

				if(FX3DRM.FadeInvert){
					Alpha = (byte)(255 - Alpha);
				}

				if(Alpha < FX3DRM.MinFadeAmount){
					Alpha = FX3DRM.MinFadeAmount;
				}

				Color32 newColor = ThisColor;
				newColor.a = Alpha;

				if(!IsNAV && FX3DRM.RenderBounds && FX3DRM.FadeBounds){
					Image_BC.color = newColor;
				}
				
				if(FX3DRM.RenderHUDID && FX3DRM.FadeHUDID){
					Image_HID.color = newColor;
				}
				
				if(IsPlayerTarget){
					if(FX3DRM.FadeTSI){
						FX3DRM.Image_TSI.color = newColor;
						FX3DRM.Image_TSISC.color = newColor;

						if(FX3DRM.RenderTSIID){
							FX3DRM.Image_TSIID.color = newColor;
						}
						if(FX3DRM.DisableTSIOS && FX3DRM.RenderHUDID && FX3DRM.FadeHUDID){
							FX3DRM.Image_TSI2.color = newColor;
						}
					}

					if(FX3DRM.RenderTLI && FX3DRM.FadeTLI){
						FX3DRM.Image_TLI.color = newColor;
					}
				}

				if(FX3DRM.FadeRID){
					Image_RIDI.color = newColor;
					Image_RIDB.color = newColor;
					Image_RIDV.color = newColor;

				}
			}
		}
	}

	void SetButtonColor(){
		Color32 ColorNormal = ThisColor;
		ColorNormal.a = (byte)100;
		Color32 ColorHover = ThisColor;
		ColorHover.a = (byte)220;
		Color32 ColorActive = ThisColor;
		ColorActive.a = (byte)255;

		if(ThisButton){
			ColorBlock ButtonColor = ThisButton.GetComponent<Button>().colors;
			ButtonColor.normalColor = ColorNormal;
			ButtonColor.highlightedColor = ColorHover;
			ButtonColor.pressedColor = ColorActive;
			ThisButton.GetComponent<Button>().colors = ButtonColor;
		}
	}

	//*************************************************************************************************************************************
	//														Called Functions
	//*************************************************************************************************************************************

	//Enable All Radara Elements For This Object
	void EnableThis(){
		EnableRID();
		Enabled = true;
		RemoveFromTargetList();
		AddToTargetList();
	}
	
	//Disable All Radara Elements For This Object
	void DisableThis(){
		ResetTSIElements();

		if(IsPlayerTarget){
			FX3DRM.ClearTarget();
		}

		DisableRID();
		RemoveFromTargetList();
		IsPlayerTarget = false;
		Enabled = false;
	}

	//Destroy This Object And All Radar Elements
	void ResetTSIElements(){
		if(IsPlayerTarget){
			FX3DRM.RTSI.SetParent(RIDI.parent);
			FX3DRM.TSI2.SetParent(ThisHID.parent);
		}
	}

	public void DestroyThis(){
		if(ObjectiveType == FX_Mission.objectiveType.Destroy){
			UpdateMissionObjective();
		}

		ResetTSIElements();
		DestroyRID();
		DestroyHUDID();
		DestroyBounds();
		RemoveFromTargetList();

		if(ThisButton){
			Destroy (ThisButton.gameObject);
		}

		Destroy(ThisTransform.gameObject);
	}

	void EnableRID(){
		Image_RIDI.enabled = true;
		EnableRIDVDIBase();
	}

		void DisableRID(){
		ResetTSIElements();
		Image_RIDI.enabled = false;
		DisableRIDVDIBase();
	}

	void DestroyRID(){
		Destroy(RIDI.gameObject);
		if(FX3DRM.RenderVDI){
			Destroy(RIDB.gameObject);
		}
	}

	void EnableRIDVDIBase(){
		if(FX3DRM.RenderRIDB){
			Image_RIDB.enabled = true;
		}
		if(FX3DRM.RenderVDI){
			Image_RIDV.enabled = true;
		}
		VDIBaseEnabled = true;
	}

	void DisableRIDVDIBase(){
		if(FX3DRM.RenderRIDB){
			Image_RIDB.enabled = false;
		}
		if(FX3DRM.RenderVDI){
			Image_RIDV.enabled = false;
		}
		VDIBaseEnabled = false;
	}

	void EnableBounds(){
		SetColorAlphaDistance();
		if(FX3DRM.RenderBounds && !IsNAV){
			Image_BC.enabled = true;
			BoundsEnabled = true;
		}
	}

	void DisableBounds(){
		if(FX3DRM.RenderBounds && !IsNAV){
			Image_BC.enabled = false;
			BoundsEnabled = false;
		}
	}

	void DestroyBounds(){
		if(FX3DRM.RenderBounds && !IsNAV){
			Destroy(BoundCorner.gameObject);
		}
	}

	void EnableHUDID(){
		if(FX3DRM.RenderHUDID){
			Image_HID.enabled = true;
			HUDIDEnabled = true;
		}
	}

	void DisableHUDID(){
		if(FX3DRM.RenderHUDID){
			Image_HID.enabled = false;
			HUDIDEnabled = false;
		}

		FX3DRM.Image_TSI2.enabled = false;
		FX3DRM.TSI2Enabled = false;
	}

	void DestroyHUDID(){
		if(FX3DRM.RenderHUDID){
			Destroy(ThisHID.gameObject);
		}
	}

	public void SetNAVActive(){
		IsActiveNAV[1] = true;
		EnableHUDID();
		EnableRID();
		HUDIDOnScreen = true;
	}

	public void SetNAVInactive(){
		IsActiveNAV[1] = false;
		DisableHUDID();
		DisableRID();
		HUDIDOnScreen = true;
	}

	void UpdateMissionObjective(){
		if(IsObjective[0]){
			for(int i = 0; i < FX3DRM.FXMM.MissionList.Count; i++){
				FX3DRM.FXMM.MissionList[i].Mission.ObjectiveDestroyed(ThisTransform);
			}
		}
	}

	public void SetAsActiveTarget(){
		IsPlayerTarget = true;

		Color32 ColorNormal = ThisColor;
		ColorNormal.a = (byte)255;

		if (ThisButton) {
			Button b = ThisButton.GetComponent<Button> ();
			ColorBlock ButtonColor = b.colors;
			ButtonColor.normalColor = ColorNormal;
			b.colors = ButtonColor;
			SetHUDColor (ThisColor);
		}
	}

	public void SetInactiveTarget(){
		IsPlayerTarget = false;
		WasPlayerTarget = false;
		Color32 ColorNormal = ThisColor;
		ColorNormal.a = (byte)100;

		if(ThisButton){
			Button b = ThisButton.GetComponent<Button>();
			ColorBlock ButtonColor = b.colors;
			ButtonColor.normalColor = ColorNormal;
			b.colors = ButtonColor;
		}
	}

	public void ResetObjectiveStatus(){
		IsObjective[0] = false;
		ObjectiveType = FX_Mission.objectiveType.None;
	}



	//Add This Object To The Players Target List
	void AddToTargetList(){
		if(!IsNAV){
			if(IsTargetable){
				FX3DRM.TargetListAll.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Neutral){
				FX3DRM.TargetListNeutral.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Friendly){
				FX3DRM.TargetListFriendly.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Hostile){
				FX3DRM.TargetListHostile.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Unknown){
				FX3DRM.TargetListAband.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Owned){
				FX3DRM.TargetListOwned.Add(ThisTransform);
			}
			if(IFFStatus == iffStatus.Objective){
				FX3DRM.TargetListObj.Add(ThisTransform);
			}
		}
		FX3DRM.UpdateDisplayedList();
	}

	//Remove This Object To The Players Target List
	void RemoveFromTargetList(){
		if(!IsNAV){
			for(int a = 0; a < FX3DRM.TargetListAll.Count; a++){
				if(ThisTransform == FX3DRM.TargetListAll[a]){
					FX3DRM.TargetListAll.RemoveAt(a);
				}
			}

			for(int n = 0; n < FX3DRM.TargetListNeutral.Count; n++){
				if(ThisTransform == FX3DRM.TargetListNeutral[n]){
					FX3DRM.TargetListNeutral.RemoveAt(n);
				}
			}

			for(int f = 0; f < FX3DRM.TargetListFriendly.Count; f++){
				if(ThisTransform == FX3DRM.TargetListFriendly[f]){
					FX3DRM.TargetListFriendly.RemoveAt(f);
				}
			}

			for(int h = 0; h < FX3DRM.TargetListHostile.Count; h++){
				if(ThisTransform == FX3DRM.TargetListHostile[h]){
					FX3DRM.TargetListHostile.RemoveAt(h);
				}
			}

			for(int ab = 0; ab < FX3DRM.TargetListAband.Count; ab++){
				if(ThisTransform == FX3DRM.TargetListAband[ab]){
					FX3DRM.TargetListAband.RemoveAt(ab);
				}
			}

			for(int o = 0; o < FX3DRM.TargetListOwned.Count; o++){
				if(ThisTransform == FX3DRM.TargetListOwned[o]){
					FX3DRM.TargetListOwned.RemoveAt(o);
				}
			}

			for(int obj = 0; obj < FX3DRM.TargetListOwned.Count; obj++){
				if(ThisTransform == FX3DRM.TargetListOwned[obj]){
					FX3DRM.TargetListOwned.RemoveAt(obj);
				}
			}
		}
		FX3DRM.UpdateDisplayedList();
	}
}