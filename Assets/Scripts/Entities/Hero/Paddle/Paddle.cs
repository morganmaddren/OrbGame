using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IPaddle : IGameEntity
{
    IHeroParent Hero { get; }
}

public class Paddle : GameEntity, IPaddle
{
    public Color defaultColor;
    public Color activeColor;
    public Color cooldownColor;
    public float range;
    
    public float minSlope;
    public float touchVelocityMax;

    public float spinMinCutoff;
    public float spinMaxCutoff;
    public float spinCoefficient;

    Hero hero;
    public IHeroParent Hero { get { return hero; } }

    BoxCollider2D boxCollider;
    ISpriteWrapper sprite;
    IOrb orb;

    enum PaddleState
    {
        Normal,
        Cooldown,
        Aiming
    }
    EntityStateMachine<PaddleState, Paddle> stateMachine;
    
    protected override void OnConstruct()
    {
        boxCollider = this.GetComponentStrict<BoxCollider2D>();
    }

    protected override void OnInitialize()
    {
        sprite = AddChild(this.GetComponentStrict<SpriteWrapper>(), resetPosition: true);

        stateMachine = new EntityStateMachine<PaddleState, Paddle>(this);
        stateMachine.RegisterInitialState(PaddleState.Normal, new NormalState());
        stateMachine.RegisterState(PaddleState.Cooldown, new CooldownState());
        stateMachine.RegisterState(PaddleState.Aiming, new AimingState());
        
        if (Hero.Side == HeroSide.Top)
        {
            sprite.Scale = new Vector2(1, -1);
        }

        if (Hero.Side == HeroSide.Top)
        {
            transform.localPosition = new Vector3(0, -1f);
        }
        else
            transform.localPosition = new Vector3(0, 1f);
    }

    private class NormalState : EntityState<Paddle>
    {
        public override void OnEnter(Paddle paddle)
        {
            paddle.sprite.Color = paddle.defaultColor;
        }
    }

    private class AimingState : EntityState<Paddle>
    {
        Timer timer;

        public override void Initialize(Paddle paddle)
        {
            timer = new Timer(.75f);
        }

        public override void OnEnter(Paddle paddle)
        {
            paddle.orb.ForceHit(paddle.orb.Direction, .25f);
            paddle.Hero.StartAiming();
            paddle.sprite.Color = paddle.activeColor;
        }

        public override void OnUpdate(Paddle paddle)
        {
            if (timer.CheckAndTick(Time.deltaTime))
            {
                paddle.HitOrb(paddle.orb);
            }
        }
    }

    private class CooldownState : EntityState<Paddle>
    {
        Timer timer;

        public override void Initialize(Paddle paddle)
        {
            timer = new Timer(.75f);
        }

        public override void OnEnter(Paddle paddle)
        {
            paddle.sprite.Color = paddle.cooldownColor;
            paddle.Hero.StopAiming();
            timer.Restart();
        }

        public override void OnUpdate(Paddle paddle)
        {
            if (timer.CheckAndTick(Time.deltaTime))
            {
                paddle.stateMachine.SetState(PaddleState.Normal);
            }
        }
    }

    protected override void OnParented(IGameEntity parent)
    {
        hero = (Hero)parent;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IOrb orb = collision.AsOrb();
        if (orb != null && stateMachine.CurrentStateKey == PaddleState.Normal)
        {
            StartAim(orb);
        }
    }

    private void StartAim(IOrb orb)
    {
        this.orb = orb;
        stateMachine.SetState(PaddleState.Aiming);
    }

    private void HitOrb(IOrb orb)
    {
        var hitVector = new Vector2(0, 1);
        if (hero.Side == HeroSide.Top)
            hitVector = new Vector2(0, -1);
        
        var hitSpeed = 0f;
        var velocity = hero.AimVector - hero.Position;
        hitSpeed = Mathf.Clamp(velocity.magnitude / touchVelocityMax, 0, 1);

        hitVector = hitVector * (1 - hitSpeed) + hitSpeed * velocity.normalized;
        if (hero.Side == HeroSide.Top && hitVector.y > 0 || hero.Side == HeroSide.Bottom && hitVector.y < 0)
            hitVector = new Vector2(hitVector.x, -hitVector.y);

        var slope = hitVector.x / hitVector.y;
        if (slope > minSlope)
        {
            hitVector = new Vector2(minSlope, Math.Sign(hitVector.y));
        }
        else if (slope < -minSlope)
        {
            hitVector = new Vector2(-minSlope, Math.Sign(hitVector.y));
        }

        var spin = MathHelper.ShortestVector(orb.Position, hero.AimVector, hero.AimSpinVector + hero.AimVector);
        var spinMagnitude = spin.magnitude;
        if (spinMagnitude < spinMinCutoff)
        {
            spinMagnitude = 0;
        }
        else
        {
            spinMagnitude = Mathf.Clamp(spinMagnitude, spinMinCutoff, spinMaxCutoff) * spinCoefficient;
            spinMagnitude *= Math.Sign(MathHelper.Determinant(orb.Position, hero.AimVector, hero.AimSpinVector + hero.AimVector));
        }

        Debug.Log("HitOrb! hitSpeed: " + hitSpeed + " dirX: " + hitVector.x + " dirY: " + hitVector.y + " spin: " + spinMagnitude);
        orb.ForceHit(hero, hitVector, 1f, spinMagnitude); 
        
        stateMachine.SetState(PaddleState.Cooldown);
        hero.HitOrb();
    }

    protected override void OnUpdate()
    {
        stateMachine.Update();
    }
}