using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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





    [Header("FX")]
    [SerializeField]
    GameObject flashEffect;




    [Header("Shoot Parameters")]
    [SerializeField]
    float fireRange = 200.0f;

    [SerializeField]
    float bulletHoleLifeTime = 4.0f;

    [SerializeField]
    float fireRate = 0.5f;

    [SerializeField]
    float bulletDamage = 5.0f;

    [SerializeField]
    float recoilForce = 4.0f;

    [SerializeField]
    int maxAmmo = 15;



    [Header("Bullet Stats")]
    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float force = 100.0F;



    [Header("Reload Parameters")]
    [SerializeField]
    float reloadTime = 1.5f;

    [SerializeField]
    TextMeshProUGUI bulletCount;





    //Private Floats
    float _currentTime;

    //Private Integers
    int _currentAmmo;

    //Private Bools
    bool _isRecoiling = false;
    bool _isReloading = false;

    //Private Misc
    Transform _cameraPlayerTransform;
    HitMarkerController _hitMarker;

    private void Awake()
    {
        _cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        _hitMarker = FindObjectOfType<HitMarkerController>();

        if (_hitMarker != null)
        {
            _hitMarker.gameObject.SetActive(false);
        }

        bulletCount = FindObjectOfType<TextMeshProUGUI>();

        _currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (_isRecoiling)
        {
            return;
        }
        HandleShoot();
        StartCoroutine(HandleReload());
        ReloadCounter();

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    private void HandleShoot()
    {
        _currentTime += Time.deltaTime;
        if (_currentAmmo > 0 && !_isReloading)
        {
            if (_currentTime >= fireRate && Input.GetButtonDown("Fire1"))
            {
                _currentAmmo--;
                GameObject flashClone = Instantiate(flashEffect, firePoint.position, Quaternion.Euler(firePoint.forward), transform);
                Destroy(flashClone, 1f);
                StartCoroutine(AddRecoil());

                RaycastHit hit;
                if (Physics.Raycast(_cameraPlayerTransform.position, _cameraPlayerTransform.forward, out hit, fireRange, hittableLayer))
                {
                    ZombieController zombie = hit.transform.GetComponent<ZombieController>();

                    _currentTime = 0.0F;
                    ShootBullet();
                    if (hit.collider.tag == "Enemy")
                    {
                        _hitMarker.BodyShot();
                        zombie.TakeDamage(bulletDamage);

                        return;
                    }
                    else if (hit.collider.tag == "EnemyHead")
                    {
                        _hitMarker.HeadShot();
                        zombie.TakeDamage(bulletDamage * 2);
                        return;
                    }
                }
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, bulletHoleLifeTime);
            }
        }
        else if (_currentAmmo < 0)
        {
            StartCoroutine(HandleReload());
        }
    }

    void ShootBullet()
    {
        AudioManager.Instance.PlaySFX("GunFire");
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private IEnumerator AddRecoil()
    {
        _isRecoiling = true;
        transform.Rotate(-recoilForce, 0f, 0f);
        transform.position = transform.position - transform.forward * (recoilForce / 50f);

        yield return new WaitForSeconds(0.1f);

        // Resetea la rotación
        transform.Rotate(recoilForce, 0f, 0f);
        _isRecoiling = false;
    }

    IEnumerator HandleReload()
    {
        if (Input.GetButtonDown("Reload"))
        {
            _isReloading = true;
            Debug.Log("Recargando...");

            yield return new WaitForSeconds(reloadTime);

            _currentAmmo = maxAmmo;
            _isReloading = false;
            Debug.Log("LISTO PARA LA ACCIÓN");
        }
    }

    void ReloadCounter()
    {
        if (!_isReloading)
        {
            bulletCount.text = _currentAmmo + "";
        }
        else
        {
            bulletCount.text = "Recargando...";
        }

    }
}
