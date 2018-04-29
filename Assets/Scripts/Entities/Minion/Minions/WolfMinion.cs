using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class WolfMinion : Minion
{
    protected override void OnKillEnemyMinion(IMinion minion)
    {
        if (minion.Buffs.GetBuff(MinionBuffs.HuntersMark) != null)
            this.Promote();
    }

    protected override void OnPromote()
    {
        this.stats.health.maxValue += 30;
        this.stats.attackDamageBase += 4;
        this.stats.goalPoints++;
    }
}