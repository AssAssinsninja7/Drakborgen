using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{

    public GameObject gameBoard;
    public List<GameObject> rooms;

    // Start is called before the first frame update
    void Start()
    {
        ShuffleStack();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
