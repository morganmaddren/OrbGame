using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public class GoalHealthBar : MonoBehaviour
{
    public float verticalOffset;

    IHealth health;
    Transform healthBar;
    float maxHealthBarScale;
    float curHealthPercent;
    float baseHealthX;

    void Awake()
    {
        health = transform.parent.GetComponentStrict<Health>();
        healthBar = transform.Find("Health");
        transform.localPosition = new Vector3(0, verticalOffset, 0);
        maxHealthBarScale = healthBar.localScale.x;
        curHealthPercent = 1;
        baseHealthX = healthBar.localPosition.x;
    }

    void Start()
    {
    }

    void Update()
    {
        float newHealthPercent = health.PercentHP;
        if (newHealthPercent != curHealthPercent)
        {
            curHealthPercent = newHealthPercent;
            var x = (1 - health.PercentHP) * maxHealthBarScale / 2;
            healthBar.localScale = new Vector3(health.PercentHP * maxHealthBarScale, healthBar.localScale.y);
            healthBar.localPosition = new Vector3(baseHealthX - x, healthBar.localPosition.y);
        }

    }
}