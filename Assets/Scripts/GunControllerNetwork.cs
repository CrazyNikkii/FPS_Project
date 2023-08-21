using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GunControllerNetwork : NetworkBehaviour
{
    // Bullets
    public NetworkObject bulletPrefab;
    public float bulletForce = 20f, upwardForce;

    // Stats
    public float timeBetweenShooting = 0.5f, spread, reloadTime = 2f, timeBetweenShots = 0f;
    public int magazineSize = 30, bulletsPerTap = 1;
    public bool allowButtonDown = true;
    int bulletsLeft, bulletsShot;

    // State
    bool shooting, readyToShoot, reloading;

    // Refs
    public Camera aimCam;
    public Transform shootingPoint;
    public AudioSource gunSound;
    public AudioClip gunSoundClip;
    public GameObject reloadingText;

    // Graphics
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // Debug
    public bool allowInvoke = true;

    public void Awake()
    {
        reloadingText = GameObject.FindWithTag("ReloadText");

        // Start with full magazine
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    void Start()
    {
        gunSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!IsOwner) return;
        Actions();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + "/" + magazineSize / bulletsPerTap);
    }

    public void Actions()
    {
        if (allowButtonDown) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;

            ShootBulletServerRpc();
        }
    }

    [ServerRpc]
    public void ShootBulletServerRpc()
    {
        if(!readyToShoot || reloading) return;
        readyToShoot = false;

        Ray ray = aimCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 shootingPointWorldPosition = CalculateShootingPointWorldPosition();
        
        Vector3 directionWithoutSpread = targetPoint - shootingPointWorldPosition;

        int randomSeed = Random.Range(int.MinValue, int.MaxValue);

        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(spreadX, spreadY, 0);

        SpawnBulletClientRpc(bulletForce, directionWithSpread.normalized, randomSeed);
    }

    [ClientRpc]
    private void SpawnBulletClientRpc(float force, Vector3 direction, int randomSeed)
    {
        Random.InitState(randomSeed);
        Vector3 shootingPosition = shootingPoint.position;
        Quaternion bulletRotation = Quaternion.LookRotation(direction);

        NetworkObject currentBullet = Instantiate(bulletPrefab, shootingPosition, bulletRotation);
        currentBullet.Spawn();

        Rigidbody bulletRigidbody = currentBullet.GetComponent<Rigidbody>();
        bulletRigidbody.velocity = direction * force;

        muzzleFlash.Play();
        gunSound.PlayOneShot(gunSoundClip, 1f);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }
    private Vector3 CalculateShootingPointWorldPosition()
{
    Transform playerTransform = transform; // Player prefab's transform
    Transform cameraHolder = playerTransform.Find("CameraHolder");
    Transform playerCam = cameraHolder.Find("PlayerCam");
    Transform gunModel = playerCam.Find("AK74");
    Transform shootingPoint = gunModel.Find("shootingPoint");

    return shootingPoint.position;
}

    public void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    public void Reload()
    {
        reloading = true;
        // Reloading logic

        Invoke("ReloadFinished", reloadTime);
        Debug.Log("Reloading");
    }

    public void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        Debug.Log("Reloading Finished");
    }
}

