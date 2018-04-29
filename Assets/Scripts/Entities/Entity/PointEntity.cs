using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IPointEntity : IGameEntity
{
	
}

public class PointEntity : GameEntity, IPointEntity
{
    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }
}