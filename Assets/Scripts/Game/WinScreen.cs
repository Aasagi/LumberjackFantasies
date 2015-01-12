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
        public UILabel TimerLabel;
        public GameObject TimerDisplay;
        public UILabel NearEndLabel;
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
            TimerDisplay.SetActive(false);
            NearEndLabel.text = "";

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
            TimerLabel.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            
            
            if (elapsedTime <= 10)
            {
                NearEndLabel.text = string.Format("{0}", (int)elapsedTime);
            }
            else if (elapsedTime <= 27)
            {
                NearEndLabel.text = "";
            }
            else if (elapsedTime <= 31)
            {
                NearEndLabel.text = "30 seconds\nremaining!";
            }
            else if (elapsedTime <= 57)
            {
                NearEndLabel.text = "";
            }
            else if (elapsedTime <= 61)
            {
                NearEndLabel.text = "1 minute\nleft!";
            }

            if (elapsedTime <= 0)
            {
                DisplayWinScreen();
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                DisplayWinScreen();
            }
        }
        #endregion
    }
}