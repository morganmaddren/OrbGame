using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IHudManager
{
	IHud CurrentHud { get; }
}

public class HudManager : MonoBehaviour, IHudManager
{
    public GameObject hud;
    public IHud CurrentHud { get; private set; }

	void Awake()
	{
        CurrentHud = hud.GetComponentStrict<Hud>();
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}
}