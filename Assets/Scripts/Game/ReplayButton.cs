using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ReplayButton : MonoBehaviour
    {
        // Use this for initialization

        #region Public Methods and Operators
        public void OnClick()
        {
            Time.timeScale = 1;
            Application.LoadLevel(Application.loadedLevel);
        }
        #endregion

        #region Methods
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
        #endregion
    }
}