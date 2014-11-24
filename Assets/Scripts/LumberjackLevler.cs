using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        #region Fields
        public GameObject Axe;
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public EventHandler LevelChanged;
        public ScoreDisplay ScoreDisplay;
        public int LevelRequirementIncrement;
        public int LogsToLevel;

        private int level = 1;
        private int logs = 0;
        private int logsThisLevel = 0;
        private int thisLevelLogRequired;
        #endregion

        #region Public Methods and Operators
        public void GiveLog(int nbrOfLogs)
        {
            Debug.Log("Log given");

            logs += nbrOfLogs;
            logsThisLevel += nbrOfLogs;

            if (logs >= LogsToLevel)
            {
                LevelUp();
            }

            ScoreDisplay.UpdateLevelProgress(logsThisLevel, thisLevelLogRequired);
        }

        public void Start()
        {
            if (LevelChanged != null)
            {
                LevelChanged(level, null);
            }

            thisLevelLogRequired = LogsToLevel - logs;
        }

        public void TakeLogs(int nbrOfLogs)
        {
            Debug.Log("Log taken");

            logs -= nbrOfLogs;
            logsThisLevel -= nbrOfLogs;

            ScoreDisplay.UpdateLevelProgress(logsThisLevel, thisLevelLogRequired);
        }

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                LevelUp();
            }
        }
        #endregion

        #region Methods
        private void LevelUp()
        {
            thisLevelLogRequired = LogsToLevel + LevelRequirementIncrement;
            LogsToLevel += thisLevelLogRequired;

            Instantiate(EffectPrefab, EffectSpawnLocation.position, EffectSpawnLocation.rotation);

            level++;
            Axe.GetComponent<AxeContainer>().IncrementLevel();
            logsThisLevel = 0;

            if (LevelChanged != null)
            {
                LevelChanged(level, null);
            }
        }
        #endregion
    }
}