using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class SlashSpell : Spell
{
    public float radius = 3;
    public float power = 2;

    public override void OnCast()
    {
        var delta = Game.Entities.Orb.Position - this.owner.Position;
        if (delta.magnitude < radius)
        {
            Game.Entities.Orb.ForceHit(this.owner, delta, power);
        }
    }

    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }
}