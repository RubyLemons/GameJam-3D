using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    Transform dest;

    bool closeRange;

    void Awake()
    {
        dest = GameObject.Find("PLAYER").transform;
    }

    void Update()
    {
        closeRange = (transform.position - dest.position).magnitude < 5;

        if (!closeRange)
        {
            agent.SetDestination(dest.position);

            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = true;
        }
    }
}
