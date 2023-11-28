using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    //Inspector Values
    [Header("General")]
    [SerializeField]
    LayerMask hittableLayer;

    [SerializeField]
    LayerMask enemyLayer;

    [SerializeField]
    GameObject bulletHolePrefab;

    [SerializeField]
    GameObject bulletPrefab;

    [Header("Shoot Parameters")]
    [SerializeField]
    float fireRange = 200.0f;

    [SerializeField]
    float bulletHoleLifeTime = 4.0f;

    [SerializeField]
    float fireRate = 0.5f;

    [SerializeField]
    float bulletDamage = 5.0f;

    [Header("Bullet Stats")]
    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float force = 100.0F;

    //Private Floats
    float _currentTime;

    //Private Misc
    Rigidbody _rb;
    Transform _cameraPlayerTransform;
    ZombieController _zombie;

    private void Awake()
    {
        _cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _rb = bulletPrefab.GetComponent<Rigidbody>();
        _zombie = FindObjectOfType<ZombieController>();
    }

    private void Update()
    {
        HandleShoot();
    }

    private void HandleShoot()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= fireRate && Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(_cameraPlayerTransform.position, _cameraPlayerTransform.forward, out hit, fireRange, hittableLayer))
            {
                _currentTime = 0.0F;
                ShootBullet();
                if (hit.collider.tag == "Enemy")
                {
                    _zombie.TakeDamage(bulletDamage);
                    return;
                }
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, bulletHoleLifeTime);
            }

        }
    }

    void ShootBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
