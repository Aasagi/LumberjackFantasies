using UnityEngine;

namespace Assets.Scripts.MainMenu
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