using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ScoreDisplay : MonoBehaviour
    {
        #region Fields
        public EventHandler LevelChanged;
        public UILabel LevelDisplay;
        public UISlider LevelProgressBar;
        public EventHandler LogsChanged;
        public UILabel LogsDisplay;
        public UILabel PlayerDisplay;
        public UILabel TreeDisplay;
        public EventHandler TreesChanged;
        private int _choppedTrees;
        private int _collectedLogs;
        private int _currentLevel;
        #endregion

        #region Public Properties
        public int ChoppedTrees
        {
            set
            {
                _choppedTrees = value;
                if (TreesChanged != null)
                {
                    TreesChanged(_choppedTrees, null);
                }
                TreeDisplay.text = _choppedTrees.ToString();
            }

            get
            {
                return _choppedTrees;
            }
        }

        public int CollectedLogs
        {
            set
            {
                _collectedLogs = value;
                if (LogsChanged != null)
                {
                    LogsChanged(_collectedLogs, null);
                }
                LogsDisplay.text = _collectedLogs.ToString();
            }
            get
            {
                return _collectedLogs;
            }
        }

        public int CurrentLevel
        {
            set
            {
                _currentLevel = value;
                if (LevelChanged != null)
                {
                    LevelChanged(_currentLevel, null);
                }
                LevelDisplay.text = _currentLevel.ToString();
            }
            get
            {
                return _currentLevel;
            }
        }
        public int PlayerNumber
        {
            get
            {
                return int.Parse(PlayerDisplay.text);
            }
        }
        #endregion

        #region Public Methods and Operators
        public void UpdateLevelProgress(int currentLogs, int logsRequiredToLevel)
        {
            LevelProgressBar.numberOfSteps = logsRequiredToLevel;
            LevelProgressBar.sliderValue = (float)currentLogs / (float)logsRequiredToLevel;
        }
        #endregion

        // Use this for initialization

        #region Methods
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
        #endregion
    }
}