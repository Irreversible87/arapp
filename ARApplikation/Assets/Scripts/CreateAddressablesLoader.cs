/*
 * CreateAddressablesLoader Class
 * 
 * A c# class to create addressable assets from location
 * 
 * addressable asset system code based on https://medium.com/@badgerdox
 * created by
 * 
 * @Autho: Badger Dox
 * @Version: 1.0
 * @Date: 2019
 * 
 * @Version: 1.1
 * @Date 12/05/2020
 * @Author: Lars Pastoor
 * 
 * -> created check if a label
 * was already called.
 * 
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class CreateAddressablesLoader
{
    private static List<string> CalledLabels = new List<string>();


/*
* Init Asset Task
* 
* Task to load addressable assets via label and
* instantiate after loading
* 
*/
    public static async Task InitAsset<T>(string assetLabel, List<T> createdObjs)
        where T : Object
    {

        // checks if a label was already called
        if (CalledLabels.Contains(assetLabel))
        {
            Debug.Log("Label: " + assetLabel + " was already called.");

        } else if (CalledLabels == null || !CalledLabels.Contains(assetLabel))
        {
            // load addressables by name or label
            var locations = await Addressables.LoadResourceLocationsAsync(assetLabel).Task;
            CalledLabels.Add(assetLabel);
            Debug.Log("CALLED: " + assetLabel);
            // instantiate loaded addressables
            foreach (var location in locations)
            {
                createdObjs.Add(await Addressables.InstantiateAsync(location).Task as T);
            }
        }
    }
}
