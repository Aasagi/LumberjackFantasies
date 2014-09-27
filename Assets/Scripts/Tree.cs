using System;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tree : MonoBehaviour
{
    public enum TreeType
    {
        TreeLevel1,
        TreeLevel2,
        TreeLevel3
    }

    public TreeType Type;
    public int Health;

    private float cutCooldown;
    private bool isFirstHit = false;
    public GameObject birdScatterPrefab;
    public GameObject treeHitPrefab;
    public GameObject treeDeathPrefab;

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
                if (rigidbody.velocity.magnitude < 0.1f)
                {
                    Instantiate(treeDeathPrefab, transform.position, new Quaternion());
                    AddOnFunctions.KillAndDestroy(gameObject);
                }
            }
        }
    }

    private void InflictDamage(int damage)
    {
        if (isFirstHit == false)
        {
            isFirstHit = true;
            var chance = Random.Range(0, 100);
            if (chance <= 20)
            {
                Instantiate(birdScatterPrefab, transform.position, new Quaternion());
            }
        }
        Health = Math.Max(0, Health - damage);
        Debug.Log("Health left: " + Health);
    }

    void OnTriggerEnter(Collider collider)
    {
        Cut(collider);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.isKinematic == true && collision.collider.tag.EndsWith("Tree") && collision.relativeVelocity.magnitude > 3.0f)
        {
            rigidbody.isKinematic = false;
            if (Health > 0)
            {
                InflictDamage(Health);
            }
        }
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
            animation.Play("JunkWiggle");
        }
        else
        {
            Timber(hitPosition, transform.position - collider.transform.position);
        }
    }

    private void Timber(Vector3 hitPosition, Vector3 direction)
    {
        rigidbody.isKinematic = false;
        Instantiate(treeDeathPrefab, hitPosition, new Quaternion());
        rigidbody.AddForce(new Vector3(500.0f * direction.x, 0.0f, 500.0f * direction.z));
    }
}
