using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IGameEntity
{
	Vector2 Position { get; }
    T AddChild<T>(T child, bool resetPosition = false) where T : IGameEntity;
    T AddChild<T>(T child, string name, bool resetPosition = false) where T : IGameEntity;
}

public abstract class GameEntity : MonoBehaviour, IGameEntity
{
    public Vector2 Position { get { return transform.position; } }
    
    public T AddChild<T>(bool resetPosition = false) where T : IGameEntity
    {
        return AddChild<T>(this.GetComponentStrict<T>());
    }

    public T AddChild<T>(T child, bool resetPosition = false) where T : IGameEntity
    {
        if (child == null) throw new ArgumentNullException("child");

        GameEntity childEntity = child as GameEntity;
        var oldpos = childEntity.transform.position;
        childEntity.transform.parent = this.transform;
        if (resetPosition)
            childEntity.transform.localPosition = Vector2.zero;
        else
            childEntity.transform.localPosition = oldpos - transform.position;
        childEntity.OnParented(this);

        return child;
    }

    public T AddChild<T>(T child, string name, bool resetPosition = false) where T : IGameEntity
    {
        if (child == null) throw new ArgumentNullException("child");
        if (name == null) throw new ArgumentNullException("name");

        GameEntity childEntity = child as GameEntity;
        childEntity.name = name;
        childEntity.transform.parent = this.transform;
        if (resetPosition)
            childEntity.transform.localPosition = Vector2.zero;
        childEntity.OnParented(this);

        return child;
    }

    void Awake()
    {
        OnConstruct();
    }

    void Update()
    {
        OnUpdate();
    }

    void Start()
    {
        OnInitialize();
    }

    protected abstract void OnConstruct();
    protected abstract void OnInitialize();
    protected abstract void OnUpdate();

    protected virtual void OnParented(IGameEntity parent) { }
}