using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyBody : MonoBehaviour
{
    public float bodyHealth = 100f;

    public void TakeDamageBody(float damage)
    {
        bodyHealth -= damage;
        if (bodyHealth <= 0)
        {
            Die();
        }
        Debug.Log("Took " + damage + " damage to body");
    }

    void Die()
    {
        Destroy(transform.parent.gameObject);
    }
}
