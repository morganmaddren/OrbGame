using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface ISideFlower : IPointGenerator
{
	
}

public class SideFlower : PointGenerator, ISideFlower
{
    public float timeToWilt;

    ISideFlowerSpawner spawner;
    Timer wiltTimer;

    public void Initialize(ISideFlowerSpawner spawner, Vector2 position)
    {
        this.spawner = spawner;
        this.transform.position = position;
    }

    public override void OnFullBump(IOrb orb, Vector2 collisionPoint)
    {
        spawner.OnFlowerHit(this, orb);
        Kill();
    }

    protected override void OnConstruct()
    {
        base.OnConstruct();

        wiltTimer = new Timer(timeToWilt);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        
        if (wiltTimer.CheckAndTick(Time.deltaTime))
        {
            spawner.OnFlowerWilt(this);
            Kill();
        }
    }
    
    void Kill()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}