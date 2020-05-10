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
 * @version 1.4
 * @Date 10/05/2020
 * 
 * -> created a new label based
 * asset creation system
 * -> changed ClearAsset() method to
 * reset the scene.
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
using System.IO;
using UnityEngine.SceneManagement;

public class ARSpawner : MonoBehaviour
{
    public List<GameObject> Assets { get; } = new List<GameObject>();
    public GameObject buttonPrefab;
    public GameObject libPanel;
    public GameObject packagePanel;
    public GameObject libMenu;
    public GameObject inAppMenu;

    private ARRaycastManager rayManager;
    private GameObject arAsset;
    private GameObject currentButton;
    private bool objectSelected = false;
    private bool assetButtonsAdded = false;
    private string jsonString;
    private string selectedLabel;
    private AssetLabels assetLabels;

    private readonly float screenWidth = Screen.width;
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
        CreateAssetLabels();
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
        }
    }
    /*
     * CreateAssetLabel method
     * 
     * reads downloaded assetList.json file
     * and creates an array out it's contents.
     * 
     */
    private void CreateAssetLabels()
    {
        string path = Application.persistentDataPath + "/assetList.json";
        jsonString = File.ReadAllText(path);
        assetLabels = JsonUtility.FromJson<AssetLabels>(jsonString);

        AddLabelButtons(assetLabels);
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
        await CreateAddressablesLoader.InitAsset(label, Assets);

        foreach (var asset in Assets)
        {
            asset.SetActive(false);
        }
    }
    /*
     * ClearAsset Method
     * 
     * simple method to clear the given
     * addressable out of memory and resets
     * the scene.
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
            Addressables.ReleaseInstance(arAsset);
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
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
     * AddLabelButtons()
     * 
     * Method to instantiate a prefabe button for
     * each addressable asset. The user then can
     * select each addressable asset from within the
     * library menu.
     * 
     */
    private void AddLabelButtons(AssetLabels assetLabels)
    {
        // for loop for all adressable assets created
        for (int i = 0; i < assetLabels.GetLenght(); i++)
        {
            // buttonIndex to clearly indentify the button after creation
            int buttonPIndex;
            // instance from prefab button
            GameObject button = Instantiate(buttonPrefab);
            // create buttons with transfrom to button panel
            button.transform.SetParent(packagePanel.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = assetLabels.label[i];
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
            button.name = "Button" + i;
            buttonPIndex = i;
            button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth);
            // add button listener a transfer the index and the button object to onClick listener
            button.GetComponent<Button>().onClick.AddListener(async () => { await OnClickPackageAsync(buttonPIndex); });
            Debug.Log(buttonPIndex);
        }
    }
    /*
     * OnClick Event
     * 
     * Checks if the button index is the same as
     * a label in assetLabels object array.
     * 
     */
    private async Task OnClickPackageAsync(int index)
    {
        for (int i = 0; i < assetLabels.GetLenght(); i++)
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
                    // call and wait for all addressables to load
                    await CreateAndWaitUntilCompleted(selectedLabel);

                    // checks if buttons already added
                    if(assetButtonsAdded == false)
                    {
                        AddAssetButtons();
                        assetButtonsAdded = true;
                    } else
                    {
                        Debug.Log("Buttons already created");
                    }
                }
                // switching between the two panels in lib menu
                packagePanel.SetActive(false);
                libPanel.SetActive(true);
            }
        }
    }
    /*
     * AddAssetButtons()
     * 
     * Method to instantiate a prefabe button for
     * each addressable asset. The user then can
     * select each addressable asset from within the
     * library menu.
     * 
     */
    private void AddAssetButtons()
    {
        // for loop for all adressable assets created
        for (int i = 0; i < Assets.Count; i++)
        {
            // buttonIndex to clearly indentify the button after creation
            int buttonIndex;
            // instance from prefab button
            GameObject button = Instantiate(buttonPrefab);
            // create buttons with transfrom to button panel
            button.transform.SetParent(libPanel.transform);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Assets[i].name;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
            button.name = "Button" + i;
            buttonIndex = i;
            button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenWidth);
            // add button listener a transfer the index and the button object to onClick listener
            button.GetComponent<Button>().onClick.AddListener(() => { OnClickLib(buttonIndex, button); });
        }
    }
    /*
     * OnClick Event
     * 
     * Checks if the button index is the same as
     * a asset on Assets list index and sets the AR Asset
     * to this asset from list.
     * 
     */
    private void OnClickLib(int index, GameObject button)
    {
        currentButton = button;
        for (int i = 0; i < Assets.Count; i++)
        {
            if (index == i)
            {
                arAsset = Assets[i];
                Debug.Log(arAsset);
                objectSelected = true;
                libMenu.SetActive(false);
                inAppMenu.SetActive(true);
            }
        }
    }
}
