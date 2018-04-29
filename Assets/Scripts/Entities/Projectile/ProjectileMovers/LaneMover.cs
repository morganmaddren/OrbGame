using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class LaneMover : ProjectileMover
{
    IProjectile projectile;
    HeroSide side;
    Vector2 goal;

    public override void Initialize(IProjectile projectile, HeroSide side)
    {
        var pos = (Vector2)transform.position;
        if (side == HeroSide.Top)
            goal = new Vector2(pos.x, Game.Levels.CurrentLevel.BottomGoal.Position.y);
        else
            goal = new Vector2(pos.x, Game.Levels.CurrentLevel.TopGoal.Position.y);

        this.projectile = projectile;
        this.side = side;
    }

	void Update()
	{
        if ((Vector2)transform.position == goal)
            this.projectile.Dissipate();

        Vector2 oldPos = transform.position;
        Vector2 target = Vector2.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        transform.position = target;
    }
}