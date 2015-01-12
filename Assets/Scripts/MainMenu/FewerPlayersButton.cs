using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class FewerPlayersButton : MonoBehaviour
    {
        #region Fields
        public NumberOfPlayersManager Manager;
        #endregion

        #region Methods
        private void OnClick()
        {
            Manager.SetNumberOfPlayers(NumberOfPlayersManager.NumberOfPlayers - 1);
        }
        #endregion
    }
}