using UnityEngine;

namespace Assets.Scripts
{
    public class LumberjackLevler : MonoBehaviour
    {
        public GameObject EffectPrefab;
        public Transform EffectSpawnLocation;
        public float LevelRequirementIncrement = 10;
        public float LogsToLevel = 10;
        private int _logs;

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
        }
    }
}