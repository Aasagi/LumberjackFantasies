using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game
{
    public class Lumberjack : MonoBehaviour
    {
        // Use this for initialization

        #region Fields
        public string AttackInputButton;
        public AxeContainer AxeContainer;
        public ScoreDisplay Display;
        public EventHandler DownedTreesChanged;
        public ParticleSystem Footsteps;
        public GameObject GroundSmashPrefab;

        public LumberjackLevler Levler;
        private Animation CurrentAnimation;

        private int _downedTrees;
        private float attackTimer;
        private CharacterController characterController;
        private float invincibilityTimer;
        private float lockAnimationTimer;
        #endregion

        #region Public Properties
        public int DownedTrees
        {
            get
            {
                return _downedTrees;
            }
            set
            {
                _downedTrees = value;
                if (DownedTreesChanged != null)
                {
                    DownedTreesChanged(_downedTrees, null);
                }
            }
        }
        #endregion

        #region Methods
        private void LevelChanged(object sender, EventArgs eventArgs)
        {
            Display.CurrentLevel = (int)sender;

            characterController = GetComponentInParent<CharacterController>();
            characterController.stepOffset += 1.0f;

            PlayLockingAnimation("Level");
            invincibilityTimer = 2.0f;
        }

        private void OnDownedTreesChanged(object sender, EventArgs eventArgs)
        {
            Display.ChoppedTrees = (int)sender;
        }

        private void OnActiveAxeChanged(object sender, EventArgs e)
        {
            Physics.IgnoreCollision(gameObject.collider, AxeContainer.ActiveAxe.collider, true);
        }

        // Update is called once per frame

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag.Equals("PickUp"))
            {
                Destroy(collider.gameObject);
                AudioSingleton.Instance.PlaySound(SoundType.PickUpLog);
                Levler.GiveLog(1);
                Display.CollectedLogs++;
            }
            if (invincibilityTimer <= 0.0f && collider.isTrigger && collider.tag.Equals("Weapon"))
            {
                PlayLockingAnimation("Fall");
                Display.CollectedLogs = Math.Max(0, Display.CollectedLogs - 5);
                invincibilityTimer = 2.0f;
            }
        }

        private void PerformGroundSmash()
        {
            var explosionHeight = Terrain.activeTerrain.SampleHeight(AxeContainer.transform.position);
            var explosionPos = new Vector3(AxeContainer.transform.position.x, explosionHeight, AxeContainer.transform.position.z);
            var groundSmash = Instantiate(GroundSmashPrefab, explosionPos, new Quaternion()) as GameObject;
            groundSmash.GetComponent<Attack>().Owner = gameObject;
        }

        private void PlayLockingAnimation(string animationName)
        {
            CurrentAnimation.Play(animationName);
            lockAnimationTimer = CurrentAnimation.clip.length;
        }

        private void Start()
        {
            CurrentAnimation = GetComponentInChildren<Animation>();
            characterController = GetComponentInParent<CharacterController>();
            Footsteps.Stop();

            Levler.LevelChanged += LevelChanged;
            DownedTreesChanged += OnDownedTreesChanged;
            AxeContainer.ActiveAxeChanged += OnActiveAxeChanged;

            AxeContainer.Initialize();
        }

        private void Update()
        {
            AxeContainer.ToggleColliderActive(attackTimer > 0.0f);

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
                CurrentAnimation["Chop"].speed = Axe.GetComponent<AxeContainer>().AxeStats.SwingSpeedMultiplayer;
                CurrentAnimation.Play("Chop");
                AudioSingleton.Instance.PlaySound(SoundType.Grunt);
                attackTimer = CurrentAnimation["Chop"].length / CurrentAnimation["Chop"].speed;
                if (AxeContainer.AxeThree.activeSelf == true && Random.Range(0, 100) > 80)
                {
                    PerformGroundSmash();
                }
            }
        }
        #endregion
    }
}