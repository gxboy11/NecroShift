using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed = 10.0F;

    [SerializeField]
    float lifeTime = 1.0F;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(transform.parent.gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }
}
