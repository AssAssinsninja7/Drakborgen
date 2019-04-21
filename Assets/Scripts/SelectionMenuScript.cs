using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class SelectionMenuScript : MonoBehaviour
{
    private string player1ID; //I want this as like "A:2" where the letter represents which testsession and the number is the uniqe participant
    private string player2ID; //These will be sent to the gameMgr who needs to spit out the tracked information later

    private bool p2HasRotationRing; //if they have check their answer as the rot ring answer then this is true vice versa

    private bool p1HasViking; //if they have choosen the viking character then this will be true and vice versa

    private bool player1Choosing; //Keeps track of whose turn it is.

    private bool hasPlayerInformation;

    private bool hasStartingPos;

    private bool hasP2StartPos; //Temp just so that i can check which one has selected their pos apart from them both having selected

    private bool informationSent; //Check if the information sent to the gameMGr has happend

    private bool hasSet;

    private Vector3 p1ChoosenStartPos = new Vector2();
    private Vector3 p2ChoosenStartPos = new Vector2();

    //private Player player1;
    //private Player player2;

    public GameManager gameManager;

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

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Dropdown p1StartposDropdown; //The chosen startpos for player1 

    [SerializeField]
    private Dropdown p2StartposDropdown; //The chosen startpos for player2

    #endregion 


    // Start is called before the first frame update
    void Start()
    {
        //player1 = gameObject.AddComponent<Player>();
        //player2 = gameObject.AddComponent<Player>();

        hasPlayerInformation = false;
        hasStartingPos = false;

        player1Choosing = true;

        continueButton.GetComponentInChildren<Text>().text = "Continue"; //Set button text to continue

        infoText.text = "Both players, enter your IDs"; //Set information text 


        p1CharacterDropdown.image.enabled = false;
        p2CharacterDropdown.image.enabled = false;

        p1CharacterDropdown.captionText.enabled = false;
        p2CharacterDropdown.captionText.enabled = false;

        p1RingDropdown.image.enabled = false;
        p2RingDropdown.image.enabled = false;

        p1RingDropdown.captionText.enabled = false;
        p2RingDropdown.captionText.enabled = false;

        p1CharImg.enabled = false;
        p2CharImg.enabled = false;

        startButton.image.enabled = false;

        p1StartposDropdown.image.enabled = false;
        p2StartposDropdown.image.enabled = false;

        p1StartposDropdown.captionText.enabled = false;
        p2StartposDropdown.captionText.enabled = false;

        //Enable keyboard to dropdown when they are entering their IDs'
        if (p1IDField)
        {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
        }
        if (p2IDField)
        {
            TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
        }
        p1IDField.keyboardType = TouchScreenKeyboardType.NumberPad; //Set the IDfields keyboard to only have numbers 0-9 (change this later?)
        p2IDField.keyboardType = TouchScreenKeyboardType.NumberPad;


        if (GameManager.instance == null) //Make sure that the gameMgr has been instantiated
        {
            Instantiate(gameManager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckMenuInput();
    }

    /// <summary>
    /// This method checks what information has been set and then updates which method is called after pressing 
    /// the continue button by setting the added listner to the new method.
    /// </summary>
     void CheckMenuInput()
    {
        if (!hasPlayerInformation && !hasStartingPos)
        {
            if (player1ID == null || player2ID == null || player1ID == string.Empty || player2ID == string.Empty) //If any of the players havent enterd their IDs then the continue will still call that method
            {
                continueButton.onClick.AddListener(SetPlayerID);
            }
            else
            {
                if (player1Choosing)
                {
                    infoText.text = player1ID + " Choose a character";
                    p1CharacterDropdown.image.enabled = true;
                    p1CharacterDropdown.captionText.enabled = true;

                    continueButton.onClick.AddListener(GetPlayerCharacter);
                }
                else
                {
                    infoText.text = player2ID + " Choose a magical ring";
                    p2RingDropdown.image.enabled = true;
                    p2RingDropdown.captionText.enabled = true;

                    continueButton.onClick.AddListener(GetPlayerRings);
                }
            }
        }
        else if (hasPlayerInformation && !hasStartingPos)
        {
            if (player1Choosing && !hasP2StartPos) //P1 turn and P2 hasn't chosen
            {
                infoText.text = player1ID + " choose your starting position.";

                p1StartposDropdown.image.enabled = true;
                p1StartposDropdown.captionText.enabled = true;
               
                continueButton.onClick.AddListener(GetP1StartPos);
            }
            else if (!player1Choosing && !hasP2StartPos)//P2 turn and P2 hasn't selected
            {
                infoText.text = player2ID + " choose your starting postion.";
                player1Choosing = false;

                p2StartposDropdown.image.enabled = true;
                p2StartposDropdown.captionText.enabled = true;

                p1StartposDropdown.image.enabled = false; //Deactivate player1 selection option     

                continueButton.onClick.AddListener(GetP2StartPos);
            }

        }
        else if (hasPlayerInformation && hasStartingPos)
        {           
            if (!hasSet)
            {
                continueButton.enabled = false; //disable the continue button
                continueButton.image.enabled = false;
                continueButton.GetComponentInChildren<Text>().text = string.Empty;

                p2StartposDropdown.image.enabled = false; //disable player2 startpos dropdown
                p2StartposDropdown.captionText.enabled = false;

                infoText.text = "Setup ready, Start the game";

                SetPlayerInfo();
            }
              
        }
    }

    /// <summary>
    /// Checks if the fields are empty and if they arn't then it will set the PlayerIDs to 
    /// whats in the fields otherwise it will highligt that it's missing and request the users
    /// to re enter the information.
    /// </summary>
    void SetPlayerID()
    {

            if (p1IDField.text != string.Empty && p1IDField.text != "Enter your ID") //now it can add errything maybe change it so it looks at the string itself
            {
                p1IDField.keyboardType = TouchScreenKeyboardType.Default;
                player1ID = p1IDField.text;
                p1IDField.image.color = Color.white;
            }
            else
            {
                p1IDField.keyboardType = TouchScreenKeyboardType.Default;
                p1IDField.image.color = Color.red;
                p1IDField.text = "Enter your ID";
            }
            
            if (p2IDField.text != string.Empty && p2IDField.text != "Enter your ID")
            {
                player2ID = p2IDField.text;
                p2IDField.image.color = Color.white;
            }
            else
            {
                p2IDField.image.color = Color.red;
                p2IDField.text = "Enter your ID";
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
            p1HasViking = true;

            //Set image to vikin
        }
        else if (p1CharacterDropdown.captionText.text == "Fransiscus Monk")
        {
            p1HasViking = false;
            //Set image to monk
        }

        p2CharacterDropdown.image.enabled = true;
        p2CharacterDropdown.captionText.enabled = true;
        p2CharacterDropdown.enabled = false;

        p1CharacterDropdown.enabled = false;

        if (p1HasViking)
        {
            p2CharacterDropdown.value = 1;
        }

        player1Choosing = false;
    }

    /// <summary>
    /// Takes in the selected magical ring from player2 and sets the "hasRotation..." to true if they have the rotation ring
    /// or false if not. Once player2 presses continue the value is set in the CheckInputManager.
    /// </summary>
    void GetPlayerRings()
    {
        if (p2RingDropdown.captionText.text == "Ring of knowledge")
        {
            p2HasRotationRing = true;
            //maybe even have a img for the rings but only if there's time
        }
        else if (p2RingDropdown.captionText.text == "Ring of faith")
        {
            p2HasRotationRing = false;
        }

        p1RingDropdown.image.enabled = true; //Show player1 ring result
        p1RingDropdown.enabled = false; //Disable the interactibility with the dropdownbox for player1

        p2RingDropdown.enabled = false; //Disable the interactibility with the dropdownbox for player2

        if (p2HasRotationRing) //if p2 selected the rot ring the selected text will be the opposite for the otehr player vice versa, aand only the player selecting can see
        {
            p1RingDropdown.value = 1;
        }

        p1RingDropdown.captionText.enabled = true;
        player1Choosing = true;
        hasPlayerInformation = true;
    }

    /// <summary>
    /// Get player1 chosen startpos and disable that as an option for player2
    /// </summary>
    void GetP1StartPos()
    {
        string blockedPosName = p1StartposDropdown.captionText.text; //Player1 chosen starting position (is then blocked as an option for player2)

        if (player1Choosing && !hasP2StartPos)
        {
            if (p1StartposDropdown.captionText.text == "Top Left")
            {
                p1ChoosenStartPos = new Vector3(0, 0, 0);
            }
            else if (blockedPosName == "Top Right")
            {
                p1ChoosenStartPos = new Vector3(9, 0, 0);
            }
            else if (blockedPosName == "Bottom Left")
            {
                p1ChoosenStartPos = new Vector3(0, 0, 6);
            }
            else if (blockedPosName == "Bottom Right")
            {
                p1ChoosenStartPos = new Vector3(9, 0, 6);
            }

            List<Dropdown.OptionData> p2startOptionList = p2StartposDropdown.options; //So that we can find which option to remove by text
            for (int i = 0; i < p2startOptionList.Count; i++)
            {
                if (p2startOptionList[i].text.Equals(blockedPosName)) //remove player1 chosen start pos as an option for player2
                {
                    p2StartposDropdown.options.RemoveAt(i);
                }
            }

            //player1.Position = p1ChoosenStartPos;
            player1Choosing = false; //player1 turn is now over
            continueButton.onClick.AddListener(GetP2StartPos);
        }      
    }

    /// <summary>
    /// Get player2 choosen startpos and then continue on to starting the game
    /// </summary>
    void GetP2StartPos()
    {
        string blockedPosName = p1StartposDropdown.captionText.text; //Player1 chosen starting position (is then blocked as an option for player2)      
    
        if (!player1Choosing && !hasP2StartPos) //So long as they don't have the same start pos
        {
            if (p2StartposDropdown.captionText.text == "Top Left")
            {
                p2ChoosenStartPos = new Vector3(0, 0, 0); 
            }
            else if (p2StartposDropdown.captionText.text == "Top Right")
            {
                p2ChoosenStartPos = new Vector3(9, 0, 0); 
            }
            else if (p2StartposDropdown.captionText.text == "Bottom Left")
            {
                p2ChoosenStartPos = new Vector3(0, 0, 6);
            }
            else if (p2StartposDropdown.captionText.text == "Bottom Right")
            {
                p2ChoosenStartPos = new Vector3(9, 0, 6);
            }

            //player2.Position = p2ChoosenStartPos;
            hasP2StartPos = true;
        }

        if (p1ChoosenStartPos != p2ChoosenStartPos && hasP2StartPos) //So long as they haven't selected the same 
        {
            hasStartingPos = true;
        }
    }

    /// <summary>
    /// Just sets the players information after all of it has been send in
    /// </summary>
    void SetPlayerInfo()
    {
        //player1.PlayerID = player1ID;
        //player1.HasViking = p1HasViking;

        //player2.PlayerID = player2ID;
        //player2.p2HasRotationRing = p2HasRotationRing;

        //player1.p2HasRotationRing = !player2.p2HasRotationRing;
        //player2.HasViking = !player1.HasViking;

        startButton.image.enabled = true;
        startButton.onClick.AddListener(StartGame); //connect the button to the method that starts the game

        hasSet = true;
    }

    void StartGame()
    {
        //send both players information to the game mgr, unload the scene, deaactivate the selection camera and activate the *ARcamera. 
        //if (!informationSent)
        //{
            gameManager.SetPlayerInformation(player1ID, player2ID, p1HasViking, p2HasRotationRing, p1ChoosenStartPos, p2ChoosenStartPos); //The players arn't instansiated as gameobject but this will be fixed once they get their prefabs/ become full objects 
            informationSent = true;
        //}
    }
}
