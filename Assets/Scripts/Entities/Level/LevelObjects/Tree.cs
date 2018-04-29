using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

public interface ITree
{
	HeroSide Side { get; }
}

enum TreeState
{
    Cooldown,
    Charging,
    Unleashing,
}

public class Tree : MonoBehaviour, ITree
{
    public HeroSide side;
    public HeroSide Side { get { return side; } }

    public Vector2 SpawnPosition { get; private set; }
    IProgressBar progressBar;
    Text cooldownText;

    public Minions minion;
    public int hitsToUnleash;
    public float cooldownSeconds;

    Timer cooldownTimer;
    TreeState state;
    int curHits;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var orb = collision.AsOrb();
        if (orb != null)
        {
            OrbHit(orb);
        }
    }

    void OrbHit(IOrb orb)
    {
        if (this.state != TreeState.Charging)
            return;

        if (orb.Owner != null && orb.Owner.Side == side)
        {
            curHits++;
            if (curHits == hitsToUnleash)
            {
                state = TreeState.Unleashing;
                curHits = 0;
            }
        }
    }

    void Awake()
	{
        cooldownTimer = new Timer(cooldownSeconds);
        state = TreeState.Cooldown;
        this.SpawnPosition = transform.Find("Spawn").position;
        this.progressBar = transform.Find("ProgressBar").GetComponentStrict<ProgressBar>();
        this.cooldownText = transform.Find("TreeCooldownText").GetComponentStrict<Text>();
	}

	void Start()
	{
        if (this.side == HeroSide.Top)
            progressBar.Color = new Color(1, .25f, .25f);
        else
            progressBar.Color = new Color(.25f, .25f, 1);
    }
	
	void Update()
	{
        if (Game.Match.State != MatchState.Playing)
            return;

		switch (state)
        {
            case TreeState.Cooldown:
                UpdateCooldown();
                break;
            case TreeState.Charging:
                UpdateCharging();
                break;
            case TreeState.Unleashing:
                UpdateUnleashing();
                break;
        }
    }

    void UpdateUnleashing()
    {
        this.progressBar.Visible = false;
        this.cooldownText.text = "";
    }

    void UpdateCooldown()
    {
        if (cooldownTimer.CheckAndTick(Time.deltaTime))
        {
            state = TreeState.Charging;
        }

        this.progressBar.Visible = false;
        //this.cooldownText.text = string.Format("{0:0}", cooldownTimer.Duration - cooldownTimer.Elapsed);
    }

    void UpdateCharging()
    {
        this.cooldownText.text = "";
        this.progressBar.Visible = true;
        this.progressBar.UpdateValue(this.curHits / (float)this.hitsToUnleash);
    }
}