using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEnumerator = System.Collections.IEnumerator;

public enum TextStyles
{
    Damage,
    Heal,
    Points
}

public class TextEffect : TransientEffect
{
    public float moveSpeed;
    public float fullOpacityDuration;
    public float duration;

    Text text;
    
    Timer overallTimer;
    
	void Awake()
	{
        text = this.GetComponentStrict<Text>();
        overallTimer = new Timer(duration);
	}

    public void Initialize(string text, TextStyles style, Vector2 position)
    {
        this.text.text = text;
        transform.position = position;
        if (style == TextStyles.Damage)
        {
            this.text.color = new Color(1, .125f, .125f);
            this.transform.localScale = new Vector3(.375f, .375f);
        }
        else if (style == TextStyles.Heal)
        {
            this.text.color = new Color(.125f, 1, .125f);
            this.transform.localScale = new Vector3(.375f, .375f);
        }
        else if (style == TextStyles.Points)
        {
            this.text.color = new Color(.75f, .75f, 1);
            this.transform.localScale = new Vector3(.675f, .675f);
        }
    }

    void Start()
	{
		
	}
	
	void Update()
	{
        if (overallTimer.CheckAndTick(Time.deltaTime))
        {
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

        float opacity = 1 - Mathf.Clamp((overallTimer.Elapsed - fullOpacityDuration) / (overallTimer.Duration - fullOpacityDuration), 0, 1);
        this.text.color = new Color(text.color.r, text.color.g, text.color.b, opacity);
        this.transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * moveSpeed);
	}
}