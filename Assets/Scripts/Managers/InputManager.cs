using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputManager
{
    Vector2? StartingTouchPoint { get; }
    Vector2? PreviousTouchPoint { get; }
    Vector2? CurrentTouchPoint { get; }
    Vector2 CurrentTouchVelocity { get; }
    Vector2 CurrentTouchAcceleration { get; }

    bool IsTouchBegin { get; }
    bool IsTouchRelease { get; }
}

public class InputManager : MonoBehaviour, IInputManager
{
    public float touchVelocityUpdateRate;
    public float touchAccelerationUpdateRate;

    int startingTime;
    int timeSinceTap { get { return Time.frameCount - startingTime; } }

    public Vector2? StartingTouchPoint { get; private set; }
    public Vector2? PreviousTouchPoint { get; private set; }
    public Vector2? CurrentTouchPoint { get; private set; }
    public Vector2 CurrentTouchVelocity { get; private set; }
    public Vector2 CurrentTouchAcceleration { get; private set; }

    public bool IsTouchBegin { get { return CurrentTouchPoint.HasValue && timeSinceTap == 0; } }
    public bool IsTouchRelease { get { return !CurrentTouchPoint.HasValue && PreviousTouchPoint.HasValue; } }
    
    void Awake()
    {
        startingTime = -1;
        CurrentTouchVelocity = new Vector2();
        CurrentTouchAcceleration = new Vector2();
    }

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update ()
    {
        var newTouchPoint = GetTouchPoint();
        if (newTouchPoint != null)
        {
            if (PreviousTouchPoint == null)
            {
                startingTime = Time.frameCount;
                StartingTouchPoint = newTouchPoint;
            }
        }
        else
        {
        }

        PreviousTouchPoint = CurrentTouchPoint;
        CurrentTouchPoint = newTouchPoint;

        if (CurrentTouchPoint != null && PreviousTouchPoint != null)
        {
            UpdateVelocity((CurrentTouchPoint.Value - PreviousTouchPoint.Value) / Time.deltaTime);
            //Debug.Log("Velocity: " + CurrentTouchVelocity);
        }
        else
        {
            UpdateVelocity(Vector2.zero);
        }
        

        // Debug.Log("Touch:(" + (Game.Input.CurrentTouchPoint.HasValue ? Game.Input.CurrentTouchPoint.ToString() : "null") + ") Duration:" + Game.Input.TimeSinceLastTap + " IsBegin:" + Game.Input.IsTouchBegin.ToString());
    }

    private void UpdateVelocity(Vector2 delta)
    {
        var dv = Mathf.Clamp(touchVelocityUpdateRate * Time.deltaTime, 0, 1);
        var da = Mathf.Clamp(touchAccelerationUpdateRate * Time.deltaTime, 0, 1);
        var newTouchVelocity = dv * delta + (1 - dv) * CurrentTouchVelocity;
        var deltaVelocity = newTouchVelocity - CurrentTouchVelocity;
        CurrentTouchAcceleration = da * deltaVelocity + (1 - da) * CurrentTouchAcceleration;
        CurrentTouchVelocity = newTouchVelocity;
    }

    private Vector3? GetTouchPoint()
    {
        if (Input.touchCount > 0)
        {
            return Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        }

        if (Input.GetMouseButton(0))
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return null;
    }
}
