using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ChildLiberator : MonoBehaviour
    {
        // Use this for initialization

        #region Methods
        [ExecuteInEditMode]
        private void Start()
        {
            transform.DetachChildren();
            Destroy(gameObject);
        }
        #endregion
    }
}