using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        public GameObject Axe;
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public EventHandler LevelChanged;
        public float LevelRequirementIncrement;
        public float LogsToLevel;
        
        private int _level = 1;
        private int _logs;

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                LevelUp();
            }
        }

        public void Start()
        {
            if (LevelChanged != null) LevelChanged(_level, null);
        }

        public void GiveLog(int nbrOfLogs)
        {
            Debug.Log("Log given");

            _logs += nbrOfLogs;

            if (_logs >= LogsToLevel)
            {
                LevelUp();
            }
        }

        public void TakeLogs(int nbrOfLogs)
        {
            Debug.Log("Log taken");

            _logs -= nbrOfLogs;
        }

        private void LevelUp()
        {
            LogsToLevel += LogsToLevel + LevelRequirementIncrement;

            Instantiate(EffectPrefab, EffectSpawnLocation.position, EffectSpawnLocation.rotation);

            _level++;
            Axe.GetComponent<AxeContainer>().IncrementLevel();
            if (LevelChanged != null) LevelChanged(_level, null);

        }
    }
}