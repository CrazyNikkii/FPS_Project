using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssaultRifleScript : MonoBehaviour
{
    // Weapon statistics
    [SerializeField] private float timeBetweenShooting, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool fullAutoMode;
    int ammoLeftInARMag, aRBulletsShot;
    public float aRDamage = 50f;
    public int aRMaxAmmo = 20;
    public int aRTotalAmmo;
    public float scopedFOV = 15f;
    private float normalFOV;

    // States
    bool aRShooting, aRReadyToShoot, aRReloading;
    public bool aRTotalAmmoLeft;
    public bool aDS = false;
    public bool aRStillActive;

    // References
    public Camera aimCam;
    public AudioSource gunSound;
    public AudioClip gunSoundClip;
    public GameObject audioManager;
    public GameObject bulletHole;
    public GameManager gm;
    public TargetDummyBody dummyTargetBody;
    public TargetDummyHead dummyTargetHead;
    public ParticleSystem muzzleFlash;
    public GameObject bHContainer;
    public Animator animator;
    public GameObject scopeRedDot;

    // HUD
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI totalAmmunitionDisplay;
    public TextMeshProUGUI reloadingText;

    // Debugging
    public bool allowInvoke = true;

    void Start()
    {
        // Sound reference
        gunSound = GetComponent<AudioSource>();
        // Set ammo values to max and set the right state
        aRTotalAmmo = aRMaxAmmo;
        aRTotalAmmoLeft = true;
        ammoLeftInARMag = magazineSize;
        aRReadyToShoot = true;
        reloadingText.SetText("");
        gunSound = audioManager.GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckMainPistolActions();

        if (Input.GetButtonDown("Fire2"))
        {
            aDS = !aDS;
            animator.SetBool("aimingDownSight", aDS);

            if (aDS)
            {
                StartCoroutine(AimingDownSight());
            }
            else
            {
                UnAimingDownSight();
            }
        }

        // Toggle Firemode
        if (Input.GetKeyDown(KeyCode.B))
        {
            fullAutoMode = !fullAutoMode;
        }

        // HUD magazine ammo counter
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(ammoLeftInARMag / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
        }

        //  HUD total ammo counter
        if (totalAmmunitionDisplay != null)
        {
            totalAmmunitionDisplay.SetText(aRTotalAmmo + "");
        }

        // Check if any ammo left
        if (aRTotalAmmo <= 0)
        {
            aRTotalAmmoLeft = false;
        }
        else
        {
            aRTotalAmmoLeft = true;
        }
    }

    void CheckMainPistolActions()
    {
        if (fullAutoMode) aRShooting = Input.GetKey(KeyCode.Mouse0);
        else aRShooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reload button and reload if trying to shoot with empty magazine
        if (Input.GetKeyDown(KeyCode.R) && ammoLeftInARMag < magazineSize && !aRReloading && aRTotalAmmoLeft)
            ReloadMainPistol();
        if (aRReadyToShoot && aRShooting && !aRReloading && ammoLeftInARMag <= 0 && aRTotalAmmoLeft)
            ReloadMainPistol();

        // Shoot if ammoleft in magazine
        if (aRReadyToShoot && aRShooting && !aRReloading && ammoLeftInARMag > 0 && gm.gamePaused == false)
        {
            aRBulletsShot = 0;
            ShootMainPistol();
        }
    }

    void ShootMainPistol()
    {
        // Set state
        aRReadyToShoot = false;

        // Raycast
        Vector3 rayOrigin = aimCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        // Check if raycast hits a dummy, headshot x2 damage
        if (Physics.Raycast(rayOrigin, aimCam.transform.forward, out hit))
        {
            TargetDummyHead targetDummyHead = hit.transform.GetComponent<TargetDummyHead>();
            TargetDummyBody targetDummyBody = hit.transform.GetComponent<TargetDummyBody>();
            if (targetDummyHead != null)
            {
                targetDummyHead.TakeDamageHead(aRDamage * 3);
            }
            else if (targetDummyBody != null)
            {
                targetDummyBody.TakeDamageBody(aRDamage);
            }

            // If not hitting a dummy, make a bullet hole
            else
            {

                //Instantiate the bullet hole on the hit point of the raycast, offset by 0.001 to avoid clipping
                GameObject bH = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                bH.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                float randomBHRot = Random.Range(0f, 360f);
                bH.transform.Rotate(0, randomBHRot, 0f);
                bH.transform.SetParent(bHContainer.transform);
                Destroy(bH, 5f);
            }


        }

        // Play sound and muzzleflash
        muzzleFlash.Play();
        animator.SetTrigger("arShoot");
        gunSound.PlayOneShot(gunSoundClip, 1f);

        // Decrease ammunition left
        ammoLeftInARMag--;
        aRBulletsShot++;

        // Debugging
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    IEnumerator AimingDownSight()
    {
        yield return new WaitForSeconds(.10f);
        normalFOV = aimCam.fieldOfView;
        aimCam.fieldOfView = scopedFOV;
        scopeRedDot.SetActive(true);
    }

    public void UnAimingDownSight()
    {
        Debug.Log("Unaiming called");
        aimCam.fieldOfView = normalFOV;
        scopeRedDot.SetActive(false);
    }

    public void ResetShot()
    {
        // Set states
        aRReadyToShoot = true;
        allowInvoke = true;
    }

    void ReloadMainPistol()
    {
        // Start reloading state
        aRReloading = true;
        reloadingText.SetText("Reloading...");

        // Wait for reloading time, then call finishing
        Invoke("ReloadAssaultRifleFinished", reloadTime);
        Debug.Log("Reloading");
    }

    public void ReloadAssaultRifleFinished()
    {
        if(aRStillActive)
        {
            // Count how many bullets were loaded into the magazine
            int reloadedAmmo = magazineSize - ammoLeftInARMag;

            // Fully loads the magazine if player has enough total ammo left. Otherwise loads all the ammo player has left into the magazine
            if (reloadedAmmo < aRTotalAmmo)
            {
                ammoLeftInARMag = magazineSize;
            }
            else
            {
                reloadedAmmo = aRTotalAmmo;
                ammoLeftInARMag = reloadedAmmo;
            }
            // Decrease total ammo by the amount of ammo reloaded into the magazine
            aRTotalAmmo = aRTotalAmmo - reloadedAmmo;

            // End reloading state
            aRReloading = false;
            reloadingText.SetText("");
            Debug.Log("Reloading Finished");

        }
    }

    void OnEnable()
    {
        aRStillActive = true;
    }

    void OnDisable()
    {
        aRStillActive = false;

        if (aRReloading)
        {
            // End reloading state
            aRReloading = false;
            reloadingText.SetText("");
            Debug.Log("Reloading Canceled");
            CancelInvoke("ReloadAssaultRifleFinished");
        }
    }
}
