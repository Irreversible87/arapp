/*
 * ARSpawner class
 * 
 * A c# class to detect planes in augmented reality
 * and to invoke ar content from addressable asset
 * system.
 * 
 * addressable asset system code based on https://medium.com/@badgerdox
 * created by Badger Dox - 2019
 * 
 * Adapted and edited code
 * 
 * @Author: Lars Pastoor
 * @Version: 1.0
 * @Date: 29/04/2020
 * 
 * -> init
 * 
 * @Version: 1.1
 * @Date: 03/05/2020
 * 
 * -> added AddButtons method and onClick event
 * -> added ActivateAssets method
 * 
 * 
 * @Version: 1.2
 * @Date: 08/05/20
 * 
 * -> fixed AddButtons function
 * -> catched Null-Pointer Reference when no
 * asset was selected
 * 
 * @version: 1.3
 * @Date: 10/05/2020
 * 
 * -> created a new label based
 * asset creation system
 * -> changed ClearAsset() method to
 * reset the scene.
 * 
 * @version: 1.4
 * @Date: 13/05/2020
 * 
 * -> changed method to create buttons
 * -> created InitButtons method
 * 
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARSpawner : MonoBehaviour
{
    public List<GameObject> Assets { get; } =
        new List<GameObject>();
    public GameObject buttonPrefab;
    public GameObject libPanel;
    public GameObject packagePanel;
    public GameObject libMenu;
    public GameObject inAppMenu;
    public GameObject loadScreen;

    private ARRaycastManager rayManager;
    private GameObject arAsset;
    private bool objectSelected = false;
    private bool labelButtonsInitiated = false;
    private string selectedLabel;
    private AssetLabels assetLabels;
    private Dictionary<GameObject, string> groups =
        new Dictionary<GameObject, string>();
    private Dictionary<GameObject, int> assetButtons =
                new Dictionary<GameObject, int>();
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
        // creates asset labels from json file
        assetLabels = JSONReader.ReadAssetLabel();

        // calls ButtonManager to create label buttons at start
        ButtonManager.CreateButtons(assetLabels, Assets, buttonPrefab, packagePanel, libPanel);
        InitButtons();
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

        // checks if a object is already selected or not
        if (objectSelected == true)
        {
            // hitcheck, if AR plane surface is hit, update position and rotation of the asset
            if (hits.Count > 0)
            {
                ActivateAsset();
                arAsset.transform.position = hits[0].pose.position;
                arAsset.transform.rotation = hits[0].pose.rotation;
            }
        }
        else
        {
            Debug.Log("No Object Selected");
            if (arAsset != null)
            {
                arAsset.SetActive(false);
            }
        }
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
    private async Task CreateAndWaitUntilCompleted(string label)
    {
        bool loaded = false;
        while (loaded == false)
        {
            loadScreen.SetActive(true);
            await CreateAddressablesLoader.InitAsset(label, Assets);
            loaded = true;
        }
        loadScreen.SetActive(false);
        foreach (var asset in Assets)
        {
            asset.SetActive(false);
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
     * InitButtons() method
     * 
     * Creates label buttons at start. If called the
     * second time, creates all buttons for the chosen
     * addressables and sets the chosen addressables active
     * by label.
     * 
     */
    private void InitButtons()
    {
        // checks if label buttons were created at start
        if (labelButtonsInitiated == false)
        {
            List<GameObject> CreatedLabelButtons = new List<GameObject>();
            CreatedLabelButtons = ButtonManager.GetCreatedLabelButtons();

            for (int i = 0; i < CreatedLabelButtons.Count; i++)
            {
                int buttonIndex = i;
                CreatedLabelButtons[i].GetComponent<Button>().
                    onClick.AddListener(async () => { await OnClickPackageAsync(buttonIndex); });
                CreatedLabelButtons[i].SetActive(true);
            }
            labelButtonsInitiated = true;
        }
        else
        {
            // gets the created asset buttons from ButtonManger
            assetButtons = ButtonManager.GetCreatedAssetButtons();
            
            foreach (KeyValuePair<GameObject, int> kvp in assetButtons)
            {
                // Sets the index for all created asset buttons
                kvp.Key.GetComponent<Button>().
                    onClick.AddListener(() => { OnClickLib(kvp.Value); });

                // adds the asset buttons to the selected label
                try
                {
                    groups.Add(kvp.Key, selectedLabel);
                }
                catch (ArgumentException)
                {
                    Debug.Log("KeyValuePair alread added to Dictionary: groups");
                }
            }
            // checks if active asset buttons are in the same label group
            foreach (KeyValuePair<GameObject, string> kvp in groups)
            {
                if (selectedLabel.Equals(kvp.Value))
                {
                    kvp.Key.SetActive(true);
                }
                else
                {
                    kvp.Key.SetActive(false);
                }
            }
        }
    }
    /*
     * OnClick Event: OnClickPackageAsync
     * 
     * Checks if a label selected and then calls
     * CreatesAndWaitUntilCompleted to load the
     * addressables by label, after that, creates
     * the corresponding buttons for the loaded
     * addressables.
     * 
     */
    private async Task OnClickPackageAsync(int index)
    {
        objectSelected = false;

        for (int i = 0; i < assetLabels.GetLabelLenght(); i++)
        {
            if (index == i)
            {
                selectedLabel = assetLabels.label[i];
                Debug.Log("SELECTED LABEL: " + selectedLabel);

                // checks if a label is selected or not
                if (selectedLabel == null)
                {
                    Debug.Log("No Label selected");
                }
                else
                {
                    // wait for addressables to load
                    bool loaded = false;
                    while (loaded == false)
                    {
                        loadScreen.SetActive(true);
                        // call and wait for all addressables to load
                        await CreateAndWaitUntilCompleted(selectedLabel);
                        loaded = true;
                    }
                    if (groups.ContainsValue(selectedLabel))
                    {
                        Debug.Log("Buttons already created");
                    }
                    else
                    {
                        // create buttons and initialise them
                        ButtonManager.CreateButtons(assetLabels, Assets, buttonPrefab, packagePanel, libPanel);
                    }
                    // init the buttons to check if asset group = label group
                    InitButtons();
                    loadScreen.SetActive(false);
                }
                // switching between the two panels in lib menu
                packagePanel.SetActive(false);
                libPanel.SetActive(true);
            }
        }
    }
    /*
     * OnClick Event
     * 
     * Checks if the button index is the same as
     * a asset on loaded addressable assets list index and sets 
     * the AR Asset to this asset from list.
     * 
     */
    private void OnClickLib(int index)
    {
        for (int i = 0; i < Assets.Count; i++)
        {
            if (index == i)
            {
                arAsset = Assets[i];
                objectSelected = true;
                libMenu.SetActive(false);
                inAppMenu.SetActive(true);
            }
        }
    }
}
