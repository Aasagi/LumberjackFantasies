﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Lumberjack : MonoBehaviour
    {

        // Use this for initialization
        private Animation CurrentAnimation;
        public GameObject Axe;
        public GameObject GroundSmashPrefab;
        public ParticleSystem Footsteps;
        private Vector3 _previousPosition;
        private float lockAnimationTimer;
        private CharacterController characterController;
        private Vector3 lockPosition;
        public ScoreDisplay Display;

        public float WalkSpeed = 2.0f;
        public LumberjackLevler Levler;
        public string AttackInputButton;
        public int PlayerIndex { get; private set; }

        private int _downedTrees;
        public EventHandler DownedTreesChanged;

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
            
            CurrentAnimation.Play("Level");
            lockAnimationTimer = CurrentAnimation.clip.length;
            lockPosition = characterController.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            if (lockAnimationTimer > 0.0f)
            {
                characterController.transform.position = lockPosition;
                lockAnimationTimer -= Time.deltaTime;
                return;
            }

            if (CurrentAnimation != null && lockAnimationTimer <= 0.0f)
            {
                if (characterController.velocity.magnitude > 0.0f)
                    CurrentAnimation.Play("Run");
                else
                    CurrentAnimation.Play("Idle");
            }
            if (Input.GetButton(AttackInputButton))
            {
                CurrentAnimation.Play("Chop");
                lockAnimationTimer = CurrentAnimation.clip.length;
                lockPosition = characterController.transform.position;
                //PerformGroundSmash();
            }
            if (_previousPosition != transform.position)
            {
                _previousPosition = transform.position;
                if (Footsteps.isPlaying == false)
                {
                    Footsteps.Play();
                }
            }
            else if (Footsteps.isStopped == false)
            {
                Footsteps.Stop();
            }
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
        }
    }
}