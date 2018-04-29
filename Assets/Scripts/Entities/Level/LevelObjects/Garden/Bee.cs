using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IBee : IBumper
{
	
}

public class Bee : Bumper, IBee
{
    public int points;
    public float speed;
    public SimpleEffect onHitEffect;

    IBeeManager manager;
    Vector2 waypoint;

    enum BeeState
    {
        Idle,
        Moving,
        Hit
    }
    EntityStateMachine<BeeState, Bee> stateMachine;

    public override void OnBump(IOrb orb, Vector2 collisionPoint)
    {
        if (orb.Owner != null)
            orb.Owner.GainUltCharge(points, Position);

        manager.OnBeeHit(this);
        stateMachine.SetState(BeeState.Hit);
    }

    public void Initialize(IBeeManager manager, Vector2 position)
    {
        this.manager = manager;
        this.transform.position = position;
    }

    protected override void OnConstruct()
    {
        stateMachine = new EntityStateMachine<BeeState, Bee>(this);
        stateMachine.RegisterInitialState(BeeState.Idle, new IdleState());
        stateMachine.RegisterState(BeeState.Moving, new MovingState());
        stateMachine.RegisterState(BeeState.Hit, new HitState());
    }
    
    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
        stateMachine.Update();
    }

    private class IdleState : EntityState<Bee>
    {
        public override void OnEnter(Bee bee)
        {
        }

        public override void OnLeave(Bee bee)
        {
        }

        public override void OnPreUpdate(Bee bee)
        {
            var x = UnityEngine.Random.Range(bee.manager.Bounds.min.x, bee.manager.Bounds.max.x);
            var y = UnityEngine.Random.Range(bee.manager.Bounds.min.y, bee.manager.Bounds.max.y);

            bee.waypoint = new Vector2(x, y);
            bee.stateMachine.SetState(BeeState.Moving);
        }

        public override void OnUpdate(Bee bee)
        {
        }
    }

    private class MovingState : EntityState<Bee>
    {
        public override void OnEnter(Bee bee)
        {
        }

        public override void OnLeave(Bee bee)
        {
        }

        public override void OnPreUpdate(Bee bee)
        {
            if (bee.Position == bee.waypoint)
                bee.stateMachine.SetState(BeeState.Idle);
        }

        public override void OnUpdate(Bee bee)
        {
            bee.transform.position = Vector2.MoveTowards(bee.transform.position, bee.waypoint, bee.speed * Time.deltaTime);
        }
    }

    private class HitState : EntityState<Bee>
    {
        public override void OnEnter(Bee bee)
        {
            bee.Kill();
        }

        public override void OnLeave(Bee bee)
        {
        }

        public override void OnPreUpdate(Bee bee)
        {
        }

        public override void OnUpdate(Bee bee)
        {
        }
    }

    void Kill()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}