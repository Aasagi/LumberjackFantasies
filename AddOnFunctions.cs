using UnityEngine;

namespace Assets.Scripts
{
    public static class AddOnFunctions
    {
        public static void KillAndDestroy(GameObject gameObject)
        {
            gameObject.BroadcastMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            Object.Destroy(gameObject);
        } 
    }
}