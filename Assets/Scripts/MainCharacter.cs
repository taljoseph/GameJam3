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
    private Vector2 _prevDir = Vector2.zero;
    private Vector2 _direction = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _dizzy = false;
    private bool _frozen = false;
    [SerializeField] private float tentacleHitDuration = 1f;
    [SerializeField] private bool hasAxe = true;
    private Animator _an;
    private bool _isFlipped = false;
    private GameObject _axeObj; 
    


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _an = GetComponent<Animator>();
        _axeObj = gameObject.transform.GetChild(0).gameObject;
        if (player.Equals("B"))
        {
            gameObject.transform.Rotate(Vector3.up * 180);
            _isFlipped = true;
        }
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
                _axeObj.SetActive(false);
                ShootAxe(this , secondPlayer);
            }
        }
        // if (_direction.x != 0 || _direction.y != 0)
        // {
        //     if (!_isWalking)
        //     {
        //         _isWalking = true;
        //         _an.SetBool("isWalking", true);
        //     }
        //
        //     if (!_srFlipped && _direction.x == 1f)
        //     {
        //         _srFlipped = false;
        //         _sr.flipX = true;
        //     }
        // }
        // else
        // {
        //     if (_isWalking)
        //     {
        //         _isWalking = false;
        //         _an.SetBool("isWalking", false);
        //     }
        // }

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
        _prevDir = _direction;
        _direction = new Vector2(x, y);
        if (_direction != _prevDir)
        {
            _an.SetInteger("x", (int) _direction.x);
            _an.SetInteger("y", (int) _direction.y);
        }

        if (_direction.x == 1 && !_isFlipped)
        {
            gameObject.transform.Rotate(Vector3.up * 180);
            _isFlipped = true;
        }
        else if (_direction.x == -1 && _isFlipped)
        {
            gameObject.transform.Rotate(Vector3.up * 180);
            _isFlipped = false;
        }
        _direction.Normalize();
    }

    void Move()
    {
        _rb.velocity = new Vector2(_direction.x * moveSpeed, _direction.y * moveSpeed);
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
        
        
        if (other.transform.tag.Equals("Crack"))
        {
            Crack crackScript = other.gameObject.GetComponent<Crack>();
            if (crackScript.GetNormalTentActive() || crackScript.GetSpecialTentActive())
            {
                float bounce = 300f; //amount of force to apply
                _rb.AddForce(other.contacts[0].normal * bounce);
                _frozen = true;
                StartCoroutine(TentacleHit());   
            }
        }
        
        
        // if (other.gameObject.tag.Equals("PointsActivator"))
        // {
        //     
        // }
    }

    public void SetHasAxe(bool val)
    {
        _axeObj.SetActive(true);
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
    
    public void ShootAxe(MainCharacter source,MainCharacter dest)
    {
        var axe = Instantiate(axePrefab, source.transform.position, Quaternion.identity);
        var shootScript = axe.GetComponent<ShootObj>();
        shootScript.SetHolder(gameObject);
        shootScript.SetSource(source);
        shootScript.SetTarget(dest);
        shootScript.SetLayer(0);
        hasAxe = false;
        //gm.AxeShot();
    }

    public void Freeze(bool val)
    {
        _frozen = val;
        _rb.velocity = Vector2.zero;
    }

}