using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathHelper
{
    public static Vector2 ShortestVector(Vector2 v, Vector2 w, Vector2 p)
    {
        // line segment vw closest vector from p
        var lengthSq = (v - w).sqrMagnitude;
        if (lengthSq == 0) return p - v;
        var t = ((p.x - v.x) * (w.x - v.x) + (p.y - v.y) * (w.y - v.y)) / lengthSq;
        return p - new Vector2(v.x + t * (w.x - v.x), v.y + t * (w.y - v.y));
    }

    public static float Determinant(Vector2 v, Vector2 w, Vector2 p)
    {
        return (w.x - v.x) * (p.y - v.y) - (w.y - v.y) * (p.x - v.x);
    }

    public static Vector2 Reflect(Vector2 direction, Vector2 normal)
    {
        return direction - 2 * (direction.x * normal.x + direction.y * normal.y) * normal;
    }

    public static Vector2 NormalizedInterpolate(Vector2 first, Vector2 second, float percent)
    {
        return ((second - first) * percent + first).normalized;
    }

    public static Vector2 Rotate(Vector2 vector, float radians)
    {
        var cos = (float)Math.Cos(radians);
        var sin = (float)Math.Sin(radians);
        return new Vector2(cos * vector.x - sin * vector.y, sin * vector.x + cos * vector.y);
    }
}
