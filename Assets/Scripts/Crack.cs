using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Crack : MonoBehaviour
{
    [SerializeField] private Color _defaultColour;
    [SerializeField] private SpriteRenderer child_sr;
    [SerializeField] private SpriteRenderer holecap;
    [SerializeField] private bool _inLevel = false;
    [SerializeField] private GManager _gm;
    private bool _normalTentActive = false;
    private bool _specialTentActive = false;
    private Animator _an;
    public int id;
    private bool _closingCrack = false;
    private SpriteRenderer _sr;
    private Collider2D _col;
    [SerializeField] private ParticleSystem _waterParticles;
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private ParticleSystem _woodParticles;
    [SerializeField] private Animator waterAn;
    [SerializeField] private float splashWaitingTime = 3;
    [SerializeField] private SoundManager sm;



    public void Awake()
    {
        _defaultColour = GetComponent<SpriteRenderer>().color;
        _an = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _col = GetComponent<Collider2D>();
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
        sm.PlaySound("crackClose");
        _closingCrack = true;
        waterAn.SetTrigger("finish");
    }

    public void Deactivate()
    {
        // gameObject.SetActive(false);
        _an.enabled = false;
        _sr.enabled = false;
        _col.enabled = false;
        _closingCrack = false;
        waterAn.enabled = false;
        waterAn.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //TODO UNCOMMENT IF USING PARTICLES
        // if (_waterParticles.isPlaying)
        // {
        //     _waterParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        // }
        // else
        // {
        //     StopCoroutine("SplashWater");
        // }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        _an.enabled = true;
        _an.Rebind();
        _sr.enabled = true;
        _col.enabled = true;
        waterAn.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        waterAn.enabled = true;
        waterAn.Rebind();
        _smokeParticles.Play();
        _woodParticles.Play();
        // child_sr.enabled = true;
        sm.PlaySound("crackOpen");
        // StartCoroutine("SplashWater");
    }

    private IEnumerator SplashWater()
    {
        yield return new WaitForSeconds(splashWaitingTime);
        //TODO UNCOMMENT IF USING PARTICLES
        // _waterParticles.Play();
        waterAn.SetTrigger("start");
        sm.PlaySound("waterLeaking");
    }

    public bool AnPlaying(String name)
    {
        return _an.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void StartAnAtState(int state)
    {
        _an.SetInteger("state", state);
    }

    public void SetChildSpriteActive(int val)
    {
        child_sr.enabled = (val == 1);
        holecap.enabled = (val == 1);
    }

    public bool IsClosingCrack()
    {
        return _closingCrack;
    }


}