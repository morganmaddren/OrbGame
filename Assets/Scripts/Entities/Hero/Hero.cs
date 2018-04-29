using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHero : IGameEntity, IIsBuffable<HeroBuffs>
{
    HeroState State { get; }
    bool Aiming { get; }

    HeroStats Stats { get; }
    HeroSide Side { get; }
    IOrbBehavior OrbBehavior { get; }

    bool IsSpellOnCooldown { get; }
    bool IsSpellEnoughMana { get; }
    float SpellCooldown { get; }
    void CastSpell();
    
    void MoveTo(Vector2 position);
    void Aim(Vector2 vector, Vector2 spin);

    void GainUltCharge(int points, Vector2 source);

    Vector2? Waypoint { get; }
}

public interface IHeroParent : IHero
{
    void StartAiming();
    void HitOrb();
    void StopAiming();
}

public enum HeroSpellCastResult
{
    Success,
    NotEnoughMana,
    OnCooldown,
}

public enum HeroSide
{
    Top,
    Bottom
}

public enum HeroState
{
    Standing,
    Running,
}

public static class HeroHelpers
{
    public static HeroSide GetOppositeSide(HeroSide side)
    {
        if (side == HeroSide.Top)
            return HeroSide.Bottom;
        return HeroSide.Top;
    }
}

public class Hero : GameEntity, IHeroParent
{
    public Spell spell;
    
    public AudioClip hitAudio;
    public AudioClip aimAudio;
    public AudioClip pointsAudio;

    public HeroStats stats;
    public HeroStats Stats { get { return stats; } }
    
    public HeroSide Side { get; private set; }
    
    Vector2 waypoint;
    public Vector2? Waypoint
    {
        get
        {
            if (waypoint.x == -1)
                return null;

            return waypoint;
        }
    }

    Vector2 queuedWaypoint;
    public Vector2 AimVector { get; private set; }
    public Vector2 AimSpinVector { get; private set; }
    public bool Aiming { get; private set; }

    BoxCollider2D boxCollider;
    RaycastHit2D[] collisionBuffer = new RaycastHit2D[5];
    ContactFilter2D collisionFilter;
    
    public IOrbBehavior OrbBehavior { get; private set; }

    Buffable<HeroBuffs, Hero, IHero> buffs;
    public IBuffable<HeroBuffs> Buffs { get { return buffs; } }
    public HeroState State { get { return stateMachine.CurrentStateKey; } }

    protected virtual void OnHeroUpdate() { }

    public virtual void OnCastSpell() { }
    public virtual HeroSpellCastResult InternalCanCast() { return HeroSpellCastResult.Success; }
    
    bool isSpellOnCooldown;
    Timer spellCooldownTimer;
    Timer regenTimer;

    IPaddle paddle;
    
    public bool IsSpellOnCooldown { get { return this.isSpellOnCooldown; } }
    public bool IsSpellEnoughMana { get { return this.stats.Mana.Value >= this.stats.spellManaCost; } }

    public float SpellCooldown
    {
        get { return this.spellCooldownTimer.Duration - this.spellCooldownTimer.Elapsed; }
    }
    
    EntityStateMachine<HeroState, Hero> stateMachine;

    AudioSource audioSource;
    
    public void MoveTo(Vector2 position)
    {
        if (stateMachine.CurrentStateKey == HeroState.Standing || stateMachine.CurrentStateKey == HeroState.Running)
            waypoint = position;
        //else if (stateMachine.CurrentStateKey == HeroState.Running)
        //    queuedWaypoint = position;
    }

    public void Aim(Vector2 vector, Vector2 spin)
    {
        if (Aiming)
        {
            AimVector = vector;
            AimSpinVector = spin;
        }
    }

    public void HitOrb()
    {
        if (this.hitAudio != null)
            audioSource.PlayOneShot(this.hitAudio, .5f);
        Aiming = false;
    }

    protected override void OnConstruct()
    {
        waypoint = new Vector2(-1, -1);
        queuedWaypoint = new Vector2(-1, -1);
        stateMachine = new EntityStateMachine<HeroState, Hero>(this);
        stateMachine.RegisterInitialState(HeroState.Standing, new StandingState());
        stateMachine.RegisterState(HeroState.Running, new RunningState());

        boxCollider = this.GetComponentStrict<BoxCollider2D>();
        OrbBehavior = this.GetComponentStrict<OrbBehavior>();
        buffs = new Buffable<HeroBuffs, Hero, IHero>(this);

        collisionFilter = new ContactFilter2D();
        var layerMask = new LayerMask();
        layerMask.value |= 1 << 8;
        collisionFilter.SetLayerMask(layerMask);

        this.stats.Initialize();

        regenTimer = new Timer(1);
        isSpellOnCooldown = true;

        this.paddle = AddChild(Game.Resources.LoadPrefab<Paddle>("Heroes/Paddle"), resetPosition: true);
        audioSource = this.GetComponentStrict<AudioSource>();

        this.spell = AddChild(Game.Objects.CreatePrefab(this.spell));
        spellCooldownTimer = new Timer(this.spell.cooldown);
    }

    public void Initialize(HeroSide side, Vector2 position)
    {
        this.transform.position = position;
        this.Side = side;
    }

    public void StartAiming()
    {
        StopMoving();
        
        if (this.aimAudio != null)  
            audioSource.PlayOneShot(this.aimAudio, .125f);
        Aiming = true;
        if (Side == HeroSide.Top)
            Aim(new Vector3(0, -1), Vector2.zero);
        else
            Aim(new Vector3(0, 1), Vector2.zero);
    }

    public void StopAiming()
    {
        Aiming = false;
    }

    void StopMoving()
    {
        Arrive();
        Arrive();
    }

    private bool GainUltCharge(int points)
    {
        if (Stats.Level.IsFull)
            return false;

        if (Stats.UltimateMeter.Value + points >= Stats.UltimateMeter.MaxValue)
        {
            Stats.Level.Value += (Stats.UltimateMeter.Value + points) / Stats.UltimateMeter.MaxValue;
            Stats.UltimateMeter.Value = (Stats.UltimateMeter.Value + points) % Stats.UltimateMeter.MaxValue;
        }
        else
            Stats.UltimateMeter.Value += points;
        return true;
    }

    public void GainUltCharge(int points, Vector2 source)
    {
        if (GainUltCharge(points))
        {
            if (Side == HeroSide.Bottom)
            {
                Game.FloatingText.CreateFloatingText("+" + points, TextStyles.Heal, source);

                if (pointsAudio != null)
                    audioSource.PlayOneShot(pointsAudio, .0625f);
            }
            else
                Game.FloatingText.CreateFloatingText("+" + points, TextStyles.Damage, source);
        }       
    }

    protected override void OnInitialize()
    {
        AddChild(Game.Resources.LoadPrefab<SpriteWrapper>("Sprites/ShadowLarge"));
    }
    
    protected override void OnUpdate()
    {
        stateMachine.Update();

        if (Game.Match.State == MatchState.Playing)
        {
            this.OnHeroUpdate();

            if (this.isSpellOnCooldown)
            {
                if (spellCooldownTimer.CheckAndTick(Time.deltaTime))
                {
                    isSpellOnCooldown = false;
                }
            }

            if (regenTimer.CheckAndTick(Time.deltaTime))
            {
                this.stats.Mana.Value += this.stats.manaRegen;
                GainUltCharge(this.stats.ultimateChargeRate);
            }

        }
    }

    private class StandingState : EntityState<Hero>
    {
        public override void OnEnter(Hero hero)
        {
        }

        public override void OnLeave(Hero hero)
        {
        }

        public override void OnPreUpdate(Hero hero)
        {
            if (!hero.IsAtWaypoint())
                hero.stateMachine.SetState(HeroState.Running);
        }

        public override void OnUpdate(Hero hero)
        {
        }
    }

    private class RunningState : EntityState<Hero>
    {
        public override void OnEnter(Hero hero)
        {
        }

        public override void OnLeave(Hero hero)
        {
        }

        public override void OnPreUpdate(Hero hero)
        {
            if (hero.IsAtWaypoint())
            {
                hero.Arrive();
                if (hero.IsAtWaypoint())
                    hero.stateMachine.SetState(HeroState.Standing);
            }
        }

        public override void OnUpdate(Hero hero)
        {
            hero.MoveTowardsWaypoint(hero.Stats.MoveSpeed);
        }
    }
    
    void MoveTowardsWaypoint(float speed)
    {
        var newPosition = Vector2.MoveTowards(Position, waypoint, speed * Time.deltaTime);

        var delta = newPosition - Position;
        if (CheckCollisionsAlongDelta(delta) == 0)
        {
            transform.position = newPosition;
        }
        else if (CheckCollisionsAlongDelta(new Vector2(delta.x, 0)) == 0)
        {
            transform.position += new Vector3(delta.x, 0);
        }
        else if (CheckCollisionsAlongDelta(new Vector2(0, delta.y)) == 0)
        {
            transform.position += new Vector3(0, delta.y);
        }
    }

    int CheckCollisionsAlongDelta(Vector2 delta)
    {
        var moveMagnitude = delta.magnitude * 1.1f;
        int collisions = boxCollider.Cast(delta, collisionFilter, collisionBuffer, moveMagnitude);

        return collisions;
    }
 
    void Arrive()
    {
        waypoint = queuedWaypoint;
        queuedWaypoint = new Vector2(-1, -1);
    }

    bool IsAtWaypoint()
    {
        if (waypoint.x == -1)
            return true;

        return DistSquaredToWaypoint() == 0;
    }

    float DistSquaredToWaypoint()
    {
        return Mathf.Pow(Position.x - waypoint.x, 2) + Mathf.Pow((Position.y - waypoint.y) * 2, 2);
    }

    public HeroSpellCastResult CanCast()
    {
        if (this.stats.Mana.Value < this.stats.spellManaCost)
            return HeroSpellCastResult.NotEnoughMana;

        if (isSpellOnCooldown)
            return HeroSpellCastResult.OnCooldown;

        return HeroSpellCastResult.Success;
    }

    public void CastSpell()
    {
        if (CanCast() == HeroSpellCastResult.Success)
        {
            this.spell.Cast();
            //this.stats.Mana.Value -= stats.spellManaCost;
            isSpellOnCooldown = true;
        }
    }
}
