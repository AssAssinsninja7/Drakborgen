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

    public ARControllScript arController; //So that we can check if we have viable planes

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
        if (instance == null) // ya
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

        Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen); //doesn't work
        Screen.orientation = ScreenOrientation.Landscape; 
    }

    // Update is called once per frame
    void Update()
    {
        if (instance != this)
        {
            Destroy(gameBoard);
        }

        currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "DrakborgenARScene")
        {
            if (!gameInitialized) //IF game hasn't been initialized
            {
                InitGame(); //initialize it
            }
            else //Start gameLogic take hit pos from ARcontroller and move players here
            {
                if (p1Turn)
                {
                    //Check which room they are stanging in and present their options

                }
                else
                {

                }
            }         
        }
    }

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
        SceneManager.LoadScene(sceneBuildIndex: 1, LoadSceneMode.Single); //1 = drakborgen scene //"DrakborgenARScene" //sceneName: "DrakborgenARScene"      
    }

    void InitGame()
    {
        if (player1 != null && player2 != null) //Make sure they aint null and load next scene
        {
            gameInitialized = true;
            arController = GameObject.Find("ARController").GetComponent<ARControllScript>();
            gameBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoard>();
            //planeInfoTexT = GameObject.Find("PlaneStatusCanvas").GetComponentInChildren<Text>();//Check to see if it finds the right text asset in the scene

            gameBoard.InitGameBoard(player1.GetComponent<Player>(), player2.GetComponent<Player>()); //Might want to send in the entire gameobj
        }
    }//Fetch the assets like arcore etc from the new scene so that the gameMgr can use it

     void CheckPlayerInput()
    {
        Ray hitRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (Physics.Raycast(hitRay, out hit))
        {
            Debug.Log(hit.transform.name);

            if (hit.transform.name == "GameBoard")
            {
                //check cell closest to the hitposition
                //gameBoard.A
            }

        }

    }
}
