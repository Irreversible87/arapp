/*
 * CreateAddressablesLoader Class
 * 
 * A c# class to create addressable assets from location
 * 
 * addressable asset system code based on https://medium.com/@badgerdox
 * created by
 * 
 * @Author Badger Dox
 * @Date: 2019
 * 
 * 
 */
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class CreateAddressablesLoader
{
    /*
     * Init Asset Task
     * 
     * Task to load addressable assets via name or label and
     * instantiate after loading
     * 
     */
    public static async Task InitAsset<T>(string assetLabel, List<T> createdObjs)
        where T : Object
    {
        // load addressables by name or label
        var locations = await Addressables.LoadResourceLocationsAsync(assetLabel).Task;
        // instantiate loaded addressables
        foreach (var location in locations)
        {
            createdObjs.Add(await Addressables.InstantiateAsync(location).Task as T);
        }  
    }
}
