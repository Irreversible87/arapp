/*
 * Main menu script
 * 
 * Simple script that invokes the main menu scene
 * at the start of the application.
 * 
 * @Author Lars Pastoor
 * @Date 20/04/2020
 * 
 * @Version 1.0
 * --- init
 * 
 * @Version 1.1
 * @Date 10/05/2020
 * 
 * -> added download routine for asset list
 */

using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    private string file_name = "assetList.json";

    public void Start()
    {
        StartCoroutine(WaitForAssetList(file_name));
    }
    public void StartApp ()
    {
        // Scenemanger loads the ar main scene with buildIndex 1
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /*
     * IENumerator WaitForAssetList
     * 
     * simple coroutine to download a assetList json file
     * which contains all available addressable asset package
     * labels
     * 
     */
    IEnumerator WaitForAssetList(string file_name)
    {
        var uri = string.Concat("https://www.lars-pastoor.de/public/", file_name);
        using (var webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath);

                var savePath = Path.Combine(Application.persistentDataPath, file_name);

                File.WriteAllText(savePath, webRequest.downloadHandler.text);
                while (!webRequest.downloadHandler.isDone)
                    yield return new WaitForEndOfFrame();
            }
        }
    }
}
