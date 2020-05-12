/*
 * JSONReader Class
 * 
 * Class that provides a method
 * to read in and parse a given 
 * json file from a given path
 * to a AssetLabels Object
 * 
 * @Author Lars Pastoor
 * @E-Mail lars.pastoor87@gmail.com
 * 
 * @Version 1.0
 * @Date 11/05/2020
 * 
 * 
 */
using System.IO;
using UnityEngine;

public static class JSONReader
{
    /*
     * ReadAssetLabel method
     * 
     * reads in json file from persistantDataPath
     * an parses it in AssetLabels assetLabel Object
     * (string).
     * 
     */
    public static AssetLabels ReadAssetLabel()
    {
        string path = Application.persistentDataPath + "/assetList.json";
        string jsonString = File.ReadAllText(path);
        AssetLabels assetLabel = JsonUtility.FromJson<AssetLabels>(jsonString);
        return assetLabel;
    }
}
