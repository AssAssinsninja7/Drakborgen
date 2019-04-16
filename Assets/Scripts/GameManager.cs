using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //this gamemgr

    public ARControllScript arController; //So that we can check if we have viable planes

    public GameObject playerModel; //Should be two here later

    private Player player1;
    private Player player2;    

    private GameBoard gameBoard;

    private Text planeInfoTexT;

    private bool hasPlayerInformation;

    // Start is called before the first frame update
    void Awake()
    {
        //Check if instance already exists
        if (instance == null) //If nah
        {
            instance = this; //Set it to this class
        }
        else if (instance != this) //If instance already exists and it's not this:
        {
            Destroy(gameObject);
        }

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(0); //When prgram starts load scene "0" which is selectionScene 
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPlayerInformation)
        {
            InitGame();
        }   
    }

    public void SetPlayerInformation(Player p1, Player p2)
    {
        player1 = p1; //Set the player
        player2 = p2;

        player1.playerModel = playerModel; //Set the uninitialisesd model to the right player (I subset this by setting the colors below)
        player2.playerModel = playerModel;

        //player1.SetPlayerColor();
        //player2.SetPlayerColor(); 
      
        SceneManager.LoadScene(sceneBuildIndex: 1); //1 = drakborgen scene

        hasPlayerInformation = true;
    }

    void InitGame()
    {

        if (player1 != null && player2 != null) //Make sure they aint null and load next scene
        {         
            Debug.Log(SceneManager.GetActiveScene().ToString());

            arController = GetComponent<ARControllScript>();
            gameBoard = GameObject.FindGameObjectWithTag("GameBoard").GetComponent<GameBoard>();
            planeInfoTexT = GameObject.Find("planeInfoText").GetComponent<Text>();//Check to see if it finds the right text asset in the scene
            Debug.Log(planeInfoTexT.GetComponent<Text>().text);
        }
    }


    void ARREnderer()
    {
        

        //Check if a planes have been found
        Debug.Log(player1.gameObject);
        if (arController.HasPlanes()) //If there are planes init game adn update gameplay
        {
            planeInfoTexT.enabled = false;

        }
        else //Find new planes
        {
            planeInfoTexT.enabled = true;
            planeInfoTexT.text = "Searching for planes";


        }
    }
}
