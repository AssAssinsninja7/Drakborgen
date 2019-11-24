using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{
    public GameObject emptyTile;

    public List<GameObject> allRooms; //All of the rooms (54st) exept Obstucted rooms

    public Queue<GameObject> roomStack; //this will be the shuffled allRooms list

    private GameObject[,] boardTiles; //The collection of hitboxes that can overlays the board "The actual Grid"

    private Vector3 boardCollider;

    //public GameObject vikingAvatar;

    //public GameObject monkAvatar;

    // Start is called before the first frame update
    void Start()
    {
        boardTiles = new GameObject[10, 7];
        roomStack = new Queue<GameObject>();

        boardCollider = GetComponent<Renderer>().bounds.size;

        InitializeBoardTilesObjects();
        CreateBoardGrid();
        ShuffleStack();

        TestPlayerPlacement();
        //place player avatar on startpos -> send in startpos from gameMRg
    }

    // Update is called once per frame
    void Update()
    {
        //Check if a touch or a click has occured, if so check what it it
        //

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //maybe 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "EmptyTile") //if its empty and next to player
                {

                    CheckPlacement(hit); //take in which player turn it is
                    Debug.Log(hit.transform.position + " a tile was hit");
                }
                else if (hit.collider.tag == "StartPos")
                {

                }
                //Add the other roomtypes here later as well and have them just call a variant of checkplacement
                //that just checks that the  hit is next to the player pos and then move rather than placing a room.
            }
        }
    }



    /// <summary>
    /// Shuffle the rooms so that the order of them is unknown
    /// and adds them to the roomstack
    /// </summary>
    void ShuffleStack()
    {
        allRooms.Shuffle<GameObject>();
        for (int i = 0; i < allRooms.Count; i++)
        {
            roomStack.Enqueue(allRooms[i]);
        }
    }

    /// <summary>
    /// Initialize the boardTiles that will act as hitboxes
    /// </summary>
    void InitializeBoardTilesObjects()
    {
        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                boardTiles[x, y] = Instantiate(emptyTile); //init the the boardTiles
                boardTiles[x, y].SetActive(true);
            }
        }
    }

    /// <summary>
    /// First it checks if the tile is at a startpos and if it is it changes its tag to StartTile
    /// then it finds the position of the left top corner of the game board,
    /// then it sets the dept of the placement for the tile, 
    /// then it sets the boardTileWidth and boardTileHeight sizes of a single tile on the board,
    /// then it sets the current emptytile's position to that of the left corner position 
    /// + the boardTiles width and height times the iterator + the offset. With that the empty
    /// tiles always has their position after the boards
    /// </summary>
    public void CreateBoardGrid()
    {
        float boardTileHeight;
        float boardTileWidth;
        float depth;

        float emptyTileHightOffset = emptyTile.GetComponent<Renderer>().bounds.size.y / 2; //offsetHeight = emptytile prefabs height
        float emptyTileWithOffset = 0.5f; //emptyTile.GetComponent<Renderer>().bounds.size.x / 2; //offsetWidth = emptytile prefabs width 

        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                if (x == 0 && y == 0 || x == 9 && y == 0 || x == 0 && y == 6 || x == 9 && y == 6)
                {
                    boardTiles[x, y].tag = "StartTile";
                }
                if (x == 4 && y == 3 || x == 5 && y == 3)
                {
                    boardTiles[x, y].tag = "TreasureTile";
                }

                Vector3 boardLeftCornerPos = new Vector3(
                    gameObject.GetComponent<Renderer>().bounds.min.x,
                    gameObject.GetComponent<Renderer>().bounds.max.y,
                    gameObject.GetComponent<Renderer>().bounds.max.z);

                depth = boardLeftCornerPos.y + (emptyTileHightOffset); //place the Emptytile ontop of the board and not inside

                boardTileWidth = boardCollider.x / boardTiles.GetLength(0); //width of a single tile on the board
                boardTileHeight = boardCollider.z / boardTiles.GetLength(1); //lenght of a single tile on the board 

                boardTiles[x, y].transform.position =
                    new Vector3(boardLeftCornerPos.x + (boardTileWidth * x + emptyTileWithOffset),
                    depth, boardLeftCornerPos.z - (boardTileHeight * y + emptyTileWithOffset));

                boardTiles[x, y].transform.parent = gameObject.transform; //Testing to see if the entire transform is needed to place the players correctly fixed floating tiles ++ but players still not
                boardTiles[x, y].transform.rotation = gameObject.transform.rotation; //set the tile rotation to that of the board (it matters when placed in with arCore)
            }
        }
        SetBoardTileNeighbors();
    }

    //Should work right
    private void SetBoardTileNeighbors()
    {
        int xMax = boardTiles.GetLength(0) - 1;
        int yMax = boardTiles.GetLength(1) - 1;

        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                if (x == 0 && y == 0) //upper left corner
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                }
                else if (x == 0 && y == yMax) //bottom left corner
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                }
                else if (x == xMax && y == yMax) //upper right corner
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                }
                else if (x == xMax && y == yMax) //bottom right corner
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                }

                if (y == 0 && x > 0 && x < xMax) //Top row (not corner tiles)
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                }
                else if (y == yMax && x > 0 && x < xMax) //Bottom row (not corner tiles)
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                }
                else if (x == 0 && y > 0 && y < yMax) //Left row (not corner tiles)
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                }
                else if (x == xMax && y > 0 && y < yMax)//Right row (not corner tiles)
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                }
                else if (x > 0 && x < xMax && y > 0 && y < yMax)//if not outerrim tiles then they have neighbors on all sides 
                {
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y - 1)); //upper neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x, y + 1)); //bottom neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x - 1, y)); //left neighbor
                    boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Add(new Vector2(x + 1, y)); //right neighbor
                }
            }
        }
    }

    /// <summary>
    /// Takes in a copy of the playerprefabs and initializes the avatars to that
    /// of the chosen character, it then sets the models positions based on the 
    /// players chosen startpos. Lastly it offsets their placement based on the
    /// character models height. 
    /// </summary>
    /// <param name="player1"></param>
    /// <param name="player2"></param>
    /// <param name="p1hasViking"></param>
    public void PlacePlayerAvatarOnStart(Vector2 player1StartPos, Vector2 player2StartPos, bool p1hasViking) //maybe add the rings as a UI feature and set them here?
    {     
        //if (p1hasViking) //check which gameObj avatar to save the player profile in (Instansiate in)
        //{
        //    //GameManager.instance.player1.GetComponent<Player>().playerModel = Instantiate(vikingAvatar); //set player1 to have the viking avatar
        //    //GameManager.instance.player2.GetComponent<Player>().playerModel = Instantiate(monkAvatar);

        //    GameObject.Find("debugCanvas").GetComponentInChildren<Text>().text = GameManager.instance.player1.transform.ToString();

        //    float heightOffset = vikingAvatar.GetComponent<Renderer>().bounds.size.y / 2;

        //    vikingAvatar.transform.parent = gameObject.transform; //testing to see if the setting the board as the parent might help with their placement
        //    monkAvatar.transform.parent = gameObject.transform;
        //    //the players might not be placed after the boards ancor.transform so this might be the problem for the players in the build
        //    //rewrite so it is instantiated once it finds a plane so that we can debug through the instant preview.

        //    vikingAvatar.transform.position = new Vector3(boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.x,
        //        boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.y + heightOffset,
        //        boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.z);
        //    //vikingAvatar.transform.position = boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position;

        //    boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].GetComponent<emptyTileScript>().hasPlayer = true; //set that this tile now has a player

        //    monkAvatar.transform.position = new Vector3(boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.x,
        //        heightOffset + boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.y,
        //        boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.z); //the heightpostion is based on the emptytile which makes it hover a littles

        //    boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].GetComponent<emptyTileScript>().hasPlayer = true;
       
        //}
        //else
        //{
        //    //GameManager.instance.player1.GetComponent<Player>().playerModel = Instantiate(monkAvatar); //set player1 to have the monk avatar
        //    //GameManager.instance.player2.GetComponent<Player>().playerModel = Instantiate(vikingAvatar);

        //    GameObject.Find("debugCanvas").GetComponentInChildren<Text>().text = GameManager.instance.player1.transform.ToString();

        //    float heightOffset = vikingAvatar.GetComponent<Renderer>().bounds.size.y / 2; 

        //    monkAvatar.transform.position = new Vector3(boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.x,
        //        boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.y + heightOffset,
        //        boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position.z);

        //    boardTiles[(int)player1StartPos.x, (int)player1StartPos.y].GetComponent<emptyTileScript>().hasPlayer = true; //set that this tile now has a player

        //    vikingAvatar.transform.position = new Vector3(boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.x,
        //        heightOffset + boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.y,
        //        boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position.z); //the heightpostion is based on the emptytile which makes it hover a little

        //    boardTiles[(int)player2StartPos.x, (int)player2StartPos.y].GetComponent<emptyTileScript>().hasPlayer = true;
        //}

    }

    public Vector3 PlaceAvatarOnBoard(Vector2 playerStartPos, float heightOffset)
    {
        Vector3 playerWorldPos;

        int xIndex = (int)playerStartPos.x;
        int yIndex = (int)playerStartPos.y;

        playerWorldPos = new Vector3(boardTiles[xIndex, yIndex].transform.position.x,
                                    boardTiles[xIndex, yIndex].transform.position.y + heightOffset,
                                    boardTiles[xIndex,yIndex].transform.position.z);

        return playerWorldPos;
    }

    private void TestPlayerPlacement()
    {
        GameManager.instance.InitGame();
    }

    void CheckPlacement(RaycastHit hit)
    {
        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                if (hit.transform.position == boardTiles[x, y].transform.position) //check if it hit a tile
                {
                    for (int i = 0; i < boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Count; i++)
                    {
                        int nX = (int)boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors[i].x; //neighbor X
                        int nY = (int)boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors[i].y; //neighbor Y

                        if (boardTiles[nX, nY].GetComponent<emptyTileScript>().hasPlayer) //maybe set a check for the right player
                        {
                            PlaceRoom(hit.transform.position, boardTiles[nX, nY].transform.position, SetEntranceAngle(nX, x, nY, y));
                            //maybe have a bool telling that he placement was succesful and have the gameMgr get that and then call 
                            //moveplayer which moves the player and activates the event
                        }
                        else
                        {
                            Debug.Log(x + " " + y + " " + "Tile wasnt next to player tile");
                        }
                    }
                }
            }
        }
    }

    void PlaceRoom(Vector3 hitPos, Vector3 currentPlayerPos, float rotationAngle)
    {
        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                if (boardTiles[x, y].transform.position == hitPos) //if we hit a tile
                {
                    #region debug for neighbors
                    //for (int i = 0; i < boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors.Count; i++)
                    //{
                    //    Debug.Log(boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors[i].ToString());

                    //}
                    //Debug.Log(x.ToString() + " :  " + y.ToString() + " originTile");
                    #endregion

                    List<Vector2> tempNeighbors = boardTiles[x, y].GetComponent<emptyTileScript>().Neighbors; //save neigbors so we can add them to the newly instanciated emptyTile[x,y]

                    Destroy(boardTiles[x, y]);
                    boardTiles[x, y] = Instantiate(roomStack.Dequeue());

                    if (boardTiles[x, y].tag == "Room") //"Event"
                    {
                        boardTiles[x, y].GetComponent<RoomScript>().Neighbors = tempNeighbors; //add the old boardTiles neighbors          
                    }
                    else if (boardTiles[x, y].tag == "CorridorRoom")
                    {
                        boardTiles[x, y].GetComponent<RoomCorridorScript>().Neighbors = tempNeighbors; //add the old boardTiles neighbors          
                    }
                    else if (boardTiles[x, y].tag == "TurnRoom")
                    {
                        boardTiles[x, y].GetComponent<RoomRotationRoomScript>().Neighbors = tempNeighbors; //add the old boardTiles neighbors          
                    }

                    boardTiles[x, y].transform.position = hitPos; //set pos to that of pressed tile
                    var rotationVector = boardTiles[x, y].transform.eulerAngles;
                    rotationVector.y = rotationAngle; //set rotation to that of the way the player is moving
                    rotationVector.x = 180; //face up

                    boardTiles[x, y].transform.eulerAngles = rotationVector;
                    boardTiles[x, y].SetActive(true);

                    //Next move player to the new room and activate rooom


                }
            }
        }
    }

    /// <summary>
    /// Takes the current clicked tiles index values and compares them to the
    /// neighbor with the player on, it then determines which way to flip it 
    /// based on how the orgin tiles index values compares to the neighbors 
    /// index values. Lastly it returns the angle which the placed tile
    /// should flip
    /// </summary>
    /// <param name="nX">neigbors x value</param>
    /// <param name="x">clicked tile x value</param>
    /// <param name="nY">neigbors y value</param>
    /// <param name="y">clicked tile y value</param>
    /// <returns></returns>
    private float SetEntranceAngle(int nX, int x, int nY, int y)
    {
        float finalAngle;

        if (x < nX && y == nY)
        {
            finalAngle = 90f; //Move left
        }
        else if (x > nX && y == nY)
        {
            finalAngle = 270f; //Move right
        }
        else if (y < nY && x == nX)
        {
            finalAngle = 180f; //Move up
        }
        else
        {
            finalAngle = 0f; //Move down
        }
        return finalAngle;
    }

    void MovePlayerAvatar(Vector3 nextPosition, Vector3 oldPosition, bool isPlayer1)
    {
        //move player
        //loop through tils check which tile has same pos, check tag and activate that tile event

        for (int y = 0; y < boardTiles.GetLength(1); y++)
        {
            for (int x = 0; x < boardTiles.GetLength(0); x++)
            {
                if (boardTiles[x, y].transform.position == oldPosition) //if the tile is equal to the new player pos, check the tag and activate the tile, yeah and move the player
                {
                    if (true)
                    {

                    }
                }
            }
        }
    }
}

/// <summary>
/// Extending the generic collection list so that we can 
/// swap the order of the gameobjects without having destroy
/// them or having another list.
/// </summary>
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rnd = new Random();
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, Random.Range(0, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}