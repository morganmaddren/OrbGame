using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ICenterRose : IPointGenerator
{
    
}

public class CenterRose : PointGenerator, ICenterRose
{
    IFlowerBed flowerBed;

    public void Initialize(IFlowerBed flowerBed)
    {
        this.flowerBed = flowerBed;
    }

    public override void OnFullBump(IOrb orb, Vector2 collisionPoint)
    {
        flowerBed.OnFlowerHit(this, orb);
    }
}