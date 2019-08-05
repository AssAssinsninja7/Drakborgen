using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject emptyTile;

    public List<GameObject> allRooms; //All of the rooms (54st) exept Obstucted rooms

    public Queue<GameObject> roomStack; //this will be the shuffled allRooms list

    private GameObject[,] emptyTiles; //The collection of hitboxes that can overlays the board "The actual Grid"

    private Vector3 boardCollider;

    public GameObject vikingAvatar;

    public GameObject monkAvatar;

    // Start is called before the first frame update
    void Start()
    {
        emptyTiles = new GameObject[10, 7];
        roomStack = new Queue<GameObject>();

        boardCollider = GetComponent<Renderer>().bounds.size;

        InitializeEmptyTilesObjects();
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
                    PlaceRoom(hit);
                    Debug.Log(hit.transform.position + " a tile was hit");
                }
                else if (hit.collider.tag == "StartPos")
                {
                    
                }
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
    /// Initialize the emptyTiles that will act as hitboxes
    /// </summary>
    void InitializeEmptyTilesObjects()
    {
        for (int y = 0; y < emptyTiles.GetLength(1); y++)
        {
            for (int x = 0; x < emptyTiles.GetLength(0); x++)
            {
                emptyTiles[x, y] = Instantiate(emptyTile); //emptyTile; //init the grid with the emptyTiles
                emptyTiles[x, y].SetActive(true);
            }
        }
    } 

    //public int GetEmptyTilesSize()
    //{
    //    return emptyTiles.Length;
    //}

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

        for (int y = 0; y < emptyTiles.GetLength(1); y++)
        {
            for (int x = 0; x < emptyTiles.GetLength(0); x++)
            {
                if (x == 0 && y == 0 || x == 9 && y == 0 || x == 0 && y == 6 || x == 9 && y == 6)
                {
                    emptyTiles[x, y].tag = "StartTile";
                }
                if (x == 4 && y == 3 || x == 5 && y == 3)
                {
                    emptyTiles[x, y].tag = "TreasureTile";
                }

                Vector3 boardLeftCornerPos = new Vector3(
                    gameObject.GetComponent<Renderer>().bounds.min.x, 
                    gameObject.GetComponent<Renderer>().bounds.max.y, 
                    gameObject.GetComponent<Renderer>().bounds.max.z);

                depth = boardLeftCornerPos.y + (emptyTileHightOffset); //place the Emptytile ontop of the board and not inside

                boardTileWidth = boardCollider.x / emptyTiles.GetLength(0); //width of a single tile on the board
                boardTileHeight = boardCollider.z / emptyTiles.GetLength(1); //lenght of a single tile on the board 

                emptyTiles[x, y].transform.position = 
                    new Vector3(boardLeftCornerPos.x + (boardTileWidth * x + emptyTileWithOffset), 
                    depth, boardLeftCornerPos.z - (boardTileHeight * y + emptyTileWithOffset));
            }
        }
    }

    /// <summary>
    /// Takes in a copy of the playerprefabs and initializes the avatars based on their chosen 
    /// Startpos and then sets the color of the avatar to that of the chosen character type
    /// </summary>
    /// <param name="player1"></param>
    /// <param name="player2"></param>
    /// <param name="p1hasViking"></param>
    public void PlacePlayerAvatarOnStart(GameObject player1, GameObject player2, bool p1hasViking)
    {              
        Vector2 player1StartPos = player1.GetComponent<Player>().Position;
        Vector2 player2StartPos = player2.GetComponent<Player>().Position;

        if (player1.GetComponent<Player>().HasViking) //check which gameObj avatar to save the player profile in (Instansiate in)
        {
            vikingAvatar = Instantiate(player1);
            monkAvatar = Instantiate(player2);

            vikingAvatar.transform.position = emptyTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position;
            monkAvatar.transform.position = emptyTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position;

            vikingAvatar.GetComponent<Player>().SetPlayerColor(p1hasViking);
            monkAvatar.GetComponent<Player>().SetPlayerColor(!p1hasViking);

            //need to offset with the height of the model
        }
        else
        {
            vikingAvatar = Instantiate(player2);
            monkAvatar = Instantiate(player1);

            vikingAvatar.transform.position = emptyTiles[(int)player2StartPos.x, (int)player2StartPos.y].transform.position;
            monkAvatar.transform.position = emptyTiles[(int)player1StartPos.x, (int)player1StartPos.y].transform.position;

            vikingAvatar.GetComponent<Player>().SetPlayerColor(!p1hasViking);
            monkAvatar.GetComponent<Player>().SetPlayerColor(p1hasViking);
        }

        
    }

    private void TestPlayerPlacement()
    {
        GameManager.instance.InitGame();
    }

    void PlaceRoom(RaycastHit hit)
    {
        for (int y = 0; y < emptyTiles.GetLength(1); y++)
        {
            for (int x = 0; x < emptyTiles.GetLength(0); x++)
            {
                if (emptyTiles[x, y].transform.position == hit.transform.position)
                {
                    //Note to self, corridor tilesen behövs skalas om då de är mer än dubbla höjden utav eventroom D:<
                    emptyTiles[x, y] = Instantiate(roomStack.Dequeue());
                    emptyTiles[x, y].transform.SetPositionAndRotation(hit.transform.position, new Quaternion(90, 0, 0, 0)); //should activate the rotation so that it is rotates up
                    emptyTiles[x, y].SetActive(true);
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