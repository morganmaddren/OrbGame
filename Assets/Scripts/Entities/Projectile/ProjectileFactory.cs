using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum Projectiles
{
    Roots
}

public class ProjectileFactory
{
    static Dictionary<Projectiles, string> projectilePaths = new Dictionary<Projectiles, string>()
    {
        { Projectiles.Roots , "Projectiles/RootsProjectile" },
    };

    public static IProjectile CreateProjectile(Projectiles projectile, HeroSide side, Vector2 position)
    {
        var prefab = Game.Resources.LoadPrefab<Projectile>(projectilePaths[projectile]);
        prefab.Initialize(side, position);

        return prefab;
    }
}