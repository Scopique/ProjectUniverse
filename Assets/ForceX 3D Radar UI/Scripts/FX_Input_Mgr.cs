using UnityEngine;
using System.Collections;

public class FX_Input_Mgr : MonoBehaviour {

	bool DisplayMenu;
	
	//enum _inputBindings {Character_Input, Flight_Input, Radar_Input}
	//_inputBindings InputBindings;
	
	//enum _flightControls {Keyboard_Mouse, Joystick_Gamepad}
	//_flightControls FlightControls;
	
	public enum _keyMod {None, Shift, Ctrl, Alt}
	
	//enum _joystickInput {Joystick_1_Axis_X, Joystick_1_Axis_Y, Joystick_1_Axis_3, Joystick_1_Axis_4, Joystick_1_Axis_5, Joystick_1_Axis_6, Joystick_1_Axis_7, Joystick_1_Axis_8, Joystick_1_Axis_9, Joystick_1_Axis_10,
		//Joystick_2_Axis_X, Joystick_2_Axis_Y, Joystick_2_Axis_3, Joystick_2_Axis_4, Joystick_2_Axis_5, Joystick_2_Axis_6, Joystick_2_Axis_7, Joystick_2_Axis_8, Joystick_2_Axis_9, Joystick_2_Axis_10}
	
	/***** Character Input Assignments ******/
	/*
	var Run = KeyCode.LeftShift;
	var Crouch = KeyCode.C;
	var Prone = KeyCode.V;
	var Jump = KeyCode.Space;
	*/
	/***** Flight Input Assignments ******/
	/*
	var UpdateAxis : boolean[] = new boolean[4];
	
	var PitchE : joystickInput = 1;
	var Pitch : String;
	var InvertPitch : boolean;
	
	var YawE : joystickInput = 0;
	var Yaw : String;
	var InvertYaw : boolean;
	
	var RollE : joystickInput = 2;
	var Roll : String;
	var InvertRoll : boolean;
	
	var ThrottleE : joystickInput = 3;
	var Throttle : String;
	var InvertThrottle : boolean;
	*/
	/***** Radar Input Assignments ******/
	
	//All Targets
	public KeyCode TargetNext = KeyCode.T;
	public KeyCode TargetPrev = KeyCode.Y;
	public KeyCode TargetClosest = KeyCode.U;
	
	public _keyMod TargetNextKM;
	public _keyMod TargetPrevKM;
	public _keyMod TargetClosestKM;

	//Friendly Targets
	//public KeyCode TargetNextF = KeyCode.R;
	//public KeyCode TargetPrevF = KeyCode.F;
	//public KeyCode TargetClosestF = KeyCode.E;

	//public _keyMod TargetNextFKM;
	//public _keyMod TargetPrevFKM;
	//public _keyMod TargetClosestFKM;

	//Hostile Targets
	public KeyCode TargetNextH = KeyCode.R;
	public KeyCode TargetPrevH = KeyCode.F;
	public KeyCode TargetClosestH = KeyCode.E;
	
	public _keyMod TargetNextHKM;
	public _keyMod TargetPrevHKM;
	public _keyMod TargetClosestHKM;
	
	//Current Target Sub Components
	public KeyCode TargetNextS = KeyCode.M;
	public KeyCode TargetPrevS = KeyCode.N;
	
	public _keyMod TargetNextSKM;
	public _keyMod TargetPrevSKM;
	
	//Clear Targets
	public KeyCode ClearTarget = KeyCode.C;
	public KeyCode ClearSubC = KeyCode.B;
	
	public _keyMod ClearTargetKM;
	public _keyMod ClearSubCKM;

	//Store Selected Target
	public KeyCode StoreTarget1 = KeyCode.F5;
	public KeyCode StoreTarget2 = KeyCode.F6;
	public KeyCode StoreTarget3 = KeyCode.F7;
	public KeyCode StoreTarget4 = KeyCode.F8;

	public _keyMod StoreTarget1KM = _keyMod.Shift;
	public _keyMod StoreTarget2KM = _keyMod.Shift;
	public _keyMod StoreTarget3KM = _keyMod.Shift;
	public _keyMod StoreTarget4KM = _keyMod.Shift;

	//Select Stored Target
	public KeyCode SelectStoredTarget1 = KeyCode.F5;
	public KeyCode SelectStoredTarget2 = KeyCode.F6;
	public KeyCode SelectStoredTarget3 = KeyCode.F7;
	public KeyCode SelectStoredTarget4 = KeyCode.F8;
	
	public _keyMod SelectStoredTarget1KM;
	public _keyMod SelectStoredTarget2KM;
	public _keyMod SelectStoredTarget3KM;
	public _keyMod SelectStoredTarget4KM;

	//Target List & Filters
	public KeyCode TargetList = KeyCode.L;
	public KeyCode FilterHostile = KeyCode.H;
	public KeyCode NAVList = KeyCode.K;
	
	public _keyMod TargetListKM;
	public _keyMod FilterHostileKM;
	public _keyMod NAVListKM;
	
	//Display
	public KeyCode Switch3D2D = KeyCode.Z;
	public KeyCode ToggleIndicators = KeyCode.X;
	
	public _keyMod Switch3D2DKM;
	public _keyMod ToggleIndicatorsKM;
	
	private bool UpdateKey;
	/*
void Start(){
SetJoystickAxis();
}

void Update(){

if(Input.GetKeyDown(KeyCode.Escape) && !DisplayMenu){
	DisplayMenu = true;
	Time.timeScale = 0;
}else if(Input.GetKeyDown(KeyCode.Escape) && DisplayMenu){
	DisplayMenu = false;
	Time.timeScale = 1;
}

BindInputs();
}


void BindInputs(){
	if(UpdateAxis[0]){
		PitchE = DetectAxis(PitchE, 0);
		SetJoystickAxis();
	}
	if(UpdateAxis[1]){
		YawE = DetectAxis(YawE, 1);
		SetJoystickAxis();
	}
	if(UpdateAxis[2]){
		RollE = DetectAxis(RollE, 2);
		SetJoystickAxis();
	}
	if(UpdateAxis[3]){
		ThrottleE = DetectAxis(ThrottleE, 3);
		SetJoystickAxis();
	}
}

void SetJoystickAxis(){
Pitch = PitchE.ToString();
Yaw = YawE.ToString();
Roll = RollE.ToString();
Throttle = ThrottleE.ToString();
}

void DetectKey(ThisInput){
	if(Input.GetKeyDown(KeyCode.A)){
		UpdateKey = false;
		return KeyCode.A;
	}else

	if(Input.GetKeyDown(KeyCode.B)){
		UpdateKey = false;
		return KeyCode.B;
	}else

	if(Input.GetKeyDown(KeyCode.C)){
		UpdateKey = false;
		return KeyCode.C;
	}else
	
	if(Input.GetKeyDown(KeyCode.D)){
		UpdateKey = false;
		return KeyCode.D;
	}else

	if(Input.GetKeyDown(KeyCode.E)){
		UpdateKey = false;
		return KeyCode.E;
	}else

	if(Input.GetKeyDown(KeyCode.F)){
		UpdateKey = false;
		return KeyCode.F;
	}else
	
	if(Input.GetKeyDown(KeyCode.G)){
		UpdateKey = false;
		return KeyCode.G;
	}else
	
	if(Input.GetKeyDown(KeyCode.H)){
		UpdateKey = false;
		return KeyCode.H;
	}else
	
	if(Input.GetKeyDown(KeyCode.I)){
		UpdateKey = false;
		return KeyCode.I;
	}else		
			
	if(Input.GetKeyDown(KeyCode.J)){
		UpdateKey = false;
		return KeyCode.J;
	}else

	
	if(Input.GetKeyDown(KeyCode.K)){
		UpdateKey = false;
		return KeyCode.K;
	}else
	
	if(Input.GetKeyDown(KeyCode.D)){
		UpdateKey = false;
		return KeyCode.D;
	}else
	
	if(Input.GetKeyDown(KeyCode.L)){
		UpdateKey = false;
		return KeyCode.L;
	}else
	
	if(Input.GetKeyDown(KeyCode.M)){
		UpdateKey = false;
		return KeyCode.M;
	}else
	
	if(Input.GetKeyDown(KeyCode.N)){
		UpdateKey = false;
		return KeyCode.N;
	}else
	
	if(Input.GetKeyDown(KeyCode.O)){
		UpdateKey = false;
		return KeyCode.O;
	}else
	
	if(Input.GetKeyDown(KeyCode.P)){
		UpdateKey = false;
		return KeyCode.P;
	}else
	
	if(Input.GetKeyDown(KeyCode.Q)){
		UpdateKey = false;
		return KeyCode.Q;
	}else
	
	if(Input.GetKeyDown(KeyCode.R)){
		UpdateKey = false;
		return KeyCode.R;
	}else
	
	if(Input.GetKeyDown(KeyCode.S)){
		UpdateKey = false;
		return KeyCode.S;
	}else
	
	if(Input.GetKeyDown(KeyCode.T)){
		UpdateKey = false;
		return KeyCode.T;
	}else
	
	if(Input.GetKeyDown(KeyCode.U)){
		UpdateKey = false;
		return KeyCode.U;
	}else
	
	if(Input.GetKeyDown(KeyCode.V)){
		UpdateKey = false;
		return KeyCode.V;
	}else
	
	if(Input.GetKeyDown(KeyCode.W)){
		UpdateKey = false;
		return KeyCode.W;
	}else
	
	if(Input.GetKeyDown(KeyCode.X)){
		UpdateKey = false;
		return KeyCode.X;
	}else

	if(Input.GetKeyDown(KeyCode.Y)){
		UpdateKey = false;
		return KeyCode.Y;
	}else
	
	if(Input.GetKeyDown(KeyCode.Z)){
		UpdateKey = false;
		return KeyCode.Z;
	}else
	
	//Function Keys

	if(Input.GetKeyDown(KeyCode.F1)){
		UpdateKey = false;
		return KeyCode.F1;
	}else
	
	if(Input.GetKeyDown(KeyCode.F2)){
		UpdateKey = false;
		return KeyCode.F2;
	}else
	
	if(Input.GetKeyDown(KeyCode.F3)){
		UpdateKey = false;
		return KeyCode.F3;
	}else
	
	if(Input.GetKeyDown(KeyCode.F4)){
		UpdateKey = false;
		return KeyCode.F4;
	}else
	
	if(Input.GetKeyDown(KeyCode.F5)){
		UpdateKey = false;
		return KeyCode.F5;
	}else
	
	if(Input.GetKeyDown(KeyCode.F6)){
		UpdateKey = false;
		return KeyCode.F6;
	}else
	
	if(Input.GetKeyDown(KeyCode.F7)){
		UpdateKey = false;
		return KeyCode.F7;
	}else
	
	if(Input.GetKeyDown(KeyCode.F8)){
		UpdateKey = false;
		return KeyCode.F8;
	}else
	
	if(Input.GetKeyDown(KeyCode.F9)){
		UpdateKey = false;
		return KeyCode.F9;
	}else
	
	if(Input.GetKeyDown(KeyCode.F10)){
		UpdateKey = false;
		return KeyCode.F10;
	}else
	
	if(Input.GetKeyDown(KeyCode.F11)){
		UpdateKey = false;
		return KeyCode.F11;
	}else
	
	if(Input.GetKeyDown(KeyCode.F12)){
		UpdateKey = false;
		return KeyCode.F12;
	}else

	// Top Row Keys
	
	if(Input.GetKeyDown(KeyCode.Alpha1)){
		UpdateKey = false;
		return KeyCode.Alpha1;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha2)){
		UpdateKey = false;
		return KeyCode.Alpha2;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha3)){
		UpdateKey = false;
		return KeyCode.Alpha3;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha4)){
		UpdateKey = false;
		return KeyCode.Alpha4;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha5)){
		UpdateKey = false;
		return KeyCode.Alpha5;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha6)){
		UpdateKey = false;
		return KeyCode.Alpha6;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha7)){
		UpdateKey = false;
		return KeyCode.Alpha7;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha8)){
		UpdateKey = false;
		return KeyCode.Alpha8;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha9)){
		UpdateKey = false;
		return KeyCode.Alpha9;
	}else
	
	if(Input.GetKeyDown(KeyCode.Alpha0)){
		UpdateKey = false;
		return KeyCode.Alpha0;
	}else

	if(Input.GetKeyDown(KeyCode.Minus)){
		UpdateKey = false;
		return KeyCode.Minus;
	}else
	
	if(Input.GetKeyDown(KeyCode.Equals)){
		UpdateKey = false;
		return KeyCode.Equals;
	}else

	if(Input.GetKeyDown(KeyCode.Backspace)){
		UpdateKey = false;
		return KeyCode.Backspace;
	}else
			
	//Keypad
	
	if(Input.GetKeyDown(KeyCode.Keypad1)){
		UpdateKey = false;
		return KeyCode.Keypad1;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad2)){
		UpdateKey = false;
		return KeyCode.Keypad2;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad3)){
		UpdateKey = false;
		return KeyCode.Keypad3;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad4)){
		UpdateKey = false;
		return KeyCode.Keypad4;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad5)){
		UpdateKey = false;
		return KeyCode.Keypad5;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad6)){
		UpdateKey = false;
		return KeyCode.Keypad6;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad7)){
		UpdateKey = false;
		return KeyCode.Keypad7;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad8)){
		UpdateKey = false;
		return KeyCode.Keypad8;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad9)){
		UpdateKey = false;
		return KeyCode.Keypad9;
	}else
	
	if(Input.GetKeyDown(KeyCode.Keypad0)){
		UpdateKey = false;
		return KeyCode.Keypad0;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadPlus)){
		UpdateKey = false;
		return KeyCode.KeypadPlus;
	}else	

	if(Input.GetKeyDown(KeyCode.KeypadMinus)){
		UpdateKey = false;
		return KeyCode.KeypadMinus;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadMultiply)){
		UpdateKey = false;
		return KeyCode.KeypadMultiply;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadDivide)){
		UpdateKey = false;
		return KeyCode.KeypadDivide;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadEquals)){
		UpdateKey = false;
		return KeyCode.KeypadEquals;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadPeriod)){
		UpdateKey = false;
		return KeyCode.KeypadPeriod;
	}else
	
	if(Input.GetKeyDown(KeyCode.KeypadEnter)){
		UpdateKey = false;
		return KeyCode.KeypadEnter;
	}else
	
	//Arrow Keys
	
	if(Input.GetKeyDown(KeyCode.UpArrow)){
		UpdateKey = false;
		return KeyCode.UpArrow;
	}else
	
	if(Input.GetKeyDown(KeyCode.DownArrow)){
		UpdateKey = false;
		return KeyCode.DownArrow;
	}else
	
	if(Input.GetKeyDown(KeyCode.LeftArrow)){
		UpdateKey = false;
		return KeyCode.LeftArrow;
	}else
	
	if(Input.GetKeyDown(KeyCode.RightArrow)){
		UpdateKey = false;
		return KeyCode.RightArrow;
	}else
	
	// Center Keys
	
	if(Input.GetKeyDown(KeyCode.Insert)){
		UpdateKey = false;
		return KeyCode.Insert;
	}else
	
	if(Input.GetKeyDown(KeyCode.Home)){
		UpdateKey = false;
		return KeyCode.Home;
	}else
	
	if(Input.GetKeyDown(KeyCode.PageUp)){
		UpdateKey = false;
		return KeyCode.PageUp;
	}else
	
	if(Input.GetKeyDown(KeyCode.PageDown)){
		UpdateKey = false;
		return KeyCode.PageDown;
	}else
	
	if(Input.GetKeyDown(KeyCode.Delete)){
		UpdateKey = false;
		return KeyCode.Delete;
	}else

	if(Input.GetKeyDown(KeyCode.End)){
		UpdateKey = false;
		return KeyCode.End;
	}

//Special Keys

	if(Input.GetKeyDown(KeyCode.Tab)){
		UpdateKey = false;
		return KeyCode.Tab;
	}else	

	if(Input.GetKeyDown(KeyCode.Return)){
		UpdateKey = false;
		return KeyCode.Return;
	}else		

	if(Input.GetKeyDown(KeyCode.LeftBracket)){
		UpdateKey = false;
		return KeyCode.LeftBracket;
	}else		

	if(Input.GetKeyDown(KeyCode.RightBracket)){
		UpdateKey = false;
		return KeyCode.RightBracket;
	}else	

	if(Input.GetKeyDown(KeyCode.Backslash)){
		UpdateKey = false;
		return KeyCode.Backslash;
	}else	
	
	if(Input.GetKeyDown(KeyCode.Slash)){
		UpdateKey = false;
		return KeyCode.Slash;
	}else		

	if(Input.GetKeyDown(KeyCode.Period)){
		UpdateKey = false;
		return KeyCode.Period;
	}else		

	if(Input.GetKeyDown(KeyCode.Semicolon)){
		UpdateKey = false;
		return KeyCode.Semicolon;
	}else		

	if(Input.GetKeyDown(KeyCode.Comma)){
		UpdateKey = false;
		return KeyCode.Comma;
	}else	
	
	if(Input.GetKeyDown(KeyCode.Quote)){
		UpdateKey = false;
		return KeyCode.Quote;
	}else		
	
	if(Input.GetKeyDown(KeyCode.BackQuote)){
		UpdateKey = false;
		return KeyCode.BackQuote;
	}else	
						
	{
		return ThisInput;
	}
}

void DetectAxis(Axis : int, i : int){
	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_X")) >= .9){
		UpdateAxis[i] = false;
		return 0;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_Y")) >= .9){
		UpdateAxis[i] = false;
		return 1;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_3")) >= .9){
		UpdateAxis[i] = false;
		return 2;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_4")) >= .9){
		UpdateAxis[i] = false;
		return 3;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_5")) >= .9){
		UpdateAxis[i] = false;
		return 4;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_6")) >= .9){
		UpdateAxis[i] = false;
		return 5;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_7")) >= .9){
		UpdateAxis[i] = false;	
		return 6;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_8")) >= .9){
		UpdateAxis[i] = false;
		return 7;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_9")) >= .9){
		UpdateAxis[i] = false;
		return 8;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_1_Axis_10")) >= .9){
		UpdateAxis[i] = false;
		return 9;
	}else
	
	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_X")) >= .9){
		UpdateAxis[i] = false;
		return 0;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_Y")) >= .9){
		UpdateAxis[i] = false;
		return 1;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_3")) >= .9){
		UpdateAxis[i] = false;
		return 2;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_4")) >= .9){
		UpdateAxis[i] = false;
		return 3;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_5")) >= .9){
		UpdateAxis[i] = false;
		return 4;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_6")) >= .9){
		UpdateAxis[i] = false;
		return 5;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_7")) >= .9){
		UpdateAxis[i] = false;	
		return 6;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_8")) >= .9){
		UpdateAxis[i] = false;
		return 7;
	}else

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_9")) >= .9){
		UpdateAxis[i] = false;
		return 8;
	}else 

	if(Mathf.Abs(Input.GetAxis("Joystick_2_Axis_10")) >= .9){
		UpdateAxis[i] = false;
		return 9;
	}else{
		return Axis;
	}
}

void OnGUI(){
	if(GUI.Button(Rect(250,10,100,50),"Set Pitch")){
		UpdateAxis[0] = true;
	}
	if(GUI.Button(Rect(250,60,100,50),"Set Yaw")){
		UpdateAxis[1] = true;
	}
	if(GUI.Button(Rect(250,110,100,50),"Set Roll")){
		UpdateAxis[2] = true;
	}
	if(GUI.Button(Rect(250,160,100,50),"Set Throttle")){
		UpdateAxis[3] = true;
	}
}
*/
}
