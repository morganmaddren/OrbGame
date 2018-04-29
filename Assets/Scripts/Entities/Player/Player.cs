using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IPlayer
{
    Heroes SelectedHero { get; }
    HeroSide Side { get; }

    int Score { get; set; }
}

public class Player : MonoBehaviour, IPlayer
{
    public Heroes SelectedHero { get; private set; }
    public HeroSide Side { get; private set; }

    int score;
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
            if (score < 0)
                score = 0;

            if (score > Game.Settings.ScoreRequiredToWin)
                score = Game.Settings.ScoreRequiredToWin;
        }
        
    }

    public void Initialize(HeroSide side, Heroes selectedHero)
    {
        this.SelectedHero = selectedHero;
        this.Side = side;
    }

    void Awake()
	{
		
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}
}