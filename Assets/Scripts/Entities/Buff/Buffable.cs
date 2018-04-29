using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IIsBuffable<TBuffKey> : IGameEntity
{
    IBuffable<TBuffKey> Buffs { get; }
}

public interface IBuffable<TBuffKey>
{
    bool Enabled { get; }

    void ApplyBuff(TBuffKey buff);
    void RemoveBuff(TBuffKey buff);
    void RemoveAllBuffs();
    
    IBuff GetBuff(TBuffKey key);
}

public interface IInteralBuffable
{
    bool Enabled { get; }
    void Remove(BuffBase buff);
}

public class Buffable<TBuffKey, TEntity, TEntityInterface>
    : IBuffable<TBuffKey>, IInteralBuffable
        where TEntity : MonoBehaviour, TEntityInterface
        where TEntityInterface : IIsBuffable<TBuffKey>
{
    TEntity entity;
    Dictionary<TBuffKey, Buff<TEntity, TEntityInterface, TBuffKey>> buffs;
    
    public bool Enabled { get; set; }
    public int BuffCount { get { return buffs.Count; } }

    public Buffable(TEntity entity)
    {
        this.entity = entity;
        buffs = new Dictionary<TBuffKey, Buff<TEntity, TEntityInterface, TBuffKey>>();
        Enabled = false;
    }

    public void ApplyBuff(TBuffKey buff)
    {
        if (buffs.ContainsKey(buff))
            buffs[buff].Reapply();
        else
        {
            var buffObj = (Buff<TEntity, TEntityInterface, TBuffKey>)BuffFactory.CreateBuff(buff);
            entity.AddChild(buffObj);
            buffObj.Initialize(entity, this);
            buffObj.Apply();


            buffs[buff] = buffObj;
        }
    }

    public IBuff GetBuff(TBuffKey key)
    {
        if (!buffs.ContainsKey(key))
            return null;

        return buffs[key];
    }

    public void RemoveAllBuffs()
    {
        TBuffKey[] keys = new TBuffKey[buffs.Count];
        buffs.Keys.CopyTo(keys, 0);
        for (int i = 0; i < keys.Length; i++)
        {
            RemoveBuff(keys[i]);
        }
    }

    public void RemoveBuff(TBuffKey buff)
    {
        if (buffs.ContainsKey(buff))
        {
            var buffObj = buffs[buff];
            buffs.Remove(buff);
            buffObj.Dissipate();
        }
    }

    public void Remove(BuffBase buff)
    {
        foreach (KeyValuePair<TBuffKey, Buff<TEntity, TEntityInterface, TBuffKey>> kvp in buffs)
        {
            if (buff == kvp.Value)
            {
                RemoveBuff(kvp.Key);
                break;
            }
        }
    }
}