using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Helper;

public class MagicBulletStats : MonoBehaviour
{
    public int damage = 5;
    public int maxBounces = 2;
    public string bulletTarget = "Monster";

    private Collider2D col;
    private Rigidbody2D rigidBody;
    private Vector3 vel;
    private float angularVel;
    private Vector3 position;
    private int currentBounces = 0;

    private void Start()
    {
        col = gameObject.GetComponent<Collider2D>();
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5);
    }

    private void FixedUpdate()
    {
        vel = rigidBody.velocity;
        angularVel = rigidBody.angularVelocity;
        position = transform.position;
    }

    void BulletExplosion()
    {
        Rigidbody2D bulletRb = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        bulletRb.velocity = Vector3.zero;
        bulletRb.freezeRotation = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.transform.tag == bulletTarget)
        {
            if (other.transform.tag == "Monster")
            {
                other.transform.gameObject.GetComponent<MonsterStats>().TakeDamage(damage);
            }
            else if (other.transform.tag == "Player")
            {

            }
            BulletExplosion();
            return;
        }
        else if (other.transform.tag == "Wall")
        {
            if (currentBounces == maxBounces)
            {
                BulletExplosion();
                return;
            }
            float speed = vel.magnitude;
            Vector2 bulletReflectionDirection = Vector2.Reflect(vel, other.contacts[0].normal);
            vel = rigidBody.velocity = bulletReflectionDirection.normalized * speed;
            currentBounces++;

            return;
        }
        else
        {
            Physics2D.IgnoreCollision(col, other.collider);
            rigidBody.velocity = vel;
            rigidBody.angularVelocity = angularVel;
            transform.position = position;
            if (rigidBody.velocity == Vector2.zero)
            {
                BulletExplosion();
            }
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}