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
 * @Version 1.1
 * @Date 03/05/2020
 * 
 * -> added AddButtons method and onClick event
 * -> added ActivateAssets method
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
    // fields to be accessed from inspector window
    [SerializeField] private string _label;
    [SerializeField] private string _name;

    public List<GameObject> Assets { get; } = new List<GameObject>();
    public GameObject buttonPrefab;
    public GameObject panel;

    private ARRaycastManager rayManager;

    private GameObject arAsset;
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

        // call and wait for all addressables to load
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
            arAsset.transform.position = hits[0].pose.position;
            arAsset.transform.rotation = hits[0].pose.rotation;

            ActivateAsset();
            
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
    private async Task CreateAndWaitUntilCompleted()
    {
        await CreateAddressablesLoader.InitAsset(_label, Assets);
        await CreateAddressablesLoader.InitAsset(_name, Assets);
        AddButtons();

        foreach (var asset in Assets)
        {
            asset.SetActive(false);
        }
    }
    /*
     * ClearAsset Method
     * 
     * simple method to clear the given
     * addressable out of memory.
     * 
     * 
     */
    private void ClearAsset(GameObject obj)
    {
        Addressables.Release(obj);
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
        for (int i = 0; i < Assets.Count; i++)
        {
            if (index == i)
            {
                arAsset = Assets[i];
                Debug.Log(arAsset);
            }
        }
    }
}
