using UnityEngine;

namespace Assets.Scripts
{
    public class Lumberjack : MonoBehaviour
    {
        // Use this for initialization
        public GameObject Axe;
        public ParticleSystem footsteps; 
        private BoxCollider axeCollider;
        public AxeSwinger axeSwinger;
        private Vector3 previousPosition;

        public float WalkSpeed = 2.0f;

        private void Start()
        {
            previousPosition = transform.position;
            footsteps.Stop();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                axeSwinger.SwingAxe();
            }
            if (previousPosition != transform.position)
            {
                previousPosition = transform.position;
                if (footsteps.isPlaying == false)
                {
                    footsteps.Play();
                }
            }
            else if (footsteps.isStopped == false)
            {
                footsteps.Stop();
            }
        }
    }
}