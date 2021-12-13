using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] private String player; // defined for A or B
    [SerializeField] private float moveSpeed = 2;
    private Vector2 direction = Vector2.zero;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GetDirection();
        Move();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetDirection()
    {
        float x = Input.GetAxisRaw("Horizontal_" + player);
        float y = Input.GetAxisRaw("Vertical_" + player);
        direction = new Vector2(x, y);
    }

    void Move()
    {
        rb.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
    }
}
