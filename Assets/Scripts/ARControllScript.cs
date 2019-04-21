using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.Rendering;
using UnityEngine.UI;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
//using Input = InstantPreviewInput;
#endif

public class ARControllScript : MonoBehaviour
{   
    public Text planeStatusInfo;
    public GameObject trackedPlanePrefab;
    public GameObject BoardPrefab;

    private List<TrackedPlane> trackedPlanes = new List<TrackedPlane>();


    //private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //Check session status
        if (Session.Status != SessionStatus.Tracking) //if we aint tracking
        {
            return;
        }
        //Fills the list trackedPlanes with the newly found planes from the current frame
        Session.GetTrackables<TrackedPlane>(trackedPlanes, TrackableQueryFilter.New);

        if (HasPlanes())
        {
            planeStatusInfo.text = "Planes found";
            planeStatusInfo.enabled = false;
        }
        else
        {
            planeStatusInfo.text = "Searcing 4 planes";
            planeStatusInfo.enabled = true;

            //Init each found grid with the game board
            FindPlanes();
        }


    }


    public void FindPlanes() //maybe have this as a bool so that the gamemgr can check to see if plane has been found
    {
        for (int i = 0; i < trackedPlanes.Count; i++)
        {
            //GameObject grid = Instantiate(BoardPrefab, Vector3.zero, Quaternion.FromToRotation(BoardPrefab.transform.position, trackedPlanes[i].CenterPose.position), transform);
            GameObject grid = Instantiate(trackedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
            grid.GetComponent<GameBoardVizualizer>().Initialize(trackedPlanes[i]);

            //gameManager.HasPlanes = HasPlanes();
            //if (Input.touchCount > 0) //If player has touched the screen init it (switch to when it has found and then have the players select)
            //{
            //    //Spawn gameboard lalalla
            //}    
        }
    }

    /// <summary>
    /// Check if we have identified planes
    /// </summary>
    /// <returns></returns>
    public bool HasPlanes()
    {
        bool result = false;

        if (trackedPlanes.Count > 0)
        {
            result = true;
        }
        return result;
    }
}
