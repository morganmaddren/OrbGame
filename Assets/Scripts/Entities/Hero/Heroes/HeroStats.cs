using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
public class HeroStats
{
    public float moveSpeedBase = 2.5f;
    public float moveSpeedMultiplier = 1;
    public float accelerationBase = 50;
    public float deaccelerationBase = 50;

    public HeroResource UltimateMeter;
    public int ultimateChargeRate = 0;

    public HeroResource Level;

    public HeroResource Mana;
    public int manaRegen = 0;

    public int spellManaCost = 10;
    public int spellCooldown = 5;

    public float orbSpeedModifier = 1;

    public int rootedCount = 0;
    public int stunnedCount = 0;

    public float MoveSpeed { get { return moveSpeedBase * moveSpeedMultiplier; } }

    public bool Rooted { get { return rootedCount > 0; } }
    public bool Stunned { get { return stunnedCount > 0; } }

    public void Initialize()
    {
        UltimateMeter.Initialize();
        Level.Initialize();
        Mana.Initialize();
    }
}
