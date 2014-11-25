using UnityEngine;

namespace Assets.Scripts.MainMenuGui
{
    public class QuitButton : MonoBehaviour
    {
        #region Methods
        private void OnClick()
        {
            Application.Quit();
        }
        #endregion
    }
}