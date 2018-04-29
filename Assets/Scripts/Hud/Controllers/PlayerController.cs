using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum PlayerControllerInputState
{
    None,
    UncertainInput,
    PotentialSpellCast,
    Moving,
}

public class PlayerController : HeroController
{
    public float tapDetectionTime;
    public float tapDetectionDistanceSquared;
    public float tapHeroMoveInitiateDistanceSquared;
    public GameObject spellButtonEffect;

    BoxCollider2D spellRegion;
    BoxCollider2D touchRegion;
    ISpriteWrapper spellButton;
    Text cooldownText;
    IHeroHealthBar manaBar;

    Vector2 initialTouchPoint;
    float initialTouchTime;
    bool beingTouched;
    bool touchWasPostAim;
    PlayerControllerInputState inputState;

    IPersistentEffect spellButtonGlow;
    IMoveGrid moveGrid;
    IWaypoint waypoint;

    protected override void OnAwake()
    {
        touchRegion = transform.Find("TouchRegion").gameObject.GetComponentStrict<BoxCollider2D>();
        GameObject spellGameObject = transform.Find("SpellButton").gameObject;
        spellRegion = spellGameObject.GetComponentStrict<BoxCollider2D>();
        spellButton = spellGameObject.transform.Find("spellbutton").GetComponentStrict<SpriteWrapper>();
        cooldownText = spellGameObject.transform.Find("cooldowntext").GetComponentStrict<Text>();
        moveGrid = transform.Find("MoveGrid").gameObject.GetComponentStrict<MoveGrid>();
        waypoint = Game.Resources.LoadPrefab<Waypoint>("Waypoints/Waypoint");
    }

    protected override void OnAttachHero(IHero hero)
    {
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
        if (hero == null)
            return;

        if (hero.IsSpellEnoughMana && !hero.IsSpellOnCooldown)
        {
            spellButton.Color = new Color(1, 1, 1);
            cooldownText.text = "";

            if (spellButtonGlow == null)
            {
                spellButtonGlow = Game.Objects.CreatePrefab<SimpleEffect>(spellButtonEffect);
                spellButton.AddChild(spellButtonGlow);
            }
        }
        else
        {            
            if (!hero.IsSpellEnoughMana)
                spellButton.Color = new Color(.75f, .75f, 1, 1f);
            else
                spellButton.Color = new Color(.75f, .75f, .75f, 1f);

            if (hero.IsSpellOnCooldown)
                cooldownText.text = string.Format("{0:0}", hero.SpellCooldown);
            else
                cooldownText.text = "";

            if (spellButtonGlow != null)
            {
                spellButtonGlow.Dissipate();
                spellButtonGlow = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
            Game.Entities.GetPlayer(hero.Side).Score++;
        
        var curPoint = Game.Input.CurrentTouchPoint;
        if (Game.Input.IsTouchBegin && curPoint.HasValue)
        {
            if (spellButton.Bounds.Contains(curPoint.Value))
                hero.CastSpell();
            else if (touchRegion.OverlapPoint(curPoint.Value))
                this.HandleTouchBegin(curPoint.Value);
        }
        else if (Game.Input.IsTouchRelease)
        {
            this.HandleTouchRelease(Game.Input.PreviousTouchPoint.Value);
        }
        else if (beingTouched)
        {
            this.HandleTouch(curPoint.Value);
        }
        else
        {
            inputState = PlayerControllerInputState.None;
        }
        
        var w = hero.Waypoint;
        if (w.HasValue)
        {
            if (w.Value != this.waypoint.Position)
            {
                this.waypoint.MoveTo(w.Value);
            }
        }
        else
        {
            this.waypoint.Hide();
        }
    }

    void HandleTouchBegin(Vector2 touch)
    {
        initialTouchPoint = touch;
        initialTouchTime = Time.timeSinceLevelLoad;
        beingTouched = true;
        touchWasPostAim = hero.Aiming;
        hero.Aim(Vector2.zero, Vector2.zero);

        this.inputState = PlayerControllerInputState.UncertainInput;

        if (!touchWasPostAim)
        {
            var point = this.moveGrid.GetClosest(touch);
            hero.MoveTo(point);
        }
        else
        {
            hero.Aim(Game.Input.StartingTouchPoint.Value, Game.Input.CurrentTouchPoint.Value - Game.Input.StartingTouchPoint.Value);
        }
    }

    void HandleTouch(Vector2 touch)
    {
        if (touchWasPostAim)
        {
            //hero.Aim(Game.Input.CurrentTouchVelocity);
            hero.Aim(Game.Input.StartingTouchPoint.Value, Game.Input.CurrentTouchPoint.Value - Game.Input.StartingTouchPoint.Value);
        }
        else if (!hero.Aiming)
        {
            hero.Aim(Vector2.zero, Vector2.zero);
            var point = this.moveGrid.GetClosest(touch);
            hero.MoveTo(point);
        }

        if (this.inputState == PlayerControllerInputState.UncertainInput)
        {
            if (HasTouchBrokenTap(touch))
                this.inputState = PlayerControllerInputState.Moving;
        }
        else if (this.inputState == PlayerControllerInputState.PotentialSpellCast)
        {
            if (HasTouchBrokenTap(touch))
                this.inputState = PlayerControllerInputState.None;
        }
    }

    void HandleTouchRelease(Vector2 touch)
    {
        beingTouched = false;
        if (HasTouchBrokenTap(touch))
            return;
    }

    bool HasTouchBrokenTap(Vector2 touch)
    {
        if (this.inputState == PlayerControllerInputState.Moving || this.inputState == PlayerControllerInputState.None)
            return true;

        if (Time.timeSinceLevelLoad - initialTouchTime > this.tapDetectionTime)
            return true;

        if ((touch - initialTouchPoint).sqrMagnitude > this.tapDetectionDistanceSquared)
            return true;

        return false;
    }
}
