using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDummyHead : MonoBehaviour
{
    public float headHealth = 100f;
    public TestingModeManager testingModeManager;

    public void Start()
    {
        testingModeManager = FindObjectOfType<TestingModeManager>();
        headHealth = 100f;
        testingModeManager.enemiesLeft = testingModeManager.numberOfDummies;
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
        testingModeManager.enemiesLeft--;
        Debug.Log("Enemy died. Remaining enemies: " + testingModeManager.enemiesLeft);
        testingModeManager.enemiesLeftText.text = testingModeManager.enemiesLeft.ToString() + ": Left";
        Destroy(transform.parent.gameObject);
    }
}
