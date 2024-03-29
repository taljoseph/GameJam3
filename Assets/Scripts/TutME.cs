using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Serialization;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class TutME : MonoBehaviour
{
    [SerializeField] private TutGM _gm;

    public TutMC player1;
    public TutMC player2;
    public GameObject bulletPref;
    public GameObject minionPref;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float timeBetweenShots = 5f;
    [SerializeField] private float timeBetweenMinions = 10f;
    [SerializeField] private float timeBetweenSwipeAttacks = 20f;
    [SerializeField] private Vector3 shootingDirection;
    private List<Crack> _minionTargets;
    public List<Minion> _minions = new List<Minion>();
    private bool _areMinionsActive;

    [FormerlySerializedAs("handA")] [SerializeField]
    private Transform handR;

    [FormerlySerializedAs("handB")] [SerializeField]
    private Transform handL;

    private bool _rotateRight = false;
    private bool _rotateLeft = false;
    private Quaternion _defRotationRight;
    private Quaternion _defRotationLeft;
    private bool handAttack = false;
    [SerializeField] private float handSpeed = 90;
    [SerializeField] private float timeBetweenHands = 2;
    [SerializeField] private GameObject indicatorPref;
    [SerializeField] private GameObject tentaclePref;
    [SerializeField] private GameObject vulTentPref;
    private bool dizzy = false;
    private int _curLevel = 0;
    [SerializeField] private List<float> normalTentacleWaitingTime;
    [SerializeField] private List<float> specialTentacleWaitingTime;
    [SerializeField] private List<float> normalTentacleAttackSize;
    [SerializeField] private List<float> specialTentacleAttackSize;
    [SerializeField] private Animator eyeAnimator;
    [SerializeField] private Animator bodyAnimator;
    private bool _angry = false;
    // private float _totalAngryTime = 0;
    [SerializeField] private SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        _defRotationRight = handR.rotation;
        _defRotationLeft = handL.rotation;
        GameObject borders = _gm.GetBorders();
        for (int i = 0; i < borders.transform.childCount; i++)
        {
            Physics2D.IgnoreCollision(handR.GetChild(0).GetComponent<Collider2D>(),
                borders.transform.GetChild(i).GetComponent<Collider2D>(), true);
            Physics2D.IgnoreCollision(handL.GetChild(0).GetComponent<Collider2D>(),
                borders.transform.GetChild(i).GetComponent<Collider2D>(), true);
        }
    }


    // Update is called once per frame

    

    private void InitMinions()
    {
        GameObject minion1 = Instantiate(minionPref, firePoint.transform.position, Quaternion.identity);
        GameObject minion2 = Instantiate(minionPref, firePoint.transform.position, Quaternion.identity);
        Minion minion1Script = minion1.GetComponent<Minion>();
        Minion minion2Script = minion2.GetComponent<Minion>();
        minion1Script.SetTarget(_minionTargets[0]);
        minion2Script.SetTarget(_minionTargets[1]);
        _areMinionsActive = true;
        _minions.Add(minion1Script);
        _minions.Add(minion2Script);
    }

    public void DestroyMinions()
    {
        if (_minions.Count >= 2 && _minions[1] != null)
        {
            Destroy(_minions[1].gameObject);
        }

        if (_minions.Count >= 1 && _minions[0] != null)
        {
            Destroy(_minions[0].gameObject);
        }

        _minions.Clear();
        _areMinionsActive = false;
    }

    private void SetShootingDirection()
    {
        TutMC target = Random.Range(0, 2) == 0 ? player1 : player2;
        shootingDirection = (target.transform.position - firePoint.transform.position).normalized;
    }

    private IEnumerator SimpleShot()
    {
        while (true)
        {
            if (!handAttack)
            {
                SetShootingDirection();
                InitBullet();
            }

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void InitBullet()
    {
        GameObject bullet = Instantiate(bulletPref, firePoint.transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetVelocityDirection(shootingDirection);
    }

    private IEnumerator HandAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenHands);
            handAttack = true;
            // int shakeTime = 3;
            // GetComponent<Transform>().DOShakePosition(shakeTime, Vector3.right * 0.2f, 20, 0, fadeOut: false);
            // yield return new WaitForSeconds(shakeTime);
            // handA.GetComponent<Rigidbody2D>().DORotate(270f, 3.5f);
            _rotateRight = true;
            yield return new WaitForSeconds(timeBetweenHands);
            // handB.GetComponent<Rigidbody2D>().DORotate(-450f, 3.5f);
            _rotateLeft = true;
            yield return new WaitForSeconds(2);
            handAttack = false;
        }
    }

    public void FixedUpdate()
    {
        if (_rotateRight)
        {
            if (handR.rotation.eulerAngles.z > (_defRotationRight.eulerAngles.z + 180) % 360 &&
                handR.rotation.eulerAngles.z < _defRotationRight.eulerAngles.z)
            {
                _rotateRight = false;
                handR.rotation = _defRotationRight;
            }


            handR.rotation =
                Quaternion.Euler(handR.rotation.eulerAngles + Vector3.forward * handSpeed * Time.deltaTime);
        }

        if (_rotateLeft)
        {
            if (handL.rotation.eulerAngles.z < (_defRotationLeft.eulerAngles.z - 180))
            {
                _rotateLeft = false;
                handL.rotation = _defRotationLeft;
            }

            handL.rotation =
                Quaternion.Euler(handL.rotation.eulerAngles + Vector3.forward * -handSpeed * Time.deltaTime);
        }
    }

    public IEnumerator VulTentAttack()
    {
        while (true)
        {
            if (dizzy)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(specialTentacleWaitingTime[_curLevel]);
                for (int i = 0; i < specialTentacleAttackSize[_curLevel]; i++)
                {
                    Crack objective = _gm.GetCrack();
                    if (objective)
                    {
                        StartCoroutine(SingleVulTentAttack(objective));
                    }
                }
            }
        }
    }

    public IEnumerator SingleVulTentAttack(Crack objective)
    {
        Vector3 pos = objective.transform.position;
        var indicator = Instantiate(indicatorPref, pos, Quaternion.identity);
        indicator.transform.parent = objective.transform.parent;
        sm.PlaySound("crackingIndicator");
        indicator.SetActive(true);
        yield return new WaitForSeconds(2);
        // yield return new WaitForSeconds(0.2f);
        // indicator.SetActive(false);
        // yield return new WaitForSeconds(0.2f);
        // indicator.SetActive(true);
        // yield return new WaitForSeconds(0.2f);
        Destroy(indicator);
        // objective.gameObject.SetActive(true);
        objective.Activate();
        objective.SetChildSpriteActive(1);
        objective.StartAnAtState(1);
        objective.GetComponent<Animator>().SetBool("tut", true);
        _gm.SpTentActiveTrue(objective);
        _gm.AddToActiveCounter(1);
    }

    public IEnumerator SingleTentacleAttack(Crack objective)
    {
        Vector3 pos = objective.transform.position;
        var indicator = Instantiate(indicatorPref, pos, Quaternion.identity);
        indicator.transform.parent = objective.transform.parent;
        sm.PlaySound("crackingIndicator");
        indicator.SetActive(true);
        yield return new WaitForSeconds(2);
        // yield return new WaitForSeconds(0.2f);
        // indicator.SetActive(false);
        // yield return new WaitForSeconds(0.2f);
        // indicator.SetActive(true);
        // yield return new WaitForSeconds(0.2f);
        Destroy(indicator);
        // objective.gameObject.SetActive(true);
        objective.Activate();
        objective.SetChildSpriteActive(1);
        objective.StartAnAtState(0);
        _gm.AddToActiveCounter(1);
    }


    public IEnumerator TentacleAttack()
    {
        while (true)
        {
            if (dizzy)
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(normalTentacleWaitingTime[_curLevel]);
                for (int i = 0; i < normalTentacleAttackSize[_curLevel]; i++)
                {
                    Crack objective = _gm.GetCrack();
                    if (objective)
                    {
                        StartCoroutine(SingleTentacleAttack(objective));
                    }
                }
            }
        }
    }

    public IEnumerator TentAttackTut1()
    {
        for (int i = 0; i < _gm.GetNumCracks(); i++)
        {
            Crack objective = _gm.GetCrack();
            if (objective)
            {
                StartCoroutine(SingleTentacleAttack(objective));
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        _gm.TutAttack1End();
    }

    public IEnumerator TentAttackTut2()
    {
        for (int i = 0; i < _gm.GetNumCracks(); i++)
        {
            Crack objective = _gm.GetCrackAt(i);
            if (i != 3)
            {
                StartCoroutine(SingleTentacleAttack(objective));
                yield return new WaitForSeconds(0.5f);
            }

            
        }
        yield return new WaitForSeconds(1f);

        Crack abc = _gm.GetCrackAt(3);
        Animator anObj = abc.gameObject.GetComponent<Animator>();
        StartCoroutine(SingleVulTentAttack(abc));
        _gm.TutAttack1End();
    }
    
    
    public IEnumerator HitPenalty()
    {
        yield return new WaitForSeconds(5);
        dizzy = false;
    }


    public void SetDizzy(bool val)
    {
        dizzy = val;
    }
    

    public void AdvanceToNextLevel()
    {
        _curLevel++;
    }
}