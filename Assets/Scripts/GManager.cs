using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{

    [SerializeField] private Transform points;
    [SerializeField] private float timeToDeactivate = 10f;
    [SerializeField] private float addTime = 2; 
    private float _timePassed = 0;
    private bool _pointsAvailable = false;
    private List<Point> _activePoints;
    private List<Point> _deactivatedPoints;
    private int _curBatch = 0;
    
    void Start()
    {
        _activePoints = new List<Point>();
        _deactivatedPoints = new List<Point>();
        ActivatePoints(points.GetChild(_curBatch));
        _curBatch = (_curBatch + 1) % points.childCount;
        _pointsAvailable = true;
    }

    private void ActivatePoints(Transform level)
    {
        for (int i = 0; i < level.transform.childCount; i++)
        {
            var point = level.GetChild(i).GetComponent<Point>();
            _activePoints.Add(point);
            point.SetActive(true);
            point.SetColour(Color.red);
        }
    }

    private void DeactivatePoints()
    {
        _timePassed = 0;
        foreach (var point in _deactivatedPoints)
        {
            point.SetColour(Color.white);
            point.SetActive(false);
        }
        _deactivatedPoints.Clear();
    }
    
    

    void Update()
    {
        print("Time Passed: " + _timePassed);
        print("Points pressed: " + Point.Pressing());
        if (_pointsAvailable)
        {
            if (Point.Pressing() >= 2)
            {
                ApplyTimer();
            }
            else if (Point.Pressing() < 2 && _timePassed != 0)
            {
                _timePassed = 0;
            }
            if (_timePassed >= timeToDeactivate)
            {
                for (int i = _activePoints.Count - 1; i >= 0; i--)
                {
                    if (_activePoints[i].IsPressed())
                    {
                        _activePoints[i].SetActive(false);
                        _activePoints[i].SetPressing(false);
                        _activePoints[i].SetColour(Color.green);
                        _deactivatedPoints.Add(_activePoints[i]);
                        _activePoints.RemoveAt(i);
                    }
                    Point.ResetTotalPressing();
                }
            }
            if (_activePoints.Count == 0)
            {
                DeactivatePoints();
                _pointsAvailable = false;
                StartCoroutine(StartNextBatch());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
            Point.ResetTotalPressing();
        }
        
        
    }

    private IEnumerator StartNextBatch()
    {
        yield return new WaitForSeconds(5);
        ActivatePoints(points.GetChild(_curBatch));
        _curBatch = (_curBatch + 1) % points.childCount;
        _pointsAvailable = true;
    }

    private void ApplyTimer()
    {
        _timePassed += Time.deltaTime * addTime;
    }
}
