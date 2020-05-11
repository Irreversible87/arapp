using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONReader
{
    public static AssetLabels ReadAssetLabel(AssetLabels assetLabel)
    {
        string path = Application.persistentDataPath + "/assetList.json";
        string jsonString = File.ReadAllText(path);
        assetLabel = JsonUtility.FromJson<AssetLabels>(jsonString);
        return assetLabel;
    }

    public static ButtonLabels ReadButtonLabel(ButtonLabels buttonLabel)
    {
        string path = Application.persistentDataPath + "/nameList.json";
        string jsonString = File.ReadAllText(path);
        buttonLabel = JsonUtility.FromJson<ButtonLabels>(jsonString);
        return buttonLabel;
    }
}
