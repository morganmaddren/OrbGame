using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceManager
{
    T LoadResource<T>(string assetName) where T : Object;
    GameObject LoadPrefab(string assetName);
    T LoadPrefab<T>(string assetName) where T : MonoBehaviour;
    void UnloadResource(string assetName);
}

public class ResourceManager : MonoBehaviour, IResourceManager
{
    Dictionary<string, Object> cache = new Dictionary<string, Object>();

    public T LoadResource<T>(string assetName) where T : Object
    {
        Object r;
        if (cache.TryGetValue(assetName, out r))
            return (T)r;

        var obj = (T)Resources.Load(assetName, typeof(T));
        cache.Add(assetName, obj);

        return obj;
    }

    public GameObject LoadPrefab(string assetName)
    {
        return Game.Objects.Create(LoadResource<GameObject>(assetName));
    }

    public T LoadPrefab<T>(string assetName) where T : MonoBehaviour
    {
        return Game.Objects.CreatePrefab<T>(LoadResource<GameObject>(assetName));
    }

    public void UnloadResource(string assetName)
    {
        Object r;
        if (cache.TryGetValue(assetName, out r))
            Resources.UnloadAsset(r);
    }
}
