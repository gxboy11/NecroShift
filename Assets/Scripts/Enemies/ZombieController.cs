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
    int coins;

    [SerializeField]
    NavMeshAgent _navAgent;

    GameObject player;

    private GameManager gameManager;

    public GameObject[] consumible;

    void Start()
    {
        _navAgent.speed = zombieSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        GameManager.Instance.SetDefeatedEnemy();
        GameManager.Instance.IncrementCoins(coins);
        AudioManager.Instance.PlaySFX("Zombie Dead");

        Destroy(transform.parent.gameObject);
        gameManager.defeatedEnemies += 1;

        int dejaConsum = Random.Range(0, 2);
        if(dejaConsum == 1)
        {
            int tipoConsum = Random.Range(0, 4);
            Destroy(Instantiate(consumible[tipoConsum], new Vector3(transform.position.x, 0.59f, transform.position.z), transform.rotation), 5);
        }
    }
}
