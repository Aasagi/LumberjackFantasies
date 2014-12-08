using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class StartButton : MonoBehaviour
    {
        #region Methods
        private void OnClick()
        {
            Application.LoadLevel("GameScene");
        }
        #endregion
    }
}