using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public AxeStats Axe;
        public float LevelRequirementIncrement;
        public float LogsToLevel;
        private int _logs;
        public int DamageLevelMultiplier;
        public float SwingSpeedLevelPlus;
        public float HitForceLevelPlus;

        public void Update()
        {
            if (Input.GetKeyUp(KeyCode.F1))
            {
                LevelUp();
            }
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
        }
    }
}