using System;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public class Tree : MonoBehaviour
{
    public int Health;

    private float cutCooldown;
    public GameObject treeHitPrefab;
    public GameObject treeDeathPrefab;
    public GameObject logPickUp;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (cutCooldown > 0.0f)
        {
            cutCooldown -= Time.deltaTime;
        }
        else
        {
            if (Health <= 0)
            {
                if (rigidbody.IsSleeping())
                {
                    Instantiate(treeDeathPrefab, transform.position, new Quaternion());
                    Instantiate(logPickUp, transform.position, new Quaternion());
                    Instantiate(logPickUp, transform.position, new Quaternion());
                    Destroy(gameObject);
                }
            }
        }
    }

    private void InflictDamage(int damage)
    {
        Health = Math.Max(0, Health - damage);
        Debug.Log("Health left: " + Health);
    }

    void OnTriggerEnter(Collider collider)
    {
        Cut(collider);
    }

    private void Cut(Collider collider)
    {
        if (Health <= 0 || cutCooldown > 0.0f)
        {
            return;
        }

        InflictDamage(10);
        var hitPosition = collider.transform.position;

        cutCooldown = 0.2f;
        Instantiate(treeHitPrefab, hitPosition, new Quaternion());
        if (Health > 0)
        {
            //rigidbody.AddForce(new Vector3(0.0f, 100.0f, 0.0f));
        }
        else
        {
            Timber(hitPosition, transform.position - collider.transform.position);
        }
    }

    private void Timber(Vector3 hitPosition, Vector3 direction)
    {
        Instantiate(treeDeathPrefab, hitPosition, new Quaternion());
        rigidbody.AddForce(new Vector3(500.0f * direction.x, 0.0f, 500.0f * direction.z));
    }
}
