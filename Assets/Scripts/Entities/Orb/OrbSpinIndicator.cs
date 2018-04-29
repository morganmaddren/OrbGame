using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IOrbSpinIndicator : IGameEntity
{
    void Spin(float spin);
}

public class OrbSpinIndicator : GameEntity, IOrbSpinIndicator
{
    public float rotationSpeed;
    public float opacityScale;
    public float sizeScale;

    IOrb orb;
    float spin;
    ISpriteWrapper sprite;

    public void Spin(float spin)
    {
        this.spin = spin;
        var c = opacityScale * Math.Abs(spin);
        var s = sizeScale * Math.Abs(spin);
        sprite.Color = new Color(1, 1, 1, Math.Min(c, 1));
        sprite.Scale = new Vector2(.75f + s, .75f + s);
    }

    protected override void OnConstruct()
    {
    }

    protected override void OnParented(IGameEntity parent)
    {
        orb = (IOrb)parent;
    }

    protected override void OnInitialize()
    {
        sprite = AddChild<SpriteWrapper>(resetPosition: true);
        sprite.Color = new Color(0, 0, 0, 0);
    }

    protected override void OnUpdate()
    {
        sprite.Rotate(spin * Time.deltaTime * rotationSpeed);
    }
}