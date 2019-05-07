using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{  
    public Tilemap boardMap;

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

    // Start is called before the first frame update
    void Start()
    {
        //Add start positions as listitems
        startPositions = new List<Vector2>();

        startPositions.Add(new Vector2(0, 0));
        startPositions.Add(new Vector2(9, 0));
        startPositions.Add(new Vector2(0, 6));
        startPositions.Add(new Vector2(9, 6));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGameBoard(Player player1, Player player2) //take in startpos
    {
        this.player1 = player1;
        this.player2 = player2;
        //init rooms and roomstack
        //set startpos
    }


    void AvailableRooms() //this is not going to be void later
    {
        List<Vector3> avaliableRooms = new List<Vector3>();

        for (int i = boardMap.cellBounds.yMin; i < boardMap.cellBounds.yMax; i++)
        {
            for (int j = boardMap.cellBounds.xMin; j < boardMap.cellBounds.xMax; j++)
            {
                Vector3Int roomPos = (new Vector3Int(j, i, (int)boardMap.transform.position.y));
                Vector3 togoRoom = boardMap.CellToWorld(roomPos);

                if (boardMap.HasTile(roomPos))
                {
                    //tile at "place"
                   
                }
                else
                {
                  
                }
            }
        }
    }

    void CreateBoardGraph()
    {
        
    }
}
