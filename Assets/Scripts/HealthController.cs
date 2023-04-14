using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
   [SerializeField]
   float maxHealth = 100;

    [SerializeField]
    Slider healthBar;

    float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = currentHealth;
        healthBar.value = currentHealth;
    }
    void OnTriggerEnter(Collider other)
    {
        BulletController controller = other.GetComponent<BulletController>();
        if (controller != null) 
        {
            TakeDamage(controller.GetDamage());
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
