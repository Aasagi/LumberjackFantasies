using System;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class AxeContainer : MonoBehaviour
    {
        #region Fields
        public GameObject AxeOne;
        public AxeStats AxeStats;
        public GameObject AxeThree;
        public GameObject AxeTwo;

        public int DamageLevelMultiplier;
        public float HitForceLevelPlus;
        public int SecondAxeLevel;
        public float SwingSpeedLevelPlus;
        public int ThirdAxeLevel;
        private GameObject _activeAxe;

        public event EventHandler ActiveAxeChanged;

        public GameObject ActiveAxe
        {
            get { return _activeAxe; }
            private set
            {
                _activeAxe = value;
                if (ActiveAxeChanged != null)
                {
                    ActiveAxeChanged(this, null);
                }
            }
        }

        #endregion

        #region Public Properties
        public int Level { get; private set; }
        #endregion

        // Use this for initialization

        #region Public Methods and Operators
        public void IncrementLevel()
        {
            AxeStats.Attack.Damage *= DamageLevelMultiplier;
            AxeStats.SwingSpeedMultiplayer += SwingSpeedLevelPlus;
            AxeStats.Attack.HitForce += HitForceLevelPlus;

            Level++;
            ActiveAxeForLevel(Level);
        }

        public void ToggleColliderActive(bool activate)
        {
            ActiveAxe.GetComponent<Collider>().isTrigger = activate;
        }
        #endregion

        #region Methods
        private void ActiveAxeForLevel(int level)
        {
            AxeOne.SetActive(false);
            AxeTwo.SetActive(false);
            AxeThree.SetActive(false);

            if (level < SecondAxeLevel)
            {
                Debug.Log("Activating axe 1");
                AxeOne.SetActive(true);
                ActiveAxe = AxeOne;
            }
            else if (level >= SecondAxeLevel && level < ThirdAxeLevel)
            {
                Debug.Log("Activating axe 2");
                AxeTwo.SetActive(true);
                ActiveAxe = AxeTwo;
            }
            else
            {
                Debug.Log("Activating axe 3");
                AxeThree.SetActive(true);
                ActiveAxe = AxeThree;
            }
        }

        public void Initialize()
        {
            ActiveAxe = AxeOne;
            Level = 1;
        }

        // Update is called once per frame
        private void Update()
        {
        }
        #endregion
    }
}