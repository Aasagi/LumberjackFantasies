using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GoToPlayer : MonoBehaviour
    {
        #region Fields
        public Transform PlayerPosition;
        public float SpeedMultiplayer;
        #endregion

        // Use this for initialization

        #region Methods
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (PlayerPosition == null)
            {
                return;
            }

            var position = transform.position;

            transform.position = Vector3.MoveTowards(
                position,
                PlayerPosition.position,
                Time.deltaTime * SpeedMultiplayer);
        }
        #endregion
    }
}