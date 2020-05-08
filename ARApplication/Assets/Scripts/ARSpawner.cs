﻿/*
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
 * @Version 1.1
 * @Date 03/05/2020
 * 
 * -> added AddButtons method and onClick event
 * -> added ActivateAssets method
 * 
 * @Version 1.2
 * @Date 07/05/2020
 * 
 * -> Added ClearAsset function
 * 
 * @Version 1.3
 * @Date 08/05/20
 * 
 * -> fixed AddButtons function
 * -> catched Null-Pointer Reference when no
 * asset was selected
 * 
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.AddressableAssets;

public class ARSpawner : MonoBehaviour
{
    private readonly string _label = "arcontent";

    public List<GameObject> Assets { get; } = new List<GameObject>();
    public GameObject buttonPrefab;
    public GameObject panel;

    private ARRaycastManager rayManager;
    private GameObject arAsset;
    private GameObject currentButton;
    private bool objectSelected = false;

    private readonly float screenWidth = Screen.width;
    /*
     * Start() method
     * 
     * will be executed once the application
     * has started
     */
    private async void Start()
    {
        // get raycaster manager objects
        rayManager = FindObjectOfType<ARRaycastManager>();

        // call and wait for all addressables to load
        await CreateAndWaitUntilCompleted();
        AddButtons();
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

        if (objectSelected == true)
        {
            if (hits.Count > 0)
            {
                ActivateAsset();
                arAsset.transform.position = hits[0].pose.position;
                arAsset.transform.rotation = hits[0].pose.rotation; 
            }
        } else
        {
            Debug.Log("No Object Selected");
        }

        // hitcheck, if AR plane surface is hit, update position and rotation of the asset
        
    }
    /*
     * CreateAndWaitUntilCompleted Task
     * 
     * Creates objects of type CreateAdressablesLoader
     * and invokes Init Asset method from CreateAddressablesLoader
     * class. Also dissables all loaded assets in scene.
     * 
     * Async task to allow await for all assets to be created
     * 
     */
    private async Task CreateAndWaitUntilCompleted()
    {
        await CreateAddressablesLoader.InitAsset(_label, Assets);

        foreach (var asset in Assets)
        {
            asset.SetActive(false);
        }
    }
    /*
     * ClearAsset Method
     * 
     * simple method to clear the given
     * addressable out of memory and removes
     * the created button for it.
     * 
     * 
     */
    public void ClearAsset()
    {
        if (arAsset == null)
        {
            Debug.Log("No AR Asset selected");
        } else
        {
            Addressables.Release(arAsset);
            Destroy(currentButton.gameObject);
            objectSelected = false;
        }

    }
    /*
     * ActivateAsset()
     * 
     * Method to check if the AR Asset is already
     * active in scene and if not, enables it.
     * Also disables all other addressables that
     * are called before.
     * 
     */
    private void ActivateAsset()
    {
        if (!arAsset.activeInHierarchy)
        {
            foreach (var asset in Assets)
            {
                asset.SetActive(false);
            }
            arAsset.SetActive(true);
        }
    }
    /*
     * AddButtons()
     * 
     * Method to instantiate a prefabe button for
     * each addressable asset. The user then can
     * select each addressable asset from within the
     * library menu.
     * 
     */
    private void AddButtons()
    {
        // for loop for all adressable assets created
        for (int i = 0; i < Assets.Count; i++)
        {
            // buttonIndex to clearly indentify the button after creation
            int buttonIndex;
            // instance from prefab button
            GameObject button = Instantiate(buttonPrefab);
            // create buttons with transfrom to button panel
            button.transform.SetParent(panel.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Assets[i].name;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
            button.name = "Button" + i;
            buttonIndex = i;

            button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth);
            // add button listener a transfer the index and the button object to onClick listener
            button.GetComponent<Button>().onClick.AddListener(() => { OnClick(buttonIndex, button); });
        }

    }
    /*
     * OnClick Event
     * 
     * Checks if the button idex is the same as
     * a asset on Assets list index and sets the AR Asset
     * to this asset from list.
     * 
     */
    private void OnClick(int index, GameObject button)
    {
        Debug.Log("Clicked on Button" + button.name + " with Index: " + index);
        currentButton = button;
        for (int i = 0; i < Assets.Count; i++)
        {
            if (index == i)
            {
                arAsset = Assets[i];
                Debug.Log(arAsset);
                objectSelected = true;
            }
        }
    }
}
