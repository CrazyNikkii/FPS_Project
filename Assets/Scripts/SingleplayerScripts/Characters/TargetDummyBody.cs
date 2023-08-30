using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyBody : MonoBehaviour
{
    public float bodyHealth = 100f;
    public GameManager gm;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
        bodyHealth = 100f;
        gm.enemiesLeft = gm.numberOfDummies;
    }

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
        gm.enemiesLeft--;
        Debug.Log("Enemy died. Remaining enemies: " + gm.enemiesLeft);
        gm.enemiesLeftText.text = gm.enemiesLeft.ToString() + ": Left";
        Destroy(transform.parent.gameObject);

    }
}
