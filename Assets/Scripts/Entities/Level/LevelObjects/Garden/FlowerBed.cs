using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IFlowerBed
{
    void OnFlowerHit(ICenterRose rose, IOrb orb);
}

public class FlowerBed : GameEntity, IFlowerBed
{
    public int pointsForFullClear;

    List<ICenterRose> flowers;
    Timer delayTimer;
    bool scoring;
    IHero scoringPlayer;

    protected override void OnConstruct()
    {
        flowers = new List<ICenterRose>();
        delayTimer = new Timer(.5f);
        scoring = false;
        foreach (var rose in this.GetComponentsInChildren<CenterRose>())
        {
            rose.Initialize(this);
            flowers.Add(rose);
        }
    }

    protected override void OnInitialize()
    {
    }
    
    public void OnFlowerHit(ICenterRose rose, IOrb orb)
    {
        bool allClear = true;
        foreach (var flower in flowers)
        {
            if (flower.State == PointGeneratorState.Full)
            {
                allClear = false;
                break;
            }
        }

        if (allClear)
        {
            if (orb.Owner != null)
            {
                scoring = true;
                scoringPlayer = orb.Owner;
                foreach (var flower in flowers)
                {
                    flower.ResetCooldown();
                }
            }
        }
    }

    protected override void OnUpdate()
    {
        if (scoring)
        {
            if (delayTimer.CheckAndTick(Time.deltaTime))
            {
                delayTimer.Restart();
                scoring = false;
                scoringPlayer.GainUltCharge(pointsForFullClear, Position);
            }
        }
    }
}