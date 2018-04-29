using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISpriteWrapper : IGameEntity
{
    bool Visible { get; set; }
    Color Color { get; set; }
    void Move(Vector2 newPosition);
    Bounds Bounds { get; }
    Vector2 LocalPosition { get; }
    Vector2 Scale { get; set; }
    void Rotate(float rotation);
}

public class SpriteWrapper : GameEntity, ISpriteWrapper
{
    public Vector2 localOffset;

    SpriteRenderer spriteRenderer;

    protected override void OnConstruct()
    {
        spriteRenderer = this.GetComponentStrict<SpriteRenderer>();
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }

    public Bounds Bounds { get { return this.spriteRenderer.bounds; } }
    public Vector2 LocalPosition { get { return transform.localPosition; } }
    public Vector2 Scale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }

    protected override void OnParented(IGameEntity parent)
    {
        transform.localPosition = localOffset;
    }

    public void Move(Vector2 newPosition)
    {
        transform.localPosition = newPosition + localOffset;
    }

    void Start()
    {
    }
	
	void Update()
	{
		
	}

    public bool Visible
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }

    public Color Color
    {
        get { return spriteRenderer.color; }
        set { spriteRenderer.color = value; }
    }

    public void Rotate(float rotation)
    {
        transform.Rotate(Vector3.forward * rotation);
    }
}