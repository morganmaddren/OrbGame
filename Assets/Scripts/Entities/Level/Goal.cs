using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IGoal : IGameEntity
{
    HeroSide Side { get; }
}

public class Goal : GameEntity, IGoal
{
    public HeroSide Side { get { return side; } }
    public HeroSide side;

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