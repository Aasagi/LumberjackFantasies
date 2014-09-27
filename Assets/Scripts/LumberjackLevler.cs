using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        public AxeStats Axe;
        public int DamageLevelMultiplier;
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public float HitForceLevelPlus;

        public EventHandler LevelChanged;
        public float LevelRequirementIncrement;
        public float LogsToLevel;
        public float SwingSpeedLevelPlus;
        private int _logs;
        private int _level = 1;

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
        }
    }
}