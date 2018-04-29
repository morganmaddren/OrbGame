using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IMoveGrid : IGameEntity
{
    Vector2 GetClosest(Vector2 point);
}

public class MoveGrid : CollectionEntity<PointEntity>, IMoveGrid
{
    public int cornerGap = 2;
    public int columns = 10;
    public int rows = 6;

    public float horizontalSpacing = .75f;
    public float verticalSpacing = .75f;

    PointEntity[,] points;

    protected override void OnConstruct()
    {
        base.OnConstruct();
        points = new PointEntity[columns, rows];
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        foreach (var point in this.Entities)
        {
            var pos = point.Position - this.Position;

            int x = GetClosestColumn(pos.x);
            int y = GetClosestRow(pos.y);
            
            if (points[x, y] != null)
                throw new Exception("Duplicate stuff");

            points[x, y] = point;

            IUnmoveableSpot unmoveable = point.GetComponent<UnmoveableSpot>();
            if (unmoveable != null)
            {
                if (unmoveable.ClosestPoints.Length == 0)
                    throw new Exception("empty unmoveable");
            }
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (points[i, j] == null)
                    throw new Exception("Not enough stuff");
            }
        }
    }

    protected override void OnUpdate()
    {
    }

    public Vector2 GetClosest(Vector2 point)
    {
        var newPoint = point - this.Position;

        int x = GetClosestColumn(newPoint.x);
        int y = GetClosestRow(newPoint.y);

        var result = points[x, y];
        IUnmoveableSpot unmoveable = result.GetComponent<UnmoveableSpot>();
        if (unmoveable != null)
        {
            float minDistSq = float.MaxValue;
            PointEntity closest = null;

            foreach (var other in unmoveable.ClosestPoints)
            {
                var sqDist = (point - other.Position).sqrMagnitude;
                if (sqDist < minDistSq)
                {
                    minDistSq = sqDist;
                    closest = other;
                }
            }

            return closest.Position;
        }

        return result.Position;

        //return new Vector2((x - ((columns - 1) / 2)) * horizontalSpacing + transform.position.x, (y - ((rows - 1) / 2)) * verticalSpacing + transform.position.y);
    }

    int GetClosestColumn(float x)
    {
        var closest = x / horizontalSpacing + ((columns - 1) / 2);
        return (int)Mathf.Clamp((float)Math.Round(closest), 0, columns - 1);
    }

    int GetClosestRow(float y)
    {
        var closest = y / verticalSpacing + ((rows - 1) / 2);
        return (int)Mathf.Clamp((float)Math.Round(closest), 0, rows - 1);
    }
}