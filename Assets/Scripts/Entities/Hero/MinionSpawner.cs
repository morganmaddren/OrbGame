using System;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using UnityObject = UnityEngine.Object;
using IEnumerator = System.Collections.IEnumerator;

public interface IMinionSpawner
{
	float SpawnCooldown { get; }
}

public class MinionSpawner : MonoBehaviour, IMinionSpawner
{
    public float spawnInterval;
    public float initialSpawnTime;

    bool running;
    float elapsed;
    IHero hero;
    bool shouldSpawn;

    ContactFilter2D collisionFilter;

    public float SpawnCooldown { get { return spawnInterval - elapsed; } }

    void Awake()
    {
        hero = this.GetComponentStrict<Hero>();
        running = true;
        shouldSpawn = false;

        collisionFilter = new ContactFilter2D();
        var layerMask = new LayerMask();
        layerMask.value |= 1 << 8;
        collisionFilter.SetLayerMask(layerMask);
        collisionFilter.useTriggers = true;
        elapsed = initialSpawnTime;
    }

    void Start()
	{
        
	}
	
	void Update()
	{
        if (Game.Match.State != MatchState.Playing)
            return;

        if (running)
        {
            if (elapsed >= spawnInterval)
            {
                shouldSpawn = true;
                running = false;
                elapsed = spawnInterval;
            }
            else
                elapsed += Time.deltaTime;
        }

        if (shouldSpawn)
        {
            /*8if (hero.TrySpawn())
            {
                shouldSpawn = false;
                running = true;
                elapsed = 0;
            }*/
        }
    }
}