using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAI : MonoBehaviour
{
    public static EnemyMeleeAI instance;

    PlayerController player;

    Collider enemyColl;

    Transform headshotBox;

    AudioSource thisAudio;

    AudioManager audioManager;

    public Animator enemyAnim;

    public NavMeshAgent agent;

    public Transform playerTransform;
    public Transform dropPoint;

    public GameObject[] drops;

    public LayerMask isPlayer;
    public LayerMask isGround;

    public Vector3 walkPoint;

    public int attackDamageMin;
    public int attackDamageMax;
    public int attackDamageValue;

    public float walkPointRange;
    public float timeBetweenAttacks;
    public float sightRange;
    public float attackRange;
    public float health;

    public bool thisDrops;
    public bool playerInSightRange;
    public bool playerInAttackRange;
    public bool isSoldier;

    public GameObject soldierBullet;

    bool alreadyAttacked;
    bool walkPointSet;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        thisAudio = GetComponent<AudioSource>();
        instance = this;
        thisDrops = (Random.value > 0.5f); // Randomizes Drops
        enemyColl = GetComponent<Collider>();
        dropPoint = gameObject.transform.GetChild(1).transform;
        playerTransform = GameObject.Find("Player").transform;
        enemyAnim = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        headshotBox = transform.Find("Headshot");
    }

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInAttackRange && !playerInSightRange)
        {
            Patrolling();
        }

        if (!playerInAttackRange && playerInSightRange)
        {
            ChasePlayer();
        }

        if(playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        if (walkPointSet)
        {
            enemyAnim.SetBool("isWalking", true);
            enemyAnim.SetBool("isAttacking", false);
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
        {
            walkPointSet = true;
        }
    }

    void ChasePlayer()
    {
        Debug.Log("On the way to Player");
        enemyAnim.SetBool("isWalking", true);
        enemyAnim.SetBool("isAttacking", false);
        agent.SetDestination(playerTransform.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        if (!alreadyAttacked)
        {
            enemyAnim.SetBool("isAttacking", true);
            Debug.Log("Attacked");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void DamagePlayer()
    {
        audioManager.Play("PlayerDamage");
        attackDamageValue = Random.Range(attackDamageMin, attackDamageMax);
        player.TakeDamagePlayer(attackDamageValue);
    }

    void DamagePlayerSoldier()
    {
        Instantiate(soldierBullet, gameObject.transform);
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamageEnemy(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            enemyAnim.SetTrigger("isDead");
        }
    }

    void DestroyEnemy()
    {
        if(thisDrops)
        {
            Instantiate(drops[Random.Range(0 , drops.Length)], dropPoint.position, dropPoint.rotation);
        }
        thisAudio.Stop();
        headshotBox.gameObject.SetActive(false);
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        enemyColl.isTrigger = true;
        enemyColl.enabled = false;
        instance.enabled = this;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
