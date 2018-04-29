using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IBuff : IGameEntity
{
    void Apply();
    void Dissipate();
}

public abstract class Buff<TEntity, TEntityInterface, TBuffKey>
    : BuffBase, IBuff 
        where TEntity : TEntityInterface 
        where TEntityInterface : IIsBuffable<TBuffKey>
{
    public float tickInterval = 0;
    public float duration = 1;
    public GameObject visualEffect;

    protected TEntityInterface entity;
    protected Timer durationTimer;
    IPersistentEffect effect;
    Timer tickTimer;
    IInteralBuffable manager;

    protected virtual void OnTick() { }
    protected virtual void OnApply() { }
    protected virtual void OnDissipate() { }

    // Override this if different behavior desired
    protected virtual void OnReapply()
    {
        durationTimer.Restart();
    }
    
    public void Initialize(TEntity entity, IInteralBuffable manager)
    {
        this.entity = entity;
        this.manager = manager;

        if (visualEffect != null)
        {
            effect = Game.Objects.CreatePrefab<SimpleEffect>(visualEffect);
            entity.AddChild(effect);
        }
    }

    protected override void OnConstruct()
    {
        tickTimer = new Timer(tickInterval);
        durationTimer = new Timer(duration);
    }

    protected override void OnUpdate()
    {
        if (!this.manager.Enabled)
            return;

        if (tickInterval > 0)
        {
            if (tickTimer.CheckAndTick(Time.deltaTime))
            {
                this.OnTick();
            }
        }

        if (duration > 0)
        {
            if (durationTimer.CheckAndTick(Time.deltaTime))
            {
                manager.Remove(this);
            }
        }
    }

    public void Reapply()
    {
        OnReapply();
    }

    public void Apply()
    {
        OnApply();
    }

    public void Dissipate()
    {
        OnDissipate();

        if (effect != null)
            effect.Dissipate();

        Destroy();
    }

    void Destroy()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }
}