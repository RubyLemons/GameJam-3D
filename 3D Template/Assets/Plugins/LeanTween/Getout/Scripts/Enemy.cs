using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy")]

    [SerializeField] NavMeshAgent agent;
    float initialSpeed;

    [Range(0, 1)] public float health = 1;


    Transform plr;
    Health plrHealth;

    [Header("Attack")]

    [Range(0, 1)] [SerializeField] float damage = 0.25f;

    [Space(10)]

    [SerializeField] float attackRange = 1f;
    float range;
    bool inRange;

    [SerializeField] float attackTime = 0.15f;
    [SerializeField] float attackSleepTime = 0.25f;
    bool attackDeb;

    void Awake()
    {
        initialSpeed = agent.speed;

        plr = GameObject.Find("PLAYER").transform;
        plrHealth = plr.GetComponent<Health>();
    }

    void Update()
    {
        health = Mathf.Clamp01(health);

        range = (transform.position - plr.position).magnitude;
        inRange = (range < attackRange);

        //Follow

        agent.SetDestination(plr.position);
        agent.isStopped = inRange;

        agent.speed = (attackDeb) ? initialSpeed : initialSpeed / 2;


        //Attack

        if (!attackDeb) {
            attackDeb = true;

            //PLAY();

            StartCoroutine(Tks.SetTimeout(() =>
            {
                if (inRange) {
                    plrHealth.value -= damage;
                }
            }, attackTime * 1000));

            //deb
            StartCoroutine(Tks.SetTimeout(() => attackDeb = false, attackSleepTime * 1000));
        }

        //DEBUG
        if (health <= 0) {
            gameObject.SetActive(false);
            health = 1;
        }
    }
}
