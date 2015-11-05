using UnityEngine;
using System.Collections;

public class DestroyThis : MonoBehaviour {

	public bool DestroyNow;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(DestroyNow){
			this.gameObject.GetComponent<FX_3DRadar_RID>().DestroyThis();
			DestroyNow = false;
		}
	}
}
