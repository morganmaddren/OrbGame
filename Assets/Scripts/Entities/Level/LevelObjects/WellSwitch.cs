using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum WellSwitchState
{
    Inactive,
    Neutral,
    Top,
    Bottom
}

public interface IWellSwitch
{
    WellSwitchState State { get; }
    void Activate();
    void Deactivate();
}

public class WellSwitch : GameEntity, IWellSwitch
{
    public GameObject activeEffect;
    IPersistentEffect effect;

    public WellSwitchState State { get; private set; }

    ISpriteWrapper blueSprite;
    ISpriteWrapper redSprite;
    ISpriteWrapper neutralSprite;

    protected override void OnConstruct()
    {
        blueSprite = transform.Find("bluedot").GetComponentStrict<SpriteWrapper>();
        redSprite = transform.Find("reddot").GetComponentStrict<SpriteWrapper>();
        neutralSprite = transform.Find("greydot").GetComponentStrict<SpriteWrapper>();
    }

    public void Activate()
    {
        if (this.State != WellSwitchState.Inactive)
            return;

        SetState(WellSwitchState.Neutral);
        effect = Game.Objects.CreatePrefab<SimpleEffect>(activeEffect);
        this.AddChild(effect);
    }

    public void Deactivate()
    {
        if (this.State == WellSwitchState.Inactive)
            return;

        SetState(WellSwitchState.Inactive);
        effect.Dissipate();
        effect = null;
    }

    protected override void OnInitialize()
    {
        SetState(WellSwitchState.Inactive);
    }

    protected override void OnUpdate()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var minion = collision.AsMinion();
        if (minion != null)
        {
            MinionStep(minion);
        }
    }

    void MinionStep(IMinion minion)
    {
        if (minion.Owner.Side == HeroSide.Top)
            SetState(WellSwitchState.Top);
        else
            SetState(WellSwitchState.Bottom);
    }

    private void SetState(WellSwitchState state)
    {
        switch (state)
        {
            case WellSwitchState.Inactive:
                redSprite.Visible = false;
                blueSprite.Visible = false;
                neutralSprite.Visible = true;
                break;
            case WellSwitchState.Neutral:
                redSprite.Visible = false;
                blueSprite.Visible = false;
                neutralSprite.Visible = true;
                break;
            case WellSwitchState.Top:
                redSprite.Visible = true;
                blueSprite.Visible = false;
                neutralSprite.Visible = false;
                break;
            case WellSwitchState.Bottom:
                redSprite.Visible = false;
                blueSprite.Visible = true;
                neutralSprite.Visible = false;
                break;
        }

        State = state;
    }
}