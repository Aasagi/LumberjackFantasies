using UnityEngine;

namespace Assets.Scripts
{
    public class AxeSwinger : MonoBehaviour
    {
        private readonly Quaternion _endRotation = Quaternion.Euler(0, 90, 0);
        private readonly Quaternion _startRotation = Quaternion.Euler(0, -90, 0);
        public GameObject AxeBase;
        public float SwingSpeedMultiplayer;
        private float _rotationProgress = 0.0f;
        private bool _swinging;

        private void Start()
        {
            AxeBase.SetActive(false);
        }

        public void SwingAxe()
        {
            if (!_swinging)
            {
                _swinging = true;
                AxeBase.SetActive(true);
            }
        }

        private void Update()
        {
            if (!_swinging) return;
            
            _rotationProgress += Time.deltaTime*SwingSpeedMultiplayer;
            AxeBase.transform.localRotation = Quaternion.Slerp(_startRotation, _endRotation, _rotationProgress);
            if (_rotationProgress >= 1.0f)
            {
                EndRotation();
            }
        }

        private void EndRotation()
        {
            _swinging = false;
            _rotationProgress = 0.0f;
            AxeBase.transform.localRotation = _startRotation;
            AxeBase.SetActive(false);
        }
    }
}