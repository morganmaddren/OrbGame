using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IProjectile : IGameEntity
{
    void Dissipate();
}

public class Projectile : GameEntity, IProjectile
{
    public GameObject sprite;

    protected virtual void OnHitAllyMinion(IMinion minion) { }
    protected virtual void OnHitEnemyMinion(IMinion minion) { }
    protected virtual void OnProjectileUpdate() { }
    protected virtual void OnCreate() { }
    protected virtual void OnRemoved() { }

    HeroSide side;
    IProjectileMover mover;
    ISpriteWrapper spriteWrapper;

    public void Initialize(HeroSide side, Vector2 position)
    {
        this.side = side;
        this.transform.position = position;

        if (mover != null)
            mover.Initialize(this, side);

        OnCreate();
    }

    protected override void OnConstruct()
    {
        mover = this.GetComponent<ProjectileMover>();
    }

    protected override void OnInitialize()
    {
        spriteWrapper = this.AddChild(Game.Objects.CreatePrefab<SpriteWrapper>(sprite));
    }

    protected override void OnUpdate()
    {
        OnProjectileUpdate();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IMinion minion = collision.gameObject.GetComponent<Minion>();
        if (minion != null)
        {
            if (minion.Owner.Side == side)
                this.OnHitAllyMinion(minion);
            else
                this.OnHitEnemyMinion(minion);
        }
    }

    public void Dissipate()
    {
        OnRemoved();

        Destroy();
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}