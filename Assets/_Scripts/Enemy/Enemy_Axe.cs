using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Enemy_Axe : MonoBehaviour
{
    [SerializeField] private GameObject impactFx;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform axeVisual;
    [SerializeField] private int axeDamage;
    private bool hasHit = false;


    private Vector3 direction;
    private Transform player;
    private float flySpeed;
    private float rotationSpeed;
    private float timer = 1;

    public void AxeSetup(float flySpeed, Transform player, float timer)
    {
        rotationSpeed = 1600;

        this.flySpeed = flySpeed;
        this.player = player;
        this.timer = timer;
    }

    private void Update()
    {
        axeVisual.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        timer -= Time.deltaTime;

        if (timer > 0)
            direction = player.position + Vector3.up - transform.position;


        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }


    void OnCollisionEnter(Collision collision)
    {
        IDamagable damagable = collision.gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = other.GetComponent<IDamagable>();
        if (hasHit) return;
        if (damagable != null)
        {
            hasHit = true;
            GameObject newFx = ObjectPool.instance.GetObject(impactFx, transform);
            newFx.transform.position = transform.position;
            damagable.TakeDamage(axeDamage);
            
             // Disable collider to prevent further triggers
            GetComponent<Collider>().enabled = false;

            ObjectPool.instance.ReturnObject(gameObject);
            ObjectPool.instance.ReturnObject(newFx, 1f);
        }
    }
}
