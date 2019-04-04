using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenuScript : MonoBehaviour
{
    private string player1ID; //I want this as like "A:2" where the letter represents which testsession and the number is the uniqe participant
    private string player2ID; //These will be sent to the gameMgr who needs to spit out the tracked information later

    private bool hasRotationRing; //if they have check their answer as the rot ring answer then this is true vice versa

    private bool playingVikingCharacter; //if they have choosen the viking character then this will be true and vice versa

    private bool player1Choosing; //Keeps track of whose turn it is.

    //All the objects loaded in through SerialieField are here below.
    #region LoadedIn
    [SerializeField]
    private InputField p1IDField; //Player1 ID input field

    [SerializeField]
    private InputField p2IDField; //Player2 ID input field

    [SerializeField]
    private Dropdown p1RingDropdown; //Player1 Ring dropdown menu

    [SerializeField]
    private Dropdown p2RingDropdown; //Player2 Ring dropdown menu

    [SerializeField]
    private Dropdown p1CharacterDropdown; //Player1 character dropdown menu

    [SerializeField]
    private Dropdown p2CharacterDropdown; //Player2 character dropdown menu

    [SerializeField]
    private Image p1CharImg; //Player1 character image (I thought they could be displayed on screen as to create clearity for who's playing who)

    [SerializeField]
    private Image p2CharImg; //Player2 character image (Maybe do the same thing for the rings but atm fuck it)

    [SerializeField]
    private Button continueButton; //The button for controlling the entering of infromation (But it's just a button, duh)

    [SerializeField]
    private Text infoText; //Displays the information for what to do in right now in the menu. 
    #endregion 


    // Start is called before the first frame update
    void Start()
    {
        player1Choosing = true;

        continueButton.GetComponentInChildren<Text>().text = "Continue"; //Set button text to continue

        infoText.text = "Both players, enter your IDs"; //Set information text 


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// This method checks what information has been set and then updates which method is called after pressing 
    /// the continue button by setting the added listner to the new method.
    /// </summary>
    void CheckMenuInput()
    {
        if (player1ID != string.Empty && player2ID != string.Empty) //Player script object is suppose to be checked noot these temp variables
        {
            player1Choosing = true;
            continueButton.onClick.AddListener(GetPlayerCharacter); 
        }


    }

    /// <summary>
    /// Takes in the player id and saves it
    /// </summary>
    void SetPlayerID()
    {
            if (p1IDField.text != string.Empty) //now it can add errything maybe change it so it looks at the string itself
            {
                player1ID = p1IDField.text;
            }

            if (p2IDField.text != string.Empty)
            {
                player2ID = p2IDField.text;
            }
    }

    /// <summary>
    /// Takes in the selected character for player1 and sets the "playingVikin..." bool to true if the 
    /// Vikin character is selected otherwise it will put the Monk as seleceted. Once player1 presses the continue button
    /// the value is set in CheckInputManager.
    /// </summary>
    void GetPlayerCharacter()
    {
        if (p1CharacterDropdown.captionText.text == "Sven Viking")
        {
            playingVikingCharacter = true;
            //Set image to vikin
        }
        else if (p1CharacterDropdown.captionText.text == "Fransiscus Monk")
        {
            playingVikingCharacter = false;
            //Set image to monk
        }
    }

    /// <summary>
    /// Takes in the selected magical ring from player2 and sets the "hasRotation..." to true if they have the rotation ring
    /// or false if not. Once player2 presses continue the value is set in the CheckInputManager.
    /// </summary>
    void SetPlayerRings()
    {
        if (p2RingDropdown.captionText.text == "Ring of faith")
        {
            hasRotationRing = false;
            //maybe even have a img for the rings but only if there's time
        }
        else if(p2RingDropdown.captionText.text == "//Ring of knowledge") 
        {
            hasRotationRing = true;
        }
    }
}
