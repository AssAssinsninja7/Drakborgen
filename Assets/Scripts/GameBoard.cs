﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{  
    public GameObject[,] boardMap = new GameObject[10,7];

    private int tileSize = 256;

    private List<Vector2> startPositions;

    private Player player1, player2;

    //RoomTiles
    private Queue<GameObject> roomStack; //roomStack is the container from which the gameboard gets the next placeable tile

    #region RoomObjects
    //e213wP2nD: "e = entrance" & "the number right after indicates the main entrance (the way you enter)
    //whilst the others represent if there are more exits (if there are any)", 
    //"w = with" & "P = portcullis" "2 = on the second entrance",
    //"n = no" & "D = door"

    //EventRooms
    [SerializeField]
    private GameObject e1nPnD; //deadend
    [SerializeField]
    private GameObject e1wPnD; //deadend with protcullis
    [SerializeField]
    private GameObject e12nPnD; //Left turn no portcullis
    [SerializeField]
    private GameObject e12wPnD; //Left turn with portcullis
    [SerializeField]
    private GameObject e13nPnD; //Straight forward
    [SerializeField]
    private GameObject e13wPwD3; //Door on the third entrance
    [SerializeField]
    private GameObject e13wPnD; //Straight walk with portcullis
    [SerializeField]
    private GameObject e14nPnD; //Right turn
    [SerializeField]
    private GameObject e14wPnD; //Right turn with portcullis
    [SerializeField]
    private GameObject e124nPnD; //T-section left and right
    [SerializeField]
    private GameObject e124wPnD; //T-section left and right (with portcullis)
    [SerializeField]
    private GameObject e134nPnD; //T-section right and forward
    [SerializeField]
    private GameObject e123nPwD3; //T-section left and forward, door on e3
    [SerializeField]
    private GameObject e134nPwD3;//T-section right and forward with door on e3
    [SerializeField]
    private GameObject e1234nPwD3; //Crossroad with door on e3
    [SerializeField]
    private GameObject e1234nPwD234; //Crossroad with doors on e2, e3 & e4
    [SerializeField]
    private GameObject e1234Blocked; //Crossroad with block
    [SerializeField]
    private GameObject e1234wPnD; //Crossroad with portcullis
    [SerializeField]
    private GameObject e1234nPnD; //Crossroad




    //CorridorRoom
    [SerializeField]
    private GameObject e12Corridor; //Left turn
    [SerializeField]
    private GameObject e13Corridor; //Forward
    [SerializeField]
    private GameObject e14Corridor; //Right
    [SerializeField]
    private GameObject e123Corridor; //T-section to the left or forward
    [SerializeField]
    private GameObject e124Corridor; // T-section to the left and right
    [SerializeField]
    private GameObject e134Corridor; //T-section to the right or forward (from entrance)
    [SerializeField]
    private GameObject e1234Corridor; //Crossroad


    //TurnRoom
    [SerializeField]
    private GameObject e1TurnRoom;

    #endregion

    //TestVariables
    private bool isInit;

    [SerializeField]
    public Tilemap tileMap;

    // Start is called before the first frame update
    void Start()
    {
        //Add start positions as listitems
        startPositions = new List<Vector2>();

        startPositions.Add(new Vector2(0, 0));
        startPositions.Add(new Vector2(9, 0));
        startPositions.Add(new Vector2(0, 6));
        startPositions.Add(new Vector2(9, 6));

        roomStack = new Queue<GameObject>();

        //TestVariable set
        isInit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit == false)
        {
            InitTestBoard();
            isInit = true;
        }


    }

    public void InitGameBoard(Player player1, Player player2) //take in startpos
    {
        this.player1 = player1;
        this.player2 = player2;

        InitializeRoomTiles();
        LoadRoomStack();
        ShuffleStack();

        //CreateBoardGraph();
        player1.transform.position = transform.GetComponentInChildren<Tilemap>().LocalToCellInterpolated(player1.Position);
        //boardMap[(int)player1.Position.x, (int)player1.Position.y].transform.position;
        player2.transform.position = transform.GetComponentInChildren<Tilemap>().LocalToCellInterpolated(player2.Position);
        //boardMap[(int)player2.Position.x, (int)player2.Position.y].transform.position;

        //Instantiate(player1, boardMap[(int)player1.Position.x, (int)player1.Position.y].transform);
        //Instantiate(player2, boardMap[(int)player2.Position.x, (int)player2.Position.y].transform);

        //Debug.Log(player1.gameObject.transform.ToString());
        //Debug.Log(player2.gameObject.transform.ToString());
        //init rooms and roomstack
        //set startpos
    }

    private void InitTestBoard()
    {
        //InitializeRoomTiles();
        CreateBoardGraph();
        LoadRoomStack();
        ShuffleStack();
    }


    public void GetNearestPointOnBoard(Vector3 hitPosition) //Select a tile
    {
        hitPosition -= transform.position;

        int xPoint = Mathf.RoundToInt(hitPosition.x / tileSize);
        int yPoint = Mathf.RoundToInt(hitPosition.y / tileSize);
        int zPoint = Mathf.RoundToInt(hitPosition.z / tileSize);

        Vector3 result = new Vector3((float)xPoint * tileSize, (float)yPoint * tileSize, (float)zPoint * tileSize);

        result += transform.position;

        AvailableRooms(result);
    }


    /// <summary>
    /// Check if the pressed area contains an available room
    /// </summary>
    /// <param name="hitPos"></param>
    public void AvailableRooms(Vector3 hitPos) //this is not going to be void later
    {
        List<Vector3> avaliableRooms = new List<Vector3>();

        for (int i = 0; i < 7; i++) // 7 = maxlength in y-axel on board
        {
            for (int j = 0; j < 10; j++) // 10 = maxlength in x-axel on board
            {
        

                if (boardMap[j,i].gameObject == null) //if there's no tile
                {
                    //and the tile is next to the player
                    if (player1.Position.x == j && player1.Position.y == i)
                    {
                        //highlight the tile
                    }
                   
                }
                else
                {
                  
                }
            }
        }
    }

    /// <summary>
    /// Set the boardMap tiles positions to that of the gameboard
    /// </summary>
    void CreateBoardGraph()
    {
        for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
        {
            for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
            {
                //boardMap[x, y].transform.position = new Vector3(transform.GetComponentInChildren<Tilemap>().transform.position.x * tileSize, 0, 
                //    transform.GetComponentInChildren<Tilemap>().transform.position.z * tileSize); //Maybe switch this to the y-pos
                //Vector3 localPos =
                //boardMap[x, y].transform.position = tileMap.Cel
                Debug.Log(boardMap[x,y].transform.position.ToString());
            }
        }
    }

    /// <summary>
    /// Initialise the roomobjects and sets
    /// them to innactive. (they will be activated
    /// when they are revealed)
    /// </summary>
    void InitializeRoomTiles()
    {
        //EventRoom
        roomStack.Enqueue(Instantiate(e1nPnD)); //deadend
        //e1nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e1wPnD)); //deadend with protcullis
        //e1wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e12nPnD)); //Left turn no portcullis
        //e12nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e12wPnD)); //Left turn with portcullis
        //e12wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e13nPnD)); //Straight forward
        //e13nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e13wPwD3)); //Door on the third entrance
        //e13wPwD3.SetActive(false);
        roomStack.Enqueue(Instantiate(e13wPnD)); //Straight walk with portcullis
        //e13wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e14nPnD)); //Right turn
        //e14nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e14wPnD)); //Right turn with portcullis
        //e14wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e124nPnD)); //T-section left and right
        //e124nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e124wPnD)); //T-section left and right (with portcullis)
        //e124wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e134nPnD)); //T-section right and forward
        //e134nPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e123nPwD3)); //T-section left and forward, door on e3
        //e123nPwD3.SetActive(false);
        roomStack.Enqueue(Instantiate(e134nPwD3)); //T-section right and forward with door on e3
        //e134nPwD3.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234nPwD3)); //Crossroad with door on e3
        //e1234nPwD3.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234nPwD234)); //Crossroad with doors on e2, e3 & e4
        //e1234nPwD234.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234Blocked)); //Crossroad with block
        //e1234Blocked.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234wPnD)); //Crossroad with portcullis
        //e1234wPnD.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234nPnD)); //Crossroad
        //e1234nPnD.SetActive(false);





        //CorridorRoom
        roomStack.Enqueue(Instantiate(e12Corridor)); //Left turn
        //e12Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e13Corridor)); //Forward
        //e13Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e14Corridor)); //Right
        //e14Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e123Corridor)); //T-section to the left or forward
        //e123Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e124Corridor)); //T-section to the left and right
        //e124Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e134Corridor)); //T-section to the right or forward (from entrance)
        //e134Corridor.SetActive(false);
        roomStack.Enqueue(Instantiate(e1234Corridor));
        //e1234Corridor.SetActive(false);

        //TurnRoom
        roomStack.Enqueue(Instantiate(e1TurnRoom));
        //e1TurnRoom.SetActive(false);
}

    /// <summary>
    /// Add in every roomType to the roomStack  
    /// </summary>
    void LoadRoomStack()
    {
        //4st 4e no restriciton
        roomStack.Enqueue(Instantiate(e1234nPnD));
        roomStack.Enqueue(Instantiate(e1234nPnD));
        roomStack.Enqueue(Instantiate(e1234nPnD));
        roomStack.Enqueue(Instantiate(e1234nPnD));
        //2st 4e with door on e3
        roomStack.Enqueue(Instantiate(e134nPwD3));
        roomStack.Enqueue(Instantiate(e134nPwD3));
        //1st 4e with portcullis
        roomStack.Enqueue(Instantiate(e1234wPnD));
        //1st 4e with doors on e1, e2 & e3
        roomStack.Enqueue(Instantiate(e1234nPwD234));

        //4st 3e with exits on e1, e2 & e3
        roomStack.Enqueue(Instantiate(e134nPnD));
        roomStack.Enqueue(Instantiate(e134nPnD));
        roomStack.Enqueue(Instantiate(e134nPnD));
        roomStack.Enqueue(Instantiate(e134nPnD));
        //1st 3e with door on e3
        roomStack.Enqueue(Instantiate(e134nPwD3));

        //4st 3e with exits on e1, e2 & e3
        //Doesn't exist yet
        //1st 3e with door on e3
        roomStack.Enqueue(Instantiate(e123nPwD3));

        //4st 3e exits on e1, e2 & e4
        roomStack.Enqueue(Instantiate(e124nPnD));
        roomStack.Enqueue(Instantiate(e124nPnD));
        roomStack.Enqueue(Instantiate(e124nPnD));
        roomStack.Enqueue(Instantiate(e124nPnD));
        //1st 2e with portcullis 
        roomStack.Enqueue(Instantiate(e124wPnD));

        //2st 2e exits on e1 & e2 (left turn)
        roomStack.Enqueue(Instantiate(e12nPnD));
        roomStack.Enqueue(Instantiate(e12nPnD));
        //1st 2e with portcullis
        roomStack.Enqueue(Instantiate(e12wPnD));

        //2st 2e exits on e1 and e4 (right turn)
        roomStack.Enqueue(Instantiate(e14nPnD));
        roomStack.Enqueue(Instantiate(e14nPnD));
        //1st 2e with portcullis
        roomStack.Enqueue(Instantiate(e14wPnD));

        //6st 2e exits on e1 and e3 (straight forward)
        roomStack.Enqueue(Instantiate(e13nPnD));
        roomStack.Enqueue(Instantiate(e13nPnD));
        roomStack.Enqueue(Instantiate(e13nPnD));
        roomStack.Enqueue(Instantiate(e13nPnD));
        roomStack.Enqueue(Instantiate(e13nPnD));
        roomStack.Enqueue(Instantiate(e13nPnD));
        //1st 2e with portcullis 
        roomStack.Enqueue(Instantiate(e13wPnD));
        //1st 2e with portcullis and door on e3
        roomStack.Enqueue(Instantiate(e13wPwD3));

        //2st 1e (Deadend)
        roomStack.Enqueue(Instantiate(e1nPnD));
        roomStack.Enqueue(Instantiate(e1nPnD));
        //1st 1e with portcullis
        roomStack.Enqueue(Instantiate(e1wPnD));

        //3st turnRooms
        roomStack.Enqueue(Instantiate(e1TurnRoom));
        roomStack.Enqueue(Instantiate(e1TurnRoom));
        roomStack.Enqueue(Instantiate(e1TurnRoom));

        //1st 4e Corridor
        roomStack.Enqueue(Instantiate(e1234Corridor));
        //1st 2e exits on e1 & e2, Corridor (left turn)
        roomStack.Enqueue(Instantiate(e12Corridor));
        //1st 2e exits on e1, & e4, Corridor (right turn)
        roomStack.Enqueue(Instantiate(e14Corridor));
        //1st 2e exits on e1 & e3, Corridor (straight forward
        roomStack.Enqueue(Instantiate(e13Corridor));

        //1st 3e exits on e1, e2, & e3. Corridor
        roomStack.Enqueue(Instantiate(e123Corridor));
        //1st 3e exits on e1, e2 & e4. Corridor
        roomStack.Enqueue(Instantiate(e124Corridor));
        //1st 3e exits on e1, e3, e4
        roomStack.Enqueue(Instantiate(e134Corridor));
    }

    /// <summary>
    /// Shuffle the rooms so that the order of them is unknown
    /// </summary>
    void ShuffleStack()
    {
        for (int i = 0; i < roomStack.Count; i++)
        {
            GameObject temp = roomStack.ToArray()[i];
            int rnd = Random.Range(0, roomStack.Count);
            roomStack.ToArray()[i] = roomStack.ToArray()[rnd];
            roomStack.ToArray()[rnd] = temp;
        }
    }

    //Have a method return the roomType maybe tag or something to the gameboard
    public void RevealRoom()//take in the pos maybe?
    {
        //roomStack.Dequeue(); //get the next room and place it 
        //move player to the next room 
        //return event to gameMgr
    }
}
