using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using UnityObject = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;

public interface IMinion : IGameEntity, IIsBuffable<MinionBuffs>
{
    IHero Owner { get; }
    MinionStats Stats { get; }

    void TakeDamage(int damage);
    void Heal(int health);
    void Kill();
    bool IsAlive { get; }

    void Promote();
}

public enum MinionAttackState
{
    None,
    Charging,
    Cooldown
}

public class Minion : GameEntity, IMinion
{
    public MinionStats stats;
    public MinionStats Stats { get { return stats; } }
    public GameObject levelUpEffect;

    MinionState state;

    IMinion target;
    Timer attackChargeTimer;
    Timer attackCooldownTimer;
    MinionAttackState attackState;

    Timer deathTimer;

    public IHero Owner { get; private set; }
    public bool IsAlive { get { return !this.stats.health.IsEmpty; } }
    
    MinionHealthBar healthBar;

    Buffable<MinionBuffs, Minion, IMinion> buffs;
    public IBuffable<MinionBuffs> Buffs { get { return buffs; } }

    BoxCollider2D collider;
    RaycastHit2D[] collisionBuffer = new RaycastHit2D[5];
    Collider2D[] colliderBuffer = new Collider2D[5];
    ContactFilter2D collisionFilter;
    ISpriteWrapper sprite;
    IMinionWaypoint waypoint;

    protected virtual void OnPromote() { }
    protected virtual void OnKillEnemyMinion(IMinion minion) { }

    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }

    public void Initialize(IHero owner, Vector2 position)
    {
        this.Owner = owner;
        this.transform.position = position;

        this.SetState(MinionState.Idle);
        this.attackState = MinionAttackState.None;
        this.stats.health.Initialize();
        this.healthBar.Initialize(this);
        this.buffs.Enabled = true;
    }
    
    public void Promote()
    {
        if (this.stats.goalPoints < this.stats.maxLevel)
        {
            if (levelUpEffect != null)
            {
                var prefab = Game.Objects.CreatePrefab<SimpleEffect>(levelUpEffect);
                this.AddChild(prefab);
            }

            this.Stats.goalPoints++;
            OnPromote();
        }
    }

    public void TakeDamage(int damage)
    {
        int effectiveDamage = (int)Math.Round(damage * this.stats.IncomingDamageMultiplier);
        this.stats.health.Value -= effectiveDamage;
        Game.FloatingText.CreateFloatingText(effectiveDamage.ToString(), TextStyles.Damage, Position);
        if (!this.IsAlive)
            Kill();
    }

    public void Heal(int health)
    {
        int amountToHeal = Mathf.Clamp(health, 0, this.stats.health.MaxValue - this.stats.health.Value);
        this.stats.health.Value += amountToHeal;

        if (amountToHeal > 0)
            Game.FloatingText.CreateFloatingText(amountToHeal.ToString(), TextStyles.Heal, Position);
    }

    public void Kill()
    {
        SetState(MinionState.Dead);
    }

    void Awake()
    {
        collider = this.GetComponentStrict<BoxCollider2D>();
        buffs = new Buffable<MinionBuffs, Minion, IMinion>(this);
        this.sprite = this.GetComponentStrict<SpriteWrapper>();

        collisionFilter = new ContactFilter2D();
        var layerMask = new LayerMask();
        layerMask.value |= 1 << 8;
        collisionFilter.SetLayerMask(layerMask);
        collisionFilter.useTriggers = true;
        
        attackChargeTimer = new Timer(stats.attackChargeTimeBase);
        attackCooldownTimer = new Timer(stats.AttackCooldown);
        deathTimer = new Timer(1);

        this.healthBar = Game.Resources.LoadPrefab<MinionHealthBar>("MinionHealthBar");
        this.healthBar.gameObject.name = "MinionHealthBar";
        this.healthBar.gameObject.transform.SetParent(gameObject.transform);

        this.waypoint = Game.Resources.LoadPrefab<MinionWaypoint>("Minions/Waypoint");
        this.waypoint.Initialize(this);

        AddChild(Game.Resources.LoadPrefab<SpriteWrapper>("Sprites/ShadowSmall"));
    }

    void Start()
    {
        
    }
	
	void Update()
	{
        this.state.OnPreUpdate(this);
        this.state.OnUpdate(this);

        if (attackState == MinionAttackState.Cooldown)
        {
            if (attackCooldownTimer.CheckAndTick(Time.deltaTime))
                attackState = MinionAttackState.None;
        }
	}
    
    private void SetState(MinionState state)
    {
        if (state == this.state)
            return;

        if (this.state != null)
            this.state.OnLeave(this);

        this.state = state;
        this.state.OnEnter(this);
    }
    
    private abstract class MinionState
    {
        public virtual void OnEnter(Minion minion) { }
        public virtual void OnPreUpdate(Minion minion) { }
        public virtual void OnUpdate(Minion minion) { }
        public virtual void OnLeave(Minion minion) { }

        public static IdleState Idle = new IdleState();
        public static MovingState Moving = new MovingState();
        public static AttackingState Attacking = new AttackingState();
        public static ScoringState Scoring = new ScoringState();
        public static DeadState Dead = new DeadState();
    }

    private class IdleState : MinionState
    {
        public override void OnEnter(Minion minion)
        {
        }

        public override void OnPreUpdate(Minion minion)
        {
            minion.DetermineState();
        }

        public override void OnLeave(Minion minion)
        {
        }

        public override void OnUpdate(Minion minion)
        {
        }
    }

    private class MovingState : MinionState
    {
        public override void OnEnter(Minion minion)
        {
        }

        public override void OnLeave(Minion minion)
        {
        }

        public override void OnPreUpdate(Minion minion)
        {
            if ((minion.Position - minion.waypoint.Position).sqrMagnitude < .000001f)
            {
                minion.transform.position = minion.waypoint.Position;
                minion.DetermineState();
            }
        }

        public override void OnUpdate(Minion minion)
        {
            if (minion.stats.Rooted || minion.stats.Stunned)
                return;

            Vector2 oldPos = minion.Position;
            Vector2 target = Vector2.MoveTowards(minion.Position, minion.waypoint.Position, minion.stats.MoveSpeed * Time.deltaTime);
            minion.transform.position = target;
        }
    }

    private class AttackingState : MinionState
    {
        public override void OnEnter(Minion minion)
        {
        }

        public override void OnPreUpdate(Minion minion)
        {
            if (!minion.IsTargetStillValid())
                minion.DetermineState();
        }

        public override void OnLeave(Minion minion)
        {
        }

        public override void OnUpdate(Minion minion)
        {
            if (minion.attackState == MinionAttackState.None)
            {
                minion.attackChargeTimer.Restart();
                minion.attackState = MinionAttackState.Charging;
            }

            if (minion.attackState == MinionAttackState.Charging)
            {
                if (minion.attackChargeTimer.CheckAndTick(Time.deltaTime))
                {
                    minion.AttackTarget();

                    minion.attackCooldownTimer.Restart(minion.stats.AttackCooldown);
                    minion.attackState = MinionAttackState.Cooldown;
                }
            }
        }
    }

    private class ScoringState : MinionState
    {
        public override void OnEnter(Minion minion)
        {
            Game.Entities.GetPlayer(minion.Owner.Side).Score += minion.Stats.goalPoints;
            Game.FloatingText.CreateFloatingText("+" + minion.Stats.goalPoints, TextStyles.Points, minion.Position);
        }

        public override void OnLeave(Minion minion)
        {
        }

        public override void OnUpdate(Minion minion)
        {
            minion.Kill();
        }
    }

    private class DeadState : MinionState
    {
        public override void OnEnter(Minion minion)
        {
            minion.stats.health.Value = 0;
            minion.collider.enabled = false;
            minion.healthBar.Visible = false;
            minion.buffs.Enabled = false;
        }

        public override void OnLeave(Minion minion)
        {
        }

        public override void OnUpdate(Minion minion)
        {
            var ratio = 1 - (minion.deathTimer.Elapsed / minion.deathTimer.Duration);
            minion.sprite.Color = new Color(ratio, ratio, ratio, ratio);

            if (minion.deathTimer.CheckAndTick(Time.deltaTime))
            {
                minion.gameObject.SetActive(false);
                Destroy(minion.gameObject);
            }
        }
    }
    
    void DetermineState()
    {
        var enemy = FindTargetEnemy();
        if (enemy != null)
        {
            this.target = enemy;
            SetState(MinionState.Attacking);
            return;
        }

        bool gotWaypoint = FindWaypoint();
        if (gotWaypoint)
        {
            SetState(MinionState.Moving);
            return;
        }

        SetState(MinionState.Idle);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGoal goal = collision.AsGoal();
        if (goal != null)
        {
            SetState(MinionState.Scoring);
        }
    }

    IMinion FindTargetEnemy()
    {
        int collisions = Physics2D.OverlapCircleNonAlloc(this.collider.bounds.center, this.stats.attackRangeBase, colliderBuffer, collisionFilter.layerMask.value);

        IMinion result = null;
        float closestDistSq = float.MaxValue;
        for (int i = 0; i < collisions; i++)
        {
            IMinion potentialResult = this.colliderBuffer[i].gameObject.GetComponent<Minion>();
            if (potentialResult != null && potentialResult.Owner != this.Owner)
            {
                float distSq = (potentialResult.Position - this.Position).sqrMagnitude;
                if (distSq < closestDistSq)
                {
                    result = potentialResult;
                    closestDistSq = distSq;
                }
            }
        }

        return result;
    }

    bool FindWaypoint()
    {
        float horizontalOffset = .25f;
        float verticalOffset = .25f;
        if (this.Owner.Side == HeroSide.Top)
            verticalOffset = -.25f;

        TryMoveResult result = this.waypoint.TryMove(new Vector2(this.Position.x, this.Position.y + verticalOffset));
        if (result == TryMoveResult.Success)
            return true;

        if (result == TryMoveResult.LookLeft)
        {
            if (this.waypoint.TryMove(new Vector2(this.Position.x - horizontalOffset, this.Position.y + verticalOffset)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x + horizontalOffset, this.Position.y + verticalOffset)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x - horizontalOffset, this.Position.y)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x + horizontalOffset, this.Position.y)) == TryMoveResult.Success)
                return true;
        }
        else
        {
            if (this.waypoint.TryMove(new Vector2(this.Position.x + horizontalOffset, this.Position.y + verticalOffset)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x - horizontalOffset, this.Position.y + verticalOffset)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x + horizontalOffset, this.Position.y)) == TryMoveResult.Success)
                return true;

            if (this.waypoint.TryMove(new Vector2(this.Position.x - horizontalOffset, this.Position.y)) == TryMoveResult.Success)
                return true;
        }

        return false;
    }

    bool IsTargetStillValid()
    {
        if (target == null || !target.IsAlive)
            return false;

        float distSq = (target.Position - this.Position).sqrMagnitude;
        return distSq <= this.stats.attackRangeBase * this.stats.attackRangeBase;
    }

    void AttackTarget()
    {
        target.TakeDamage(this.stats.AttackDamage);
        if (!target.IsAlive)
            OnKillEnemyMinion(target);
    }
}