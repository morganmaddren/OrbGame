using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class NatureHero : Hero
{
    protected override void OnHeroUpdate()
    {
        /*if (this.state == HeroState.Standing)
            this.Buffs.ApplyBuff(HeroBuffs.Meditate);
        else
            this.Buffs.RemoveBuff(HeroBuffs.Meditate);*/
    }
    
    public override void OnCastSpell()
    {
        ProjectileFactory.CreateProjectile(Projectiles.Roots, this.Side, this.Position);
    }
}