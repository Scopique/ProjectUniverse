using UnityEngine;
using System.Collections;

public class FactionRelations : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject.Find ("_GameMgr").GetComponent<FX_Faction_Mgr>().GetRelations(0);
	}
}
