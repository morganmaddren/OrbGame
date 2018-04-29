using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectManager
{
    GameObject Create(GameObject prefab, Vector3 pos);
    GameObject Create(GameObject prefab);
    T CreatePrefab<T>(GameObject prefab) where T : MonoBehaviour;

    T CreatePrefab<T>(T prefab) where T : GameEntity;
}

public class ObjectManager : MonoBehaviour, IObjectManager
{
    void Awake()
    {

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject Create(GameObject prefab, Vector3 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity);
    }

    public GameObject Create(GameObject prefab)
    {
        return Instantiate(prefab);
    }

    public T CreatePrefab<T>(GameObject prefab) where T : MonoBehaviour
    {
        var component = Instantiate(prefab).GetComponent<T>();
        if (component == null)
            throw new InvalidOperationException(typeof(T).ToString());

        return component;
    }

    public T CreatePrefab<T>(T prefab) where T : GameEntity
    {
        return CreatePrefab<T>(prefab.gameObject);
    }
}
