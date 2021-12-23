using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemy : MonoBehaviour
{
    [SerializeField] private GManager _gm;

    public MainCharacter player1;
    public MainCharacter player2;
    public GameObject bulletPref;
    public GameObject minionPref;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float timeBetweenShots = 5f;
    [SerializeField] private float timeBetweenMinions = 10f;
    [SerializeField] private Vector3 shootingDirection;
    private List<Point> _minionTargets;
    public List<Minion> _minions = new List<Minion>();
    private bool _areMinionsActive;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SimpleShot());
        StartCoroutine(SendMinions());
    }

    // Update is called once per frame


    public void SetMinionTargets()
    {
        _minionTargets = _gm.GetRandomTargets();
    }

    private IEnumerator SendMinions()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenMinions);
            if (!_areMinionsActive)
            {
                SetMinionTargets();
                if (_minionTargets != null)
                {
                    InitMinions();
                }
            }
        }
    }

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
        if (_minions.Count >= 1 && _minions[0] != null)
        {
            Destroy(_minions[0].gameObject, 0.5f);
        }
        if (_minions.Count >= 2 && _minions[1] != null)
        {
            Destroy(_minions[1].gameObject, 0.5f);
        }
        _minions.Clear();
        _areMinionsActive = false;
    }

    private void SetShootingDirection()
    {
        MainCharacter target = Random.Range(0, 2) == 0 ? player1 : player2;
        shootingDirection = (target.transform.position - firePoint.transform.position).normalized;
    }

    private IEnumerator SimpleShot()
    {
        while (true)
        {
            SetShootingDirection();
            InitBullet();
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void InitBullet()
    {
        GameObject bullet = Instantiate(bulletPref, firePoint.transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetVelocityDirection(shootingDirection);
    }
}