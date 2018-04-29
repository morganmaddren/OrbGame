using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using UnityObject = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;

public interface ILevel
{
    Vector2 OrbSpawn { get; }
    Vector2 TopHeroSpawn { get; }
    Vector2 BottomHeroSpawn { get; }
    Vector2 GetHeroSpawn(HeroSide side);

    IGoal TopGoal { get; }
    IGoal BottomGoal { get; }
    IGoal GetGoal(HeroSide side);
    IGoal GetOpponentGoal(HeroSide side);
}

public class Level : MonoBehaviour, ILevel
{
    public Vector2 OrbSpawn { get; private set; }
    public Vector2 TopHeroSpawn { get; private set; }
    public Vector2 BottomHeroSpawn { get; private set; }
    
    public IGoal TopGoal { get; private set; }
    public IGoal BottomGoal { get; private set; }

    void Awake()
    {
        this.TopGoal = transform.Find("GoalTop").GetComponentStrict<Goal>();
        this.BottomGoal = transform.Find("GoalBottom").GetComponentStrict<Goal>();

        var spawnTransform = transform.Find("SpawnPositions");
        this.OrbSpawn = spawnTransform.Find("OrbSpawn").transform.position;
        this.TopHeroSpawn = spawnTransform.Find("TopHeroSpawn").transform.position;
        this.BottomHeroSpawn = spawnTransform.Find("BottomHeroSpawn").transform.position;
    }

    public IGoal GetGoal(HeroSide side)
    {
        if (side == HeroSide.Bottom)
            return BottomGoal;
        return TopGoal;
    }

    public Vector2 GetHeroSpawn(HeroSide side)
    {
        if (side == HeroSide.Bottom)
            return BottomHeroSpawn;
        return TopHeroSpawn;
    }

    public IGoal GetOpponentGoal(HeroSide side)
    {
        if (side == HeroSide.Top)
            return BottomGoal;
        return TopGoal;
    }

    void Start()
	{
        
	}
	
	void Update()
	{
		
	}
}