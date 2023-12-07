using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    List<WeaponController> startingWeapons = new List<WeaponController>();

    [SerializeField]
    Transform weaponParentSocket; //Donde se coloca el arma

    [SerializeField]
    Transform defaultWeaponPosition; //Posicion inicial de las armas

    [SerializeField]
    Transform aimWeaponPosition; //Posicion default para apuntar

    [SerializeField]
    public int activeWeaponIndex { get; private set; }

    private WeaponController[] weaponSlots = new WeaponController[2];


    // Start is called before the first frame update
    void Start()
    {
        activeWeaponIndex = -1;

        foreach (WeaponController startingWeapon in startingWeapons)
        {
            AddWeapon(startingWeapon);
        }
        SwitchWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon();
        }

    }

    private void SwitchWeapon()
    {
        int tempIndex = (activeWeaponIndex + 1) % weaponSlots.Length;

        foreach (WeaponController weapon in weaponSlots)
        {
            if (weapon != null) weapon.gameObject.SetActive(false);
        }

        AudioManager.Instance.PlaySFX("Switch Weapon");
        weaponSlots[tempIndex].gameObject.SetActive(true);
        activeWeaponIndex = tempIndex;
    }

    void AddWeapon(WeaponController weaponPrefab)
    {
        weaponParentSocket.position = defaultWeaponPosition.position;

        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                WeaponController weaponClone = Instantiate(weaponPrefab, weaponParentSocket);
                weaponClone.gameObject.SetActive(true);

                weaponSlots[i] = weaponClone;
                return;
            }
        }
    }
}
