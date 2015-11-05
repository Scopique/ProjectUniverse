using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FX_Class_Mgr : MonoBehaviour {

	[System.Serializable]
	public class objectClassList{
		public string ClassName = "New Class";
		public List<string> SubClassName = new List<string>(1);
		public List<Sprite> ClassSprite = new List<Sprite>(1);
		public List<Vector3> IDOffset = new List<Vector3>(1);
	}
	
	public List<objectClassList> ObjectClassList = new List<objectClassList>(1);
}