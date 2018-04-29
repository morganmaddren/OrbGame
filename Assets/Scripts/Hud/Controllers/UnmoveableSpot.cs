using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IUnmoveableSpot
{
    PointEntity[] ClosestPoints { get; }
}

public class UnmoveableSpot : MonoBehaviour, IUnmoveableSpot
{
    public PointEntity[] points;
    public PointEntity[] ClosestPoints { get { return points; } }
}