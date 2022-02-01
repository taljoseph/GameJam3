using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSM : MonoBehaviour
{
    [SerializeField] private Crack _crack;
    // Start is called before the first frame update
    
    // Update is called once per frame
    public void PlayWaterLeakingSound()
    {
        _crack.PlayWaterSound();
    }
}
