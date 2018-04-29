using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class NatureMinion : Minion
{
    protected override void OnPromote()
{
        this.Stats.health.maxValue += 20;
        this.Stats.health.Value += 20;
        this.Stats.attackDamageBase += 2;
    }
}