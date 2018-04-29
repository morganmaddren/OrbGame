using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ICollectionEntity<T> : IGameEntity where T : IGameEntity
{
	
}

public class CollectionEntity<T> : GameEntity, ICollectionEntity<T> where T : IGameEntity
{
    public List<T> Entities { get; private set; }

    protected override void OnConstruct()
    {
        Entities = new List<T>();
    }

    protected override void OnInitialize()
    {
        Entities.Clear();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var t = this.transform.GetChild(i);
            var entity = t.GetComponentStrict<T>(); // does this work through interface?

            this.AddChild(entity, resetPosition: false);
            Entities.Add(entity);
        }
    }

    protected override void OnUpdate()
    {
    }
}