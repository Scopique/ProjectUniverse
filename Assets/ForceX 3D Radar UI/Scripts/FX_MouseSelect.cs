using UnityEngine;
using System.Collections;

public class FX_MouseSelect : MonoBehaviour {

	public Transform ThisParent;

	public void SetTarget(){
		GameObject.Find("_GameMgr").GetComponent<FX_3DRadar_Mgr>().SetTarget(ThisParent);
	}
}
