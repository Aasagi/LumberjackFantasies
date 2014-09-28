﻿using System;
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
    public PickUp pickupSpawner;

    private AxeStats _axeThatCutMe;

    // Use this for initialization
    void Start()
    {
        if (pickupSpawner == null)
        {
            pickupSpawner = GetComponent<PickUp>();
        }
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
        if (collider.tag.Equals("Attack"))
        {
            Cut(collider, collider.GetComponent<Attack>());
        }
        else if (collider.tag.Equals("Weapon"))
        {
            var axe = collider.GetComponentInParent<AxeStats>();
            _axeThatCutMe = axe;
            Cut(collider, axe.GetComponent<Attack>());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rigidbody.isKinematic == true && collision.collider.tag.Equals("Tree") && collision.relativeVelocity.magnitude > 3.0f)
        {
            _axeThatCutMe = collision.gameObject.GetComponent<CuttableTree>()._axeThatCutMe;
            InflictDamage(Health);
            Timber(collider.transform.position, transform.position - collider.transform.position, collision.relativeVelocity.magnitude * 300.0f, false);
        }
    }

    private void Cut(Collider collider, Attack attack)
    {
        if (attack.Explosive == false)
        {
            if (Health <= 0 || _cutCooldown > 0.0f)
            {
                return;
            }
        }

        InflictDamage(attack.Damage);
        var hitPosition = collider.transform.position;

        _cutCooldown = 0.2f;
        Instantiate(TreeHitPrefab, hitPosition, new Quaternion());
        if (Health > 0)
        {
            animation.Play("JunkWiggle");
        }
        else
        {
            Timber(hitPosition, transform.position - collider.transform.position, attack.HitForce, attack.Explosive, attack.transform.position);
            if (_axeThatCutMe != null)
            {
                var componentInParent = _axeThatCutMe.GetComponentInParent<Lumberjack>();
                pickupSpawner.PlayerPosition = componentInParent.transform;
            }
        }
    }

    private void Timber(Vector3 hitPosition, Vector3 direction, float hitForce, bool explosive, Vector3 attackPosition = new Vector3())
    {
        if (_axeThatCutMe != null)
        {
            _axeThatCutMe.DownedTrees++;
        }
        rigidbody.isKinematic = false;
        Instantiate(TreeDeathPrefab, hitPosition, new Quaternion());
        if (explosive)
        {
            rigidbody.AddExplosionForce(hitForce, attackPosition, 6.0f);
        }
        else
        {
            rigidbody.AddForce(new Vector3(hitForce * direction.x, 0.0f, hitForce * direction.z));
        }
    }
}