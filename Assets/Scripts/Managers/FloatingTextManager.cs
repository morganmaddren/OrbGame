using System;
using System.Collections.Generic;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

public interface IFloatingTextManager
{
    ITransientEffect CreateFloatingText(string text, TextStyles style, Vector2 position);
}

public class FloatingTextManager : MonoBehaviour, IFloatingTextManager
{
    public GameObject canvas;

	void Awake()
	{
		
	}

	void Start()
	{
		
	}
	
	void Update()
	{
		
	}

    public ITransientEffect CreateFloatingText(string text, TextStyles style, Vector2 position)
    {
        var effect = Game.Resources.LoadPrefab<TextEffect>("Effects/TransientEffects/TextEffect");
        canvas.AddChild(effect);
        effect.Initialize(text, style, position);

        return effect;
    }
}