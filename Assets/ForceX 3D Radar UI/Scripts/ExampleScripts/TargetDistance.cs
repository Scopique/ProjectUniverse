using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TargetDistance : MonoBehaviour {

	public Text TargetDistTxt;
	bool Enabled = true;
	FX_3DRadar_Mgr FX3DRM;

	void Awake(){
		FX3DRM = GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>();
	}

	// Update is called once per frame
	void Update () {
		if(FX3DRM.CurTarget){
			if(!Enabled){
				//Turn on the text when we have a target
				TargetDistTxt.enabled = true;
				Enabled = true;
			}
			float DisplayDistance = Vector3.Distance(FX3DRM.CurTarget.position, FX3DRM.PlayerPos) * FX3DRM.WorldScale;

			if(DisplayDistance < 1000){
				TargetDistTxt.text = FX3DRM.CurTarget.name.ToString() + " Distance: " + DisplayDistance.ToString("0. :m");
			}else{
				TargetDistTxt.text = FX3DRM.CurTarget.name.ToString() + " Distance: " + (DisplayDistance *.001).ToString("#.0 :km");
			}
		}else if(Enabled){
			//Turn off the text when we do not have target
			TargetDistTxt.enabled = false;
			Enabled = false;
		}
	}
}
