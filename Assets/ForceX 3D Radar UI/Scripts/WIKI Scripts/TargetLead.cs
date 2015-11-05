
//http://wiki.unity3d.com/index.php/Calculating_Lead_For_Projectiles

using UnityEngine;
using System.Collections;

public class TargetLead : MonoBehaviour {

	//first-order intercept using absolute target position
	public static Vector3 FirstOrderIntercept(Vector3 PlayerPos, Vector3 PlayerVel, Vector3 TargetPos, Vector3 TargetVel, float ProjectileVelocity){

		Vector3 TargetRelPos = TargetPos - PlayerPos;
		Vector3 targetRelativeVelocity = TargetVel - PlayerVel;
		float t = FirstOrderInterceptTime(ProjectileVelocity, TargetRelPos, targetRelativeVelocity);
		return TargetPos + t*(targetRelativeVelocity);
	}
	//first-order intercept using relative target position
	public static float FirstOrderInterceptTime(float ProjectileVelocity, Vector3 TargetRelPos, Vector3 TargetRelVel){
		float velocitySquared = TargetRelVel.sqrMagnitude;
		if(velocitySquared < 0.001f)
			return 0f;
		
		float a = velocitySquared - ProjectileVelocity*ProjectileVelocity;
		
		//handle similar velocities
		if (Mathf.Abs(a) < 0.001f){
			float t = -TargetRelPos.sqrMagnitude/(2f * Vector3.Dot(TargetRelVel, TargetRelPos));
			return Mathf.Max(t, 0f); //don't shoot back in time
		}
		
		float b = 2f*Vector3.Dot(TargetRelVel, TargetRelPos);
		float c = TargetRelPos.sqrMagnitude;
		float determinant = b*b - 4f*a*c;
		
		if (determinant > 0f) { //determinant > 0; two intercept paths (most common)
			float	t1 = (-b + Mathf.Sqrt(determinant))/(2f*a),
			t2 = (-b - Mathf.Sqrt(determinant))/(2f*a);
			if (t1 > 0f) {
				if (t2 > 0f)
					return Mathf.Min(t1, t2); //both are positive
				else
					return t1; //only t1 is positive
			} else
				return Mathf.Max(t2, 0f); //don't shoot back in time
		} else if (determinant < 0f) //determinant < 0; no intercept path
			return 0f;
		else //determinant = 0; one intercept path, pretty much never happens
			return Mathf.Max(-b/(2f*a), 0f); //don't shoot back in time
	}
}
