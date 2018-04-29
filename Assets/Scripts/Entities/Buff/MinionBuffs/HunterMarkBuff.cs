using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class HunterMarkBuff : MinionBuff
{
    protected override void OnApply()
    {
        this.entity.Stats.incomingDamageMultiplier += .25f;
    }

    protected override void OnDissipate()
    {
        this.entity.Stats.incomingDamageMultiplier -= .25f;

        if (!entity.IsAlive)
        {
            Game.Entities.GetHero(HeroHelpers.GetOppositeSide(entity.Owner.Side)).Stats.Mana.Value++;
        }
    }
}