using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunController : MonoBehaviour
{
    // Bullets
    public GameObject bullet;
    public float bulletForce, upwardForce;

    // Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonDown;
    int bulletsLeft, bulletsShot;

    // State
    bool shooting, readyToShoot, reloading;

    // Refs
    public Camera aimCam;
    public Transform shootingPoint;

    // Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // Debug
    public bool allowInvoke = true;

    public void Awake()
    {
        // Start with full magazine
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    void Start()
    {
        
    }


    void Update()
    {
        Actions();

        if(ammunitionDisplay != null)
        ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    public void Actions()
    {
        // Firemode Check
        if(allowButtonDown) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if(Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            Shoot();
        }
    }

    public void Shoot()
    {
        readyToShoot = false;
        // Hit point found by raycast
        Ray ray = aimCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Checks if hit
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        targetPoint = hit.point;
        else
        targetPoint = ray.GetPoint(75); //random point faraway

        // Calculates direction
        Vector3 directionWithoutSpread = targetPoint - shootingPoint.position;
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        // Calculates new direction with the spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        // Spawn the bullet
        GameObject currentBullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletForce, ForceMode.Impulse);

        // Toggle when muzzleflash
        //if (muzzleFlash != null)
        //Instantiate(muzzleFlash, shootingPoint.position, Quaternion.identity,);

        bulletsLeft--;
        bulletsShot++;

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
        // This is for Shotguns
        //If (bulletShot < bulletsPerTap && bulletsLeft > 0)
            //Invoke("Shoot", timeBetweenShots);
    }

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    public void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    public void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
