using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISettingsManager
{
	float MinimumBounceAngle { get; }
    int ScoreRequiredToWin { get; }
}

public class SettingsManager : MonoBehaviour, ISettingsManager
{
    public float minimumBounceAngle;
    public float MinimumBounceAngle { get { return minimumBounceAngle; } }

    public int scoreRequiredToWin;
    public int ScoreRequiredToWin { get { return scoreRequiredToWin; } }

	void Awake()
	{
		
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}
}