using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class MorePlayersButton : MonoBehaviour
    {
        #region Fields
        public NumberOfPlayersManager Manager;
        #endregion

        #region Methods
        private void OnClick()
        {
            Manager.NumberOfPlayers++;
        }
        #endregion
    }
}