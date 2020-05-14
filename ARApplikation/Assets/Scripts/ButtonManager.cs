/*
 * ButtonManager Class
 * 
 * A c# class to create all necessary buttons
 * at runtime for the given input of asset labels
 * and addressable assets.
 * 
 * @Author: Lars Pastoor
 * @Version: 1.0
 * @Date: 13/05/2020
 * 
 * -> init
 * 
 * @Version 1.1
 * @Date: 14/05/2020
 * 
 * -> Changed static class to monobehaviour
 */

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject prefabButton;
    public GameObject libPanel;
    public GameObject packagePanel;

    private bool labelButtonsCreated = false;
    private bool assetButtonsCreated = false;
    private List<GameObject> CreatedLabelButtons
        = new List<GameObject>();
    private Dictionary<GameObject, int> CreatedAssetButtons
        = new Dictionary<GameObject, int>();
    /*
     * GetCreatedLabelButton() method
     * 
     * Returns the created label button list.
     * 
     * @return List<GameObject>
     */
    public List<GameObject> GetCreatedLabelButtons()
    {
        return CreatedLabelButtons;
    }
    /*
     * GetCreatedAssetButtons() method
     * 
     * Returns the created asset buttons dictionary.
     * 
     * @return Dictionary<GameObject, Int>
     */
    public Dictionary<GameObject, int> GetCreatedAssetButtons()
    {
        return CreatedAssetButtons;
    }
    /*
     * CreateButtons() method
     * 
     * A method to create all necessary buttons dynamicly.
     * 
     */
    public void CreateButtons(AssetLabels assetLabels, List<GameObject> Assets)
    {
        int counter;
        int tmp;
        // checks if label buttons were created once
        if (!labelButtonsCreated)
        {
            counter = assetLabels.GetLabelLenght();
            tmp = 0;
        }
        // checks if asset buttons were not created once
        else if (!assetButtonsCreated)
        {
            counter = Assets.Count;
            tmp = 0;
        }
        // if label and asset buttons were created once
        else
        {
            // set the new counter to incomming asset list - already created asset buttons
            counter = Assets.Count - CreatedAssetButtons.Count;
            // ieterator to get the index right at creation
            tmp = CreatedAssetButtons.Count;
        }
        for (int i = 0; i < counter; i++)
        {
            // creates label buttons only once at start
            if (!labelButtonsCreated)
            {
                GameObject button = GameObject.Instantiate(prefabButton); // instantiates a prefab button
                button.name = "PackageButton" + i;
                button.transform.SetParent(packagePanel.transform);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = assetLabels.label[i]; // sets label text by given labels
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                CreatedLabelButtons.Add(button); // adds the created button to list
            }
            else
            {
                // try and catch block to check if a keyValuePair is already in dictionary
                try
                {
                    int buttonIndex = i+tmp; // calculate index if asset buttons were created before
                    GameObject button = GameObject.Instantiate(prefabButton);
                    button.name = "AssetButton" + buttonIndex;
                    button.transform.SetParent(libPanel.transform);
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Assets[buttonIndex].name;
                    button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
                    button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                    CreatedAssetButtons.Add(button, buttonIndex); // adds the created button to dictionary
                }
                // if button already in dictionary, catches the exception
                catch (ArgumentException)
                {
                    Debug.Log("Button already created");
                }

            }
        }
        // checks if label buttons were created and if not, sets them inactive in hierachy
        if (!labelButtonsCreated)
        {
            foreach (GameObject item in CreatedLabelButtons)
            {
                item.SetActive(false);
            }
            labelButtonsCreated = true; // will be true after first call
        }
        // sets all created asset buttons to inactive in hierachy
        else
        {
            foreach (KeyValuePair<GameObject, int> kvp in CreatedAssetButtons)
            {
                kvp.Key.SetActive(false);
            }
            assetButtonsCreated = true; // will be true after first call
        }
    }
}
