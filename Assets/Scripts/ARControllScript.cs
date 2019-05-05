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

    public Camera arCamera; 
    public GameObject planeStatusInfo;
    public GameObject trackedPlanePrefab;
    public GameObject BoardPrefab;

    private List<TrackedPlane> newPlanes = new List<TrackedPlane>();
    private List<TrackedPlane> allPlanes = new List<TrackedPlane>();

    private GameObject boardObject;

    private TrackableHit currentHit;

    //ARcontroller UI & Session controll (Exit when fukked)
    private bool isQuitting = false;
    private bool showSearchUI = true;


    // Start is called before the first frame update
    void Start()
    {
        SetScreenSize();
    }

    // Update is called once per frame
    void Update()
    {
        //Check ARcore sesh from connection error
        QuitOnConnectionErrors();

        // Check that motion tracking is tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            if (!isQuitting && Session.Status.IsValid())
            {
                planeStatusInfo.SetActive(true);
            }
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //Fills the list newPlanes with the newly found planes from the current frame
        Session.GetTrackables<TrackedPlane>(newPlanes, TrackableQueryFilter.New);
        //Find new planes and place out the trackedPlaneprefab where they have been found
        FindPlanes();

        //Fills the list allPlanes with the newly found planes from the current frame
        Session.GetTrackables<TrackedPlane>(allPlanes);
        HasPlanes();
        //Disale the UI based on if we found planes
        planeStatusInfo.SetActive(showSearchUI);       

        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }

        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;     

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            currentHit = hit;
            if (boardObject == null) //Create only one board
            {
                //Instanciate the board facing up based on where the user hit
                boardObject = Instantiate(BoardPrefab, new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Distance), Quaternion.Euler(90, 0, 0)); //hit.Pose.position

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                // world evolves.
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make board model a child of the anchor.
                boardObject.transform.parent = anchor.transform;
            }
            else //Check where the user pressed () send info to gamemgr 
            {

            }          
        }
    }

    /// <summary>
    /// Return the last presshit by the player (to gameMgr)
    /// </summary>
    /// <returns></returns>
    public TrackableHit GetCurrentHit()
    {
        return currentHit;
    }

    void SetScreenSize()
    {
        // set the desired aspect ratio (Hardcoded to 16:9)
        float targetScreenRatio = 16.0f / 10.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetScreenRatio;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = arCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            arCamera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = arCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            arCamera.rect = rect;
        }
    }

    /// <summary>
    /// Check if any new planes has been from the current frame and then places them if any were found. 
    /// (Session.GetTrackables = is finding all new planes from this frame)
    /// </summary>
    private void FindPlanes() 
    {
        for (int i = 0; i < newPlanes.Count; i++)
        {           
            GameObject worldGrid = Instantiate(trackedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
            worldGrid.GetComponent<GameBoardVizualizer>().Initialize(newPlanes[i]);
        }
    }

    /// <summary>
    /// Check if we have identified planes and disables the searcing ui if we have
    /// </summary>
    /// <returns></returns>
    private void HasPlanes()
    {
        showSearchUI = true;
        for (int i = 0; i < allPlanes.Count; i++)
        {
            if (allPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchUI = false;
                break;
            }
        }
    }


    /// <summary>
    /// Quit the application if there was a connection error for the ARCore session.
    /// </summary>
    private void QuitOnConnectionErrors()
    {
        if (isQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            //_ShowAndroidToastMessage("Camera permission is needed to run this application.");
            isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            //_ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
            isQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void DoQuit()
    {
        Application.Quit();
    }
}
