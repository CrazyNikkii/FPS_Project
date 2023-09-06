using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;
    public GameObject bullet;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public float attackSpeed;
    public bool hasAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if(!playerInSightRange && !playerInAttackRange)
        {
            Patrol();
        }

        if(playerInSightRange && !playerInAttackRange)
        {
            Chase();
        }

        if(playerInSightRange && playerInAttackRange)
        {
            Attack();
        }
    }

    void Patrol()
    {
        if(!walkPointSet)
        {
            SetWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SetWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
    }

    void Chase()
    {
        Debug.Log("Chasing");
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        Debug.Log("Attacking");
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);

            hasAttacked = true;
            Invoke("ResetAttack", attackSpeed);
        }
    }

    void ResetAttack()
    {
        hasAttacked = false;
    }
}
