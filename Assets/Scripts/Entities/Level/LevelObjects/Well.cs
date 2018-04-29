using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IWell
{
	
}

enum WellState
{
    Inactive,
    Active,
    Charging,
}

public class Well : MonoBehaviour, IWell
{
    public float gravity;
    public float cooldown;
    public float pushDelay;
    public float pushSpeed;
    public float pushVariation;

    CircleCollider2D circleCollider;
    CircleCollider2D center;
    WellState state;

    Timer cooldownTimer;
    Timer pushTimer;
    Vector2 pushDirection;

    ISpriteWrapper activeSprite;
    ISpriteWrapper inactiveSprite;

    HeroSide ownerSide;
    UnityEngine.Random random;


    void Awake()
	{
        circleCollider = this.GetComponentStrict<CircleCollider2D>();
        center = this.transform.Find("Center").GetComponentStrict<CircleCollider2D>();

        activeSprite = this.transform.Find("well").GetComponentStrict<SpriteWrapper>();
        inactiveSprite = this.transform.Find("wellinactive").GetComponentStrict<SpriteWrapper>();

        cooldownTimer = new Timer(cooldown);
        pushTimer = new Timer(pushDelay);
        SetState(WellState.Inactive);
        random = new UnityEngine.Random();
    }

	void Start()
	{
		
	}
	
    void SetState(WellState state)
    {
        switch (state)
        {
            case WellState.Inactive:
                activeSprite.Visible = false;
                inactiveSprite.Visible = true;
                break;
            case WellState.Active:
                activeSprite.Visible = true;
                inactiveSprite.Visible = false;
                break;
        }

        this.state = state;
    }

    public void OnCenterHit(IOrb orb)
    {
        if (state != WellState.Active)
        return;

        if (orb.Owner.Side == HeroSide.Top)
            pushDirection = new Vector2(0, -1);
        else
            pushDirection = new Vector2(0, 1);

        orb.ForceHit(Vector2.zero, 0);
        SetState(WellState.Charging);

    }

	void Update()
	{
		switch (state)
        {
            case WellState.Inactive:
                UpdateInactive();
                break;
            case WellState.Active:
                UpdateActive();
                break;
            case WellState.Charging:
                UpdateCharging();
                break;
        }
	}
    
    void UpdateInactive()
    {
        if (cooldownTimer.CheckAndTick(Time.deltaTime))
        {
            SetState(WellState.Active);
        }
    }

    void UpdateActive()
    {

    }

    void UpdateCharging()
    {
        if (pushTimer.CheckAndTick(Time.deltaTime))
        {
            float offset = UnityEngine.Random.Range(-pushVariation, pushVariation);
            Vector2 direction = pushDirection + new Vector2(offset, 0);
            
            Game.Entities.Orb.ForceHit(direction.normalized, pushSpeed);
            SetState(WellState.Inactive);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var orb = collision.AsOrb();
        if (orb != null)
        {
            UpdateOrb(orb);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var orb = collision.AsOrb();
        if (orb != null)
        {
            UpdateOrb(orb);
        }
    }

    void UpdateOrb(IOrb orb)
    {
        if (this.state != WellState.Active)
            return;

        PullOrb(orb);
    }

    void PullOrb(IOrb orb)
    {
        var delta = (Vector2)transform.position - orb.Position;
        var scale = 1 - Mathf.Clamp(delta.magnitude / (orb.Radius + circleCollider.radius), 0, 1);
        var scaleSq = (float)Math.Pow(scale, 2);
        orb.Pull(delta, gravity * scaleSq);

        Debug.Log("delta=" + delta + ", scale=" + scale + ", scaleSq=" + scaleSq + ", gravity*scaleSq=" + gravity * scaleSq);
    }
}