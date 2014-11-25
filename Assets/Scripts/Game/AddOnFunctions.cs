using UnityEngine;

namespace Assets.Scripts
{
    public static class AddOnFunctions
    {
        private static int _playerNumberAssignment = 1;

        public static int GetPlayerNumberAssigned()
        {
            return _playerNumberAssignment++;
        }
        public static void KillAndDestroy(GameObject gameObject)
        {
            gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            Object.Destroy(gameObject);
        } 
    }
}