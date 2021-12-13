using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private Color defaultColour;
    private bool switchedOn = false;
    private static int pressing = 0;

    public void Awake()
    {
        defaultColour = GetComponent<SpriteRenderer>().color;
    }

    public void SetColour(Color newColour)
    {
        GetComponent<SpriteRenderer>().color = newColour;
    }

    public void SwitchStatus()
    {
        switchedOn = !switchedOn;
    }

    public bool IsOn()
    {
        return switchedOn;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (switchedOn)
        {
            pressing++;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (switchedOn)
        {
            pressing--;
        }
    }

    public static int Pressing()
    {
        return pressing;
    }
}

