using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrackAttack : MonoBehaviour
{
    [SerializeField] private GManager _gm;
    [SerializeField] private Animator _mouthAn;
    [SerializeField] private Animator _waterBurstAn;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    private float[] _angles = {45, 0, -45};
    private int _counter = 0;
    [SerializeField] private SoundManager sm;
    private bool shouldAttack = false;

    
    private void OnKrakenUp()
    {
        if (!_gm.IsGameOver())
        {
            _mouthAn.SetTrigger("roar");
            sm.PlaySound("roar");            
        }
        if (_gm.IsLastLevel())
        {
            if (!shouldAttack)
            {
                shouldAttack = true;
                return;
            }
            // int rand = UnityEngine.Random.Range(0, 2);
            // Transform target = rand == 0 ? p1 : p1;
            Transform burstTransform = _waterBurstAn.gameObject.transform;
            // Transform mouthTransform = _mouthAn.gameObject.transform;
            
            // // Get Angle in Radians
            // float AngleRad = Mathf.Atan2(target.position.y - mouthTransform.position.y, target.position.x - mouthTransform.position.x);
            // // Get Angle in Degrees
            // float AngleDeg = (180 / Mathf.PI) * AngleRad;
            // // Rotate Object
            // burstTransform.rotation = Quaternion.Euler(0, 0, AngleDeg);
            // // _waterBurstAn.gameObject.transform.LookAt(target.transform, Vector3.right);
            // burstTransform.right = target.position - burstTransform.position;
            
            // burstTransform.rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position);
            burstTransform.rotation = Quaternion.Euler(0, 0, _angles[_counter]);
            _counter = (_counter + 1) % _angles.Length; 
            _waterBurstAn.SetTrigger("burst");
            sm.PlaySound("waterAttack");
        }
    }
}
