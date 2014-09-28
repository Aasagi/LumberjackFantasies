﻿using System;
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
        private Vector3 _previousPosition;
        private float lockAnimationTimer;
        private CharacterController characterController;
        private Vector3 lockPosition;
        private Quaternion lockRotation;
        public ScoreDisplay Display;

        public float WalkSpeed = 2.0f;
        public LumberjackLevler Levler;
        public string AttackInputButton;
        public int PlayerIndex { get; private set; }

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
            PlayerIndex = AddOnFunctions.GetPlayerNumberAssigned();
            _previousPosition = transform.position;
            Footsteps.Stop();

            Levler.LevelChanged += LevelChanged;
            DownedTreesChanged += OnDownedTreesChanged;

            Display.PlayerNumber = PlayerIndex;
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
            if (invincibilityTimer > 0.0f)
            {
                invincibilityTimer -= Time.deltaTime;
            }
            if (lockAnimationTimer > 0.0f)
            {
                characterController.transform.position = lockPosition;
                characterController.transform.rotation = lockRotation;
                lockAnimationTimer -= Time.deltaTime;
                return;
            }
            _axeContainer.ToggleColliderActive(false);

            if (characterController.velocity.magnitude > 0.0f)
            {
                CurrentAnimation.Play("Run");
                if (Footsteps.isPlaying == false)
                {
                    Footsteps.Play();
                }
            }
            else
            {
                CurrentAnimation.Play("Idle2");
                Footsteps.Stop();
            }
            if (Input.GetButton(AttackInputButton))
            {
                _axeContainer.ToggleColliderActive(true);
                PlayLockingAnimation("Chop");
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
            lockPosition = characterController.transform.position;
            lockRotation = characterController.transform.rotation;
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