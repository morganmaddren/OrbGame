using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISpawnPosition
{
	HeroSide Side { get; }
}

public class SpawnPosition : MonoBehaviour, ISpawnPosition
{
    public HeroSide side;
    public HeroSide Side { get { return side; } }
}