using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum Minions
{
    NatureMinion,
    WolfMinion,
    NeutralOwlBear,
}

public class MinionFactory
{
    static Dictionary<Minions, string> minionPaths = new Dictionary<Minions, string>()
    {
        { Minions.NatureMinion , "Minions/NatureMinion" },
        { Minions.WolfMinion , "Minions/WolfMinion" },
        { Minions.NeutralOwlBear , "Minions/Neutral/NeutralOwlBearMinion" },
    };

    public static IMinion CreateMinion(GameObject entityRoot, Minions minion, IHero owner, Vector2 position)
    {
        var prefab = Game.Resources.LoadPrefab<Minion>(minionPaths[minion]);
        entityRoot.AddChild(prefab);
        prefab.Initialize(owner, position);

        return prefab;
    }
}