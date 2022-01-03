using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GManager : MonoBehaviour
{
    [SerializeField] private Transform cracks;
    [SerializeField] private float timeToDeactivate = 10f;
    private float _timePassed = 0;
    [SerializeField] private float addTime = 2;
    public MainEnemy kraken;
    private bool _shipHasCracks = false;
    private List<Crack> levelCracks;
    private List<Crack> _repairedCracks;
    private int _curBatch = 0;
    [SerializeField] private int unrepairedCracksCounter = 0;
    [SerializeField] private GameObject borders;
    [SerializeField] private float speedPenaltyTimer = 5;

    [SerializeField] private MainCharacter p1;
    [SerializeField] private MainCharacter p2;

    private List<List<int>> levels = new List<List<int>>();


    public List<Crack> GetRandomTargets()
    {
        var numOfInactivePoints = levelCracks.Count - unrepairedCracksCounter;
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

        var p1 = _repairedCracks[ind1];
        var p2 = _repairedCracks[ind2];
        return new List<Crack>() {p1, p2};
    }
    

    public void DizzyStat(MainCharacter player)
    {
        player.SetSpeed(player.GetSpeed() / 2);
        StartCoroutine(HitPenalty(player));
    }

    public IEnumerator HitPenalty(MainCharacter player)
    {
        yield return new WaitForSeconds(speedPenaltyTimer);
        player.SetSpeed(player.GetSpeed() * 2);
        player.SetDizzy(false);
    }

    void Awake()
    {
        for (int i = 0; i < cracks.childCount; i++)
        {
            cracks.GetChild(i).GetComponent<Crack>().SetId(i);
        }

        List<int> level1 = new List<int>() {0, 8, 16, 24, 32, 40, 48, 6, 12, 18, 30, 36, 42};
        List<int> level2 = new List<int>() {};
        for (int i = 0; i < 7; i ++)
        {
            for (int j = 0; j < 7; j+=2)
            {
                level2.Add(i * 7 + j);
            }
        }
        List<int> level3 = new List<int>() {0, 1, 7, 8, 5, 6, 12, 13, 35, 36, 42, 43, 40, 41, 47, 48, 24, 23, 25, 31, 17};
        levels.Add(level1);
        levels.Add(level2);
        levels.Add(level3);

    }

    void Start()
    {
        levelCracks = new List<Crack>();
        _repairedCracks = new List<Crack>();
        InitCracks(0);
        _curBatch = 0;
    }

    private void InitCracks(int level)
    {
        for (int j = 0; j < levels[level].Count ; j++)
        {
            var cur = cracks.GetChild(levels[level][j]).GetComponent<Crack>();
            levelCracks.Add(cur);
            cur.gameObject.SetActive(true);
            cur.SetInLevel(true);
        }
        _repairedCracks.Clear();
        _shipHasCracks = true;
        unrepairedCracksCounter = levels[level].Count;
    }

    private void LevelCracksReset()
    {
        _timePassed = 0;
        foreach (var crack in levelCracks)
        {
            crack.gameObject.SetActive(false);
            crack.SetInLevel(false);
        }

        levelCracks.Clear();
    }

    void Update()
    {
        if (_shipHasCracks)
        {
            if (unrepairedCracksCounter <= 0 && _curBatch < levels.Count - 1)
            {
                _curBatch++;
                LevelCracksReset();
                _shipHasCracks = false;
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
        InitCracks(_curBatch);
    }

    public GameObject GetBorders()
    {
        return borders;
    }

    public void AxeShot()
    {
        p1.Freeze(true);
        p2.Freeze(true);
    }

    public void AxeHitPlayer()
    {
        p1.Freeze(false);
        p2.Freeze(false);
    }

    public void DecreaseCrackCount()
    {
        unrepairedCracksCounter--;
        print(unrepairedCracksCounter);
    }
    

}


[System.Serializable]
public class PointList
{
    public List<Crack> list;
}

[System.Serializable]
public class PointListL
{
    public List<PointList> list;
}