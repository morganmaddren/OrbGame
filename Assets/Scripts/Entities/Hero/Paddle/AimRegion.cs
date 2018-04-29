using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IAimRegion : IGameEntity
{
    void Clear();
}

public class AimRegion : GameEntity, IAimRegion
{
    public Color defaultColor;
    public Color activeColor;

    IPaddle paddle;
    
    RaycastHit2D[] collisionBuffer = new RaycastHit2D[5];
    ContactFilter2D collisionFilter;

    ISpriteWrapper sprite;
    
    enum AimRegionState
    {
        Normal,
        Cooldown
    }
    EntityStateMachine<AimRegionState, AimRegion> stateMachine;
    
    protected override void OnConstruct()
    {
        collisionFilter = new ContactFilter2D();
        var layerMask = new LayerMask();
        layerMask.value |= 1 << 9;
        collisionFilter.SetLayerMask(layerMask);
    }

    protected override void OnInitialize()
    {
        sprite = AddChild(this.GetComponentStrict<SpriteWrapper>(), resetPosition: true);
        sprite.Color = this.defaultColor;

        stateMachine = new EntityStateMachine<AimRegionState, AimRegion>(this);
        stateMachine.RegisterInitialState(AimRegionState.Normal, new NormalState());
        stateMachine.RegisterState(AimRegionState.Cooldown, new CooldownState());

        if (paddle.Hero.Side == HeroSide.Top)
        {
            transform.localPosition = new Vector3(0, -6);
        }
        else
            transform.localPosition = new Vector3(0, 6);
    }

    protected override void OnParented(IGameEntity parent)
    {
        paddle = (IPaddle)parent;
    }

    private class NormalState : EntityState<AimRegion>
    {
        public override void OnEnter(AimRegion aimRegion)
        {
            aimRegion.paddle.Hero.StopAiming();
            aimRegion.sprite.Color = aimRegion.defaultColor;
        }
    }

    private class CooldownState : EntityState<AimRegion>
    {
        public override void OnEnter(AimRegion aimRegion)
        {
            aimRegion.paddle.Hero.StartAiming();
            aimRegion.sprite.Color = aimRegion.activeColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IOrb orb = collision.AsOrb();
        if (orb != null)
        {
            HitOrb(orb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IOrb orb = collision.AsOrb();
        if (orb != null)
        {
            stateMachine.SetState(AimRegionState.Normal);
        }
    }

    void HitOrb(IOrb orb)
    {
        stateMachine.SetState(AimRegionState.Cooldown);

        /*var futureOrbPos = orb.Position + orb.Direction * 2;

        int collisions = Physics2D.RaycastNonAlloc(orb.Position, orb.Direction, collisionBuffer, 3, collisionFilter.layerMask);
        for (int i = 0; i < collisions; i++)
        {
            IPaddle collidedPaddle = collisionBuffer[i].collider.GetComponent<Paddle>();
            if (collidedPaddle != null && collidedPaddle == this.paddle)
            {
            }
        }*/
    }

    public void Clear()
    {
        stateMachine.SetState(AimRegionState.Normal);
    }

    protected override void OnUpdate()
    {
    }
}