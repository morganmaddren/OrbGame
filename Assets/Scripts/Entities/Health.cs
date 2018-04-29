using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IHasHealth
{
    IHealth Health { get; }

    void TakeDamage(int damage);
    void Heal(int health);
    void Kill();
}

public interface IHealth
{
	int HP { get; }
    int MaxHP { get; }
    bool IsAlive { get; }
    bool IsFullHP { get; }
    float PercentHP { get; }
}

public interface IHealthInternal : IHealth
{
    void TakeDamage(int damage);
    void Heal(int health);
    void Kill();
}

public class Health : MonoBehaviour, IHealthInternal
{
    public int maxHP;

    public int MaxHP { get { return maxHP; } }
    public int HP { get; private set; }
    public bool IsAlive { get { return HP > 0; } }
    public bool IsFullHP { get { return HP == maxHP; } }
    public float PercentHP { get { return HP / (float)maxHP; } }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP < 0)
            HP = 0;
    }

    public void Heal(int health)
    {
        HP += health;
        if (HP > maxHP)
            HP = maxHP;
    }

    public void Kill()
    {
        HP = 0;
    }

	void Awake()
	{
        HP = maxHP;
	}
}