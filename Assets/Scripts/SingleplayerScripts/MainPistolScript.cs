using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainPistolScript : MonoBehaviour
{
    // Weapon statistics
    [SerializeField] private float timeBetweenShooting, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonDown;
    int bulletsLeft, bulletsShot;
    public float damage = 50f;
    public int maxAmmo = 20;
    public int totalAmmo;

    // States
    bool shooting, readyToShoot, reloading;
    public bool totalAmmoLeft;

    // References
    public Camera aimCam;
    public AudioSource gunSound;
    public AudioClip gunSoundClip;
    public GameObject bulletHole;
    public GameManager gm;
    public TargetDummyBody dummyTargetBody;
    public TargetDummyHead dummyTargetHead;
    public ParticleSystem muzzleFlash;

    // HUD
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI totalAmmunitionDisplay;
    public TextMeshProUGUI reloadingText;

    // Debugging
    public bool allowInvoke = true;

    void Awake()
    {
        // Set ammo values to max and set the right state
        totalAmmo = maxAmmo;
        totalAmmoLeft = true;
        bulletsLeft = magazineSize;
        readyToShoot = true;
        reloadingText.SetText("");
    }

    void Start()
    {
        // Sound reference
        gunSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckMainPistolActions();

        // HUD ammocounter
        if (ammunitionDisplay != null)
        {
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
        }

        if (totalAmmunitionDisplay != null)
        {
            totalAmmunitionDisplay.SetText(totalAmmo + "");
        }

        // Check if any ammo left
        if (totalAmmo <= 0)
        {
            totalAmmoLeft = false;
        }
        else
        {
            totalAmmoLeft = true;
        }
    }

    void CheckMainPistolActions()
    {
        // Shooting button
        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reload button and reload if trying to shoot with empty magazine
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && totalAmmoLeft)
            ReloadMainPistol();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0 && totalAmmoLeft)
            ReloadMainPistol();

        // Shoot if ammoleft in magazine
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && gm.gamePaused == false)
        {
            bulletsShot = 0;
            ShootMainPistol();
        }
    }

    void ShootMainPistol()
    {
        // Set state
        readyToShoot = false;

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
                targetDummyHead.TakeDamageHead(damage * 2);
            }
            else if (targetDummyBody != null)
            {
                targetDummyBody.TakeDamageBody(damage);
            }

            // If not hitting a dummy, make a bullet hole
            else
            {
                // Instantiate the bullet hole on the hit point of the raycast, offset by 0.001 to avoid clipping
                GameObject bH = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
                bH.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                float randomBHRot = Random.Range(0f, 360f);
                bH.transform.Rotate(0, randomBHRot, 0f);
                Destroy(bH, 5f);
            }

            
        }

        // Play sound and muzzleflash
        muzzleFlash.Play();
        gunSound.PlayOneShot(gunSoundClip, 1f);

        // Decrease ammonition left
        bulletsLeft--;
        bulletsShot++;
        
        // Debugging
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    public void ResetShot()
    {
        // Set states
        readyToShoot = true;
        allowInvoke = true;
    }

    void ReloadMainPistol()
    {
        // Start reloading state
        reloading = true;
        reloadingText.SetText("Reloading...");

        // Wait for reloading time, then call finishing
        Invoke("ReloadMainPistolFinished", reloadTime);
        Debug.Log("Reloading");
    }

    public void ReloadMainPistolFinished()
    {
        int reloadedAmmo = magazineSize - bulletsLeft;

        if (reloadedAmmo < totalAmmo)
        {
            bulletsLeft = magazineSize;
        }
        else
        {
            reloadedAmmo = totalAmmo;
            bulletsLeft = reloadedAmmo;
        }
        totalAmmo = totalAmmo - reloadedAmmo;

        // End reloading state
        reloading = false;
        reloadingText.SetText("");
        Debug.Log("Reloading Finished");
    }
}
