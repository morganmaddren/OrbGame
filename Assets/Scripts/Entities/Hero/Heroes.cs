using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum Heroes
{
    NatureHero,
    WolfHero,
    PrincessHero
}

public class HeroFactory
{
    static Dictionary<Heroes, string> heroesPaths = new Dictionary<Heroes, string>()
    {
        { Heroes.NatureHero , "Heroes/NatureHero" },
        { Heroes.WolfHero , "Heroes/WolfHero" },
        { Heroes.PrincessHero , "Heroes/PrincessHero" },
    };

    public static IHero CreateHero(GameObject entityRoot, Heroes hero, HeroSide side, Vector2 position)
    {
        var prefab = Game.Resources.LoadPrefab<Hero>(heroesPaths[hero]);
        entityRoot.AddChildEntity(prefab);
        prefab.Initialize(side, position);

        return prefab;
    }
}