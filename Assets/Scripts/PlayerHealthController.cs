using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100;

    [SerializeField]
    Slider healthBar;

    float currentHealth;

    [SerializeField]
    float damage = 5.0F;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            TakeDamage(damage);
        }
        else
        {
            return;
        }
    }
    public void TakeDamage(float value)
    {
        currentHealth -= Mathf.Abs(value);
        if (currentHealth < 0.0F)
        {
            currentHealth = 0.0F;
            Destroy(gameObject);
            return;
        }
        healthBar.value = currentHealth;
    }
}
