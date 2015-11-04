using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/*########################################################################
 * Handles display of the crew, and management of their helth display
 * ------------------------------------------------------------------
 * A crew hotbar button sits on the side of the screen and provides a 
 * portrait of the crew member, and a slider which represents the current
 * health of the crew member. 
 * 
 * This script should be placed on the PANEL object which represents the
 * crew "button". It's not a traditional button, but should allow for
 * clicking and event handling just the same. 
 * 
 * This script does three things: Updates the portrait with the portrait
 * of the crew member assigned to this slot, handles the click of the 
 * portrait pseudo button, and adjusts the health of the crew member.
 *######################################################################*/
public class CrewHotbarButton : MonoBehaviour
{

    #region Inspector Variables

    public int slotID = 0;

    #endregion

    #region Local Variables

    CrewMemberDataObject cmdo;    
    float crewMemberHealthPercent = 100.0f;      //% means never having to update the max health on the bar

    #endregion

    #region Unity Methods
    
    // Use this for initialization
    void Start()
    {
        CrewMemberDataObject cm = PlayerController.playerController.GetCrewInSlot(slotID);
        AssignCrewMember(cm);
    }

    // Update is called once per frame
    void Update()
    {
        //Do we have a crew member assigned to this slot?
        //Check the PlayerController for info on the crew.
        //If the crew assigned to this slot differs from the
        //  crew ID we have assigned in private var, then update
        //  everything about this: Portrait, vars, and health display.
        //Otherwise, just deal with the health display.

        
    }

    #endregion

    #region Public Methods
    
    //Add a click handler here.
    public void PortraitClick()
    {
        //Do something. We have the cmdo data attached, so we can work with that.
    }

    public void AssignCrewMember(CrewMemberDataObject newCrewMember)
    {
        cmdo = newCrewMember;

        //Assign the portrait
        Sprite portraitSprite = Resources.Load<Sprite>("CrewPortraits64x64/" + cmdo.Portrait);
        AssignPortrait(portraitSprite);

        //Set the health bar to start
        crewMemberHealthPercent = ((cmdo.HealthCurrent*1.0f) / (cmdo.HealthTotal*1.0f)) * 100.0f;
        AssignCurrentHealthPercent(crewMemberHealthPercent);

        //Clean up
        Resources.UnloadAsset(portraitSprite);

    }

    #endregion

    #region Private Methods

    private void AssignPortrait(Sprite PortraitSprite)
    {
        //Get the CrewPortrait GO, and it's IMAGE compontent
        GameObject crewPortrait = gameObject.transform.GetChild(0).gameObject;
        Image crewImageComponent = crewPortrait.GetComponent<Image>();
        crewImageComponent.sprite = PortraitSprite;
    }

    private void AssignCurrentHealthPercent(float HealthPercent)
    {
        //Get the slider
        GameObject crewHealth = gameObject.transform.GetChild(1).gameObject;
        Slider crewHealthComponent = crewHealth.GetComponent<Slider>();
        crewHealthComponent.value = HealthPercent;
    }

    #endregion
}
