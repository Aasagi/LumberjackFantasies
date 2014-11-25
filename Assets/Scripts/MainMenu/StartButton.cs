using UnityEngine;

namespace Assets.Scripts.MainMenuGui
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