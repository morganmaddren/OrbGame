using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

public interface IMinionHealthBar
{
    bool Visible { get; set; }
}

public class MinionHealthBar : MonoBehaviour, IMinionHealthBar
{
    public float verticalOffset;
    public Color bottomColor;
    public Color topColor;

    public bool Visible
    {
        get { return this.gameObject.activeSelf; }
        set { this.gameObject.SetActive(value); }
    }

    IMinion owner;
    Transform healthBar;
    float maxHealthBarScale;
    float curHealthPercent;
    float baseHealthX;
    ISpriteWrapper colorFrame;
    Text levelText;

    public void Initialize(IMinion owner)
    {
        this.owner = owner;
        healthBar = transform.Find("Health");
        colorFrame = transform.Find("colorframe").GetComponentStrict<SpriteWrapper>();
        if (owner.Owner.Side == HeroSide.Top)
            colorFrame.Color = topColor;
        else
            colorFrame.Color = bottomColor;

        levelText = transform.Find("leveltext").GetComponentStrict<Text>();
        
        transform.localPosition = new Vector3(0, verticalOffset, 0);
        maxHealthBarScale = healthBar.localScale.x;
        curHealthPercent = 1;
        baseHealthX = healthBar.localPosition.x;
    }

    void Awake()
    {

    }

    void Start()
    {
    }

    void Update()
    {
        float newHealthPercent = owner.Stats.health.ValueAsPercent;
        if (newHealthPercent != curHealthPercent)
        {
            curHealthPercent = newHealthPercent;
            var x = (1 - newHealthPercent) * maxHealthBarScale / 2;
            healthBar.localScale = new Vector3(newHealthPercent * maxHealthBarScale, healthBar.localScale.y);
            healthBar.localPosition = new Vector3(baseHealthX - x, healthBar.localPosition.y);
        }

        levelText.text = owner.Stats.goalPoints.ToString();
    }
}