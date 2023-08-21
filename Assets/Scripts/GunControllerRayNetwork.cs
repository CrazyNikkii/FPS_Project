using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class GunControllerRayNetwork : NetworkBehaviour
{
    // Stats
    public float timeBetweenShooting = 0.5f, reloadTime = 2f;
    public int magazineSize = 30, bulletsPerTap = 1;
    public bool allowButtonDown = true;
    int bulletsLeft;

    // State
    bool shooting, readyToShoot, reloading;

    // Refs
    public Camera aimCam;
    public Transform shootingPoint;
    public AudioSource gunSound;
    public AudioClip gunSoundClip;

    // Graphics
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;

    // Debug
    public bool allowInvoke = true;

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
            bulletsLeft -= bulletsPerTap;

            ShootRayServerRpc();
        }
    }

   [ServerRpc]
    private void ShootRayServerRpc()
    {
        Ray ray = aimCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 rayOrigin = ray.origin;
        Vector3 rayDirection = ray.direction;

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Process the hit, apply damage, etc.
        }

        RpcShootEffectsClientRpc(rayOrigin, rayDirection); // Call RpcShootEffectsClientRpc on clients
    }

    [ClientRpc]
    private void RpcShootEffectsClientRpc(Vector3 origin, Vector3 direction)
    {
        // Play muzzle flash, gun sound, etc.
        // You can implement the visual and audio effects on the client here.
        PlayMuzzleFlashAndSound();
    }

    private void PlayMuzzleFlashAndSound()
    {
        muzzleFlash.Play();
        gunSound.PlayOneShot(gunSoundClip, 1f);
    }


    private void Reload()
    {
        reloading = true;
        // Reloading logic

        Invoke("ReloadFinished", reloadTime);
        Debug.Log("Reloading");
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        Debug.Log("Reloading Finished");
    }
} 
