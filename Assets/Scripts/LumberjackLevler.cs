using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        public AxeStats Axe;
        public GameObject AxeOne;
        public GameObject AxeThree;
        public GameObject AxeTwo;
        public int DamageLevelMultiplier;
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public float HitForceLevelPlus;
        public EventHandler LevelChanged;
        public float LevelRequirementIncrement;
        public float LogsToLevel;
        public int SecondAxeLevel;
        public float SwingSpeedLevelPlus;
        public int ThirdAxeLevel;

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

        private void LevelUp()
        {
            LogsToLevel += LogsToLevel + LevelRequirementIncrement;

            Instantiate(EffectPrefab, EffectSpawnLocation.position, EffectSpawnLocation.rotation);

            Axe.Damage *= DamageLevelMultiplier;
            Axe.SwingSpeedMultiplayer += SwingSpeedLevelPlus;
            Axe.HitForce += HitForceLevelPlus;

            _level++;
            if (LevelChanged != null) LevelChanged(_level, null);
            ActiveAxeForLevel(_level);
        }

        private void ActiveAxeForLevel(int level)
        {
            AxeOne.SetActive(false);
            AxeTwo.SetActive(false);
            AxeThree.SetActive(false);

            if (level < SecondAxeLevel)
            {
                AxeOne.SetActive(true);
            }
            else if (level >= SecondAxeLevel && level < ThirdAxeLevel)
            {
                AxeTwo.SetActive(true);
            }
            else
            {
                AxeThree.SetActive(true);
            }
        }
    }
}