using UnityEngine;

namespace Assets.Scripts
{
    public class FewerPlayersButton : MonoBehaviour
    {
        #region Fields
        public MainMenuGui.NumberOfPlayersManager Manager;
        #endregion

        #region Methods
        private void OnClick()
        {
            Manager.NumberOfPlayers--;
        }
        #endregion
    }
}