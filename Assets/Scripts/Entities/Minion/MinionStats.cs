using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

[Serializable]
public class MinionStats
{
    public HeroResource health;
    public float incomingDamageMultiplier = 1;

    public int attackDamageBase = 10;
    public float attackRangeBase = .75f;
    public float attackChargeTimeBase = .25f;
    public float attackCooldownBase = .775f;
    public float attackDamageMultiplier = 1;
    public float attackSpeedMultiplier = 1;

    public float moveSpeedBase = .75f;
    public float moveSpeedMultiplier = 1;

    public int goalPoints = 1;
    public int maxLevel = 3;

    public int rootedCount = 0;
    public int stunnedCount = 0;
    
    public int AttackDamage { get { return (int)Math.Round(attackDamageBase * attackDamageMultiplier); } }
    public float AttackCooldown { get { return (attackChargeTimeBase + attackCooldownBase) / attackSpeedMultiplier - attackChargeTimeBase; } }
    public float MoveSpeed { get { return moveSpeedBase * moveSpeedMultiplier; } }

    public bool Rooted { get { return rootedCount > 0; } }
    public bool Stunned { get { return stunnedCount > 0; } }

    public float IncomingDamageMultiplier { get { return Mathf.Clamp(incomingDamageMultiplier, .25f, 1.5f); } }
}