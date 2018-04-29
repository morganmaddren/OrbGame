using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrbState
{
    Spawning,
    Moving,
}

public interface IOrb : IGameEntity
{
    float Radius { get; }
    Vector2 Direction { get; }
    float SpeedMultiplier { get; }
    float Spin { get; }
    IHero Owner { get; }
    
    float TimeSinceLastBumperHit { get; }
    
    void Serve();
    void Hit(IHero hero, Vector2 normalVector, float speedMultiplier);
    void ForceHit(IHero hero, Vector2 direction, float speedMultiplier, float rotation = 0f);
    void ForceHit(Vector2 direction, float speedMultiplier, float rotation = 0f);
    void Pull(Vector2 direction, float magnitude);
}

public class Orb : GameEntity, IOrb
{
    public GameObject defaultOrbSprite;
    public Vector2 spawnPosition;
    public float startSpeed;
    public float speedRamp;
    public Vector2 startDirection;
    public float minBounceAngle;

    Vector3 direction;
    float speed;
    CircleCollider2D collider;
    OrbState state;
    
    Timer respawnTimer;
    HeroSide scoredSide;

    IOrbBehavior currentBehavior;
    IOrbSpinIndicator spinIndicator;

    ISpriteWrapper currentSprite;
    Dictionary<HeroSide, ISpriteWrapper> sprites;
    ISpriteWrapper defaultSprite;

    float timeOfLastBumperHit;

    public float Radius { get { return this.collider.radius; } }
    public Vector2 Direction { get { return this.direction; } }
    public IHero Owner { get { return currentBehavior != null ? currentBehavior.Owner : null; } }
    public float TimeSinceLastBumperHit { get { return Time.timeSinceLevelLoad - timeOfLastBumperHit; } }
    public float SpeedMultiplier { get; private set; }
    public float Spin { get; private set; }

    protected override void OnConstruct()
    {
        defaultSprite = this.AddChild(Game.Objects.CreatePrefab<SpriteWrapper>(defaultOrbSprite));
        ClearOrbBehavior();

        direction = startDirection;
        speed = startSpeed;
        collider = this.GetComponentStrict<CircleCollider2D>();

        respawnTimer = new Timer(1);
        state = OrbState.Spawning;
        scoredSide = HeroSide.Bottom;

        sprites = new Dictionary<HeroSide, ISpriteWrapper>();
    }
    
    public void Pull(Vector2 direction, float magnitude)
    {
        Vector2 dir = this.direction * this.speed;
        Vector2 delta = direction * magnitude * Time.deltaTime;
        dir += delta;
        this.speed = dir.magnitude;
        this.direction = dir.normalized;
    }

    public void Serve()
    {
        var direction = new Vector2(0, -1);
        //if (scoredSide == HeroSide.Top)
        //    direction = new Vector2(0, 1);

        this.state = OrbState.Moving;
        ForceHit(direction, 1);
    }

    public void ForceHit(IHero hero, Vector2 direction, float speedMultiplier, float rotation = 0f)
    {
        ApplyOrbBehavior(hero.OrbBehavior);
        ForceHit(direction, speedMultiplier, rotation);
    }
    
    public void ForceHit(Vector2 direction, float speedMultiplier, float rotation = 0f)
    {
        this.SpeedMultiplier = speedMultiplier;
        this.direction = direction.normalized;
        this.speed = startSpeed * speedMultiplier;
        this.Spin = rotation;
        this.spinIndicator.Spin(this.Spin);
        //rigidBody.velocity = direction * startSpeed * speedMultiplier;
    }
    
    public void Hit(IHero hero, Vector2 normalVector, float speedMultiplier)
    {
        ApplyOrbBehavior(hero.OrbBehavior);
    }

    protected override void OnInitialize()
    {
        this.transform.position = Game.Levels.CurrentLevel.OrbSpawn;
        spinIndicator = this.AddChild<OrbSpinIndicator>(this.GetComponentInChildren<OrbSpinIndicator>(), resetPosition: false);
    }

    protected override void OnUpdate()
    {
        if (Game.Match.State != MatchState.Playing)
            return;

        switch (state)
        {
            case OrbState.Spawning:
                UpdateSpawning();
                break;
            case OrbState.Moving:
                UpdateMoving();
                break;
        }

        if (currentBehavior != null)
            currentBehavior.OnUpdate();
    }
    
    void UpdateMoving()
    {
        direction = MathHelper.Rotate(direction, Spin * Time.deltaTime);
        transform.position = new Vector3(transform.position.x + direction.x * speed * Time.deltaTime, transform.position.y + direction.y * speed * Time.deltaTime, transform.position.y + direction.y * speed * Time.deltaTime);
    }

    void UpdateSpawning()
    {
        if (this.respawnTimer.CheckAndTick(Time.deltaTime))
        {
            Serve();
        }
    }

    void Respawn()
    {
        ClearOrbBehavior();
        this.state = OrbState.Spawning;
        this.transform.position = Game.Levels.CurrentLevel.OrbSpawn;
        respawnTimer.Restart();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IGoal goal = collision.gameObject.GetComponent<Goal>();
        if (goal != null)
        {
            if (currentBehavior != null)
                currentBehavior.OnScore();

            Game.Entities.GetEnemyPlayer(goal.Side).Score += 1;
            this.scoredSide = goal.Side;
            Game.FloatingText.CreateFloatingText("+1", TextStyles.Points, Game.Entities.GetEnemyHero(goal.Side).Position);
            Respawn();
        }

        IMinion minion = collision.gameObject.GetComponent<Minion>();
        if (minion != null)
        {
            if (currentBehavior != null)
            {
                if (minion.Owner.Side == currentBehavior.Owner.Side)
                    currentBehavior.OnHitAllyMinion(minion);
                else
                    currentBehavior.HitEnemyMinion(minion);
            }
        }

        IBumper bumper = collision.AsBumper();
        if (bumper != null)
        {
            timeOfLastBumperHit = Time.timeSinceLevelLoad;
            Vector2 collisionPoint = collision.bounds.ClosestPoint(this.Position);
            bumper.OnBump(this, collisionPoint);
        }
    }

    private void ApplyOrbBehavior(IOrbBehavior behavior)
    {
        if (currentBehavior != null)
            currentBehavior.OnUnapply();

        this.currentBehavior = behavior;

        currentBehavior.OnApply();

        if (!sprites.ContainsKey(currentBehavior.Owner.Side))
            sprites[currentBehavior.Owner.Side] = this.AddChild(currentBehavior.CreateSprite());

        SetActiveSprite(sprites[currentBehavior.Owner.Side]);
    }

    private void ClearOrbBehavior()
    {
        if (currentBehavior != null)
            currentBehavior.OnUnapply();

        this.currentBehavior = null;

        SetActiveSprite(defaultSprite);
    }

    private void SetActiveSprite(ISpriteWrapper sprite)
    {
        if (sprite == currentSprite)
            return;

        if (currentSprite != null)
            currentSprite.Visible = false;

        sprite.Visible = true;
        currentSprite = sprite;
    }
}
