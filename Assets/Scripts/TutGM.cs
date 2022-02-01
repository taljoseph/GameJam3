using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using TMPro;

public class TutGM : MonoBehaviour
{
    [SerializeField] private Transform cracks;
    [SerializeField] private float timeToDeactivate = 10f;
    private float _timePassed = 0;
    [SerializeField] private float addTime = 2;
    public TutME kraken;
    private bool _shipHasCracks = false;
    private List<Crack> allCracks;
    private List<Crack> _inactiveCracks;
    private int _curBatch = 0;
    [SerializeField] private int activeCracksCounter = 0;
    [SerializeField] private GameObject borders;
    [SerializeField] private float speedPenaltyTimer = 5;

    [SerializeField] private TutMC p1;
    [SerializeField] private TutMC p2;
    
    [SerializeField] private List<int> drowningThresholds;
    [SerializeField] private List<int> hitsPerLevel;
    private int _curHits = 0;
    private List<List<int>> levels = new List<List<int>>();
    [SerializeField] private Transform camera;
    private GameObject _curAxe = null;
    [SerializeField] private float characterSpawnTime = 3;
    [SerializeField] private SoundManager sm;

    [SerializeField] private Animator smt;
    [SerializeField] private Animator pressStart;
    [SerializeField] private Animator black;
    [SerializeField] private int congestionIterations;
    [SerializeField] private GameObject ui1;
    private Animator _instructionsAn;
    [SerializeField] private GameObject quitSkip;


    
    //TUT STUFF
    private Vector3 _posP1;
    private Vector3 _posP2;
    private int _totalPasses = 0;
    private int _totalCracksFixed = 0;
    private List<bool> _tutLevel = new List<bool>(){false,false,false,false};
    private bool _inTutMenu = true;
    private int _curLevel = 0;
    private bool _spTentActive = false;
    private Crack _spTentCrack = null;
    

    public void DizzyStat(TutMC player)
    {
        player.SetSpeed(player.GetSpeed() / 2);
        StartCoroutine(HitPenalty(player));
    }

    public IEnumerator HitPenalty(TutMC player)
    {
        yield return new WaitForSeconds(speedPenaltyTimer);
        player.SetSpeed(player.GetSpeed() * 2);
        player.SetDizzy(false);
    }

    void Awake()
    {
        _posP1 = p1.transform.position;
        _posP2 = p2.transform.position;
        levels.Add(new List<int>(){3,4,8,9});
        levels.Add(new List<int>(){1,2,3,4,5,6,7});
        allCracks = new List<Crack>();
        _inactiveCracks = new List<Crack>();
        for (int i = 0; i < cracks.childCount; i++)
        {
            Crack crack = cracks.GetChild(i).GetComponent<Crack>();
            crack.SetId(i);
            allCracks.Add(crack);
        }

        _instructionsAn = ui1.GetComponent<Animator>();
    }

    private IEnumerator TransitionToTutorial()
    {
        smt.SetTrigger("startPressed");
        yield return new WaitForSeconds(1);
        pressStart.SetTrigger("startPressed");
        ui1.SetActive(true);
        quitSkip.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _inTutMenu = false;
        p1.SetFrozen(false);
        p2.SetFrozen(false);
        TutLevel0();
    }
    
    private void TutLevel0()
    {
        foreach (int child in levels[0])
        {
            _inactiveCracks.Add(allCracks[child]);;
        }
        activeCracksCounter = 0;
        _tutLevel[0] = true;
    }

    private void TutLevel1()
    {
        _instructionsAn.SetTrigger("closeMessage");
        p1.Freeze(true);
        p2.Freeze(true);
        p1.ResetChar();
        p2.ResetChar();
        StartCoroutine(kraken.TentAttackTut1());
        // add code for Text
    }

    private void TutLevel2()
    {
        _instructionsAn.SetTrigger("closeMessage");
        _inactiveCracks.Clear();
        foreach (int child in levels[1])
        {
            if (child == 4)
            {
               allCracks[child].SetShouldSplash(false); 
            }
            _inactiveCracks.Add(allCracks[child]);;
        }
        _curLevel++;
        activeCracksCounter = 0;
        p1.Freeze(true);
        p2.Freeze(true);
        p1.ResetChar();
        p2.ResetChar();
        StartCoroutine(kraken.TentAttackTut2());
    }

    public void TutAttack1End()
    {
        p1.Freeze(false);
        p2.Freeze(false);
    }





    void Update()
    {
        for (int i = 0; i < congestionIterations; i++)
        {
            Debug.Log("crash unity");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Tutorial");

        }
        camera.transform.rotation = Quaternion.Euler(
            0*Mathf.Sin(Time.realtimeSinceStartup),
            0, 
            Mathf.Sin(Time.realtimeSinceStartup) * 0.5f);

        if (_inTutMenu)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(TransitionToTutorial());
            }
        }
        if (_tutLevel[0])
        {
            if (_totalPasses == 4)
            {
                _tutLevel[0] = false;
                _tutLevel[1] = true;
                TutLevel1();
            }
        }
        else if (_tutLevel[1])
        {
            if (_totalCracksFixed == levels[0].Count)
            {
                _tutLevel[1] = false;
                _tutLevel[2] = true;
                TutLevel2();
            }
        }
        else if (_tutLevel[2])
        {
            if (_spTentActive)
            {
                if (!_spTentCrack.GetComponent<SpriteRenderer>().enabled)
                {
                    StartCoroutine("BlackAndLoad");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("shlomiScene");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Application.Quit();
        }
    }

    public IEnumerator BlackAndLoad()
    {
        _instructionsAn.SetTrigger("closeMessage");
        black.SetTrigger("blackIn");
        camera.DOShakePosition(1, Vector3.right * 0.2f, 20, 0, fadeOut: false);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("shlomiScene");   
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
        _totalPasses++;
    }

    public void CrackFix(Crack cr)
    {
        activeCracksCounter--;
        _inactiveCracks.Add(cr);
        _totalCracksFixed++;

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

    public Crack GetCrackAt(int index)
    {
        return _inactiveCracks[index];
    }

    public void AxeHitTentacle()
    {
        sm.PlaySound("tentacleHit");
        sm.PlaySound("hitReaction");
        //kraken.SetDizzy(true);
        //StartCoroutine(kraken.HitPenalty());
    }


    public void AddToActiveCounter(int num)
    {
        activeCracksCounter += num;
        Debug.Log(activeCracksCounter);
    }

    public void CharacterDied(TutMC playerScript, Collider2D col, Rigidbody2D rb, Animator an)
    {
        if (playerScript.player.Equals("A"))
        {
            sm.PlaySound("player1Death");
        }
        else
        {
            sm.PlaySound("player2Death");
        }
        playerScript.SetDead(true);
        col.enabled = false;
        an.SetTrigger("hit");
        rb.velocity = Vector2.zero;
        if (_curAxe != null) // Axe was thrown 
        {
            Destroy(_curAxe);
            _curAxe = null;
        }
        TutMC handAxeTo = playerScript.Equals(p1) ? p2 : p1;
        handAxeTo.SetHasAxe(true);
        playerScript.SetHasAxe(false);
    }

    public void CharHitPenalty(TutMC playerScript, GameObject playerGO, SpriteRenderer sr, Collider2D col, Transform p2Trans)
    {
        playerGO.SetActive(false);
        StartCoroutine(CharacterRespawn(playerScript, playerGO, sr, col, p2Trans));
    }
    
    public IEnumerator CharacterRespawn(TutMC playerScript, GameObject playerGO, SpriteRenderer sr, Collider2D col, Transform p2Trans)
    {
        yield return new WaitForSeconds(characterSpawnTime);
        playerGO.SetActive(true);
        playerScript.SetInvincible(true);
        playerGO.transform.position = p2Trans.position;
        col.enabled = true;
        playerScript.SetDead(false);
        sm.PlaySound("playerRespawn");
        for (int i = 0; i < 14; i++)
        {
            yield return new WaitForSeconds(0.1f);
            sr.enabled = !sr.enabled;
        }
        playerScript.SetInvincible(false);
    }

    public int GetTotalPasses()
    {
        return _totalPasses;
    }

    public int GetNumCracks()
    {
        return levels[_curLevel].Count;
    }

    public void SpTentActiveTrue(Crack cr)
    {
        _spTentActive = true;
        _spTentCrack = cr;
    }
}