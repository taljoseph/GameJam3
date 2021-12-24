using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAnMinion : MonoBehaviour
{
    [SerializeField] private Point point;
    [SerializeField] private GManager _gm;

    private void Start()
    {
        _gm = FindObjectOfType<GManager>();
    }

    private void PointDeactivate()
    {
        //Debug.Log("should call point minion success");
        _gm.MinionPointSuccess(point);
    }
}