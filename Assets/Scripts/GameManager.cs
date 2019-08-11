using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using GoogleARCore;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //this gamemgr

    public GameObject arController; //So that we can check if we have viable planes

    public GameObject playerModel; //Should be two here later

    public GameObject player1;
    public GameObject player2;    

    private BoardScript boardScript;

    private Text planeInfoTexT;

    private bool gameInitialized;

    private string currentSceneName;

    public bool HasPlanes { get; set; }

    //DrakborgenController
    private bool p1Turn; //player1's turn (starting players turn)
    


    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null) //if yes instansiate it 
        {
            instance = this; //Set it to this class

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);           
        }
        else if (instance != this) //If instance already exists and it's not this:
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(player1);
        DontDestroyOnLoad(player2);

        gameInitialized = false;
        p1Turn = true;

        Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen); //doesn't work
        Screen.orientation = ScreenOrientation.LandscapeLeft; 
    }

    // Update is called once per frame
    void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "DrakborgenARScene")
        {
            if (arController != GameObject.Find("ARController"))
            {
                arController = GameObject.Find("ARController");
            }

            //if (!gameInitialized) //IF game hasn't been initialized
            //{
            //    InitGame(); //initialize it
            //}
            //else //Start gameLogic take hit pos from ARcontroller and move players here
            //{               
            if (gameInitialized)
            {
                if (p1Turn)
                {
                    //ToDO: Check which room they are stanging in and present their options
                    CheckPlayerInput(true); //just calling the basic function for trying to place tile in scene
                }
                else
                {

                }
            }
                
            //}         
        }
        //if (currentSceneName == "boardScriptTestScene")
        //{
        //    if (!gameInitialized)
        //    {
        //        //TestSceneInit();
        //    }

        //}
    }

    /// <summary>
    /// Sets the playerinformation sent in from the selectionmenu
    /// </summary>
    /// <param name="p1ID">player1s enterd ID</param>
    /// <param name="p2ID">player2s enterd ID</param>
    /// <param name="p1HasViking">Does player1 have vikin character?</param>
    /// <param name="p2HasRotRing">Does player2 have the rotationring?</param>
    /// <param name="p1StartPos">player1s starting position</param>
    /// <param name="p2StartPos">player2s strarting position</param>
    public void SetPlayerInformation(string p1ID, string p2ID, bool p1HasViking, bool p2HasRotRing, Vector2 p1StartPos, Vector2 p2StartPos)
    {      
        if (p1ID != null && p1ID != string.Empty && p1StartPos != null)
        {
            Undo.RecordObject(player1, "player1 info has been set"); //allows me to override the prefabs values so that they get saved
            player1.GetComponent<Player>().PlayerID = p1ID;
            Undo.RecordObject(player1, "player1 character has been set");
            player1.GetComponent<Player>().HasViking = p1HasViking;
            Undo.RecordObject(player1, "player1 ring has been set");
            player1.GetComponent<Player>().HasRotationRing = !p2HasRotRing; //Opposite of player2's choice
            Undo.RecordObject(player1, "player1 Start position has been set");
            player1.GetComponent<Player>().Position = p1StartPos;           
        }
        if (p2ID != null && p2ID != string.Empty && p2StartPos != null)
        {
            Undo.RecordObject(player2, "player2 id has been set");       
            player2.GetComponent<Player>().PlayerID = p2ID;
            Undo.RecordObject(player2, "player2 character has been set");
            player2.GetComponent<Player>().HasViking = !p1HasViking; //opposite of player1's choice
            Undo.RecordObject(player2, "player2 ring has been set");
            player2.GetComponent<Player>().HasRotationRing = p2HasRotRing;
            Undo.RecordObject(player2, "player2 Start position has been set");
            player2.GetComponent<Player>().Position = p2StartPos;          
        }
        SceneManager.LoadScene(sceneBuildIndex: 2, LoadSceneMode.Single); //1 = drakborgen scene //"DrakborgenARScene", 2 = drakborgen testscene
    }


    //private void TestSceneInit()
    //{
    //    gameInitialized = true;
    //    GameObject board = GameObject.Find("boardScript2");
    //    boardScript = board.GetComponent<boardScript>();                 //arboardScript;
    //    boardScript.InitboardScript(player1.GetComponent<Player>(), player2.GetComponent<Player>()); //Might want to send in the entire gameobj
        
    //    Debug.Log(boardScript);
    //}

    /// <summary>
    /// Initialize the game by setting the refrenses to the boards script in scnene
    /// and then calls its method to place the player characters by creating copies 
    /// that has the right "color based on what character the players chose"
    /// </summary>
    public void InitGame()
    {
        if (player1 != null && player2 != null) //Make sure they aint null and load next scene
        {
            boardScript = GameObject.Find("drakborgenBoard3").GetComponent<BoardScript>();                  
            boardScript.PlacePlayerAvatarOnStart(player1, player2, player1.GetComponent<Player>().HasViking); //might want to init the players here so that the prefabs values are contained with the avatar
            p1Turn = true;

            Debug.Log(GameManager.instance.name + " has been successfully moved");
            gameInitialized = true;
        }
       
    }

    /// <summary>
    /// Check the players input on screen and then if they hit the board call the method that checks where on the 
    /// board it was hit in ARController
    /// </summary>
     void CheckPlayerInput(bool isPlayer1)
    {
        ////arController.
        //arController.GetComponent<ARControllScript>().CheckUserHit();
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //maybe 
        //{
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        if (hit.collider.tag == "EmptyTile")
        //        {
        //            //RevealRoom(hit.transform.position);
        //            boardScript.RevealRoom(hit);
        //        }
        //    }
        //}

        //check if any avaliable exits for player "if yes: check user input" "if no: player cant contunie -> gameover for them, next player"
    }
}
