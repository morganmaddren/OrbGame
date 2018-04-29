using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

public interface IScoreBar
{
    void AttachHero(HeroSide side, IHero hero);
}

public class ScoreBar : MonoBehaviour, IScoreBar
{
    public float barLength = 3.25f;
        
    ISpriteWrapper friendlyTip;
    ISpriteWrapper friendlyBar;
    ISpriteWrapper enemyTip;
    ISpriteWrapper enemyBar;

    IHero friendlyHero;
    IHero enemyHero;
    
    Text friendlyText;
    Text enemyText;

	void Awake()
    {
        friendlyTip = this.transform.Find("friendlyhptip").GetComponentStrict<SpriteWrapper>();
        friendlyBar = this.transform.Find("friendlyhpbar").GetComponentStrict<SpriteWrapper>();
        enemyTip = this.transform.Find("enemyhptip").GetComponentStrict<SpriteWrapper>();
        enemyBar = this.transform.Find("enemyhpbar").GetComponentStrict<SpriteWrapper>();
        
        friendlyText = this.transform.Find("friendlytext").GetComponentStrict<Text>();
        enemyText = this.transform.Find("enemytext").GetComponentStrict<Text>();
    }

    public void AttachHero(HeroSide side, IHero hero)
    {
        if (side == HeroSide.Bottom)
            friendlyHero = hero;
        else
            enemyHero = hero;
    }
    
	void Start()
	{
		
	}
	
	void Update()
	{
        if (friendlyHero != null)
        {
            UpdateFriendlyBar();
        }
        if (enemyHero != null)
        {
            UpdateEnemyBar();
        }
    }
    
    void UpdateFriendlyBar()
    {
        var percent = friendlyHero.Stats.UltimateMeter.ValueAsPercent;
        float tipPos = percent * -barLength;
        friendlyTip.Move(new Vector2(tipPos, 0));

        float centerPos = tipPos / 2;
        friendlyBar.Move(new Vector2(centerPos, 0));
        float barLen = 6 * (tipPos + friendlyTip.Bounds.size.x / 2);
        friendlyBar.Scale = new Vector2(barLen, 1);

        friendlyText.text = friendlyHero.Stats.Level.Value.ToString();
    }

    void UpdateEnemyBar()
    {
        var percent = enemyHero.Stats.UltimateMeter.ValueAsPercent;
        float tipPos = percent * barLength;
        enemyTip.Move(new Vector2(tipPos, 0));

        float centerPos = tipPos / 2;
        enemyBar.Move(new Vector2(centerPos, 0));
        float barLen = 6 * (tipPos - enemyTip.Bounds.size.x / 2);
        enemyBar.Scale = new Vector2(barLen, 1);

        enemyText.text = enemyHero.Stats.Level.Value.ToString();
    }

    float CalculatePercent(int score)
    {
        return score / (float)Game.Settings.ScoreRequiredToWin;
    }
}