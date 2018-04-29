using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IHeroController
{
    void AttachHero(IHero hero);
}

public abstract class HeroController : MonoBehaviour, IHeroController
{
    protected IHero hero;

    public void AttachHero(IHero hero)
    {
        this.hero = hero;
        OnAttachHero(hero);
    }

    protected abstract void OnUpdate();
    protected abstract void OnAwake();
    protected abstract void OnStart();
    protected abstract void OnAttachHero(IHero hero);

    void Awake()
	{
        OnAwake();
	}

	void Start()
	{
        OnStart();
	}
	
	void Update()
	{
        OnUpdate();
	}
}