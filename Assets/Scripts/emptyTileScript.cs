using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emptyTileScript : MonoBehaviour
{
    public bool hasTile;
    public bool hasPlayer; 

    public List<Vector2> Neighbors { get; set; }


    // Start is called before the first frame update
    void Awake()
    {
        Neighbors = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
