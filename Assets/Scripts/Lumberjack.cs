using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Lumberjack : MonoBehaviour
    {
        // Use this for initialization
        public GameObject Axe;
        public ParticleSystem Footsteps; 
        private Vector3 _previousPosition;
        public ScoreDisplay Display;

        public float WalkSpeed = 2.0f;
        public LumberjackLevler Levler;

        private void Start()
        {
            _previousPosition = transform.position;
            Footsteps.Stop();

            Levler.LevelChanged += LevelChanged;
            Axe.GetComponent<AxeStats>().DownedTreesChanged += DownedTreesChanged;
        }

        private void DownedTreesChanged(object sender, EventArgs eventArgs)
        {
            Display.ChoppedTrees = (int) sender;
        }

        private void LevelChanged(object sender, EventArgs eventArgs)
        {
            Display.CurrentLevel = (int) sender;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {

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