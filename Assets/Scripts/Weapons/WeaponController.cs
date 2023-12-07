using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum ShotType
{
    Automatic,
    Manual
}

public class WeaponController : MonoBehaviour
{

    //Inspector Values
    [Header("General")]
    [SerializeField]
    LayerMask hittableLayer;

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

    [SerializeField]
    ShotType shotType;



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
    bool _isShooting = false;

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

        HandleInputs();
        StartCoroutine(HandleReload());
        ReloadCounter();

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    void HandleInputs()
    {
        if (shotType == ShotType.Manual)
        {
            _isShooting = Input.GetButtonDown("Fire1");
            HandleShoot();

        }
        else if (shotType == ShotType.Automatic)
        {
            _isShooting = Input.GetButton("Fire1");
            HandleShoot();
        }
    }

    private void HandleShoot()
    {
        _currentTime += Time.deltaTime;
        if (_currentAmmo > 0 && !_isReloading)
        {
            if (_currentTime >= fireRate && _isShooting)
            {
                _currentAmmo--;

                AudioManager.Instance.PlaySFX("GunFire");

                GameObject flashClone = Instantiate(flashEffect, firePoint.position, Quaternion.Euler(firePoint.forward), transform); //Efecto Flash
                Destroy(flashClone, 1f);

                StartCoroutine(AddRecoil());

                RaycastHit hit;
                if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange, hittableLayer)) //Disparo
                {
                    ZombieController zombie = hit.transform.GetComponent<ZombieController>();

                    _currentTime = 0.0F;
                    ShootBullet();
                    if (hit.collider.tag == "Enemy")
                    {
                        HitMarkerController.Instance.BodyShot();
                        zombie.TakeDamage(bulletDamage);

                        return;
                    }
                    else if (hit.collider.tag == "EnemyHead")
                    {
                        HitMarkerController.Instance.HeadShot();
                        zombie.TakeDamage(bulletDamage * 2);
                        return;
                    }
                }
                GameObject bulletHoleClone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleClone, bulletHoleLifeTime);
            }
        }
        else if (_currentAmmo <= 0 && Input.GetButtonDown("Fire1")) //No se usa _isShooting porque en "Automatic" se satura
        {
            AudioManager.Instance.PlaySFX("Clean Gun");
        }
    }

    void ShootBullet()
    {
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

            AudioManager.Instance.PlaySFX("Reload");


            Debug.Log(gameObject.name);
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
