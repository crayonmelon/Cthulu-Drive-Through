using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private LayerMask groundMask;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        chasePlayer();
    }

    private void chasePlayer()
    {
        agent.SetDestination(player.position);
    }
}
