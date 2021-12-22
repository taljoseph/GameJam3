using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{

    [SerializeField] private Transform points;
    [SerializeField] private float timeToDeactivate = 10f;
    private float _timePassed = 0;
    [SerializeField] private float addTime = 2; 
    private bool _pointsAvailable = false;
    private List<Point> _levelPoints;
    private int _curBatch = 0;
    private List<Point> _currPressing  = new List<Point>();
    private int _activePointsCounter = 0;
    [SerializeField] private List<PointListL> pointsNeighbours;

    public void AddPressing(Point p)
    {
        _currPressing.Add(p);
        p.SetColour(Color.cyan);
        if (_currPressing.Count >= 2)
        {
            foreach (var point in _currPressing)
            {
                point.SetAnimActive(true);
                var an = point.GetAnimator();
                an.GetComponent<SpriteRenderer>().enabled = true;
                an.Play("timer");
            }
            var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            {
                for (int i = 0; i < cur.Count; i++)
                {
                    if (cur[i].IsInLevel() && cur[i].IsOn())
                    {
                        cur[i].SetColour(Color.cyan);
                    }
                }
            }
            
            
        } 
    }

    public void RemovePressing(Point p)
    {
        _currPressing.Remove(p);
        p.SetColour(Color.red);
    }

    public void StopTimers()
    {
        foreach (var point in _currPressing)
        {
            var an = point.GetAnimator();
            an.Rebind();
            an.GetComponent<SpriteRenderer>().enabled = false;

            // point.SetColour(Color.cyan);
        }

        if (_currPressing.Count >= 2)
        {
            var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            {
                for (int i = 0; i < cur.Count; i++)
                {
                    if (cur[i].IsInLevel() && cur[i].IsOn() && cur[i] != _currPressing[0] && cur[i] != _currPressing[1])
                    {
                        cur[i].SetColour(Color.red);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Called when the clock finished its loop
    /// </summary>
    /// <param name="point"></param>
    public void PointSuccess(Point point)
    {
        var an = point.GetAnimator();
        an.Rebind();
        an.GetComponent<SpriteRenderer>().enabled = false;
        point.SetAnimActive(false);
        if (_currPressing.Count >= 2)
        {
            var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            {
                for (int i = 0; i < cur.Count; i++)
                {
                    if (cur[i].IsInLevel() && cur[i].IsOn())
                    {
                        cur[i].SetColour(Color.green);
                        cur[i].SetActive(false); 
                        _activePointsCounter--;
                    }
                }
            }
        }
        _currPressing.Clear();
    }
    
    
    void Start()
    {
        _levelPoints = new List<Point>();
        InitPoints(0);
        _curBatch = (_curBatch + 1) % points.childCount;
    }

    private void InitPoints(int round)
    {
        var temp = new List<Point>();
        for (int j = 0; j < round + 1; j++)
        {
            var curCircle = points.GetChild(j);
            for (int i = 0; i < curCircle.childCount; i++)
            {
                var point = curCircle.GetChild(i).GetComponent<Point>();
                _levelPoints.Add(point);
                point.SetActive(true);
                point.SetColour(Color.red);
                point.SetInLevel(true);
                if (point.NumTouching() > 0 && point.NumTouching() != 2)
                {
                    temp.Add(point);
                }
            }
        }
        foreach (var p in temp)
        {
            AddPressing(p);
        }
        _pointsAvailable = true;
        _activePointsCounter = _levelPoints.Count;
    }

    private void LevelPointsReset()
    {
        _timePassed = 0;
        foreach (var point in _levelPoints)
        {
            point.SetColour(Color.white);
            point.SetInLevel(false);
        }
        _levelPoints.Clear();
    }
    
    void Update()
    {
        if (_pointsAvailable)
        {
            // if (Point.Pressing() >= 2)
            // {
            //     List<Point> nowOn = new List<Point>();
            //     foreach (var point in _activePoints)
            //     {
            //         if (point.NumTouching() >= 1)
            //         {
            //             nowOn.Add(point);
            //         }
            //     }
            // }
            
            // if (Point.Pressing() >= 2)
            // {
            //     ApplyTimer();
            // }
            // else if (Point.Pressing() < 2 && _timePassed != 0)
            // {
            //     _timePassed = 0;
            // }
            // if (_timePassed >= timeToDeactivate)
            // {
            //     for (int i = _activePoints.Count - 1; i >= 0; i--)
            //     {
            //         if (_activePoints[i].IsPressed())
            //         {
            //             _activePoints[i].SetActive(false);
            //             _activePoints[i].SetPressing(false);
            //             _activePoints[i].SetColour(Color.green);
            //             _deactivatedPoints.Add(_activePoints[i]);
            //             _activePoints.RemoveAt(i);
            //         }
            //         Point.ResetTotalPressing();
            //     }
            // }
            if (_activePointsCounter <= 0)
            {
                LevelPointsReset();
                _pointsAvailable = false;
                StartCoroutine(StartNextBatch());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("SampleScene");
        }
        
        
    }

    private IEnumerator StartNextBatch()
    {
        yield return new WaitForSeconds(5);
        InitPoints(_curBatch);
        _curBatch = (_curBatch + 1) % points.childCount;
    }

    private void ApplyTimer()
    {
        _timePassed += Time.deltaTime * addTime;
    }
}


[System.Serializable]
public class PointList
{
    public List<Point> list;
}

[System.Serializable]
public class PointListL
{
    public List<PointList> list;
}