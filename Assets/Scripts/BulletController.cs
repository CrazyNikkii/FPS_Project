using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    public Rigidbody rb;
    public GameObject bulletHole;
    public Vector3 bulletHitPoint;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag != "Player")
            {
            ContactPoint bulletHitPoint = collision.contacts[0];
            Quaternion hitPointRot = Quaternion.FromToRotation(Vector3.up, bulletHitPoint.normal);

            Vector3 offset = bulletHitPoint.normal * 0.001f; // Offset by 0.001 units along the normal direction
            Vector3 pos = bulletHitPoint.point + offset;

            Quaternion randomRot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f); // Random rotation around the Y-axis
            hitPointRot *= randomRot; // Combine the random rotation with the surface normal rotation

            Vector3 randomScale = Vector3.one * Random.Range(0.02f, 0.04f); // Random scale between 0.02 and 0.04

            Instantiate(bulletHole, pos, hitPointRot).transform.localScale = randomScale;


            }
            Destroy(gameObject);
        }
}
