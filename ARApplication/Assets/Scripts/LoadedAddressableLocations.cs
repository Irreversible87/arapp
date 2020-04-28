using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Threading.Tasks;
public class LoadedAddressableLocations : MonoBehaviour
{
    [SerializeField] private string _label;

    public IList<IResourceLocation> AssetLocations { get; } = new List<IResourceLocation>();

    private void Start()
    {
        InitAndWaitUntilLoaded(_label);
    }

    public async Task InitAndWaitUntilLoaded(string label)
    {
        await AddressableLocationLoader.GetAll(label, AssetLocations);

        foreach (var location in AssetLocations)
        {
            // Assets loaded
            Debug.Log(location.PrimaryKey);
        }
    }
}
