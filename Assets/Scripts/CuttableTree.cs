using System;
using Assets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class CuttableTree : MonoBehaviour
{
    public enum TreeType
    {
        TreeLevel1,
        TreeLevel2,
        TreeLevel3
    }

    public TreeType Type;
    public int Health;

    private float _cutCooldown;
    private bool _isFirstHit = false;
    public GameObject BirdScatterPrefab;
    public GameObject TreeHitPrefab;
    public GameObject TreeDeathPrefab;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_cutCooldown > 0.0f)
        {
            _cutCooldown -= Time.deltaTime;
        }
        else
        {
            if (Health <= 0)
            {
                if (rigidbody.velocity.magnitude < 0.1f)
                {
                    Instantiate(TreeDeathPrefab, transform.position, new Quaternion());
                    AddOnFunctions.KillAndDestroy(gameObject);
                }
            }
        }
    }

    private void InflictDamage(int damage)
    {
        if (_isFirstHit == false)
        {
            _isFirstHit = true;
            var chance = Random.Range(0, 100);
            if (chance <= 20)
            {
                Instantiate(BirdScatterPrefab, transform.position, new Quaternion());
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
        if (rigidbody.isKinematic == true && collision.collider.tag.Equals("Tree") && collision.relativeVelocity.magnitude > 3.0f)
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
        if (Health <= 0 || _cutCooldown > 0.0f)
        {
            return;
        }

        var axe = collider.GetComponent<AxeStats>();
        if (axe == null)
        {
            Debug.Log("Could not find axe stats");
        }
       
        InflictDamage(axe.Damage);
        var hitPosition = collider.transform.position;

        _cutCooldown = 0.2f;
        Instantiate(TreeHitPrefab, hitPosition, new Quaternion());
        if (Health > 0)
        {
            animation.Play("JunkWiggle");
        }
        else
        {
            Timber(hitPosition, transform.position - collider.transform.position, axe.HitForce);
            axe.DownedTrees++;
        }
    }

    private void Timber(Vector3 hitPosition, Vector3 direction, float hitForce)
    {
        rigidbody.isKinematic = false;
        Instantiate(TreeDeathPrefab, hitPosition, new Quaternion());
        rigidbody.AddForce(new Vector3(hitForce * direction.x, 0.0f, hitForce * direction.z));
    }
}