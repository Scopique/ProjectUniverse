using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetStatus : MonoBehaviour {

	public Slider TargetShields;
	public Slider TargetArmor;
	public Slider TargetHull;

	[Range(0.0f, 1.0f)]
	public float T_CurShields;
	[Range(0.0f, 1.0f)]
	public float T_CurArmor;
	[Range(0.0f, 1.0f)]
	public float T_CurHull;

	// Update is called once per frame

	void Update () {
		TargetShields.value = (T_CurShields);
		TargetArmor.value = (T_CurArmor);
		TargetHull.value = (T_CurHull);
	}
}
