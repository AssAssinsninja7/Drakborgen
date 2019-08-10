using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCorridorScript : MonoBehaviour
{

    public List<Vector2> Neighbors { get; set; }

    
    void Awake()
    {
        Neighbors = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
