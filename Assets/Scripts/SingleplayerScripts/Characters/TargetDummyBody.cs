using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyBody : MonoBehaviour
{
    public float bodyHealth = 100f;
    public TestingModeManager testingModeManager;

    public void Start()
    {
        testingModeManager = FindObjectOfType<TestingModeManager>();
        bodyHealth = 100f;
        testingModeManager.enemiesLeft = testingModeManager.numberOfDummies;
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
        testingModeManager.enemiesLeft--;
        Debug.Log("Enemy died. Remaining enemies: " + testingModeManager.enemiesLeft);
        testingModeManager.enemiesLeftText.text = testingModeManager.enemiesLeft.ToString() + ": Left";
        Destroy(transform.parent.gameObject);

    }
}
