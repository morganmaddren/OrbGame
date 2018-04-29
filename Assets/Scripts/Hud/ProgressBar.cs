using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IProgressBar
{
	bool Visible { get; set; }
    Color Color { get; set; }
    void UpdateValue(float value);
}

public class ProgressBar : MonoBehaviour, IProgressBar
{
    public bool Visible
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }

    public Color Color
    {
        get { return this.healthBar.Color; }
        set { this.healthBar.Color = value; }
    }
    
    float maxHealthBarScale;
    float curHealthPercent;
    float baseHealthX;
    ISpriteWrapper healthBar;

    void Awake()
    {
        healthBar = transform.Find("Health").GetComponentStrict<SpriteWrapper>();
        
        maxHealthBarScale = healthBar.Scale.x;
        curHealthPercent = 1;
        baseHealthX = healthBar.LocalPosition.x;
    }

    void Start()
    {
    }

    public void UpdateValue(float value)
    {
        if (value != curHealthPercent)
        {
            curHealthPercent = value;
            var x = (1 - value) * maxHealthBarScale / 2;
            healthBar.Scale = new Vector3(value * maxHealthBarScale, healthBar.Scale.y);
            healthBar.Move(new Vector3(baseHealthX - x, healthBar.LocalPosition.y));
        }
    }

    void Update()
    {
    }
}