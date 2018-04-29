using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISideFlowerSpawner
{
    void OnFlowerHit(ISideFlower flower, IOrb orb);
    void OnFlowerWilt(ISideFlower flower);
}

public class SideFlowerSpawner : GameEntity, ISideFlowerSpawner
{
    public GameObject flowerToSpawn;
    public float spawnChancePerSecond;
    public int maxFlowers;
    
    List<Vector2> spawnPositions;
    List<ISideFlower> flowers;

    public void OnFlowerHit(ISideFlower flower, IOrb orb)
    {
        flowers.Remove(flower);
    }

    public void OnFlowerWilt(ISideFlower flower)
    {
        flowers.Remove(flower);
    }

    protected override void OnConstruct()
    {
        flowers = new List<ISideFlower>();
        spawnPositions = new List<Vector2>();
        for (int i = 0; i < this.transform.childCount; i++)
            spawnPositions.Add(this.transform.GetChild(i).position);
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
        if (flowers.Count < maxFlowers)
        {
            if (UnityEngine.Random.value < spawnChancePerSecond * Time.deltaTime)
            {
                var spawn = spawnPositions[UnityEngine.Random.Range(0, spawnPositions.Count)];
                bool spawnUsed = false;
                foreach (ISideFlower flower in flowers)
                {
                    if (flower.Position == spawn)
                    {
                        spawnUsed = true;
                        break;
                    }
                }

                if (!spawnUsed)
                {
                    var flower = Game.Objects.CreatePrefab<SideFlower>(flowerToSpawn);
                    flower.Initialize(this, spawn);
                    flowers.Add(flower);
                }
            }
        }
    }
}