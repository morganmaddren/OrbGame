using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class NatureOrbBehavior : OrbBehavior
{
    public override void OnHitAllyMinion(IMinion minion)
    {
        minion.Buffs.ApplyBuff(MinionBuffs.Regrow);
        minion.Promote();
    }
}