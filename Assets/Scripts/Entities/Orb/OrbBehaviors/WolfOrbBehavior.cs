using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class WolfOrbBehavior : OrbBehavior
{
    public override void OnHitEnemyMinion(IMinion minion)
    {
        minion.Buffs.ApplyBuff(MinionBuffs.HuntersMark);
    }
}