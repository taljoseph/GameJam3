using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
    [SerializeField] private Transform camera;
    private GameObject _curAxe = null;
    [SerializeField] private float characterSpawnTime = 3;
    


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
                SceneManager.LoadScene("Lose Scene"); //lose screen
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }            
        }

        camera.transform.rotation = Quaternion.Euler(
            Mathf.Sin(Time.realtimeSinceStartup),
            0, 
            Mathf.Sin(Time.realtimeSinceStartup) * 0.5f);
    }

    public GameObject GetBorders()
    {
        return borders;
    }

    public void AxeShot(GameObject obj)
    {
        _curAxe = obj;
    }

    public void AxeHitPlayer()
    {
        _curAxe = null;
    }

    public void CrackFix(Crack cr)
    {
        activeCracksCounter--;
        _inactiveCracks.Add(cr);
        
    }

    public Crack GetCrack()
    {
        if (_inactiveCracks.Count > 0)
        {
            int targetInd = Random.Range(0, _inactiveCracks.Count);
            Crack target = _inactiveCracks[targetInd];
            _inactiveCracks.RemoveAt(targetInd);
            //activeCracksCounter++;
            return target;
        }
        return null;
    }

    public void AxeHitTentacle()
    {
        _curHits++;
        camera.DOShakePosition(0.5f, Vector3.right * 0.2f, 20, 0, fadeOut: false);
        if (_curHits >= hitsPerLevel[_curBatch])
        {
            _curHits = 0;
            _curBatch++;
            Debug.Log("advanced to next level!");
            if (_curBatch >= drowningThresholds.Count)
            {
                SceneManager.LoadScene("Win Scene");
            }
            kraken.AdvanceToNextLevel();
            // move to next level
        }
        
        
        //kraken.SetDizzy(true);
        //StartCoroutine(kraken.HitPenalty());
    }


    public void AddToActiveCounter(int num)
    {
        activeCracksCounter += num;
        Debug.Log(activeCracksCounter);
    }

    public void CharacterDied(MainCharacter playerScript, Collider2D col, Rigidbody2D rb, Animator an)
    {
        playerScript.SetDead(true);
        col.enabled = false;
        an.SetTrigger("hit");
        rb.velocity = Vector2.zero;
        if (_curAxe != null) // Axe was thrown 
        {
            Destroy(_curAxe);
            _curAxe = null;
        }
        MainCharacter handAxeTo = playerScript.Equals(p1) ? p2 : p1;
        handAxeTo.SetHasAxe(true);
        playerScript.SetHasAxe(false);
    }

    public void CharHitPenalty(MainCharacter playerScript, GameObject playerGO, SpriteRenderer sr, Collider2D col, Transform p2Trans)
    {
        playerGO.SetActive(false);
        StartCoroutine(CharacterRespawn(playerScript, playerGO, sr, col, p2Trans));
    }
    
    public IEnumerator CharacterRespawn(MainCharacter playerScript, GameObject playerGO, SpriteRenderer sr, Collider2D col, Transform p2Trans)
    {
        yield return new WaitForSeconds(characterSpawnTime);
        playerGO.SetActive(true);
        playerScript.SetInvincible(true);
        playerGO.transform.position = p2Trans.position;
        col.enabled = true;
        playerScript.SetDead(false);
        for (int i = 0; i < 14; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sr.enabled = !sr.enabled;
        }
        playerScript.SetInvincible(false);
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