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
 */

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ButtonManager
{
    private static bool labelButtonsCreated = false;
    private static bool assetButtonsCreated = false;
    private static List<GameObject> CreatedLabelButtons
        = new List<GameObject>();
    private static Dictionary<GameObject, int> CreatedAssetButtons
        = new Dictionary<GameObject, int>();
    /*
     * GetCreatedLabelButton() method
     * 
     * Returns the created label button list.
     * 
     * @return List<GameObject>
     */
    public static List<GameObject> GetCreatedLabelButtons()
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
    public static Dictionary<GameObject, int> GetCreatedAssetButtons()
    {
        return CreatedAssetButtons;
    }
    /*
     * CreateButtons() method
     * 
     * A method to create all necessary buttons dynamicly.
     * 
     */
    public static void CreateButtons(AssetLabels assetLabels, List<GameObject> Assets, GameObject prefabButton,
        GameObject packagePanel, GameObject libPanel)
    {
        int counter;
        int tmp;
        // checks if label buttons were created once
        if (labelButtonsCreated == false)
        {
            counter = assetLabels.GetLabelLenght();
            tmp = 0;
        }
        // checks if asset buttons were not created once
        else if (assetButtonsCreated == false)
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
            if (labelButtonsCreated == false)
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
        if (labelButtonsCreated == false)
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
