using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IProjectileMover
{
    void Initialize(IProjectile projectile, HeroSide side);
}

public abstract class ProjectileMover : MonoBehaviour, IProjectileMover
{
    public float speed;

    public abstract void Initialize(IProjectile projectile, HeroSide side);
}