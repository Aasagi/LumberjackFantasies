﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PickUp : MonoBehaviour
    {
        #region Fields
        public int MaxSpawn = 1;
        public int MinSpawn = 1;
        public Transform PlayerPosition;
        public List<GameObject> SpawnableObjects;
        #endregion

        #region Public Methods and Operators
        public void SpawnPickups(Transform playerPosition)
        {
            var numberOfPickups = Random.Range(MinSpawn, MaxSpawn + 1);

            for (var spawnIndex = 0; spawnIndex < numberOfPickups; spawnIndex++)
            {
                var objectToSpawn = Random.Range(0, SpawnableObjects.Count);
                var newObject =
                    (GameObject)Instantiate(SpawnableObjects[objectToSpawn], transform.position, new Quaternion());
                var woodenLog = newObject.GetComponent<GoToPlayer>();
                if (woodenLog != null)
                {
                    woodenLog.PlayerPosition = playerPosition;
                }
            }
        }
        #endregion

        #region Methods
        private void OnDeath()
        {
            SpawnPickups(PlayerPosition);
        }

        // Use this for initialization
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