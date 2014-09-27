using UnityEngine;

namespace Assets.Scripts
{
    public class Lumberjack : MonoBehaviour
    {
        // Use this for initialization
        public GameObject Axe;
        public ParticleSystem Footsteps; 
        public AxeSwinger AxeSwinger;
        private Vector3 _previousPosition;

        public float WalkSpeed = 2.0f;

        private void Start()
        {
            _previousPosition = transform.position;
            Footsteps.Stop();
         //   axeCollider = Axe.GetComponent<BoxCollider>();

           // axeCollider.enabled = false;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                AxeSwinger.SwingAxe();
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
            }
        }
    }
}