using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Point : MonoBehaviour
{
    // private static bool _lastResort = false; // if there's only 1 active point
    private Color _defaultColour;
    private bool _switchedOn = false; // point active
    private int _touching = 0; // point being touched, not necessarily active.
    private int _minionTouching = 0; // point being touched, not necessarily active.
    private bool _animActive = false;
    private bool _inLevel = false;
    [SerializeField] private GManager _gm;
    [SerializeField] private Animator _animator;
    public int id;

    public void Awake()
    {
        _defaultColour = GetComponent<SpriteRenderer>().color;
        // _animator.Play("timer");
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
    /// Checks if the point is active (active meaning turned on by the adversary, not necessarily pressed)
    /// </summary>
    /// <returns>true if active, false otherwise</returns>
    public bool IsOn()
    {
        return _switchedOn;
    }


    public bool AnimActive()
    {
        return _animActive;
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _touching++;
            if (_switchedOn)
            {
                if (_touching == 1) // if 2 touching then only one button is pressed, so total pressing shouldn't +1
                {
                    _gm.AddPressing(this);
                }
            }
        }
        else if (other.gameObject.tag.Equals("Minion"))
        {
            Minion minion = other.gameObject.GetComponent<Minion>();
            if (id == minion.target.id)
            {
                _minionTouching++;
                if (!_switchedOn)
                {
                    if (_minionTouching == 1)
                    {
                        _gm.AddMinionPressing(this);
                    }
                }
                
            }
        }
    }


    /// <summary>
    /// Sets the pressing condition and decreases the total num of presses, given that the
    /// point is active
    /// </summary>
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _touching--;
            if (_switchedOn)
            {
                if (_animActive)
                {
                    _gm.StopTimers();
                }

                if (_touching == 0) // if 1 is touching then the button is still being pressed, so total shouldn't -1
                {
                    _gm.RemovePressing(this);
                }
            }            
        }
        else if (other.gameObject.tag.Equals("Minion"))
        {
            Minion minion = other.gameObject.GetComponent<Minion>();
            if (id == minion.target.id)
            {
                Debug.Log("Point 123");
                _minionTouching--;
                if (_minionTouching == 0)
                {
                    _gm.RemoveMinionPressing(this);
                }                
            }

        }

    }

    public void MinionLeft()
    {
        _minionTouching--;
    } 


    /// <summary>
    /// Returns the number of players touching this point, regardless of if active or not 
    /// </summary>
    /// <returns></returns>
    public int NumTouching()
    {
        return _touching;
    }

    public void SetAnimActive(bool val)
    {
        _animActive = val;
    }

    public bool IsInLevel()
    {
        return _inLevel;
    }

    public void SetInLevel(bool val)
    {
        _inLevel = val;
    }

    public int GetId()
    {
        return id;
    }
}