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
    private bool _normalTentActive = false;
    private bool _specialTentActive = false;
    private Animator _an;
    public int id;


    public void Awake()
    {
        _defaultColour = GetComponent<SpriteRenderer>().color;
        _an = GetComponent<Animator>();
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

    public void SetNormalTent(int val)
    {
        _normalTentActive = (val == 1);
    }

    public void SetSpecialTent(int val)
    {
        _specialTentActive = (val == 1);
    }

    public bool GetNormalTentActive()
    {
        return _normalTentActive;
    }

    public bool GetSpecialTentActive()
    {
        return _specialTentActive;
    }

    public void CloseCrack()
    {
        _an.SetTrigger("fixed");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    public bool AnPlaying(String name)
    {
        return _an.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void StartAnAtState(int state)
    {
        _an.SetInteger("state", state);
    }
}