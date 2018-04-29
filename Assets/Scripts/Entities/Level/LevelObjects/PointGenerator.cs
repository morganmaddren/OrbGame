using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IPointGenerator : IGameEntity
{
    PointGeneratorState State { get; }
    void ResetCooldown();
}

public enum PointGeneratorState
{
    Full,
    Cooldown,
}

public class PointGenerator : Bumper, IPointGenerator
{
    public float initialCooldown;
    public float cooldown;
    public float cooldownRandomModifier;

    public int points;
    public Sprite fullSprite;
    public Sprite cooldownSprite;
    public SimpleEffect onHitEffect;
    public float growDuration;

    public PointGeneratorState State { get; private set; }

    Timer cooldownTimer;
    float durationTime;
    SpriteRenderer spriteRenderer;
    Vector2 initialSpriteScale;

    public virtual void OnFullBump(IOrb orb, Vector2 collisionPoint) { }

    public override void OnBump(IOrb orb, Vector2 collisionPoint)
    {
        if (State != PointGeneratorState.Full)
            return;

        ResetCooldown();

        if (orb.Owner != null)
        {
            orb.Owner.GainUltCharge(points, Position);
        }

        if (onHitEffect != null)
        {
            this.AddChild(Game.Objects.CreatePrefab<SimpleEffect>(onHitEffect.gameObject));
        }
        
        OnFullBump(orb, collisionPoint);
    }

    public void ResetCooldown()
    {
        State = PointGeneratorState.Cooldown;
        cooldownTimer.Restart();
        cooldownTimer.Duration = cooldown + UnityEngine.Random.Range(-cooldownRandomModifier, cooldownRandomModifier);
        spriteRenderer.sprite = cooldownSprite;
    }

    protected override void OnConstruct()
    {
        this.spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        initialSpriteScale = spriteRenderer.transform.localScale;
        spriteRenderer.transform.localScale = new Vector3(0, 0, 0);
        this.State = PointGeneratorState.Cooldown;
        cooldownTimer = new Timer(initialCooldown);
        spriteRenderer.sprite = cooldownSprite;
    }

    protected override void OnUpdate()
    {
        if (Game.Match.State != MatchState.Playing)
            return;

        if (State == PointGeneratorState.Cooldown)
        {
            durationTime = 0;
            if (cooldownTimer.CheckAndTick(Time.deltaTime))
            {
                State = PointGeneratorState.Full;
                spriteRenderer.transform.localScale = new Vector3(0, 0, 0);
                spriteRenderer.sprite = fullSprite;
            }
        }
        else if (State == PointGeneratorState.Full)
        {
            durationTime += Time.deltaTime;
            float size = Mathf.Clamp(durationTime / growDuration, 0, 1);
            spriteRenderer.transform.localScale = new Vector2(size * initialSpriteScale.x, size * initialSpriteScale.y);
        }
    }

    protected override void OnInitialize()
    {
    }
}