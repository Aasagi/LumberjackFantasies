using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class NumberOfPlayersManager : MonoBehaviour
    {
        #region Fields
        public UILabel NumberOfPlayersLabel;
        private static int nbrOfPlayers = 4;
        #endregion

        public static int NumberOfPlayers
        {
            get
            {
                return nbrOfPlayers;
            }
        }
        #region Public Properties
        public void SetNumberOfPlayers(int value)
        {
            nbrOfPlayers = LimitToRange(value, 1, 4);
            NumberOfPlayersLabel.text = nbrOfPlayers.ToString(CultureInfo.InvariantCulture);
        }
        #endregion

        // Use this for initialization

        #region Public Methods and Operators
        public static int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum)
            {
                return inclusiveMinimum;
            }
            if (value > inclusiveMaximum)
            {
                return inclusiveMaximum;
            }
            return value;
        }
        #endregion

        #region Methods
        private void Start()
        {
            SetNumberOfPlayers(nbrOfPlayers);
        }
        #endregion
    }
}