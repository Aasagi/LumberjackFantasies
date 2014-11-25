using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class NumberOfPlayersManager : MonoBehaviour
    {
        #region Fields
        public UILabel NumberOfPlayersLabel;
        private int nbrOfPlayers = 4;
        #endregion

        #region Public Properties
        public int NumberOfPlayers
        {
            get
            {
                return nbrOfPlayers;
            }

            set
            {
                nbrOfPlayers = LimitToRange(value, 2, 4);
                NumberOfPlayersLabel.text = nbrOfPlayers.ToString(CultureInfo.InvariantCulture);
                //GameManager.NumberOfPlayers = nbrOfPlayers;
            }
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
            NumberOfPlayers = nbrOfPlayers;
        }
        #endregion
    }
}