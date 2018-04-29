using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IPersistentEffect : IGameEntity
{
    void Dissipate();
}

public abstract class PersistentEffect : GameEntity, IPersistentEffect
{
    public abstract void Dissipate();
}