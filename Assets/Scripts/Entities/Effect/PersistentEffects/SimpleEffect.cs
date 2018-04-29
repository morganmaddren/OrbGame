using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum TransitionStyle
{
    Fade,
    Grow
}

public enum PersistStyle
{
    Fade,
    Grow,
    Wiggle
}

enum EffectState
{
    Entering,
    Persisting,
    Dissipating,
}

public class SimpleEffect : PersistentEffect
{
    public TransitionStyle enterTransitionStyle;
    public float enterTransitionDuration = .5f;

    public TransitionStyle exitTransitionStyle;
    public float exitTransitionDuration = .5f;

    public PersistStyle persistStyle;
    public float persistIntensity = .25f;
    public float persistOffset = .75f;
    public float persistFrequency = 1f;
    public float persistDuration = 0;

    ISpriteWrapper renderer;
    Timer enterTimer;
    Timer exitTimer;
    Timer persistTimer;
    float curTime;
    EffectState state;

    Vector2 maxScale;

    protected override void OnConstruct()
    {
        renderer = this.GetComponentStrict<SpriteWrapper>();
        enterTimer = new Timer(enterTransitionDuration);
        exitTimer = new Timer(exitTransitionDuration);
        if (persistDuration > 0)
            persistTimer = new Timer(persistDuration);
        state = EffectState.Entering;
        maxScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }

    protected override void OnInitialize()
    {
        UpdateTransitionStyle(enterTransitionStyle, 0);
    }
    
    protected override void OnParented(IGameEntity parent)
    {
        parent.AddChild(renderer);
    }

    protected override void OnUpdate()
    {
        switch (state)
        {
            case EffectState.Entering:
                UpdateEnter();
                break;
            case EffectState.Persisting:
                UpdatePersist();
                break;
            case EffectState.Dissipating:
                UpdateExit();
                break;
        }
    }
    
    void UpdateEnter()
    {
        float percent;
        if (enterTimer.CheckAndTick(Time.deltaTime))
        {
            state = EffectState.Persisting;
            percent = 1;
        }
        else
        {
            percent = enterTimer.Elapsed / enterTimer.Duration;
        }

        UpdateTransitionStyle(enterTransitionStyle, percent);
    }

    void UpdatePersist()
    {
        if (persistDuration > 0 && persistTimer.CheckAndTick(Time.deltaTime))
            state = EffectState.Dissipating;

        float value = (float)(Math.Cos(curTime * persistFrequency * 2f * Math.PI) * persistIntensity + persistOffset);

        if (persistStyle == PersistStyle.Fade)
            renderer.Color = new Color(1, 1, 1, value);
        else if (persistStyle == PersistStyle.Grow)
            transform.localScale = new Vector3(value * maxScale.x, value * maxScale.y);
        else if (persistStyle == PersistStyle.Wiggle)
            transform.localPosition = new Vector3(0, value);

        curTime += Time.deltaTime;
    }

    void UpdateExit()
    {
        if (exitTimer.CheckAndTick(Time.deltaTime))
        {
            Destroy(this.gameObject);
        }

        float percent = exitTimer.Elapsed / exitTimer.Duration;
        UpdateTransitionStyle(exitTransitionStyle, 1 - percent);
    }

    void UpdateTransitionStyle(TransitionStyle style, float value)
    {
        if (style == TransitionStyle.Fade)
            renderer.Color = new Color(1, 1, 1, value);
        else if (style == TransitionStyle.Grow)
            transform.localScale = new Vector3(value * maxScale.x, value * maxScale.y);
    }

    void ResetSprite()
    {
        renderer.Color = new Color(1, 1, 1, 1);
        transform.localScale = maxScale;
        transform.localPosition = Vector3.zero;
    }

    public override void Dissipate()
    {
        ResetSprite();
        this.state = EffectState.Dissipating;
    }
}