using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //this gamemgr

    private Player player1;
    private Player player2;

    private GameBoard gameBoard; 

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
        
    }

    public void InitGame(Player p1, Player p2)
    {
        player1 = p1;
        player2 = p2;

        if (player1 != null && player2 != null)
        {
            SceneManager.LoadScene(1); //1= drakborgen scene
            //gameBoard.InitGameBoard(); Send in player startpos
        }
    }
}
