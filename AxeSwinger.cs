using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class AxeSwinger : MonoBehaviour
    {
        public GameObject AxeBase;
        public float SwingSpeedMultiplayer;
        private bool swinging;

        private void Start()
        {
            //AxeBase.SetActive(false);
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
                
                var rotation = AxeBase.transform.localRotation;
                rotation.y += Time.deltaTime*SwingSpeedMultiplayer;
                if (rotation.y >= 0.3f)
                {
                    EndRotation();
                }

                AxeBase.transform.localRotation = rotation;
            }
        }

        private void EndRotation()
        {
            swinging = false;
            var newRotation = new Quaternion(0, -0.25f, 0, 0);
            AxeBase.transform.localRotation = newRotation;
            AxeBase.SetActive(false);
        }
    }
}