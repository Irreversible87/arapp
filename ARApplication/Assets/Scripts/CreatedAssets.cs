using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CreatedAssets : MonoBehaviour
{
    private LoadedAddressableLocations _loadedLocations;
    private ARRaycastManager rayManager;

    [field: SerializeField] private List<GameObject> Assets { get; } = new List<GameObject>();

    private void Start()
    {
        rayManager = FindObjectOfType<ARRaycastManager>();

        CreateAndWaitUntilCompleted();
    }

    private void Update()
    {

        // raycast from center of the screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // hitcheck -> if AR plane surface is hit, update position and rotation
        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }

    private async Task CreateAndWaitUntilCompleted()
    {
        _loadedLocations = GetComponent<LoadedAddressableLocations>();

        await Task.Delay(TimeSpan.FromSeconds(1));

        await CreateAddressablesLoader.ByLoadedAddress(_loadedLocations.AssetLocations, Assets);

        foreach (var asset in Assets)
        {
            Debug.Log(asset.name);
        }
    }
}
