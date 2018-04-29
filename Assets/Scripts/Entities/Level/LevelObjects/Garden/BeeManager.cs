using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IBeeManager : IGameEntity
{
    void OnBeeHit(IBee bee);
    void OnBeeLeave(IBee bee);

    Bounds Bounds { get; }
}

public class BeeManager : GameEntity, IBeeManager
{
    public Bee beePrefab;
    public int maxBees;
    public float initialCooldown;
    public float spawnCooldownBase;
    public float spawnCooldownRoll;

    List<Vector2> spawnPositions;
    List<IBee> bees;

    Timer spawnTimer;
    bool spawning;

    BoxCollider2D box;
    public Bounds Bounds { get { return this.box.bounds; } }

    public void OnBeeHit(IBee bee)
    {
        bees.Remove(bee);
        TryRollTimer();
    }

    public void OnBeeLeave(IBee bee)
    {
        bees.Remove(bee);
        TryRollTimer();
    }

    void SpawnBee()
    {
        Vector2 spawn = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count - 1)];
        var bee = Game.Objects.CreatePrefab<Bee>(beePrefab.gameObject);
        bee.Initialize(this, spawn);
        bees.Add(bee);

        TryRollTimer();
    }

    void TryRollTimer()
    {
        if (bees.Count == this.maxBees)
        {
            spawning = false;
            return;
        }

        spawning = true;
        spawnTimer.Restart();
        spawnTimer.Duration = spawnCooldownBase + UnityEngine.Random.Range(-spawnCooldownRoll, spawnCooldownRoll);
    }

    protected override void OnConstruct()
    {
        spawning = true;
        spawnTimer = new Timer(initialCooldown);
        bees = new List<IBee>();
        spawnPositions = new List<Vector2>();
        for (int i = 0; i < this.transform.childCount; i++)
            spawnPositions.Add(this.transform.GetChild(i).position);

        this.box = this.GetComponentStrict<BoxCollider2D>();
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
        if (spawning)
        {
            if (spawnTimer.CheckAndTick(Time.deltaTime))
            {
                SpawnBee();
            }
        }
    }
}