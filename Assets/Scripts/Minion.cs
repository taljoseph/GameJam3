using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Minion : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    public Point target;
    public List<Vector3> intermediateTargets;
    public int intermediateNum = 5;
    public int pathLength = 3;
    private int _curStageInPath = 0;
    private Vector3 _curTarget;
    private float epsilon = 0.1f;
    private bool _finalTargetIsSet = false;
    private MainEnemy _satan;
    private GManager _gm;
    

    // Start is called before the first frame update
    void Awake()
    {
        _satan = FindObjectOfType<MainEnemy>();
        _gm = FindObjectOfType<GManager>();
        _curTarget = intermediateTargets[Random.Range(0, intermediateNum)];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_curTarget, transform.position) < epsilon)
        {
            if (_curStageInPath < pathLength)
            {
                _curTarget = intermediateTargets[Random.Range(0, intermediateNum)];
                _curStageInPath++;
            }
            else if (!_finalTargetIsSet)
            {
                // TODO: maybe need to ask now for targets from Satan? perhaps during the trip the board state changed...
                _curTarget = target.transform.position;
                _finalTargetIsSet = true;
            }
        }
    }

    private void FixedUpdate()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, _curTarget, step);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            var player = col.gameObject.GetComponent<MainCharacter>();
            if (player.IsInDashMode())
            {
                _gm.MinionGotHit();
            }
        }
    }

    public void SetTarget(Point pTarget)
    {
        target = pTarget;
    }

    public bool IsFinalTargetSet()
    {
        return _finalTargetIsSet;
    }
}