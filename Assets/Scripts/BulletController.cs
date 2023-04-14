using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    float speed = 5.0F;

    [SerializeField]
    float damage = 10.0F;

    [SerializeField]
    float lifeTime = 5.0F;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    public float GetDamage() { return damage; }

}
