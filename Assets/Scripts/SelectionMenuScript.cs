using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SelectionMenuScript : MonoBehaviour
{
    private string player1ID; //I want this as like "A:2" where the letter represents which testsession and the number is the uniqe participant
    private string player2ID; //These will be sent to the gameMgr who needs to spit out the tracked information later

    private bool HasRotationRing; //if they have check their answer as the rot ring answer then this is true vice versa

    private bool playingVikingCharacter; //if they have choosen the viking character then this will be true and vice versa

    private bool player1Choosing; //Keeps track of whose turn it is.

    private bool hasPlayerInformation;

    private bool hasStartingPos;

    private Player player1;
    private Player player2;

    private GameManager gameManager;

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
        player1 = new Player();
        player2 = new Player();

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
                    p1CharacterDropdown.captionText.enabled = true;
                    continueButton.onClick.AddListener(GetPlayerCharacter);
                }
                else
                {
                    p2RingDropdown.captionText.enabled = true;
                    continueButton.onClick.AddListener(GetPlayerRings);
                }
            }

            if (player1ID != null && player2ID != null && player1ID != string.Empty && player2ID != string.Empty) //Player script object is suppose to be checked noot these temp variables
            {
                if (player1Choosing == true)
                {
                    infoText.text = player1ID + " Choose a character";
                    p1CharacterDropdown.image.enabled = true;
                }
                else
                {
                    infoText.text = player2ID + " Choose a magical ring";
                    p2RingDropdown.image.enabled = true;
                }
            }
        }
        else if(hasPlayerInformation && !hasStartingPos)
        {
            
            GetStartingPositions();
            
            if(hasStartingPos)
            {
                continueButton.enabled = false;
                continueButton.image.enabled = false;
                continueButton.GetComponentInChildren<Text>().text = string.Empty;

                infoText.text = "Setup ready, Start the game";

                SetPlayerInfo();

                startButton.onClick.AddListener(StartGame); //connect the button to the method that starts the game
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
            player1ID = p1IDField.text;
            p1IDField.image.color = Color.white;
        }
        else
        {
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
            playingVikingCharacter = true;
            
            //Set image to vikin
        }
        else if (p1CharacterDropdown.captionText.text == "Fransiscus Monk")
        {
            playingVikingCharacter = false;
            //Set image to monk
        }

        p2CharacterDropdown.image.enabled = true;
        p2CharacterDropdown.captionText.enabled = true;
        p2CharacterDropdown.enabled = false;

        p1CharacterDropdown.enabled = false;

        if (playingVikingCharacter)
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
            HasRotationRing = true;
            //maybe even have a img for the rings but only if there's time
        }
        else if(p2RingDropdown.captionText.text == "Ring of faith") 
        {
            HasRotationRing = false;
        }

        p1RingDropdown.image.enabled = true;
        p1RingDropdown.enabled = false;

        p2RingDropdown.enabled = false;

        if (HasRotationRing) //if p2 selected the rot ring the selected text will be the opposite for the otehr player vice versa, aand only the player selecting can see
        {
            p1RingDropdown.value = 1;
        }

        p1RingDropdown.captionText.enabled = true;     
        player1Choosing = true;
        hasPlayerInformation = true;
    }

    void GetStartingPositions()
    {
        string blockedPosName = p1StartposDropdown.captionText.text; //Player1 chosen starting position (is then blocked as an option for player2)
        Vector2 chosenStartPosition = new Vector2();

        if (player1Choosing)
        {
            infoText.text = player1ID + "choose your starting position.";
            p1StartposDropdown.image.enabled = true;
                        
            if (p1StartposDropdown.captionText.text == "Top Left")
            {
                chosenStartPosition = new Vector2(0, 0);
            }
            else if (blockedPosName == "Top Right")
            {
                chosenStartPosition = new Vector2(9, 0);
            }
            else if (blockedPosName == "Bottom Left")
            {
                chosenStartPosition = new Vector2(0, 6);
            }
            else if (blockedPosName == "Bottom Right")
            {
                chosenStartPosition = new Vector2(9, 6);
            }

            player1.Position = chosenStartPosition;

            player1Choosing = false;

            continueButton.onClick.AddListener(GetStartingPositions);        
        }
        else
        {
            infoText.text = player2ID + "choose your starting postion.";

            p1StartposDropdown.image.enabled = false;
            p2StartposDropdown.image.enabled = true;

            if (p2StartposDropdown.captionText.text == "Top Left" && p2StartposDropdown.captionText.text != blockedPosName)
            {
                chosenStartPosition = new Vector2(0, 0);
            }
            else if (p2StartposDropdown.captionText.text == "Top Right" && p2StartposDropdown.captionText.text != blockedPosName)
            {
                chosenStartPosition = new Vector2(9, 0);
            }
            else if (p2StartposDropdown.captionText.text == "Bottom Left" && p2StartposDropdown.captionText.text != blockedPosName)
            {
                chosenStartPosition = new Vector2(0, 6);
            }
            else if (p2StartposDropdown.captionText.text == "Bottom Right" && p2StartposDropdown.captionText.text != blockedPosName)
            {
                chosenStartPosition = new Vector2(9, 6);
            }

            player2.Position = chosenStartPosition;
            hasStartingPos = true;
        }
        
    }

    /// <summary>
    /// Just sets the players information after all of it has been send in
    /// </summary>
    void SetPlayerInfo()
    {
        player1.PlayerID = player1ID;
        player1.HasViking = playingVikingCharacter;

        player2.PlayerID = player2ID;
        player2.HasRotationRing = HasRotationRing;

        player1.HasRotationRing = !player2.HasRotationRing;
        player2.HasViking = !player1.HasViking;

        startButton.image.enabled = true;
    }

    void StartGame()
    {
        //send both players information to the game mgr, unload the scene, deaactivate the selection camera and activate the *ARcamera. 
        gameManager.InitGame(player1, player2);       
    }
}
