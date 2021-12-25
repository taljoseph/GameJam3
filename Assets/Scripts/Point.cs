using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Point : MonoBehaviour
{
    // private static bool _lastResort = false; // if there's only 1 active point
    [SerializeField] private Color _defaultColour;
    [SerializeField] private bool _isSatanPoint = false; // point active
    [SerializeField] private int _playerTouching = 0; // point being touched, not necessarily active.
    // [SerializeField] private int _minionTouching = 0; // point being touched, not necessarily active. TODO redundant 
    [FormerlySerializedAs("_animActive")] [SerializeField] private bool _animActivePlayer = false;
    [SerializeField] private bool _animActiveMinion = false;
    [SerializeField] private bool _inLevel = false;
    [SerializeField] private GManager _gm;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator _minionAnimator;
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
        _isSatanPoint = value;
    }

    /// <summary>
    /// Checks if the point is active (active meaning turned on by the adversary, not necessarily pressed)
    /// </summary>
    /// <returns>true if active, false otherwise</returns>
    public bool IsSatanPoint()
    {
        return _isSatanPoint;
    }


    public bool AnimActive()
    {
        return _animActivePlayer;
    }

    public Animator GetPlayerAnimator()
    {
        return _playerAnimator;
    }

    public Animator GetMinionAnimator()
    {
        return _minionAnimator;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _playerTouching++;
            if (_isSatanPoint)
            {
                if (_playerTouching ==
                    1) // if 2 touching then only one button is pressed, so total pressing shouldn't +1
                {
                    _gm.AddPressing(this);
                }
            }
        }
        else if (other.gameObject.tag.Equals("Minion"))
        {
            Minion minion = other.gameObject.GetComponent<Minion>();
            // _minionTouching++; TODO redundant
            if (!_isSatanPoint && id == minion.target.id && minion.IsFinalTargetSet())
            {
                // if (_minionTouching == 1) TODO redundant
                // {
                    _gm.AddMinionPressing(this);
                // }
            }
        }
        else if (other.gameObject.tag.Equals("PointsActivator"))
        {
            PointsActivator pointsActivator = other.gameObject.GetComponent<PointsActivator>();
            pointsActivator.HandlePointCollision(this);
        }
        else if (other.gameObject.tag.Equals("Hand"))
        {
            _gm.HandTouching(this);
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
            _playerTouching--;
            if (_isSatanPoint)
            {
                if (_animActivePlayer)
                {
                    _gm.StopPlayerTimers();
                }

                if (_playerTouching ==
                    0) // if 1 is touching then the button is still being pressed, so total shouldn't -1
                {
                    _gm.RemovePressing(this);
                }
            }
        }
        else if (other.gameObject.tag.Equals("Minion"))
        {
            Minion minion = other.gameObject.GetComponent<Minion>();

            // _minionTouching--; TODO redundant
            if (id == minion.target.id && minion.IsFinalTargetSet())
            {
                if (_animActiveMinion)
                {
                    _gm.StopMinionTimers();
                }

                // if (_minionTouching == 0) TODO redundant
                // {
                    // _gm.RemoveMinionPressing(this); 
                // }
            }
        }
    }

    // public void MinionLeft() TODO redundant
    // {
    //     _minionTouching--;
    // }


    /// <summary>
    /// Returns the number of players touching this point, regardless of if active or not 
    /// </summary>
    /// <returns></returns>
    public int NumPlayersTouching()
    {
        return _playerTouching;
    }

    public void SetAnimActivePlayer(bool val)
    {
        _animActivePlayer = val;
    }

    public void SetAnimActiveMinion(bool val)
    {
        _animActiveMinion = val;
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

    public void RestorePointColor()
    {
        Debug.Log("restoring color");
        if (_isSatanPoint)
        {
            SetColour(Color.red);
        }
        else
        {
            SetColour(Color.green);
        }
    }
}