using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IOrbBehavior
{
    IHero Owner { get; }
    SpriteWrapper CreateSprite();

    void HitEnemyMinion(IMinion minion);
    void OnHitAllyMinion(IMinion minion);
    void OnHitEnemyHero(IHero hero);
    void OnBounce();
    void OnUpdate();
    void OnApply();
    void OnUnapply();
    void OnScore();
}

public abstract class OrbBehavior : MonoBehaviour, IOrbBehavior
{
    public GameObject sprite;
    public int damage;

    public IHero Owner { get; private set; }

    public virtual void OnApply() { }
    public virtual void OnBounce() { }
    public virtual void OnHitAllyMinion(IMinion minion) { }
    public virtual void OnHitEnemyHero(IHero hero) { }
    public virtual void OnHitEnemyMinion(IMinion minion) { }
    public virtual void OnUnapply() { }
    public virtual void OnUpdate() { }
    public virtual void OnScore() { }

    public SpriteWrapper CreateSprite()
    {
        SpriteWrapper wrapper = Game.Objects.CreatePrefab<SpriteWrapper>(sprite);
        wrapper.Visible = false;
        return wrapper;
    }

    public void HitEnemyMinion(IMinion minion)
    {
        minion.TakeDamage(damage);
        OnHitEnemyMinion(minion);
    }

    void Awake()
	{
        Owner = this.GetComponentStrict<Hero>();	
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}
}