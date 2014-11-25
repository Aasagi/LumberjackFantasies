using System;
using System.Timers;
using UnityEngine;

namespace Assets
{
    public class WinScreen : MonoBehaviour
    {
        public ScoreDisplay PlayerOneScore;
        public ScoreDisplay PlayerTwoScore;
        public GameObject PlayerOneWinScreen;
        public GameObject PlayerTwoWinScreen;
        public UILabel TimerDisplay;
        private float _elapsedTime;
        public float MatchDurationSecconds;
        public GameObject ReplayButton;

        // Use this for initialization
        void Start ()
        {
            _elapsedTime = MatchDurationSecconds;
        }
	
        // Update is called once per frame
        void Update ()
        {
            _elapsedTime -= Time.deltaTime;

            var time = TimeSpan.FromSeconds(_elapsedTime);
            TimerDisplay.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);

            if (_elapsedTime <= 0)
            {
                DisplayWinScreen();
            }
        }

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
    }
}
