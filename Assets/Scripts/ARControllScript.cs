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
    public GameObject debugCanvas;

    public GameObject VikingPrefab;
    public GameObject MonkPrefab;

    public Text debugText;

    private List<TrackedPlane> newPlanes = new List<TrackedPlane>();
    private List<TrackedPlane> allPlanes = new List<TrackedPlane>();

    private GameObject boardObject;

    private TrackableHit currentHit;

    private Anchor anchor;

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
        // Raycast agains a "non physical" arObject in the scene (for debuggin)
        RaycastHit mouseHit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out mouseHit, Mathf.Infinity);

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) || Frame.Raycast(mouseHit.point.x, mouseHit.point.y, raycastFilter, out hit)) //
        {
            currentHit = hit;
            if (boardObject == null) //Create only one board
            {
                Vector3 finHitPos = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Distance + 13.0f);
                //Instanciate the board facing up based on where the user hit
                boardObject = Instantiate(BoardPrefab, finHitPos, Quaternion.Euler(0, 0, 0)); //hit.Pose.position

                anchor = hit.Trackable.CreateAnchor(hit.Pose);

                // Make board model a child of the anchor.
                boardObject.transform.parent = anchor.transform;

                SetPlayerAvatars(GameManager.instance.player1.GetComponent<Player>().HasViking, finHitPos);

                GameManager.instance.InitGame();

                debugCanvas.GetComponentInChildren<Text>().text = hit.Distance.ToString();
          

            }        
        } 
    }

    private void SetPlayerAvatars(bool p1HasViking, Vector3 hitPos)
    {
        float heightOffset = GameManager.instance.player1.GetComponent<Player>().vikingModel.GetComponent<Renderer>().bounds.size.y / 2;
        hitPos.y += heightOffset;
        GameManager.instance.player1.GetComponent<Player>().InitializePlayerModel(hitPos);
        GameManager.instance.player2.GetComponent<Player>().InitializePlayerModel(hitPos);

        


        GameManager.instance.player1.GetComponent<Player>().transform.parent = boardObject.transform; //Set players transform parent to that of the board
        GameManager.instance.player2.GetComponent<Player>().transform.parent = boardObject.transform; //which gets its' parent from the ancorpoint

        //GameManager.instance.player1.GetComponent<Player>().SetBoardPosition(boardObject.GetComponent<BoardScript>()
        //           .PlaceAvatarOnBoard(GameManager.instance.player1.GetComponent<Player>().Position, heightOffset));


        //GameManager.instance.player2.GetComponent<Player>().SetBoardPosition(boardObject.GetComponent<BoardScript>()
        //    .PlaceAvatarOnBoard(GameManager.instance.player2.GetComponent<Player>().Position, heightOffset));

        GameManager.instance.player1.GetComponent<Player>().PlayerModel.transform.position =
            boardObject.GetComponent<BoardScript>()
                .PlaceAvatarOnBoard(GameManager.instance.player1.GetComponent<Player>().Position, heightOffset); //Set the player postions based on that of the grid of emptyTiles ontop 

        GameManager.instance.player2.GetComponent<Player>().PlayerModel.transform.position =
            boardObject.GetComponent<BoardScript>()
                .PlaceAvatarOnBoard(GameManager.instance.player2.GetComponent<Player>().Position, heightOffset);

        debugText.text = GameManager.instance.player1.GetComponent<Player>().PlayerModel.transform.position.ToString();
        
    }


    public void CheckUserHit()
    {
        // Raycast against the location the player touched to search for planes.
        //TrackableHit hit;
        //TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon | TrackableHitFlags.FeaturePointWithSurfaceNormal;

        //if (Frame.Raycast(boardObject.transform.position.x, boardObject.transform.position.y, raycastFilter, out hit))
        //{
        //    boardObject.GetComponent<boardScript>().RevealroomAR(hit, debugCanvas);
        //}
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
            rect.x = 0.0f;
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
            Debug.Log(i.ToString() + " many planes");
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
