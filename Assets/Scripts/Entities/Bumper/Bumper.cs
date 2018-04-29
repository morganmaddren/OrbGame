using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IBumper : IGameEntity
{
    void OnBump(IOrb orb, Vector2 collisionPoint);
    void OnCenterBump(IOrb orb);
}

public abstract class Bumper : GameEntity, IBumper
{
    public virtual void OnBump(IOrb orb, Vector2 collisionPoint) { }
    public virtual void OnCenterBump(IOrb orb) { }
}