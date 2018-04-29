using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static T GetComponentStrict<T>(this MonoBehaviour me)
    {
        T result = me.GetComponent<T>();
        if (result == null)
            throw new System.Exception(typeof(T).Name + " is null");

        return result;
    }

    public static T GetComponentStrict<T>(this GameObject me)
    {
        T result = me.GetComponent<T>();
        if (result == null)
            throw new System.Exception(typeof(T).Name + " is null");

        return result;
    }

    public static T GetComponentStrict<T>(this Transform me)
    {
        return me.gameObject.GetComponentStrict<T>();
    }

    public static T ThrowIfNull<T>(this T me)
    {
        if (me == null)
            throw new System.Exception(typeof(T).Name + " is null");

        return me;
    }

    public static T AddChildEntity<T>(this GameObject me, T child) where T : IGameEntity
    {
        GameEntity childEntity = child as GameEntity;
        childEntity.transform.parent = me.transform;
        childEntity.transform.localPosition = Vector2.zero;

        return child;
    }

    public static T AddChildEntity<T>(this GameObject me, T child, string name) where T : IGameEntity
    {
        GameEntity childEntity = child as GameEntity;
        childEntity.name = name;
        childEntity.transform.parent = me.transform;
        childEntity.transform.localPosition = Vector2.zero;

        return child;
    }

    public static T AddChild<T>(this MonoBehaviour me, T child) where T : MonoBehaviour
    {
        child.transform.parent = me.transform;
        child.transform.localPosition = Vector2.zero;

        return child;
    }

    public static T AddChild<T>(this MonoBehaviour me, T child, string name) where T : MonoBehaviour
    {
        child.name = name;
        return me.AddChild(child);
    }

    public static T AddChild<T>(this GameObject me, T child) where T : MonoBehaviour
    {
        child.transform.SetParent(me.transform);
        child.transform.localPosition = Vector2.zero;

        return child;
    }

    public static T AddChild<T>(this GameObject me, T child, string name) where T : MonoBehaviour
    {
        child.name = name;
        return me.AddChild(child);
    }
    
    public static IOrb AsOrb(this Collider2D me)
    {
        if (me.GetComponent<OrbCenter>() != null)
            return null;

        return me.GetComponentInParent<Orb>();
    }

    public static IMinion AsMinion(this Collider2D me)
    {
        return me.GetComponent<Minion>();
    }

    public static IBumper AsBumper(this Collider2D me)
    {
        return me.GetComponent<Bumper>();
    }

    public static IGoal AsGoal(this Collider2D me)
    {
        return me.GetComponent<Goal>();
    }

    public static ISpawnPosition AsSpawnPosition(this Collider2D me)
    {
        return me.GetComponent<SpawnPosition>();
    }
}
