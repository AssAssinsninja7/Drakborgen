using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ARController : MonoBehaviour
{

    List<TrackedPlane> trackedPlanes = new List<TrackedPlane>();

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
    }
}
