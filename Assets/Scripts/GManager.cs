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
    private List<Crack> allCracks;
    private List<Crack> _inactiveCracks;
    private int _curBatch = 0;
    [SerializeField] private int activeCracksCounter = 0;
    [SerializeField] private GameObject borders;
    [SerializeField] private float speedPenaltyTimer = 5;

    [SerializeField] private MainCharacter p1;
    [SerializeField] private MainCharacter p2;
    [SerializeField] private List<int> drowningThresholds;
    [SerializeField] private List<int> hitsPerLevel;
    private int _curHits = 0;
    private List<int> levels = new List<int>();
    


    public List<Crack> GetRandomTargets()
    {
        var numOfInactivePoints = allCracks.Count - activeCracksCounter;
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

        var p1 = _inactiveCracks[ind1];
        var p2 = _inactiveCracks[ind2];
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
        allCracks = new List<Crack>();
        _inactiveCracks = new List<Crack>();
        for (int i = 0; i < cracks.childCount; i++)
        {
            Crack crack = cracks.GetChild(i).GetComponent<Crack>();
            crack.SetId(i);
            crack.SetInLevel(true);
            
            _inactiveCracks.Add(crack);
            allCracks.Add(crack);

        }
        activeCracksCounter = 0;


    }

    

    // private void InitCracks(int level)
    // {
    //     for (int j = 0; j < levels[level].Count ; j++)
    //     {
    //         var cur = cracks.GetChild(levels[level][j]).GetComponent<Crack>();
    //         allCracks.Add(cur);
    //         cur.SetInLevel(true);
    //     }
    //     _shipHasCracks = true;
    //     activeCracksCounter = 0;
    // }

    

    void Update()
    {
        if (_curBatch <= drowningThresholds.Count - 1)
        {
            //Debug.Log(_curBatch);
            
            if (activeCracksCounter >= drowningThresholds[_curBatch])
            {
                SceneManager.LoadScene("Main Menu");
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }            
        }

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

    public void CrackFix(Crack cr)
    {
        _inactiveCracks.Add(cr);
        activeCracksCounter--;
    }

    public Crack GetCrack()
    {
        if (_inactiveCracks.Count > 0)
        {
            int targetInd = Random.Range(0, _inactiveCracks.Count);
            Crack target = _inactiveCracks[targetInd];
            _inactiveCracks.RemoveAt(targetInd);
            activeCracksCounter++;
            return target;
        }
        return null;
    }

    public void AxeHitTentacle()
    {
        _curHits++;
        if (_curHits >= hitsPerLevel[_curBatch])
        {
            _curHits = 0;
            _curBatch++;
            Debug.Log("advanced to next level!");
            if (_curBatch >= drowningThresholds.Count)
            {
                SceneManager.LoadScene("Main Menu");
            }
            kraken.AdvanceToNextLevel();
            // move to next level
        }
        
        
        //kraken.SetDizzy(true);
        //StartCoroutine(kraken.HitPenalty());
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