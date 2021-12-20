using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEnemy : MonoBehaviour
{
    public MainCharacter player1;
    public MainCharacter player2;
    public GameObject bulletPref;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private float timeBetweenShots = 3f;

    [SerializeField] private Vector3 shootingDirection;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SimpleShot());
    }

    // Update is called once per frame
    void Update()
    {
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