using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    float enemyHealth = 10.0f;

    [SerializeField]
    float zombieSpeed = 10.0f;

    [SerializeField]
    NavMeshAgent _navAgent;

    GameObject player;

    void Start()
    {
        _navAgent.speed = zombieSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _navAgent.SetDestination(player.gameObject.transform.position);
    }

    public void TakeDamage(float damage)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;
            Debug.Log("Enemigo dañado!" + enemyHealth);
            if (enemyHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        AudioManager.Instance.PlaySFX("Zombie Dead");

        Destroy(transform.parent.gameObject);
    }
}
