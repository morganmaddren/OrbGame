using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using UnityObject = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;

public interface ILevelManager
{
    ILevel CurrentLevel { get; }
}

public class LevelManager : MonoBehaviour, ILevelManager
{
    public ILevel CurrentLevel { get; private set; }

    public GameObject level;

    void Awake()
    {
        CurrentLevel = level.GetComponentStrict<Level>();
    }

    void Start()
	{
	}
	
	void Update()
	{
		
	}
}