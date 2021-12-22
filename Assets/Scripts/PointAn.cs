using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAn : MonoBehaviour
{
    [SerializeField] private Point point;
    [SerializeField] private GManager _gm;

    private void PointDeactivate()
    {
        _gm.PointSuccess(point);
    }
}
