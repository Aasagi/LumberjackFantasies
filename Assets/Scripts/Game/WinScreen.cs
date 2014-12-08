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
        public GameObject MyReplayButton;
        public UILabel TimerDisplay;
        private float elapsedTime;
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

            MyReplayButton.SetActive(true);

            Time.timeScale = 0;
            //ReplayButton.SetActive(true);
        }

        // Use this for initialization
        private void Start()
        {
            elapsedTime = MatchDurationSecconds;
        }

        // Update is called once per frame
        private void Update()
        {
            elapsedTime -= Time.deltaTime;

            var time = TimeSpan.FromSeconds(elapsedTime);
            TimerDisplay.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            if (elapsedTime <= 0)
            {
                DisplayWinScreen();
            }
        }
        #endregion
    }
}