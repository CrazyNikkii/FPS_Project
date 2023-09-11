using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public bool allowSwitch = false;
    // Weapon
    public int selectedWeapon = 0;

    // References
    public GameObject aRStand;
    public AssaultRifleScript assaultRifleScript;
    public GameObject pistolStand;
    public MainPistolScript mainPistolScript;

    void Start()
    {
        
    }

    void Update()
    {
        if(allowSwitch ==  true)
        {
            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Select the weapon with scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
            {
                selectedWeapon++;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
            {
                selectedWeapon--;
            }
        }

        // Select the weapon with numbers
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        // Change the weapon to selected weapon
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

        // Resets player ads state if changing weapon during ads
        if (assaultRifleScript.aDS == true)
        {
            if (aRStand.activeInHierarchy == false)
            {
                assaultRifleScript.UnAimingDownSight();
                assaultRifleScript.aDS = false;
            }
        }
        if (mainPistolScript.aDS == true)
        {
            if (pistolStand.activeInHierarchy == false)
            {
                mainPistolScript.UnAimingDownSight();
                mainPistolScript.aDS = false;
            }
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            i++;
        }
    }
}
