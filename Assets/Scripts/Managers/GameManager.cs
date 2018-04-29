using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static IInputManager Input { get { return GameManager.Instance.Input.ThrowIfNull(); } }
    public static IObjectManager Objects { get { return GameManager.Instance.Objects.ThrowIfNull(); } }
    public static IResourceManager Resources { get { return GameManager.Instance.Resources.ThrowIfNull(); } }
    public static ILevelManager Levels { get { return GameManager.Instance.Levels.ThrowIfNull(); } }
    public static IEntityManger Entities { get { return GameManager.Instance.Entities.ThrowIfNull(); } }
    public static IMatchManager Match { get { return GameManager.Instance.Match.ThrowIfNull(); } }
    public static IFloatingTextManager FloatingText { get { return GameManager.Instance.FloatingText.ThrowIfNull(); } }
    public static ISettingsManager Settings { get { return GameManager.Instance.Settings.ThrowIfNull(); } }
    public static IHudManager Hud { get { return GameManager.Instance.Hud.ThrowIfNull(); } }
    public static ISoundManager Sounds { get { return GameManager.Instance.Sounds.ThrowIfNull(); } }
}

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    static object instanceLock = new object();
    static bool isQuitting = false;
    bool isInitialized = false;

    // Components
    public InputManager Input { get; private set; }
    public ObjectManager Objects { get; private set; }
    public ResourceManager Resources { get; private set; }
    public LevelManager Levels { get; private set; }
    public EntityManager Entities { get; private set; }
    public MatchManager Match { get; private set; }
    public FloatingTextManager FloatingText { get; private set; }
    public SettingsManager Settings { get; private set; }
    public HudManager Hud { get; private set; }
    public SoundManager Sounds { get; private set; }

    void Awake()
    {
        instance = this;
        Input = this.GetComponentStrict<InputManager>();
        Objects = this.GetComponentStrict<ObjectManager>();
        Resources = this.GetComponentStrict<ResourceManager>();
        Levels = this.GetComponentStrict<LevelManager>();
        Entities = this.GetComponentStrict<EntityManager>();
        Match = this.GetComponentStrict<MatchManager>();
        FloatingText = this.GetComponentStrict<FloatingTextManager>();
        Settings = this.GetComponentStrict<SettingsManager>();
        Hud = this.GetComponentStrict<HudManager>();
        Sounds = this.GetComponentStrict<SoundManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                throw new Exception("GameManager isntance null");

            return instance;
        }
    }
}
