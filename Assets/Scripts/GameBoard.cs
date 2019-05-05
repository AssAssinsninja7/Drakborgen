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
    //Queue of Rooms

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
                    //no tile at "place"
                }
            }
        }
    }

    void CreateBoardGraph()
    {
        
    }
}
