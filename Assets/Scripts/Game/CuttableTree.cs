using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game
{
    public class CuttableTree : MonoBehaviour
    {
        #region Fields
        public GameObject BirdScatterPrefab;
        public int Health;
        public GameObject TreeDeathPrefab;
        public GameObject TreeHitPrefab;
        public TreeType Type;
        public PickUp pickupSpawner;

        private Attack _attackThatHitMe;
        private float _cutCooldown;
        private bool _isFirstHit = false;
        #endregion

        #region Enums
        public enum TreeType
        {
            TreeLevel1,
            TreeLevel2,
            TreeLevel3
        }
        #endregion

        #region Methods
        private void Cut(Collider collider, Attack attack)
        {
            if (attack == null)
            {
                Debug.Log("Attack is null :(");
            }

            if (attack.Explosive == false)
            {
                if (Health <= 0 || _cutCooldown > 0.0f)
                {
                    return;
                }
            }

            _attackThatHitMe = attack;

            InflictDamage(attack.Damage);
            var hitPosition = collider.transform.position;

            _cutCooldown = 0.2f;
            Instantiate(TreeHitPrefab, hitPosition, new Quaternion());
            if (Health > 0)
            {
                var animation = GetComponentInParent<Animation>();
                if (animation != null)
                {
                    animation.Play("JunkWiggle");
                }
            }
            else
            {
                var explosionRadius = attack.Explosive ? collider.GetComponent<SphereCollider>().radius : 0.0f;
                Timber(
                    hitPosition,
                    transform.position - collider.transform.position,
                    attack.HitForce,
                    attack.Explosive,
                    attack.transform.position,
                    explosionRadius);
            }
        }

        // Use this for initialization

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
            AudioSingleton.Instance.PlaySound(SoundType.WoodChop);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (rigidbody.isKinematic == true && collision.collider.tag.Equals("Tree")
                && collision.relativeVelocity.magnitude > 3.0f)
            {
                _attackThatHitMe = collision.gameObject.GetComponent<CuttableTree>()._attackThatHitMe;
                InflictDamage(Health);
                Timber(
                    collider.transform.position,
                    transform.position - collider.transform.position,
                    collision.relativeVelocity.magnitude * 300.0f,
                    false);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag.Equals("Attack"))
            {
                Cut(collider, collider.GetComponent<Attack>());
            }
            else if (collider.tag.Equals("Weapon"))
            {
                var axe = collider.GetComponentInParent<AxeStats>();
                Cut(collider, axe.GetComponent<Attack>());
            }
        }

        private void Start()
        {
            if (pickupSpawner == null)
            {
                pickupSpawner = GetComponent<PickUp>();
            }
        }

        private void Timber(
            Vector3 hitPosition,
            Vector3 direction,
            float hitForce,
            bool explosive,
            Vector3 attackPosition = new Vector3(),
            float explosionRadius = 0.0f)
        {
            if (_attackThatHitMe != null)
            {
                if (_attackThatHitMe.Owner != null)
                {
                    var lumberjack = _attackThatHitMe.Owner.GetComponent<Lumberjack>();
                    pickupSpawner.PlayerPosition = lumberjack.transform;
                    pickupSpawner.OwningPlayer = lumberjack.Display.PlayerNumber;
                    lumberjack.DownedTrees++;
                }
            }
            rigidbody.isKinematic = false;
            Instantiate(TreeDeathPrefab, hitPosition, new Quaternion());
            if (explosive)
            {
                rigidbody.AddExplosionForce(hitForce, attackPosition, explosionRadius);
            }
            else
            {
                rigidbody.AddForce(new Vector3(hitForce * direction.x, 0.0f, hitForce * direction.z));
            }

            gameObject.layer = LayerMask.NameToLayer("FallenTrees");
        }

        private void Update()
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
        #endregion
    }
}