using UnityEngine;

namespace Assets.Scripts
{
    public class MorePlayersButton : MonoBehaviour
    {
        #region Fields
        public MainMenuGui.NumberOfPlayersManager Manager;
        #endregion

        #region Methods
        private void OnClick()
        {
            Manager.NumberOfPlayers++;
        }
        #endregion
    }
}