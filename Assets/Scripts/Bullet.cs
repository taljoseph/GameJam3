using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float speed = 5;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVelocityDirection(Vector3 direction)
    {
        if (rb == null)
        {
            Debug.Log("it is null");
        }
        rb.velocity = direction * 5;
    }
    
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Border"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.tag.Equals("Player1"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.tag.Equals("Player2"))
        {
            Destroy(gameObject);
        }
    }
}
