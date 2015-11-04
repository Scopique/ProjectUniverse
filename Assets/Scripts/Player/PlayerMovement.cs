using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {


    
     /*http://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/
     * https://code.google.com/p/unity3d-witchtraining/source/browse/trunk/+unity3d-witchtraining/Assets/scripts/Player/ThirdPersonFlyingController.cs?r=67
     * https://keithmaggio.wordpress.com/2011/07/01/unity-3d-code-snippet-flight-script/
     * http://answers.unity3d.com/questions/10026/first-person-flight-controls.html?page=1&pageSize=5&sort=votes
     * http://answers.unity3d.com/questions/236646/rigidbody-physics-flight-simulator-script-help.html 
     */


    #region Inspector Variables

    public bool isMouseControl = false;

    public float currentThrustSpeed = 0.0f;
    public float maxForwardThrustSpeed = 100.0f;
    public float maxReverseThrustSpeed = 100.0f;
    public float rollSpeed = 2.0f;
    public float pitchSpeed = 5.0f;
    public float yawSpeed = 5.0f;

    #endregion

    #region Local Variables

    Rigidbody rb;

    #endregion

    #region Unity Methods
    
    // Use this for initialization
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            isMouseControl = !isMouseControl;
        }
        
        if (isMouseControl)
        {
            //Get the values from the PlayerController based on the equipped engine
            //TODO: Do we NEED to update ship velocity values in Update?
            //  Values only change when gear changes, and gear can only change
            //  when not using it.
            pitchSpeed = PlayerController.playerController.CurrentPitch;
            yawSpeed = PlayerController.playerController.CurrentYaw;
            rollSpeed = PlayerController.playerController.CurrentRoll;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            //Don't let them exceed the max thrust, and don't let them go in reverse
            if (currentThrustSpeed + (Input.mouseScrollDelta.y * 5) <= PlayerController.playerController.CurrentThrust 
                && currentThrustSpeed + (Input.mouseScrollDelta.y * 5) >= 0)
            { 
                currentThrustSpeed += (Input.mouseScrollDelta.y) * 5;
            }

            float yaw = -Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;
            float pitch = Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;
            float roll = -Input.GetAxis("Horizontal") * rollSpeed * Time.deltaTime;
            Vector3 direction = new Vector3(yaw, pitch, currentThrustSpeed);
        
            transform.Rotate(new Vector3(yaw, pitch, roll));
        
            transform.Translate(0, 0, currentThrustSpeed * Time.deltaTime);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    #endregion

    #region Private Methods
    


    #endregion
}
