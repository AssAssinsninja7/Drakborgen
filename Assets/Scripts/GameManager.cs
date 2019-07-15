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

    private GameBoard gameBoard;

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
        if (currentSceneName == "GameBoardTestScene")
        {
            if (!gameInitialized)
            {
                TestSceneInit();
            }

        }
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
    public void SetPlayerInformation(string p1ID, string p2ID, bool p1HasViking, bool p2HasRotRing, Vector3 p1StartPos, Vector3 p2StartPos)
    {
        if (p1ID != null && p1ID != string.Empty && p1StartPos != null)
        {         
            player1.GetComponent<Player>().PlayerID = p1ID;
            player1.GetComponent<Player>().HasViking = p1HasViking;
            player1.GetComponent<Player>().HasRotationRing = !p2HasRotRing; //Opposite of player2's choice
            player1.GetComponent<Player>().Position = p1StartPos;
        }
        if (p2ID != null && p2ID != string.Empty && p2StartPos != null)
        {          
            player2.GetComponent<Player>().PlayerID = p2ID;
            player2.GetComponent<Player>().HasViking = !p1HasViking; //opposite of player1's choice
            player2.GetComponent<Player>().HasRotationRing = p2HasRotRing;
            player2.GetComponent<Player>().Position = p2StartPos;
        }
        SceneManager.LoadScene(sceneBuildIndex: 1, LoadSceneMode.Single); //1 = drakborgen scene //"DrakborgenARScene", 2 = drakborgen testscene
    }


    private void TestSceneInit()
    {
        gameInitialized = true;
        GameObject board = GameObject.Find("GameBoard2");
        gameBoard = board.GetComponent<GameBoard>();                 //arGameBoard;
        gameBoard.InitGameBoard(player1.GetComponent<Player>(), player2.GetComponent<Player>()); //Might want to send in the entire gameobj
        
        Debug.Log(gameBoard);
    }

    /// <summary>
    /// Initialize the game by setting the refrenses and calling the 
    /// gameBoards initialize
    /// </summary>
    public void InitGame()
    {
        if (player1 != null && player2 != null) //Make sure they aint null and load next scene
        {
            gameInitialized = true;           
            gameBoard = GameObject.Find("GameBoard2").GetComponent<GameBoard>();                    //arGameBoard;
            gameBoard.InitGameBoard(player1.GetComponent<Player>(), player2.GetComponent<Player>()); //Might want to send in the entire gameobj
            p1Turn = true;

            Debug.Log(GameManager.instance.name + " has been successfully moved");
        }
        gameInitialized = true;
    }//Fetch the assets like arcore etc from the new scene so that the gameMgr can use it

    /// <summary>
    /// Check the players input on screen and then if they hit the board call the method that checks where on the 
    /// board it was hit in ARController
    /// </summary>
     void CheckPlayerInput(bool isPlayer1)
    {
        //arController.
        arController.GetComponent<ARControllScript>().CheckUserHit();
        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //maybe 
        //{
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //    {
        //        if (hit.collider.tag == "EmptyTile")
        //        {
        //            //RevealRoom(hit.transform.position);
        //            gameBoard.RevealRoom(hit);
        //        }
        //    }
        //}


    }
}
