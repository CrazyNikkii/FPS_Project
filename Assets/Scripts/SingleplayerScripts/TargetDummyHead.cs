using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyHead : MonoBehaviour
{
    public float headHealth = 100f;
    public GameManager gm;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
        headHealth = 100f;
        gm.enemiesLeft = gm.numberOfDummies;
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
        gm.enemiesLeft--;
        Debug.Log("Enemy died. Remaining enemies: " + gm.enemiesLeft);
        gm.enemiesLeftText.text = gm.enemiesLeft.ToString() + ": Left";
        Destroy(transform.parent.gameObject);
    }
}
