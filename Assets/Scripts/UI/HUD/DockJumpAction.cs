using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DockJumpAction : MonoBehaviour
{

    #region Inspector Variables

    public bool buttonActive = false;           //Only active when it can be used

    #endregion

    #region Local Variables

    Button button;                              //The button component of the game object
    Text buttonText;                            //The text component on the first child

    string StructureTag;                        //What did we bump into?

    #endregion

    #region Unity Methods
    
    // Use this for initialization
	void Start () {
	    //Add a listener for the different kinds of actions to take
        Messenger.AddListener<string>("PlayerStructureProximity", PlayerStructureProximity_Handler);
       
        button = gameObject.GetComponent<Button>();
        buttonText = gameObject.transform.GetChild(0).gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        button.interactable = buttonActive;

        if (Input.GetKey(KeyCode.F) && buttonActive)
        {
            //DoTheThing
            PlayerStructureProximity_Action();
        }
	}
    
    #endregion

    #region Public Methods
    
    #endregion

    #region Private Methods

    private void PlayerStructureProximity_Handler(string Tag)
    {
        StructureTag = Tag;

        switch (Tag)
        {
            case "Station":
                buttonText.text = "Dock";
                buttonActive = true;
                break;
            case "Jumpgate":
                buttonText.text = "Jump";
                buttonActive = true;
                break;
            case "Exit":
                buttonText.text = "---";
                buttonActive = false;
                break;
            default:
                buttonText.text = "---";
                buttonActive = false;
                break;
        }
    }

    private void PlayerStructureProximity_Action()
    {
        //Take an action based on the StructureTag value?
        switch(StructureTag)
        {
            case "Station":
                Messenger.Broadcast("OpenPlayerDockingMenu");
                break;
            case "Jumpgate":
                Messenger.Broadcast("OpenPlayerJumpingMenu");
                break;
            case "Exit":
                Messenger.Broadcast("CloseStructureMenu");
                break;
            default:
                break;
        }
    }

    #endregion
}
