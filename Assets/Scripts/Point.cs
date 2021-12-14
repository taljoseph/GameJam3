using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    private Color defaultColour;
    private bool switchedOn = false;
    private bool pressing = false;
    private static int totalPressing = 0;
    private int touching = 0;

    public void Awake()
    {
        defaultColour = GetComponent<SpriteRenderer>().color;
    }

    public void SetColour(Color newColour)
    {
        GetComponent<SpriteRenderer>().color = newColour;
    }

    public void SetActive(bool value)
    {
        switchedOn = value;
    }

    public bool IsOn()
    {
        return switchedOn;
    }

    public bool IsPressed()
    {
        return pressing;
    }

    public void SetPressingUp(bool value)
    {
        pressing = value;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        touching++;
        SetPressingUp();
    }

    public void SetPressingUp()
    {
        if (switchedOn)
        {
            totalPressing++;
            pressing = true;
        }
    }

    public void SetPressingDown()
    {
        if (switchedOn)
        {
            totalPressing--;
            pressing = false;
        }
    }
    

    public void OnTriggerExit2D(Collider2D other)
    {
        touching--;
        SetPressingDown();
    }

    public static int Pressing()
    {
        return totalPressing;
    }

    public static void ResetTotalPressing()
    {
        totalPressing = 0;
    }

    public int NumTouching()
    {
        return touching;
    }
}

