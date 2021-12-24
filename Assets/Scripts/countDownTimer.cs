using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countDownTimer : MonoBehaviour
{
    public Text timetText;
    public float startingTime = 25f;
    private float curTime = 0f;
    private bool flag = true;
    private GManager gameManagerScript;
   

    private void Start()
    {
        gameManagerScript = FindObjectOfType<GManager>();
        curTime = startingTime;
    }

    private void Update()
    {
        curTime -= 1 * Time.deltaTime;
        timetText.text = curTime.ToString("0");
        if (curTime <= 0)
        {
            //gameManagerScript.endGame();
        }
    }
}