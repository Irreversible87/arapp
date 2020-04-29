/*
 * ARSpawner Class
 * 
 * A c# class to detect planes in augmented reality
 * and to invoke ar content from addressable asset
 * system.
 * 
 * addressable asset system code based on https://medium.com/@badgerdox
 * created by Badger Dox - 2019
 * 
 * Adapted and edited code
 * @Author Lars Pastoor
 * @E-Mail lars.pastoor87@gmail.com
 * 
 * @Version 1.0
 * @Date 29/04/2020
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARSpawner : MonoBehaviour
{
    // fields to be accessed from inspector window
    [SerializeField] private string _label;
    [SerializeField] private string _name;

    private List<GameObject> Assets { get; } = new List<GameObject>();
    private ARRaycastManager rayManager;

    /*
     * Start() method
     * 
     * will be executed once the application
     * has started
     */
    private void Start()
    {

        // get raycaster manager objects
        rayManager = FindObjectOfType<ARRaycastManager>();

        // create addressable assets
        CreateAndWaitUntilCompleted();

    }

    /*
     * Update() method
     * 
     * Generates a list of detected planes from raycast
     * 
     * Called once every frame
     */
    void Update()
    {
        // raycast from center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // hitcheck, if AR plane surface is hit, update position and rotation of the asset
        if (hits.Count > 0)
        {
            Assets[0].transform.position = hits[0].pose.position;
            Assets[0].transform.rotation = hits[0].pose.rotation;
        }

    }
    /*
     * CreateAndWaitUntilCompleted Task
     * 
     * Creates objects of type CreateAdressablesLoader
     * and invokes Init Asset method from CreateAddressablesLoader
     * class
     * 
     * Async task to allow await for all assets to be created
     * 
     */
    private async Task CreateAndWaitUntilCompleted()
    {
        await CreateAddressablesLoader.InitAsset(_label, Assets);
        await CreateAddressablesLoader.InitAsset(_name, Assets);

        foreach (var asset in Assets)
        {
            // shows a console log with every loaded asset
            Debug.Log(asset.name);
        }
    }
}
