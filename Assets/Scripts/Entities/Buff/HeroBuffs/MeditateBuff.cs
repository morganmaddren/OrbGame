using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class MeditateBuff : HeroBuff
{
    public int manaPerTick;
    public int minionHealthPerTick;

    protected override void OnTick()
    {
        this.entity.Stats.Mana.Value += manaPerTick;
    }
}