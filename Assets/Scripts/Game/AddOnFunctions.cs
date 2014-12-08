using UnityEngine;

namespace Assets.Scripts.Game
{
    public static class AddOnFunctions
    {
        #region Static Fields
        private static int _playerNumberAssignment = 1;
        #endregion

        #region Public Methods and Operators
        public static int GetPlayerNumberAssigned()
        {
            return _playerNumberAssignment++;
        }

        public static void KillAndDestroy(GameObject gameObject)
        {
            gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            Object.Destroy(gameObject);
        }
        #endregion
    }
}