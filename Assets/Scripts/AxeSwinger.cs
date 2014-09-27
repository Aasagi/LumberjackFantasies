using UnityEngine;

namespace Assets.Scripts
{
    public class AxeSwinger : MonoBehaviour
    {
        private readonly Vector3 endRotationEuler = new Vector3(0, 90, 0);
        private readonly Vector3 startRotationEuler = new Vector3(0, -90, 0);
        public GameObject AxeBase;
        public float SwingSpeedMultiplayer;
        private float swingProgress = 0;
        private bool swinging;

        private void Start()
        {
            AxeBase.SetActive(false);
        }

        public void SwingAxe()
        {
            if (!swinging)
            {
                swinging = true;
                AxeBase.SetActive(true);
            }
        }

        private void Update()
        {
            if (swinging)
            {
                swingProgress += Time.deltaTime*SwingSpeedMultiplayer;

                Quaternion newRotation = Quaternion.Lerp(Quaternion.Euler(startRotationEuler),
                    Quaternion.Euler(endRotationEuler), swingProgress);
                AxeBase.transform.localRotation = newRotation;

                if (swingProgress >= 1.0f)
                {
                    EndRotation();
                }
            }
        }

        private void EndRotation()
        {
            swinging = false;
            swingProgress = 0.0f;
            AxeBase.transform.localRotation = Quaternion.Euler(startRotationEuler);
            AxeBase.SetActive(false);
        }
    }
}