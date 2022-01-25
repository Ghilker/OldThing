using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

public class MagicBulletStats : MonoBehaviour
{
    public int damage = 5;
    public GameObject bRender1;
    public GameObject bRender2;

    private Collider col;
    private Rigidbody rigidBody;
    private Vector3 vel;
    private Vector3 angularVel;
    private Vector3 position;
    private ParticleSystem particles;

    private void Start()
    {
        col = gameObject.GetComponent<Collider>();
        rigidBody = gameObject.GetComponent<Rigidbody>();
        particles = GetComponent<ParticleSystem>();
    }

    private void FixedUpdate()
    {
        vel = rigidBody.velocity;
        angularVel = rigidBody.angularVelocity;
        position = transform.position;
    }

    void BulletExplosion()
    {
        Rigidbody bulletRb = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = false;
        bRender1.GetComponent<Renderer>().enabled = false;
        bRender2.GetComponent<Renderer>().enabled = false;
        bulletRb.velocity = Vector3.zero;
        bulletRb.freezeRotation = true;
        particles.Play();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Monster")
        {
            other.transform.gameObject.GetComponent<MonsterStats>().TakeDamage(damage);
        }
        if (other.transform.tag == "Obstacle")
        {
            Physics.IgnoreCollision(col, other.collider);
            rigidBody.velocity = vel;
            rigidBody.angularVelocity = angularVel;
            transform.position = position;
            return;
        }
        BulletExplosion();
        Destroy(gameObject, particles.main.duration);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}