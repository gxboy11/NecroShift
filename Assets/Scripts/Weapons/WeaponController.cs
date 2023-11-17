using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Bullet Stats")]
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float force = 100.0F;

    [SerializeField]
    float fireRate = 0.5f;

    [Header("Sonido")]
    [SerializeField]
    AudioSource soundControl;

    [SerializeField]
    AudioClip ShootSound;

    [SerializeField]
    AudioSource soundControl2;

    [SerializeField]
    AudioClip destroySound;

    float _currentTime;

    Rigidbody _rb;


    void Awake()
    {

        _rb = bulletPrefab.GetComponent<Rigidbody>();
    }

    void Update()
    {
        _currentTime += Time.deltaTime;

        // Verifica si ha pasado el tiempo de fireRate y si el botón "Fire1" está presionado
        if (_currentTime >= fireRate && Input.GetButton("Fire1"))
        {
            _currentTime = 0.0F;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        soundControl.PlayOneShot(ShootSound);

    }
}
