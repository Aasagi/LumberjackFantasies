using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class WinScreen : MonoBehaviour
    {
        #region Fields
        public float MatchDurationSecconds;
        public ScoreDisplay PlayerOneScore;
        public GameObject PlayerOneWinScreen;
        public ScoreDisplay PlayerTwoScore;
        public GameObject PlayerTwoWinScreen;
        public GameObject ReplayButton;
        public UILabel TimerDisplay;
        private float _elapsedTime;
        #endregion

        #region Methods
        private void DisplayWinScreen()
        {
            if (PlayerOneScore.CollectedLogs > PlayerTwoScore.CollectedLogs)
            {
                PlayerOneWinScreen.SetActive(true);
            }
            else
            {
                PlayerTwoWinScreen.SetActive(true);
            }

            Time.timeScale = 0;
            //ReplayButton.SetActive(true);
        }

        // Use this for initialization
        private void Start()
        {
            _elapsedTime = MatchDurationSecconds;
        }

        // Update is called once per frame
        private void Update()
        {
            _elapsedTime -= Time.deltaTime;

            var time = TimeSpan.FromSeconds(_elapsedTime);
            TimerDisplay.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            if (_elapsedTime <= 0)
            {
                DisplayWinScreen();
            }
        }
        #endregion
    }
}