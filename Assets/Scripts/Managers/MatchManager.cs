using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public enum MatchState
{
    Spawn,
    Pregame,
    Countdown,
    Playing,
    Ending
}

public interface IMatchManager
{
	MatchState State { get; }
}

public class MatchManager : MonoBehaviour, IMatchManager
{
    public float centerTapRadius;
    public float countdownTime;
    public GameObject readyEffect;
    public GameObject beginEffect;
    public GameObject winEffect;
    public GameObject loseEffect;
    public Vector2 textEffectPosition;
    
    public MatchState State { get; private set; }

    Timer countdownTimer;

	void Awake()
	{
        countdownTimer = new Timer(countdownTime);
        State = MatchState.Spawn;
	}

	void Start()
	{
	}
	
	void Update()
	{
        switch (State)
        {
            case MatchState.Spawn:
                UpdateSpawn();
                break;
            case MatchState.Pregame:
                UpdatePregame();
                break;
            case MatchState.Countdown:
                UpdateCountdown();
                break;
            case MatchState.Playing:
                UpdatePlaying();
                break;
        }
	}

    void UpdateSpawn()
    {
        Game.Entities.CreateHero(Heroes.PrincessHero, HeroSide.Bottom);
        Game.Entities.CreateHero(Heroes.WolfHero, HeroSide.Top);
        State = MatchState.Pregame;
    }

    void UpdateCountdown()
    {
        if (countdownTimer.CheckAndTick(Time.deltaTime))
        {
            CreateEffect(this.beginEffect);
            this.State = MatchState.Playing;
            Game.Entities.Orb.Serve();
        }
    }

    void UpdatePregame()
    {
        if (Game.Input.IsTouchBegin)
        {
            var point = Game.Input.CurrentTouchPoint.Value;
            if ((point - Game.Levels.CurrentLevel.OrbSpawn).sqrMagnitude <= this.centerTapRadius * this.centerTapRadius)
            {
                this.State = MatchState.Countdown;
                CreateEffect(this.readyEffect);
            }
        }
    }

    void UpdatePlaying()
    {
        bool topWins = Game.Entities.TopPlayer.Score == Game.Settings.ScoreRequiredToWin;
        bool bottomWins = Game.Entities.BottomPlayer.Score == Game.Settings.ScoreRequiredToWin;
        if (topWins || bottomWins)
        {
            this.State = MatchState.Ending;
            if (bottomWins)
                CreateEffect(this.winEffect);
            else
                CreateEffect(this.loseEffect);
        }
    }

    void CreateEffect(GameObject effect)
    {
        if (effect != null)
        {
            var prefab = Game.Objects.CreatePrefab<SimpleEffect>(effect);
            this.AddChild(prefab);
            prefab.transform.localPosition = textEffectPosition;
        }
    }
}