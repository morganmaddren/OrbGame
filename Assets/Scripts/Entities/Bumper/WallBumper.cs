using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class WallBumper : Bumper
{
    public override void OnBump(IOrb orb, Vector2 collisionPoint)
    {
        Vector2 normal = (orb.Position - collisionPoint).normalized;
        Vector2 newDirection = orb.Direction - 2 * (orb.Direction.x * normal.x + orb.Direction.y * normal.y) * normal;

        float newSpin = orb.Spin;
        newDirection = MathHelper.Rotate(newDirection, newSpin * .5f);
        
        if (Math.Abs(newSpin) < .25f)
            newSpin = 0;
        else
            newSpin -= .25f * Math.Sign(newSpin);

        orb.ForceHit(newDirection, orb.SpeedMultiplier, newSpin);
    }

    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }
}