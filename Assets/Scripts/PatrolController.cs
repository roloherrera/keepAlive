
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolController : MonoBehaviour
{
    enum PatrolStates
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("Target")]
    [SerializeField]
    Transform target;

    [SerializeField]
    LayerMask whatIsPlayer;

    [SerializeField]
    Animator animator;

    [Header("Patrol")]
    [SerializeField]
    LayerMask whatIsGround;
    [SerializeField]
    float walkPointRange;
    [SerializeField]
    float sightRange;

    [Header("Attack")]
    [SerializeField]
    float attackRange;
    [SerializeField]
    float timeBetweenAttacks = 0.5F;

    NavMeshAgent agent;
    Vector3 walkPoint;
    PatrolStates currentState;

    bool isWalkPointSet;
    bool isAlreadyAttacking;
    bool isTargetInSightRange;
    bool isTargetInAttackRange;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();   
    }
    void Update()
    {
        isTargetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        isTargetInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        currentState =
            isTargetInSightRange && isTargetInAttackRange
            ? PatrolStates.Attack
            : isTargetInSightRange && !isTargetInAttackRange
                ? PatrolStates.Chase
                : PatrolStates.Patrol;

        switch (currentState)
        {
            case PatrolStates.Patrol: HandlePatrol(); break;
            case PatrolStates.Chase: HandleChase(); break;
            case PatrolStates.Attack: HandleAttack(); break;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere (transform.position, attackRange);

    }

    void HandleAttack()
    {
        agent.SetDestination(transform.position);
        animator.SetBool("isWalking", false);

        transform.LookAt(target);

        if (!isAlreadyAttacking)
        {
            isAlreadyAttacking = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
            animator.SetBool("isAttacking", true);
        }
    }

    void ResetAttack()
    {
        isAlreadyAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    void HandleChase()
    {
        agent.SetDestination(target.position);
        animator.SetBool("isWalking", true);
    }

    void HandlePatrol()
    {
        if (!isWalkPointSet)
        {
            FindNextWalkPoint();
            if (isWalkPointSet)
            {
                agent.SetDestination(walkPoint);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1.0F) 
        {
            isWalkPointSet = false; 
        }
    }


    private void FindNextWalkPoint()
    {
        float positionX = Random.Range(-walkPointRange, walkPointRange);
        float positionZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + positionX, transform.position.y, transform.position.z + positionZ);

        if (Physics.Raycast(walkPoint, -transform.up, whatIsGround)) { isWalkPointSet = true; }
    }
}
