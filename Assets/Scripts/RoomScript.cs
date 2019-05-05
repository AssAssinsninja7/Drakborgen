using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    // Every tile will have this script

    // Maybe move this enum to gameManager later
    public enum RoomType { EventRoom, CorridorRoom, RotationRoom }
    // public so we can set in editor 
    public RoomType myRoomType;
    int myRoomRotation = 0;
    //                                   e1,     e2,   e3,     e4      public so we can set in editor 
    public bool[] myExitDirections = { false, false, false, false };

    void Start()
    {

    }

    void Init(int aRoomRotation)
    {
        if (aRoomRotation>360)
        {
            aRoomRotation -= 360;
        }
        myRoomRotation = aRoomRotation;
    }
    public bool[] GetExitDirections()
    {
        if (myRoomRotation > 0)
        {
            bool[] returnValue = myExitDirections;
            if (myRoomRotation == 90)
            {
                // rotate it to new position
                returnValue[0] = myExitDirections[3];
                returnValue[1] = myExitDirections[0];
                returnValue[2] = myExitDirections[1];
                returnValue[3] = myExitDirections[2];
                return returnValue;
            }
            else if (myRoomRotation == 180)
            {
                returnValue[0] = myExitDirections[2];
                returnValue[1] = myExitDirections[3];
                returnValue[2] = myExitDirections[0];
                returnValue[3] = myExitDirections[1];
                return returnValue;
            }
            else // == 270
            {
                returnValue[0] = myExitDirections[1];
                returnValue[1] = myExitDirections[2];
                returnValue[2] = myExitDirections[3];
                returnValue[3] = myExitDirections[0];
                return returnValue;
            }
        }

        return myExitDirections;
    }
    public RoomType GetRoomType()
    {
        return myRoomType;
    }

    public int getRotation()
    {
        return myRoomRotation;
    }
}
