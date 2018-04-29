using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IWaypoint : IGameEntity
{
    void MoveTo(Vector2 position);
    void Hide();
}

public class Waypoint : GameEntity, IWaypoint
{
    ISpriteWrapper wrapper;

    public void MoveTo(Vector2 position)
    {
        this.wrapper.Visible = true;
        this.transform.position = position;
    }

    public void Hide()
    {
        this.wrapper.Visible = false;
    }

    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
        wrapper = this.AddChild(this.GetComponentInChildren<SpriteWrapper>(), resetPosition: false);
        wrapper.Visible = false;
    }

    protected override void OnUpdate()
    {
    }
}