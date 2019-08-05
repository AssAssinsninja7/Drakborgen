using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Queue<GameObject> roomStack; //roomStack is the container from which the boardScript gets the next placeable tile


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
        roomStack = new Queue<GameObject>();
        LoadRoomStack();
        ShuffleStack();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Add in every roomType to the roomStack  
    /// </summary>
    void LoadRoomStack()
    {
        //4st 4e no restriciton
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        roomStack.Enqueue(e1234nPnD);
        //2st 4e with door on e3
        roomStack.Enqueue(e134nPwD3);
        roomStack.Enqueue(e134nPwD3);
        //1st 4e with portcullis
        roomStack.Enqueue(e1234wPnD);
        //1st 4e with doors on e1, e2 & e3
        roomStack.Enqueue(e1234nPwD234);

        //4st 3e with exits on e1, e2 & e3
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        roomStack.Enqueue(e134nPnD);
        //1st 3e with door on e3
        roomStack.Enqueue(e134nPwD3);

        //4st 3e with exits on e1, e2 & e3
        //Doesn't exist yet
        //1st 3e with door on e3
        roomStack.Enqueue(e123nPwD3);

        //4st 3e exits on e1, e2 & e4
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        roomStack.Enqueue(e124nPnD);
        //1st 2e with portcullis 
        roomStack.Enqueue(e124wPnD);

        //2st 2e exits on e1 & e2 (left turn)
        roomStack.Enqueue(e12nPnD);
        roomStack.Enqueue(e12nPnD);
        //1st 2e with portcullis
        roomStack.Enqueue(e12wPnD);

        //2st 2e exits on e1 and e4 (right turn)
        roomStack.Enqueue(e14nPnD);
        roomStack.Enqueue(e14nPnD);
        //1st 2e with portcullis
        roomStack.Enqueue(e14wPnD);

        //6st 2e exits on e1 and e3 (straight forward)
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        roomStack.Enqueue(e13nPnD);
        //1st 2e with portcullis 
        roomStack.Enqueue(e13wPnD);
        //1st 2e with portcullis and door on e3
        roomStack.Enqueue(e13wPwD3);

        //2st 1e (Deadend)
        roomStack.Enqueue(e1nPnD);
        roomStack.Enqueue(e1nPnD);
        //1st 1e with portcullis
        roomStack.Enqueue(e1wPnD);

        //3st turnRooms
        roomStack.Enqueue(e1TurnRoom);
        roomStack.Enqueue(e1TurnRoom);
        roomStack.Enqueue(e1TurnRoom);

        //1st 4e Corridor
        roomStack.Enqueue(e1234Corridor);
        //1st 2e exits on e1 & e2, Corridor (left turn)
        roomStack.Enqueue(e12Corridor);
        //1st 2e exits on e1, & e4, Corridor (right turn)
        roomStack.Enqueue(e14Corridor);
        //1st 2e exits on e1 & e3, Corridor (straight forward
        roomStack.Enqueue(e13Corridor);

        //1st 3e exits on e1, e2, & e3. Corridor
        roomStack.Enqueue(e123Corridor);
        //1st 3e exits on e1, e2 & e4. Corridor
        roomStack.Enqueue(e124Corridor);
        //1st 3e exits on e1, e3, e4
        roomStack.Enqueue(e134Corridor);
    }

    /// <summary>
    /// Shuffle the rooms so that the order of them is unknown
    /// </summary>
    void ShuffleStack()
    {
        for (int i = 0; i < roomStack.Count; i++)
        {
            GameObject temp = roomStack.ToArray()[i];
            int rnd = Random.Range(0, roomStack.Count);
            roomStack.ToArray()[i] = roomStack.ToArray()[rnd];
            roomStack.ToArray()[rnd] = temp;
        }
    }

    //Have a method return the roomType maybe tag or something to the boardScript
    public GameObject RevealRoom()
    {
        return roomStack.Dequeue();
    }
}
