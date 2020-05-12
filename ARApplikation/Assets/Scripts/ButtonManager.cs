using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class ButtonManager
{
    private static bool labelButtonsCreated = false;
    private static List<GameObject> CreatedLabelButtons = new List<GameObject>();
    private static List<GameObject> CreatedAssetButtons = new List<GameObject>();

    public static List<GameObject> GetCreatedLabelButtons()
    {
        return CreatedLabelButtons;
    }
    public static List<GameObject> GetCreatedAssetButtons()
    {
        return CreatedAssetButtons;
    }
    public static void CreateButtons(AssetLabels assetLabels, List<GameObject> Assets, GameObject buttonPrefab,
        GameObject packagePanel, GameObject libPanel)
    {
        int counter = 0;
        GameObject assetButton = null;
        if (labelButtonsCreated == false)
        {
            counter = assetLabels.GetLabelLenght();
        }
        else if (labelButtonsCreated == true)
        {
            counter = Assets.Count;
        }
        for (int i = 0; i < counter; i++)
        {
            if (labelButtonsCreated == false)
            {
                GameObject labelButton = Object.Instantiate(buttonPrefab);
                labelButton.name = "PackageButton" + i;
                labelButton.transform.SetParent(packagePanel.transform);
                labelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = assetLabels.label[i];
                labelButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
                labelButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
                CreatedLabelButtons.Add(labelButton);
            }
            else if (labelButtonsCreated == true)
            {
                assetButton = Object.Instantiate(buttonPrefab);
                assetButton.name = "AssetButton" + i;
                assetButton.transform.SetParent(libPanel.transform);
                assetButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Assets[i].name;
                assetButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 75;
                assetButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
            }
        }
        if (labelButtonsCreated == false)
        {
            foreach (GameObject button in CreatedLabelButtons)
            {
                button.SetActive(false);
            }
            labelButtonsCreated = true;
        }
        else if (labelButtonsCreated == true)
        {
            foreach (GameObject button in CreatedAssetButtons)
            {
                Debug.Log(button);
                button.SetActive(false);
            }
        }
    }
}
