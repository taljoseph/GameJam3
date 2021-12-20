using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private Color defaultColour;
    private bool _switchedOn = false; // point active
    // private bool pressing = false; // point being pressed
    private static int _totalPressing = 0; 
    private int _touching = 0; // point being touched, not necessarily active.
    private static bool _lastResort = false; // if there's only 1 active point
    [SerializeField] private List<List<Point>> _neighbours = new List<List<Point>>();

    public void Awake()
    {
        defaultColour = GetComponent<SpriteRenderer>().color;
    }

    /// <summary>
    /// Sets new colour
    /// </summary>
    /// <param name="newColour"> The colour to which we set the point to </param> 
    public void SetColour(Color newColour)
    {
        GetComponent<SpriteRenderer>().color = newColour;
    }

    /// <summary>
    /// Sets the active state of a point 
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        _switchedOn = value;
    }

    /// <summary>
    /// Checks if the point is active
    /// </summary>
    /// <returns>true if active, false otherwise</returns>
    public bool IsOn()
    {
        return _switchedOn;
    }

    // /// <summary>
    // /// Check is this point is being pressed. Can be true only if point is active
    // /// </summary>
    // /// <returns>True if it's being pressed, false otherwise</returns>
    // public bool IsPressed()
    // {
    //     return pressing;
    // }

    // /// <summary>
    // /// Sets the current status of pressing
    // /// </summary>
    // /// <param name="value"></param>
    // public void SetPressing(bool value)
    // {
    //     pressing = value;
    // }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _touching++;
        SetPressingUp();
    }

    /// <summary>
    /// Sets the pressing condition and increases the total num of presses, given that the
    /// point is active
    /// </summary>
    public void SetPressingUp()
    {
        if (_switchedOn)
        {
            if (_touching == 1) // if 2 touching then only one button is pressed, so total pressing shouldn't +1
            {
                _totalPressing++;
                SetColour(Color.cyan);
            }
            // pressing = true;
        }
    }

    /// <summary>
    /// Sets the pressing condition and decreases the total num of presses, given that the
    /// point is active
    /// </summary>
    public void SetPressingDown()
    {
        if (_switchedOn)
        {
            if (_touching == 0) // if 1 is touching then the button is still being pressed, so total shouldn't -1
            {
                _totalPressing--;
                SetColour(Color.red);
            }
            // pressing = false;
        }
    }
    

    public void OnTriggerExit2D(Collider2D other)
    {
        _touching--;
        SetPressingDown();
    }

    /// <summary>
    /// Returns the total number of points being pressed
    /// </summary>
    /// <returns></returns>
    public static int Pressing()
    {
        return _totalPressing;
    }

    /// <summary>
    /// Resets the total points being pressed
    /// </summary>
    public static void ResetTotalPressing()
    {
        _totalPressing = 0;
    }

    public static void UpTotalPressing()
    {
        _totalPressing++;
    }

    /// <summary>
    /// Returns the number of players touching this point, regardless of if active or not 
    /// </summary>
    /// <returns></returns>
    public int NumTouching()
    {
        return _touching;
    }
}

