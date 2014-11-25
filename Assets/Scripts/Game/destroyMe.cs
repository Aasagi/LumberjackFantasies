using UnityEngine;

namespace Assets.Scripts.Game
{
    public class destroyMe : MonoBehaviour
    {
        #region Fields
        public float Countdown;
        #endregion

        #region Methods
        private void Start()
        {
            Destroy(gameObject, Countdown);
        }
        #endregion
    }
}