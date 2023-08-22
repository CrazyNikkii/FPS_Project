using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletControllerNetwork : NetworkBehaviour
{
    public GameObject bulletHolePrefab;

    [ClientRpc]
    void SpawnBulletHoleClientRpc(Vector3 hitPoint, Vector3 hitNormal)
    {
        Quaternion hitPointRot = Quaternion.FromToRotation(Vector3.up, hitNormal);
        Vector3 offset = hitNormal * 0.001f;
        Vector3 pos = hitPoint + offset;

        Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        hitPointRot *= randomRot;

        Vector3 randomScale = Vector3.one * Random.Range(0.02f, 0.04f);

        Instantiate(bulletHolePrefab, pos, hitPointRot).transform.localScale = randomScale;
    }

    [ServerRpc(RequireOwnership = false)]
    void DespawnBulletServerRpc()
    {
        NetworkObject bulletNetworkObject = GetComponent<NetworkObject>();
        bulletNetworkObject.Despawn(true);
    }

   // [NetworkStart]
    //void NetworkStart()
    //{
        // Call the spawn RPC after the object has properly spawned
       // if (IsServer)
      //  {
      //      DespawnBulletServerRpc();
      //  }
   // }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsServer)
            return;

        if (collision.gameObject.CompareTag("Player"))
            return;

        ContactPoint bulletHitPoint = collision.contacts[0];

        Vector3 hitPoint = bulletHitPoint.point;
        Vector3 hitNormal = bulletHitPoint.normal;

        // Spawn bullet hole on clients
        SpawnBulletHoleClientRpc(hitPoint, hitNormal);
    }
}






