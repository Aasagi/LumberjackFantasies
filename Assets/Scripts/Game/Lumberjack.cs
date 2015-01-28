using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game
{
    public class Lumberjack : MonoBehaviour
    {
        // Use this for initialization

        #region Fields
        private enum AnimationName
        {
            IdleAnimation,
            RunAnimation,
            ChopAnimation,
            FallAnimation,
            LevelAnimation,
            NoAnimation
        }
        public string AttackInputButton;
        public AxeContainer AxeContainer;
        public ScoreDisplay Display;
        public EventHandler DownedTreesChanged;
        public ParticleSystem Footsteps;
        public GameObject GroundSmashPrefab;

        public GameObject LumberJackModel;

        public LumberjackLevler Levler;
        private Animation CurrentAnimation;

        private int _downedTrees;
        private float attackTimer;
        private CharacterController characterController;
        private float invincibilityTimer;
        private float lockAnimationTimer;

        private AnimationName currentAnimation = AnimationName.NoAnimation;
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

            var controller = GetComponentInParent<ThirdPersonController>();
            controller.walkSpeed *= Levler.RunSpeedMultiplayer;
            CurrentAnimation["Run"].speed *= Levler.RunSpeedMultiplayer;

            PlayLockingAnimation(AnimationName.LevelAnimation);
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
                PlayLockingAnimation(AnimationName.LevelAnimation);
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

        private void PlayLockingAnimation(AnimationName animationName)
        {
            PlayAnimation(animationName);
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
            if (Network.isClient)
                return;

            CurrentAnimation = GetComponentInChildren<Animation>();

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

            if (characterController.velocity.magnitude > 0.1f)
            {
                PlayAnimation(AnimationName.RunAnimation);
                if (Footsteps.isPlaying == false)
                {
                    Footsteps.Play();
                }
            }
            else
            {
                PlayAnimation(AnimationName.IdleAnimation);
                Footsteps.Stop();
            }
            if (Input.GetButton(AttackInputButton) && attackTimer <= 0.0f)
            {
                CurrentAnimation["Chop"].speed = AxeContainer.AxeStats.SwingSpeedMultiplayer;
                PlayAnimation(AnimationName.ChopAnimation);
                AudioSingleton.Instance.PlaySound(SoundType.Grunt);
                attackTimer = CurrentAnimation["Chop"].length / CurrentAnimation["Chop"].speed;
                if (AxeContainer.AxeThree.activeSelf == true && Random.Range(0, 100) > 80)
                {
                    PerformGroundSmash();
                }
            }
        }

        private void PlayAnimation(AnimationName animationName)
        {
            if (currentAnimation == animationName)
                return;

            var blendAnimation = false;
            var animationString = GetAnimationToPlay(animationName, out blendAnimation);
            if (string.IsNullOrEmpty(animationString) == false)
            {
                if (Network.peerType != NetworkPeerType.Disconnected)
                {
                    networkView.RPC("PlayAnimationName", RPCMode.All, animationString, blendAnimation);
                }
                else
                {
                    PlayAnimationName(animationString, blendAnimation);
                }

                currentAnimation = animationName;
            }
        }

        [RPC]
        private void PlayAnimationName(string animationName, bool blendAnimation)
        {
            if(blendAnimation)
                CurrentAnimation.Blend(animationName);
            else
                CurrentAnimation.Play(animationName);
        }

        private string GetAnimationToPlay(AnimationName animationName, out bool blendAnimation)
        {
            string animationString = null;
            blendAnimation = false;

            switch (animationName)
            {
                case AnimationName.IdleAnimation:
                    if (attackTimer <= 0.0f)
                    {
                        animationString = "Idle2";
                    }
                    break;
                case AnimationName.RunAnimation:
                    if (attackTimer <= 0.0f)
                    {
                        animationString = "Run";
                    }
                    else
                    {
                        animationString = "Run";
                        blendAnimation = true;
                    }
                    break;
                case AnimationName.ChopAnimation:
                    animationString = "Chop";
                    break;
                case AnimationName.FallAnimation:
                    animationString = "Fall";
                    break;
                case AnimationName.LevelAnimation:
                    animationString = "Level";
                    break;
            }

            return animationString;
        }

        #endregion
    }
}