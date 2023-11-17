using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    float enemyHealth = 10.0f;

    [SerializeField]
    NavMeshAgent _navAgent;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _navAgent.SetDestination(player.gameObject.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Enemigo ha tocado! " + other.gameObject.tag + " " + enemyHealth);
            if (enemyHealth > 0)
            {
                enemyHealth--;
                if (enemyHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Enemigo ha tocado! " + other.gameObject.tag + " " + enemyHealth);
            if (enemyHealth > 0)
            {
                enemyHealth--;
                if (enemyHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
