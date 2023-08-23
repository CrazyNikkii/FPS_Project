using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices.ComTypes;

public class MainPistolScript : MonoBehaviour
{
    // Stats
    [SerializeField] private float timeBetweenShooting, reloadTime, timeBetweenShots;
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonDown;
    int bulletsLeft, bulletsShot;

    // State
    bool shooting, readyToShoot, reloading;

    // Refs
    public Camera aimCam;
    public AudioSource gunSound;
    public AudioClip gunSoundClip;
    public GameObject reloadingText;
    public GameObject bulletHole;
    public GameManager gm;

    // Graphics
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // Debug
    public bool allowInvoke = true;

    public void Awake()
    {
        //reloadingText = GameObject.FindWithTag("ReloadText");

        // Start with full magazine
        bulletsLeft = magazineSize;
        readyToShoot = true;
        //reloadingText.SetActive(false);
        
    }

    void Start()
    {
        // Get weapon sound
        gunSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        MainPistolActions();

        if(ammunitionDisplay != null)
        ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    void MainPistolActions()
    {
        shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) ReloadMainPistol();
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) ReloadMainPistol();

        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && gm.gamePaused == false)
        {
            bulletsShot = 0;
            ShootMainPistol();
        }
    }

    void ShootMainPistol()
    {
        readyToShoot = false;

        Vector3 rayOrigin = aimCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, aimCam.transform.forward, out hit))
        {
            GameObject bH = Instantiate(bulletHole, hit.point + hit.normal * 0.001f, Quaternion.identity) as GameObject;
            bH.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            float randomBHRot = Random.Range(0f, 360f);
            bH.transform.Rotate(0, randomBHRot, 0f);
            Destroy(bH, 5f);
        }

        // Muzzleflash And Sound
        muzzleFlash.Play();
        gunSound.PlayOneShot(gunSoundClip, 1f);

        bulletsLeft--;
        bulletsShot++;

         if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void ReloadMainPistol()
    {
        reloading = true;
        //reloadingText.SetActive(true);
        Invoke("ReloadMainPistolFinished", reloadTime);
        Debug.Log("Reloading");
    }

    public void ReloadMainPistolFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        //reloadingText.SetActive(false);
        Debug.Log("Reloading Finished");
    }
}
