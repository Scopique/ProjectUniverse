
//Script is used to disable all targets and their subcomponents for the purpose of testing draw calls used by the Radar UI.

using UnityEngine;
using System.Collections;

public class DisableMR : MonoBehaviour {
	public bool bDisable;
	private bool Disabled;
	// Use this for initialization
	void Update () {
		if(bDisable && !Disabled){
			Disable ();
		}
		if(!bDisable && Disabled){
			Enable ();
		}
	}
	
	// Update is called once per frame
	void Enable(){
		foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if(gameObj.name == "Target" || gameObj.name == "SubComponent0" || gameObj.name == "SubComponent1" || gameObj.name == "SubComponent2" || gameObj.name == "SubComponent3"){
				gameObj.GetComponent<Renderer>().enabled = true;
			}
		}
		Disabled = false;
	}

	void Disable () {
		foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[]){
			if(gameObj.name == "Target" || gameObj.name == "SubComponent0" || gameObj.name == "SubComponent1" || gameObj.name == "SubComponent2" || gameObj.name == "SubComponent3"){
				gameObj.GetComponent<Renderer>().enabled = false;
			}
		}
		Disabled = true;
	}
}
