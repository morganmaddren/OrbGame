using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum HeroResources
{
    Mana,
}

public interface IHeroResource
{
    int Value { get; set; }
    int MaxValue { get; }
    bool IsFull { get; }
    bool IsEmpty { get; }
    float ValueAsPercent { get; }
}

[Serializable]
public class HeroResource : IHeroResource
{
    public int startingValue = 100;
    public int maxValue = 100;

    public int MaxValue { get { return maxValue; } }
    public bool IsFull { get { return Value == MaxValue; } }
    public bool IsEmpty { get { return Value == 0; } }
    public float ValueAsPercent { get { return Value / (float)MaxValue; } }

    int value;
    public int Value
    {
        get { return value; }
        set
        {
            this.value = value;
            if (this.value > MaxValue)
                this.value = MaxValue;
            if (this.value < 0)
                this.value = 0;
        }
    }

    public HeroResource Initialize()
    {
        Value = startingValue;
        return this;
    }
    
    public HeroResource()
    {
    }
}