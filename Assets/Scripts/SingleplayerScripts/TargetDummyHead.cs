using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyHead : MonoBehaviour
{
    public float headHealth = 100f;
    public GameManager gm;

    public void Start()
    {

    }


    public void TakeDamageHead(float damage)
    {
        headHealth -= damage;
        if (headHealth <= 0)
        {
            Die();
        }
        Debug.Log("Took " + damage + " damage to head");
    }

    void Die()
    {
        Destroy(transform.parent.gameObject);
        gm.enemiesLeft--;
    }
}
