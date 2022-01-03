using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Crack : MonoBehaviour
{
    [SerializeField] private Color _defaultColour;
    [SerializeField] private bool _inLevel = false;
    [SerializeField] private GManager _gm;
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

    public void SetId(int val)
    {
        id = val;
    }
}