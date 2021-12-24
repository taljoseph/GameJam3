using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private String player; // defined for A or B
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private KeyCode dashKey;
    [SerializeField] private bool inDashMode = false;
    [SerializeField] private int dashDuration;
    [SerializeField] private int dashCooldownDuration;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private bool _canDash = true;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GetDirection();
        Move();
        if (Input.GetKeyDown(dashKey) && _canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private IEnumerator Dash()
    {
        _sr.color = Color.magenta;
        inDashMode = true;
        _canDash = false;
        yield return new WaitForSeconds(dashDuration); // dash duration
        _sr.color = Color.blue;
        inDashMode = false;
        //  TODO : should show the user that he is in dash cooldown
        yield return new WaitForSeconds(dashCooldownDuration); // dash cooldown
        _canDash = true;
    }

    void GetDirection()
    {
        float x = Input.GetAxisRaw("Horizontal_" + player);
        float y = Input.GetAxisRaw("Vertical_" + player);
        direction = new Vector2(x, y);
    }

    void Move()
    {
        _rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }

    public bool IsInDashMode()
    {
        return inDashMode;
    }
}