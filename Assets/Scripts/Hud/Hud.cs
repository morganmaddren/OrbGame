using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IHud
{
    IHeroController TopHeroController { get; }
    IHeroController BottomHeroController { get; }
    IHeroController GetHeroController(HeroSide side);

    IScoreBar ScoreBar { get; }
}

public class Hud : MonoBehaviour, IHud
{
    public IHeroController TopHeroController { get; private set; }
    public IHeroController BottomHeroController { get; private set; }
    public IScoreBar ScoreBar { get; private set; }

    public IHeroController GetHeroController(HeroSide side)
    {
        if (side == HeroSide.Bottom)
            return BottomHeroController;
        return TopHeroController;
    }

    void Awake()
	{
        TopHeroController = transform.Find("TopHeroController").GetComponentStrict<HeroController>();
        BottomHeroController = transform.Find("BottomHeroController").GetComponentStrict<HeroController>();
        ScoreBar = transform.Find("ScoreBar").GetComponentStrict<ScoreBar>();
    }

	void Start()
	{
		
	}

    void Update()
	{
		
	}
}