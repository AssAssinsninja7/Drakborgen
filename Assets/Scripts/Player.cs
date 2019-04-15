using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerID { get; set; }
    public bool HasViking { get; set; }
    public bool HasRotationRing { get; set; }

    public Vector2 Position { get; set; }

    public GameObject playerModel;


    // Start is called before the first frame update
    void Start()
    {
        //playerModel.GetComponent<GameObject>(); This one is stull null while the player is initialised
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializePlayer()
    {
        
    }

    /// <summary>
    /// Temp set capsule color to red if its the viking and to yellow if it's not
    /// </summary>
    public void SetPlayerColor()
    {      
        if (HasViking)
        {
            playerModel.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            playerModel.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
