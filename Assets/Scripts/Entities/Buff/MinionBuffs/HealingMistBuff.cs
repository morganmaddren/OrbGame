using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class HealingMistBuff : MinionBuff
{
    public int healPerTick;

    protected override void OnTick()
    {
        this.entity.Heal(healPerTick);
    }
}