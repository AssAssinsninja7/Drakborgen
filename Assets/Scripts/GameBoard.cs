using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    public GameObject[,] boardMap = new GameObject[10, 7];

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
    private Queue<Vector3> tileMapPositions = new Queue<Vector3>();
    private Vector3 tileCellScale;

    Grid grid;

    [SerializeField]
    public Tilemap tileMap;

    [SerializeField]
    private GameObject emptyTilePrefab;

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

        grid = GetComponentInChildren<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isInit == false)
        //{
        //    InitTestBoard();
        //    isInit = true;
        //}

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //maybe 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "EmptyTile")
                {
                    RevealRoom(hit);
                }
            }
        }
    }

    public void InitGameBoard(Player player1, Player player2) //take in startpos
    {
        this.player1 = player1; //this only holds a ref to the player sent in, maybe instanciate a new one and return the ref to gamemgr
        this.player2 = player2;

        InitializeRoomTiles();
        LoadRoomStack();
        ShuffleStack();

        CreateBoardGraph();
        player1.transform.position = transform.GetComponentInChildren<Tilemap>().LocalToCellInterpolated(player1.Position);
        //boardMap[(int)player1.Position.x, (int)player1.Position.y].transform.position;
        player2.transform.position = transform.GetComponentInChildren<Tilemap>().LocalToCellInterpolated(player2.Position);
        //boardMap[(int)player2.Position.x, (int)player2.Position.y].transform.position;

        //Instantiate(player1, boardMap[(int)player1.Position.x, (int)player1.Position.y].transform);
        //Instantiate(player2, boardMap[(int)player2.Position.x, (int)player2.Position.y].transform);
    }

    private void InitTestBoard() //For the testScene
    {
        //InitializeRoomTiles();
        InitBoardTiles();
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

        //AvailableRooms(result);
    }

    /// <summary>
    /// Check if the pressed area contains an available room
    /// </summary>
    /// <param name="hitPos"></param>
    public void AvailableRooms(Vector3 playerPos) //this is not going to be void later
    {
        List<Vector3> avaliableRooms = new List<Vector3>();

        for (int y = 0; y < boardMap.GetLength(1); y++) // 7 = maxlength in y-axel on board
        {
            for (int x = 0; x < boardMap.GetLength(0); x++) // 10 = maxlength in x-axel on board
            {
               
            }
        }
    }

    /// <summary>
    /// Set the boardMap tiles positions to that of the gameboards tileMap 
    /// </summary>
    void CreateBoardGraph()
    {
        /*The offset is needed because the cellToWorld gets the center pos
         of the tile in the tilemap */
    
        float offSet = grid.cellSize.x / 2;

        for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++) //yMax
        {
            for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++) //xMax
            {
                Vector3Int localPos = (new Vector3Int(x, y, 0));

                if (tileMap.HasTile(localPos))
                {
                    Vector3 tileCenterPos = tileMap.CellToWorld(localPos);
                    tileCenterPos.x += offSet;
                    tileCenterPos.y += offSet;
                    tileMapPositions.Enqueue(tileCenterPos);
                }
            }
        }
        tileCellScale = tileMap.cellBounds.size; //So that we can reduce the roomTiles size (scale it after the cell size)
       
        SetBoardTilePositions();
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
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        //2st 4e with door on e3
        roomStack.Enqueue(e134nPwD3);
        roomStack.Enqueue(e134nPwD3);
        //1st 4e with portcullis
        roomStack.Enqueue(e1234wPnD);
        //1st 4e with doors on e1, e2 & e3
        roomStack.Enqueue(e1234nPwD234);

        //4st 3e with exits on e1, e2 & e3
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        //1st 3e with door on e3
        roomStack.Enqueue(e134nPwD3);

        //4st 3e with exits on e1, e2 & e3
        //Doesn't exist yet
        //1st 3e with door on e3
        roomStack.Enqueue(e123nPwD3);

        //4st 3e exits on e1, e2 & e4
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        //1st 2e with portcullis 
        roomStack.Enqueue(e124wPnD);

        //2st 2e exits on e1 & e2 (left turn)
        roomStack.Enqueue(e12nPnD);
        roomStack.Enqueue(e12nPnD);
        //1st 2e with portcullis
        roomStack.Enqueue(e12wPnD);

        //2st 2e exits on e1 and e4 (right turn)
        roomStack.Enqueue(e14nPnD);
        roomStack.Enqueue(e14nPnD);
        //1st 2e with portcullis
        roomStack.Enqueue(e14wPnD);

        //6st 2e exits on e1 and e3 (straight forward)
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        //1st 2e with portcullis 
        roomStack.Enqueue(e13wPnD);
        //1st 2e with portcullis and door on e3
        roomStack.Enqueue(e13wPwD3);

        //2st 1e (Deadend)
        roomStack.Enqueue(e1nPnD);
        roomStack.Enqueue(e1nPnD);
        //1st 1e with portcullis
        roomStack.Enqueue(e1wPnD);

        //3st turnRooms
        roomStack.Enqueue(e1TurnRoom);
        roomStack.Enqueue(e1TurnRoom);
        roomStack.Enqueue(e1TurnRoom);

        //1st 4e Corridor
        roomStack.Enqueue(e1234Corridor);
        //1st 2e exits on e1 & e2, Corridor (left turn)
        roomStack.Enqueue(e12Corridor);
        //1st 2e exits on e1, & e4, Corridor (right turn)
        roomStack.Enqueue(e14Corridor);
        //1st 2e exits on e1 & e3, Corridor (straight forward
        roomStack.Enqueue(e13Corridor);

        //1st 3e exits on e1, e2, & e3. Corridor
        roomStack.Enqueue(e123Corridor);
        //1st 3e exits on e1, e2 & e4. Corridor
        roomStack.Enqueue(e124Corridor);
        //1st 3e exits on e1, e3, e4
        roomStack.Enqueue(e134Corridor);

        foreach (GameObject room in roomStack)
        {
            room.transform.position = new Vector3(-3500f, 0.0f, 0.0f);
        }
    }

    void InitBoardTiles()
    {
        for (int y = 0; y < boardMap.GetLength(1); y++) // 7 = maxlength in y-axel on board
        {
            for (int x = 0; x < boardMap.GetLength(0); x++) // 10 = maxlength in x-axel on board
            {
                boardMap[x, y] = Instantiate(emptyTilePrefab);
            }
        }
    }

    /// <summary>
    /// Sets the final position for all the empty rooms so that they are in 
    /// relation the the gameboards tilegrid
    /// </summary>
    void SetBoardTilePositions()
    {
        for (int y = 0; y < boardMap.GetLength(1); y++)
        {
            for (int x = 0; x < boardMap.GetLength(0); x++)
            {
                Vector3 tilePos = tileMapPositions.Dequeue();
                tilePos.y -= 0.5f; //Adjust so that the tilePos isnt going from the center of the tileMap.cell
                tilePos.z += 0.5f;
                boardMap[x, y].transform.SetPositionAndRotation(tilePos, new Quaternion(0,0,0,0)); //the quaternion should be based on the boards rotation
            }
        }
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
    public void RevealRoom(RaycastHit hit)//take in the pos maybe?
    {
        //roomStack.Dequeue(); //get the next room and place it 
        //move player to the next room 
        //return event to gameMgr

        for (int y = 0; y < boardMap.GetLength(1); y++)
        {
            for (int x = 0; x < boardMap.GetLength(0); x++)
            {
                if (boardMap[x, y].transform.position == hit.transform.position)
                {
                    Debug.Log("A tile was hit" + boardMap[x, y].transform.position);

                    if (boardMap[x, y].gameObject.tag == "EmptyTile") //if there's no tile
                    {
                        Vector3 boardTilePos = boardMap[x, y].transform.position;                     
                        Quaternion boardTileRotation = new Quaternion(90, 0, 0, 0); //dont have this hardcoded later

                        boardMap[x, y] = Instantiate(roomStack.Dequeue());
                        boardMap[x, y].transform.SetPositionAndRotation(boardTilePos, boardTileRotation);
                        boardMap[x, y].SetActive(true);                     
                    }
                }
            }
        }
    }
}
