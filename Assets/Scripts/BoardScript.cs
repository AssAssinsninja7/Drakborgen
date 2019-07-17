using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject emptyTile;

    public List<GameObject> rooms; //All of the rooms (54st) exept Obstucted rooms

    private GameObject[,] emptyTiles; //The collection of hitboxes that can overlays the board "The actual Grid"

    private Vector3 boardCollider;

    // Start is called before the first frame update
    void Start()
    {
        emptyTiles = new GameObject[10, 7];

        boardCollider = GetComponent<Renderer>().bounds.size;

        InitializeEmptyTilesObjects();
        CreateBoardGrid();
        ShuffleStack();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0) //maybe 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "EmptyTile")
                {
                    //PlaceRoom(hit);
                    Debug.Log(hit.transform.position + " a tile was hit");
                }
            }
        }
    }

  

    /// <summary>
    /// Shuffle the rooms so that the order of them is unknown
    /// </summary>
    void ShuffleStack()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject temp = rooms.ToArray()[i];
            int rnd = Random.Range(0, rooms.Count);
            rooms.ToArray()[i] = rooms.ToArray()[rnd];
            rooms.ToArray()[rnd] = temp;
        }
    }

    void InitializeEmptyTilesObjects()
    {
        for (int y = 0; y < emptyTiles.GetLength(1); y++)
        {
            for (int x = 0; x < emptyTiles.GetLength(0); x++)
            {
                emptyTiles[x,y] = emptyTiles[x, y] = Instantiate(emptyTile); //init the grid with the emptyTiles
                emptyTiles[x, y].SetActive(true);
            }
        }
    } 

    void CreateBoardGrid()
    {
        float vertical;
        float horizontal;
        float depth;

        for (int y = 0; y < emptyTiles.GetLength(1); y++)
        {
            for (int x = 0; x < emptyTiles.GetLength(0); x++)
            {
                
              
                depth = (boardCollider.y / 2) + (emptyTiles[x, y].transform.GetComponent<Renderer>().bounds.size.y * 2); //place the Emptytile ontop of the board and not inside

                horizontal = boardCollider.x / emptyTiles.GetLength(0); //need to be fixed
                vertical = boardCollider.z / emptyTiles.GetLength(1); 
               

                emptyTiles[x, y].transform.position = new Vector3(horizontal * x, depth, vertical * y);
            }
        }
    }

    void PlaceRoom()
    {

    }
}
