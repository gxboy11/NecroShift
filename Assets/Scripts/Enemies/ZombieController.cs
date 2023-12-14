using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    float enemyHealth = 10.0f;

    [SerializeField]
    float zombieSpeed = 10.0f;

    [SerializeField]

    float attackRange = 2.0f;

    [SerializeField]
    Animator _animator;
    int coins;

    [SerializeField]
    NavMeshAgent _navAgent;

    GameObject player;

    bool isMoving = false;
    bool isAttacking = false;
    bool isDying = false;

    NavMeshAgent _navAgent;

    [SerializeField]
    float offsetDistance = 1.0f;

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _navAgent.speed = zombieSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        isMoving = _navAgent.velocity.magnitude > 0.1f;


        _animator.SetBool("Moving", isMoving);

        if (enemyHealth <= 0 && !isDying)
        {
            StartCoroutine(DieCoroutine());
        }
        else
        {

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRange && !isAttacking && !isDying)
            {
                StartCoroutine(AttackCoroutine());
            }
            else if (!isDying)
            {

                _navAgent.SetDestination(player.transform.position);
            }
        }
    }


    IEnumerator DieCoroutine()
    {
        isDying = true;


        _navAgent.isStopped = true;


        _animator.SetTrigger("Die");
        
        GameManager.Instance.SetDefeatedEnemy();
        GameManager.Instance.IncrementCoins(coins);

        AudioManager.Instance.PlaySFX("Zombie Dead");

        yield return new WaitForSeconds(4.0F);


        _navAgent.SetDestination(transform.position);

        Vector3 finalPosition = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.position = finalPosition;

        _navAgent.enabled = false;


        isDying = false;


        Destroy(gameObject);
    }





    IEnumerator AttackCoroutine()
    {

        _navAgent.isStopped = true;
        isAttacking = true;


        _animator.SetTrigger("Attack");


        yield return new WaitForSeconds(2.0F);


        if (isAttacking)
        {

            player.GetComponent<PlayerHealthController>().TakeDamage(10.0f); 
        }

        // Resume movement after the attack animation
        _navAgent.isStopped = false;
        isAttacking = false;
        _animator.ResetTrigger("Attack");
    }

    public void TakeDamage(float damage)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;
            Debug.Log("Enemigo da�ado!" + enemyHealth);
            if (enemyHealth <= 0)
            {
                StartCoroutine(DieCoroutine());
            }
        }
    }
}
