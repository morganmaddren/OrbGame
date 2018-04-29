using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class AIController : HeroController
{
    public float maxSpeed;

    protected override void OnAttachHero(IHero hero)
    {

    }

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
        if (hero == null)
            return;

        var delta = Game.Entities.Orb.Position.x - hero.Position.x;
        if (Math.Abs(delta) > .125 && !hero.Aiming)
        {
            hero.MoveTo(new Vector2(Game.Entities.Orb.Position.x, hero.Position.y));
        }

        Vector2 aim = new Vector2(UnityEngine.Random.Range(1.5f, 7.5f), UnityEngine.Random.Range(5.5f, 10.5f));
        hero.Aim(aim, new Vector2(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3)));
    }
}