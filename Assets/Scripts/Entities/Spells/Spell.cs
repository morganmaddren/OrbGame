using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISpell
{
    void Cast();
}

public abstract class Spell : GameEntity, ISpell
{
    public float cooldown;

    protected IHero owner;

    protected override void OnParented(IGameEntity parent)
    {
        this.owner = (IHero)parent;
    }
    
    public void Cast()
    {
        OnCast();
    }

    public abstract void OnCast();
}