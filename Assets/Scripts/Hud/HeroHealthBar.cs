using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IHeroHealthBar
{
    void AttachHero(IHero hero);
}

public class HeroHealthBar : MonoBehaviour, IHeroHealthBar
{
    public float verticalOffset;

    IHero hero;
    Transform healthBar;
    float maxHealthBarScale;
    float curHealthPercent;
    float baseHealthX;

    public void AttachHero(IHero hero)
    {
        this.hero = hero;
    }

	void Awake()
	{
        healthBar = transform.Find("Mana");
        transform.localPosition = new Vector3(0, verticalOffset, 0);
        maxHealthBarScale = healthBar.localScale.x;
        curHealthPercent = 1;
        baseHealthX = healthBar.localPosition.x;
    }

	void Start()
	{
	}
	
	void Update()
	{
        if (hero == null)
            return;

        float newHealthPercent = hero.Stats.Mana.ValueAsPercent;
        if (newHealthPercent != curHealthPercent)
        {
            curHealthPercent = newHealthPercent;
            var x = (1 - hero.Stats.Mana.ValueAsPercent) * maxHealthBarScale / 2;
            healthBar.localScale = new Vector3(hero.Stats.Mana.ValueAsPercent * maxHealthBarScale, healthBar.localScale.y);
            healthBar.localPosition = new Vector3(baseHealthX - x, healthBar.localPosition.y);
        }
        
	}
}