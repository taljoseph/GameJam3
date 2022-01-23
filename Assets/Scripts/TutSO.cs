using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum ActivatorGoal
// {
//     CompleteCapture,
//     StartCapture,
//     StopCapture
// }

// public enum Possessor
// {
//     Player,
//     Satan,
// }

public class TutSO : MonoBehaviour
{
    [SerializeField] private float speed = 80;
    [SerializeField] private SpriteRenderer childSprite;
    public TutMC target;
    public TutMC source;
    private TutGM _gm;
    private Rigidbody2D _rb;
    private bool _isShot = false;
    private GameObject curHolder = null;
    

    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<TutGM>();
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetHolder(GameObject player)
    {
        curHolder = player;
    }

    public GameObject GetHolder()
    {
        return curHolder;
    }

    // Update is called once per frame
    void Update()
    {
        if (_rb != null)
        {
            Vector3 velocity = speed * (target.transform.position - transform.position).normalized;
            _rb.velocity = velocity;
        }

    }

    public void SetTarget(TutMC t)
    {
        this.target = t;
    }
    
    public void SetSource(TutMC s)
    {
        source = s;
    }
    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    // public void HandlePointCollision(Point point)
    // {
    //     var pointScript = point;
    //     if (goal == ActivatorGoal.CompleteCapture)
    //     {
    //         if (possessor == Possessor.Player)
    //         {
    //             if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.green);
    //                 pointScript.SetActive(false);
    //                 _gm.AddToPlayerPoints(pointScript);
    //             }
    //         }
    //         else // possessor == Possessor.Satan
    //         {
    //             if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.red);
    //                 pointScript.SetActive(true);
    //                 _gm.RemoveFromPlayerPoints(pointScript);
    //             }
    //         }
    //     }
    //     else if (goal == ActivatorGoal.StartCapture && pointScript.gameObject != destination.gameObject && pointScript.gameObject != source.gameObject)
    //     {
    //         if (possessor == Possessor.Player)
    //         {
    //             if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.cyan);
    //             }
    //         }
    //         else // possessor == Possessor.Satan
    //         {
    //             if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.yellow);
    //             }
    //         }
    //     }
    //     else if (goal == ActivatorGoal.StopCapture && pointScript.gameObject != destination.gameObject && pointScript.gameObject != source.gameObject)
    //     {
    //         // Debug.Log("source point: " +pointScript.gameObject);
    //         // Debug.Log("current point: " +pointScript.gameObject);
    //         // Debug.Log("destination point: " +destination.gameObject);
    //         if (possessor == Possessor.Player)
    //         {
    //             if (pointScript.IsInLevel() && pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.red);
    //             }
    //         }
    //         else // possessor == Possessor.Satan
    //         {
    //             if (pointScript.IsInLevel() && !pointScript.IsSatanPoint())
    //             {
    //                 pointScript.SetColour(Color.green);
    //             }
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var GOtag = other.gameObject.tag;
        if (GOtag.Equals("Player"))
        {
            TutMC player = other.GetComponent<TutMC>();
            if (!GetHolder().Equals(other.gameObject))
            {
                //_gm.AxeHitPlayer();
                player.SetHasAxe(true);
                _gm.AxeHitPlayer();
                Destroy(gameObject);
            }
        }

        if (GOtag.Equals("Crack"))
        {
            Crack crackScript = other.GetComponent<Crack>();
            if (!crackScript.IsClosingCrack())
            {
                _gm.CrackFix(other.GetComponent<Crack>());
                if (crackScript.GetNormalTentActive())
                {
                    // add 
                }

                if (crackScript.GetSpecialTentActive())
                {
                    other.gameObject.GetComponent<Animator>().SetTrigger("hit"); //TODO change place maybe?
                    _gm.AxeHitTentacle();
                }
                crackScript.CloseCrack();
            }
        }

    }

    public void HandleCrackCollision(Crack crack)
    {
        GameObject crackGO = crack.gameObject;
        if (crackGO.activeSelf)
        {
            crackGO.SetActive(false);
        }
    }
    
    public void HandleOctoCollision(){} // ADD CODE. 


    
}