using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class S_PlacementIndiciator : MonoBehaviour
{
    private ARRaycastManager rayManager;
    private GameObject visual;

    // Start is called before the first frame update
    void Start()
    {
        // get AR components
        rayManager = FindObjectOfType<ARRaycastManager>();
        visual = transform.GetChild(0).gameObject;

        // hide placement indiciator visual
        visual.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // raycast from center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // hitcheck -> if AR plane surface is hit, update position and rotation
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;

            // enable the visual if it's disabled
            if (!visual.activeInHierarchy)
                visual.SetActive(true);
        }

    }
}
