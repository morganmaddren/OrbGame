using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class RootsProjectile : Projectile
{
    protected override void OnHitEnemyMinion(IMinion minion)
    {
        minion.Buffs.ApplyBuff(MinionBuffs.Entangled);
    }
}