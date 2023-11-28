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

    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.gameObject.CompareTag("Bullet"))
    //    {
    //        if (enemyHealth > 0)
    //        {

    //            enemyHealth--;
    //            Debug.Log("Enemigo ha tocado! " + other.gameObject.tag + " " + enemyHealth);
    //            Destroy(other.transform.parent.gameObject);
    //            if (enemyHealth <= 0)
    //            {
    //                Destroy(gameObject);
    //            }
    //        }
    //    }
    //}

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
        Destroy(transform.parent.gameObject);
    }
}
