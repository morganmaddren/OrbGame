using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IMinionWaypoint : IGameEntity
{
    void Initialize(IMinion minion);

    TryMoveResult TryMove(Vector2 position);
}

public enum TryMoveResult
{
    Success,
    LookLeft,
    LookRight
}

public class MinionWaypoint : GameEntity, IMinionWaypoint
{
    IMinion minion;

    BoxCollider2D collider;
    RaycastHit2D[] collisionBuffer = new RaycastHit2D[5];
    Collider2D[] colliderBuffer = new Collider2D[5];
    ContactFilter2D collisionFilter;

    protected override void OnConstruct()
    {
    }

    protected override void OnInitialize()
    {
    }

    protected override void OnUpdate()
    {
    }

    public void Initialize(IMinion minion)
    {
        this.minion = minion;

        collider = this.GetComponentStrict<BoxCollider2D>();
        collisionFilter = new ContactFilter2D();
        var layerMask = new LayerMask();
        layerMask.value |= 1 << 8;
        collisionFilter.SetLayerMask(layerMask);
        collisionFilter.useTriggers = true;
    }

    public TryMoveResult TryMove(Vector2 position)
    {
        Vector2 oldPos = transform.position;
        transform.position = position;
        int collisions = collider.Cast(Vector2.zero, collisionFilter, collisionBuffer, 0);

        bool hitMinion = false;
        bool hitWall = false;
        TryMoveResult result = TryMoveResult.Success;
        for (int i = 0; i < collisions; i++)
        {
            var minion = collisionBuffer[i].collider.AsMinion();
            if (minion != null)
            {
                if (minion != this.minion)
                {
                    hitMinion = true;
                    if (collisionBuffer[i].collider.bounds.center.x > oldPos.x)
                        result = TryMoveResult.LookLeft;
                    else
                        result = TryMoveResult.LookRight;
                }

                continue;
            }

            hitWall = true;
            if (collisionBuffer[i].collider.bounds.center.x > oldPos.x)
                result = TryMoveResult.LookLeft;
            else
                result = TryMoveResult.LookRight;
        }

        if (hitMinion || hitWall)
        {
            transform.position = oldPos;
        }

        return result;
    }
}