using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{

    [SerializeField] private GameObject points;
    [SerializeField] private float timeToDeactivate = 10f;
    private float timePassed = 0;
    

    private List<Point> Points;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Point.Pressing() >= 2)
        {
            ApplyTimer();
        }
        else
        {
            timePassed = 0;
        }
    }

    private void ApplyTimer()
    {
        timePassed += Time.deltaTime * timeToDeactivate;
    }
}
