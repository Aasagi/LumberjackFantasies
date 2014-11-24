using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Lumberjack : MonoBehaviour
    {

        // Use this for initialization
        private Animation CurrentAnimation;
        private AxeContainer _axeContainer;
        public GameObject Axe;
        public GameObject GroundSmashPrefab;
        public ParticleSystem Footsteps;
        private float lockAnimationTimer;
        private CharacterController characterController;
        public ScoreDisplay Display;

        public LumberjackLevler Levler;
        public string AttackInputButton;
        private float attackTimer;

        private int _downedTrees;
        public EventHandler DownedTreesChanged;
        private float invincibilityTimer;

        public int DownedTrees
        {
            get { return _downedTrees; }
            set
            {
                _downedTrees = value;
                if (DownedTreesChanged != null) DownedTreesChanged(_downedTrees, null);
            }
        }

        private void Start()
        {
            _axeContainer = Axe.GetComponent<AxeContainer>();
            CurrentAnimation = GetComponentInChildren<Animation>();
            characterController = GetComponentInParent<CharacterController>();
            Footsteps.Stop();

            Levler.LevelChanged += LevelChanged;
            DownedTreesChanged += OnDownedTreesChanged;
        }

        private void OnDownedTreesChanged(object sender, EventArgs eventArgs)
        {
            Display.ChoppedTrees = (int)sender;
        }

        private void LevelChanged(object sender, EventArgs eventArgs)
        {
            Display.CurrentLevel = (int)sender;

            PlayLockingAnimation("Level");
            invincibilityTimer = 2.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            _axeContainer.ToggleColliderActive(attackTimer > 0.0f);

            characterController.enabled = lockAnimationTimer <= 0.0f;

            if (invincibilityTimer > 0.0f)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            if (attackTimer > 0.0f)
            {
                attackTimer -= Time.deltaTime;
            }
            if (lockAnimationTimer > 0.0f)
            {
                lockAnimationTimer -= Time.deltaTime;
                return;
            }

            if (characterController.velocity.magnitude > 0.0f)
            {
                if (attackTimer <= 0.0f)
                {
                    CurrentAnimation.Play("Run");
                }
                else
                {
                    CurrentAnimation.Blend("Run");
                }
                if (Footsteps.isPlaying == false)
                {
                    Footsteps.Play();
                }
            }
            else
            {
                if (attackTimer <= 0.0f)
                {
                    CurrentAnimation.Play("Idle2");
                }
                Footsteps.Stop();
            }
            if (Input.GetButton(AttackInputButton) && attackTimer <= 0.0f)
            {
                CurrentAnimation.Play("Chop");
                attackTimer = CurrentAnimation.clip.length;
                if (_axeContainer.AxeThree.activeSelf == true && Random.Range(0, 100) > 80)
                {
                    PerformGroundSmash();
                }
            }
        }

        private void PlayLockingAnimation(string animationName)
        {
            CurrentAnimation.Play(animationName);
            lockAnimationTimer = CurrentAnimation.clip.length;
        }

        private void PerformGroundSmash()
        {
            var explosionHeight = Terrain.activeTerrain.SampleHeight(Axe.transform.position);
            var explosionPos = new Vector3(Axe.transform.position.x, explosionHeight, Axe.transform.position.z);
            var groundSmash = Instantiate(GroundSmashPrefab, explosionPos, new Quaternion()) as GameObject;
            groundSmash.GetComponent<Attack>().Owner = gameObject;
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.tag.Equals("PickUp"))
            {
                Destroy(collider.gameObject);
                Levler.GiveLog(1);
                Display.CollectedLogs++;
            }
            if (invincibilityTimer <= 0.0f && collider.isTrigger && gameObject.tag.Equals("Player") && collider.tag.Equals("Weapon2"))
            {
                PlayLockingAnimation("Fall");
                Display.CollectedLogs = Math.Max(0, Display.CollectedLogs-5);
                invincibilityTimer = 2.0f;
            }
            if (invincibilityTimer <= 0.0f && collider.isTrigger && gameObject.tag.Equals("Player2") && collider.tag.Equals("Weapon"))
            {
                PlayLockingAnimation("Fall");
                Display.CollectedLogs = Math.Max(0, Display.CollectedLogs - 5);
                invincibilityTimer = 2.0f;
            }
        }
    }
}