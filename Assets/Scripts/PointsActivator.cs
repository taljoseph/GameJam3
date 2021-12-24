using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActivatorGoal
{
    CompleteCapture,
    StartCapture,
    StopCapture
}

public enum Possessor
{
    Player,
    Satan,
}

public class PointsActivator : MonoBehaviour
{
    [SerializeField] private float speed = 80;
    public Point destination;
    public Point source;
    public Possessor possessor;
    public ActivatorGoal goal;
    private GManager _gm;
    private Rigidbody2D _rb;
    private bool _isShot = false;

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GManager>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isShot && destination != null && _rb != null)
        {
            Vector3 velocity = speed * (destination.transform.position - transform.position).normalized;
            _rb.velocity = velocity;
            _isShot = true;
            // float step =  speed * Time.deltaTime; // calculate distance to move
            // transform.position = Vector3.MoveTowards(transform.position, destination.transform.position, step);               
        }
    }

    public void SetDestination(Point dest)
    {
        destination = dest;
    }

    public void SetPossessor(Possessor pos)
    {
        possessor = pos;
    }

    public void SetGoal(ActivatorGoal g)
    {
        goal = g;
    }
    public void SetSource(Point s)
    {
        source = s;
    }
    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    public void HandlePointCollision(Point point)
    {
        var pointScript = point;
        if (goal == ActivatorGoal.CompleteCapture)
        {
            if (possessor == Possessor.Player)
            {
                if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.green);
                    pointScript.SetActive(false);
                    _gm.AddToPlayerPoints(pointScript);
                }
            }
            else // possessor == Possessor.Satan
            {
                if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.red);
                    pointScript.SetActive(true);
                    _gm.RemoveFromPlayerPoints(pointScript);
                }
            }
        }
        else if (goal == ActivatorGoal.StartCapture && pointScript.gameObject != destination.gameObject && pointScript.gameObject != source.gameObject)
        {
            if (possessor == Possessor.Player)
            {
                if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.cyan);
                }
            }
            else // possessor == Possessor.Satan
            {
                if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.yellow);
                }
            }
        }
        else if (goal == ActivatorGoal.StopCapture && pointScript.gameObject != destination.gameObject && pointScript.gameObject != source.gameObject)
        {
            // Debug.Log("source point: " +pointScript.gameObject);
            // Debug.Log("current point: " +pointScript.gameObject);
            // Debug.Log("destination point: " +destination.gameObject);
            if (possessor == Possessor.Player)
            {
                if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.red);
                }
            }
            else // possessor == Possessor.Satan
            {
                if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
                {
                    pointScript.SetColour(Color.green);
                }
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("PointsActivator"))
        {
            Destroy(gameObject, 0.01f);
        }
    }
}