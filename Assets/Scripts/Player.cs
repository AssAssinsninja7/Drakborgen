using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerID { get; set; }
    public bool HasViking { get; set; }
    public bool HasRotationRing { get; set; }

    public Vector2 Position { get; set; }

    public GameObject vikingModel;
    public GameObject monkModel;

    public GameObject PlayerModel { get; private set; }


    // Start is called before the first frame update
    void Awake()
    {
        //playerModel.GetComponent<GameObject>(); This one is stull null while the player is initialised
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializePlayerModel(Vector3 hitPos)
    {
        if (HasViking)
        {
           PlayerModel = Instantiate(vikingModel, hitPos, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
        else
        {
           PlayerModel = Instantiate(monkModel, hitPos, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        }
    }

    public void SetBoardPosition(Vector3 boardPos)
    {
        PlayerModel.transform.position = boardPos;
    }

    /// <summary>
    /// Temp set capsule color to red if its the viking and to yellow if it's not
    /// </summary>
    public void SetPlayerModel()
    {
        if (HasViking)
        {
            vikingModel.SetActive(true);
            monkModel.SetActive(false);
        }
        else
        {
            vikingModel.SetActive(false);
            monkModel.SetActive(true);
        }
    }
}
