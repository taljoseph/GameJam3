using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour
{
    [SerializeField] private Transform points;
    [SerializeField] private float timeToDeactivate = 10f;
    public GameObject activatorPref;

    private float _timePassed = 0;
    [SerializeField] private float addTime = 2;
    public MainEnemy satan;
    private bool _pointsAvailable = false;
    private List<Point> _levelPoints;
    private List<Point> _playerLevelPoints;
    private int _curBatch = 0;
    [SerializeField] private List<Point> _currPressing = new List<Point>();
    [SerializeField] private List<Point> _currMinionPressing = new List<Point>();
    [SerializeField] private int _satanPointsCounter = 0;

    [SerializeField] private List<PointListL> pointsNeighbours;

    public List<Point> GetRandomTargets()
    {
        var numOfInactivePoints = _levelPoints.Count - _satanPointsCounter;
        if (numOfInactivePoints < 2)
        {
            return null;
        }

        var ind1 = Random.Range(0, numOfInactivePoints);
        var ind2 = Random.Range(0, numOfInactivePoints);
        while (ind1 == ind2)
        {
            ind2 = Random.Range(0, numOfInactivePoints);
        }

        var p1 = _playerLevelPoints[ind1];
        var p2 = _playerLevelPoints[ind2];
        return new List<Point>() {p1, p2};
    }

    public void ShootActivator(Point source, Point dest, Possessor possessor, ActivatorGoal goal)
    {
        var activator = Instantiate(activatorPref, source.transform.position, Quaternion.identity);
        var activatorScript = activator.GetComponent<PointsActivator>();
        activatorScript.SetSource(source);
        activatorScript.SetDestination(dest);
        activatorScript.SetGoal(goal);
        activatorScript.SetPossessor(possessor);
        activatorScript.SetLayer(possessor == Possessor.Player ? 3 : 7);
    }


    public void AddPressing(Point p)
    {
        _currPressing.Add(p);
        p.SetColour(Color.cyan);
        if (_currPressing.Count >= 2)
        {
            foreach (var point in _currPressing)
            {
                point.SetAnimActive(true);
                var an = point.GetPlayerAnimator();
                an.GetComponent<SpriteRenderer>().enabled = true;
                an.Play("timer");
            }

            ShootActivator(p, _currPressing[0], Possessor.Player, ActivatorGoal.StartCapture);
            ShootActivator(_currPressing[0], p, Possessor.Player, ActivatorGoal.StartCapture);
            // var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            // {
            //     for (int i = 0; i < cur.Count; i++)
            //     {
            //         if (cur[i].IsInLevel() && cur[i].IsSatanPoint())
            //         {
            //             cur[i].SetColour(Color.cyan);
            //             
            //         }
            //     }
            // }
        }
    }


    public void AddMinionPressing(Point p)
    {
        //Debug.Log("minion pressing!");
        _currMinionPressing.Add(p);
        p.SetColour(Color.yellow);
        if (_currMinionPressing.Count >= 2)
        {
            foreach (var point in _currMinionPressing)
            {
                point.SetAnimActive(true);
                var an = point.GetMinionAnimator();
                an.GetComponent<SpriteRenderer>().enabled = true;
                an.Play("timer");
            }

            ShootActivator(p, _currMinionPressing[0], Possessor.Satan, ActivatorGoal.StartCapture);
            ShootActivator(_currMinionPressing[0], p, Possessor.Satan, ActivatorGoal.StartCapture);
        }

        // _currMinionPressing.Add(p);
        // //p.SetColour(Color.cyan);
        // if (_currMinionPressing.Count >= 2)
        // {
        //     var cur = pointsNeighbours[_currMinionPressing[0].GetId()].list[_currMinionPressing[1].GetId()].list;
        //     {
        //         satan.DestroyMinions();
        //
        //         RemoveMinionPressing(_currMinionPressing[0]);
        //         RemoveMinionPressing(_currMinionPressing[1]);
        //         _currMinionPressing.Clear();
        //         for (int i = 0; i < cur.Count; i++)
        //         {
        //             if (cur[i].IsInLevel() && !cur[i].IsSatanPoint())
        //             {
        //                 cur[i].SetColour(Color.red);
        //                 cur[i].SetActive(true);
        //                 _satanPointsCounter++;
        //                 _playerLevelPoints.Remove(cur[i]);
        //             }
        //         }
        //     }
        // }
    }


    public void RemovePressing(Point p)
    {
        _currPressing.Remove(p);
        p.SetColour(Color.red);
    }

    public void RemoveMinionPressing(Point p)
    {
        p.MinionLeft();
    }

    public void StopPlayerTimers()
    {
        foreach (var point in _currPressing)
        {
            //Debug.Log("stopping animation");
            var an = point.GetPlayerAnimator();
            an.Rebind();
            an.GetComponent<SpriteRenderer>().enabled = false;

            // point.SetColour(Color.cyan);
        }

        if (_currPressing.Count >= 2)
        {
            ShootActivator(_currPressing[1], _currPressing[0], Possessor.Player, ActivatorGoal.StopCapture);
            ShootActivator(_currPressing[0], _currPressing[1], Possessor.Player, ActivatorGoal.StopCapture);
            // var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            // {
            //     for (int i = 0; i < cur.Count; i++)
            //     {
            //         if (cur[i].IsInLevel() && cur[i].IsSatanPoint() && cur[i] != _currPressing[0] && cur[i] != _currPressing[1])
            //         {
            //             cur[i].SetColour(Color.red);
            //         }
            //     }
            // }
        }
    }

    public void StopMinionTimers()
    {
        foreach (var point in _currMinionPressing)
        {
            var an = point.GetMinionAnimator();
            an.Rebind();
            an.GetComponent<SpriteRenderer>().enabled = false;

            // point.SetColour(Color.cyan);
        }

        if (_currMinionPressing.Count >= 2)
        {
            ShootActivator(_currMinionPressing[1], _currMinionPressing[0], Possessor.Satan, ActivatorGoal.StopCapture);
            ShootActivator(_currMinionPressing[0], _currMinionPressing[1], Possessor.Satan, ActivatorGoal.StopCapture);
            // var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            // {
            //     for (int i = 0; i < cur.Count; i++)
            //     {
            //         if (cur[i].IsInLevel() && cur[i].IsSatanPoint() && cur[i] != _currPressing[0] && cur[i] != _currPressing[1])
            //         {
            //             cur[i].SetColour(Color.red);
            //         }
            //     }
            // }
        }
    }

    
    
    
    /// <summary>
    /// Called when the clock finished its -
    /// </summary>
    /// <param name="point"></param>
    public void PlayerPointSuccess(Point point)
    {
        var an = point.GetPlayerAnimator();
        an.Rebind();
        an.GetComponent<SpriteRenderer>().enabled = false;
        point.SetAnimActive(false);
        //Debug.Log("curr Pressing count: " + _currPressing.Count);
        if (_currPressing.Count >= 2)
        {
            ShootActivator(_currPressing[1], _currPressing[0], Possessor.Player, ActivatorGoal.CompleteCapture);
            ShootActivator(_currPressing[0], _currPressing[1], Possessor.Player, ActivatorGoal.CompleteCapture);
            // var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            // {
            //     for (int i = 0; i < cur.Count; i++)
            //     {
            //         if (cur[i].IsInLevel() && cur[i].IsSatanPoint())
            //         {
            //             cur[i].SetColour(Color.green);
            //             cur[i].SetActive(false);
            //             _satanPointsCounter--;
            //             _playerLevelPoints.Add(cur[i]);
            //         }
            //     }
            // }
        }

        _currPressing.Clear();
    }

    public void MinionGotHit()
    {
        foreach (var point in _currMinionPressing)
        { 
            point.RestorePointColor();
        }
        StopMinionTimers();
        satan.DestroyMinions();
        foreach (var point in _currMinionPressing)
        {
            RemoveMinionPressing(point); 
            
        }
        _currMinionPressing.Clear();
    }
    
    public void MinionPointSuccess(Point point)
    {
        var an = point.GetMinionAnimator();
        an.Rebind();
        an.GetComponent<SpriteRenderer>().enabled = false;
        point.SetAnimActive(false);
        if (_currMinionPressing.Count >= 2)
        {
            ShootActivator(_currMinionPressing[1], _currMinionPressing[0], Possessor.Satan, ActivatorGoal.CompleteCapture);
            ShootActivator(_currMinionPressing[0], _currMinionPressing[1], Possessor.Satan, ActivatorGoal.CompleteCapture);
            satan.DestroyMinions();
            RemoveMinionPressing(_currMinionPressing[0]);
            RemoveMinionPressing(_currMinionPressing[1]);
            _currMinionPressing.Clear();
            // var cur = pointsNeighbours[_currPressing[0].GetId()].list[_currPressing[1].GetId()].list;
            // {
            //     for (int i = 0; i < cur.Count; i++)
            //     {
            //         if (cur[i].IsInLevel() && cur[i].IsSatanPoint())
            //         {
            //             cur[i].SetColour(Color.green);
            //             cur[i].SetActive(false);
            //             _satanPointsCounter--;
            //             _playerLevelPoints.Add(cur[i]);
            //         }
            //     }
            // }
        }

        _currMinionPressing.Clear();
    }
    
    

    public void AddToPlayerPoints(Point p)
    {
        _satanPointsCounter--;
        _playerLevelPoints.Add(p);
    }

    public void RemoveFromPlayerPoints(Point p)
    {
        _satanPointsCounter++;
        _playerLevelPoints.Remove(p);
    }


    void Start()
    {
        _levelPoints = new List<Point>();
        _playerLevelPoints = new List<Point>();
        InitPoints(2);
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
                if (point.NumPlayersTouching() > 0 && point.NumPlayersTouching() != 2)
                {
                    temp.Add(point);
                }
            }

            _playerLevelPoints = new List<Point>();
        }

        foreach (var p in temp)
        {
            AddPressing(p);
        }

        _pointsAvailable = true;
        _satanPointsCounter = _levelPoints.Count;
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
            if (_satanPointsCounter <= 0)
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