using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAn : MonoBehaviour
{
    [SerializeField] private Crack crack;
    [SerializeField] private GManager _gm;

    private void Start()
    {
        _gm = FindObjectOfType<GManager>();
    }

    private void PointDeactivate()
    {
        Debug.Log("should call point success");
        // _gm.PlayerPointSuccess(point);
    }
}
