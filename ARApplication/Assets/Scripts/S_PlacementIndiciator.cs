/*
 * Placement indicator script
 * 
 * A script that detects planes in augmented
 * reality via raycast hit detection. If a plane
 * is detected, a marker is shown through the display.
 * 
 * This code is based on:
 * https://vrgamedevelopment.pro/how-to-create-an-augmented-reality-app-in-unity/
 * visited April. 15th 2020
 * 
 * Coder: Daniel Buckley
 * Date: 25/09/2019
 * 
 * Rewritten by Lars Pastoor
 * Date: 15/04/2020
 * Version 1.0
 * 
 * 
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AddressableAssets;


public class S_PlacementIndiciator : MonoBehaviour
{
    private GameObject visual;
    private ARRaycastManager rayManager;

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
