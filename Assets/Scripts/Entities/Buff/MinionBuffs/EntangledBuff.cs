using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class EntangledBuff : MinionBuff
{
    public int damagePerTick;

    protected override void OnApply()
    {
        entity.Stats.rootedCount++;
    }

    protected override void OnTick()
    {
        entity.TakeDamage(damagePerTick);
    }

    protected override void OnDissipate()
    {
        entity.Stats.rootedCount--;
    }
}