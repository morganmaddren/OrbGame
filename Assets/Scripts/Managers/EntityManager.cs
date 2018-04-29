using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IEntityManger
{
    IHero TopHero { get; }
    IHero BottomHero { get; }

    IHero GetHero(HeroSide side);
    IHero GetEnemyHero(HeroSide side);

    IHero CreateHero(Heroes hero, HeroSide side);

    IOrb Orb { get; }

    IPlayer TopPlayer { get; }
    IPlayer BottomPlayer { get; }
    IPlayer GetPlayer(HeroSide side);
    IPlayer GetEnemyPlayer(HeroSide side);
    
    IMinion CreateMinion(Minions minion, IHero owner, Vector2 position);
}

public class EntityManager : MonoBehaviour, IEntityManger
{
    public GameObject entityRoot;

    public IHero TopHero { get; private set; }
    public IHero BottomHero { get; private set; }

    public GameObject orb;
    public IOrb Orb { get; private set; }

    public IPlayer TopPlayer { get; private set; }
    public IPlayer BottomPlayer { get; private set; }

    public IPlayer GetPlayer(HeroSide side)
    {
        if (side == HeroSide.Top)
            return TopPlayer;
        return BottomPlayer;
    }

    public IPlayer GetEnemyPlayer(HeroSide side)
    {
        if (side == HeroSide.Bottom)
            return TopPlayer;
        return BottomPlayer;
    }

    public IHero GetEnemyHero(HeroSide side)
    {
        if (side == HeroSide.Top)
            return BottomHero;
        return TopHero;
    }

    public IHero GetHero(HeroSide side)
    {
        if (side == HeroSide.Bottom)
            return BottomHero;
        return TopHero;
    }

    public IMinion CreateMinion(Minions minion, IHero owner, Vector2 position)
    {
        var result = MinionFactory.CreateMinion(entityRoot, minion, owner, position);
        return result;
    }

    public IHero CreateHero(Heroes hero, HeroSide side)
    {
        var result = HeroFactory.CreateHero(entityRoot, hero, side, Game.Levels.CurrentLevel.GetHeroSpawn(side));
        if (side == HeroSide.Top)
        {
            Game.Hud.CurrentHud.ScoreBar.AttachHero(HeroSide.Top, result);
            TopHero = result;
        }
        else
        {
            Game.Hud.CurrentHud.ScoreBar.AttachHero(HeroSide.Bottom, result);
            BottomHero = result;
        }

        Game.Hud.CurrentHud.GetHeroController(side).AttachHero(result);

        return result;
    }

    void Awake()
    {
        Orb = orb.GetComponentStrict<Orb>();

        var topPlayer = new GameObject();
        var topPlayerComponent = topPlayer.AddComponent<Player>();
        topPlayerComponent.Initialize(HeroSide.Top, Heroes.WolfHero);
        TopPlayer = topPlayerComponent;

        var bottomPlayer = new GameObject();
        var bottomPlayerComponent = bottomPlayer.AddComponent<Player>();
        bottomPlayerComponent.Initialize(HeroSide.Bottom, Heroes.NatureHero);
        BottomPlayer = bottomPlayerComponent;
    }

	void Start()
    {
    }
	
	void Update()
	{
		
	}
}