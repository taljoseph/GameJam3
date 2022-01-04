using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private String player; // defined for A or B
    [SerializeField] private MainCharacter secondPlayer;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private GManager gm;
    [SerializeField] private KeyCode shootKey;
    public GameObject axePrefab; //TODO shouldn't be prefab, also no need for instantiating and destroying all the time. 
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _dizzy = false;
    private bool _frozen = false;
    [SerializeField] private float tentacleHitDuration = 1f;
    [SerializeField] private bool hasAxe = true;
    


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!_frozen)
        {
            GetDirection();
        }
        if (Input.GetKeyDown(shootKey))
        {
            if (hasAxe)
            {
                ShootAxe(transform.position , secondPlayer.transform.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_frozen)
        {
            Move();
        }
    }

    void GetDirection()
    {
        float x = Input.GetAxisRaw("Horizontal_" + player);
        float y = Input.GetAxisRaw("Vertical_" + player);
        direction = new Vector2(x, y).normalized;
    }

    void Move()
    {
        _rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        
    }

    public void SetSpeed(float val)
    {
        moveSpeed = val;
    }
    
    public float GetSpeed()
    {
        return moveSpeed;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag.Equals("Hand") && _dizzy == false)
        {
            _dizzy = true;
            gm.DizzyStat(this);
        }

        if (other.transform.tag.Equals("Tentacle"))  
        {
            float bounce = 300f; //amount of force to apply
            _rb.AddForce(other.contacts[0].normal * bounce);
            _frozen = true;
            StartCoroutine(TentacleHit());
        }
        
        
        // if (other.gameObject.tag.Equals("PointsActivator"))
        // {
        //     
        // }
    }

    public void SetHasAxe(bool val)
    {
        hasAxe = val;
    }
    
    private IEnumerator TentacleHit()
    {
        yield return new WaitForSeconds(tentacleHitDuration);
        _frozen = false;
    }

    public void SetDizzy(bool val)
    {
        _dizzy = val;
    }
    
    public void ShootAxe(Vector3 source,Vector3 dest)
    {
        var axe = Instantiate(axePrefab, source, Quaternion.identity);
        var shootScript = axe.GetComponent<ShootObj>();
        shootScript.SetHolder(gameObject);
        shootScript.SetSource(source);
        shootScript.SetDestination(dest);
        shootScript.SetLayer(0);
        hasAxe = false;
        gm.AxeShot();
    }

    public void Freeze(bool val)
    {
        _frozen = val;
        _rb.velocity = Vector2.zero;
    }

}