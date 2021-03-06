﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Game
{
    public class TreeSpawner : MonoBehaviour
    {
        #region Fields
        public List<GameObject> TreePrefabs;
        public Terrain terrain;

        private List<GameObject> TreeTypeCollectorObjects = new List<GameObject>(); 
        #endregion

        // Use this for initialization

        #region Methods
        private int GetTreeToSpawn(float positioningFromMiddle)
        {
            var chance = Random.value;

            if (positioningFromMiddle < 30)
            {
				if (chance <= 0.2)
                {
                    return 2;
                }
				if (chance <= 0.4)
                {
                    return 1;
                }
            }
            else if (positioningFromMiddle < 60)
            {
                if (chance <= 0.1)
                {
                    return 2;
                }
                if (chance <= 0.3)
                {
                    return 1;
                }
            }

            if (chance <= 0.05)
            {
                return 2;
            }
            if (chance <= 0.15)
            {
                return 1;
            }

            return 0;
        }

        private void Start()
        {
            for (var i = 0; i < 3; i++)
            {
                var node = new GameObject();
                node.transform.parent = terrain.transform;
                node.name = "Type " + i + " Trees";
                TreeTypeCollectorObjects.Add(node);
            }

            if (Network.peerType != NetworkPeerType.Disconnected && Network.isClient)
                return;

            if (TreePrefabs.Count > 0)
            {
                var halfMapSizeX = terrain.terrainData.size.x * 0.35f;
                var halfMapSizeZ = terrain.terrainData.size.z * 0.35f;
                for (var i = 0; i < 1500; i++)
                {
                    var xPos = Random.Range(-halfMapSizeX, halfMapSizeX);
                    var zPos = Random.Range(-halfMapSizeZ, halfMapSizeZ);
                    var positioningFromMiddle = halfMapSizeX - new Vector2(xPos, zPos).magnitude;
                    var yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos));
                    var scale = Random.Range(0.8f, 1.5f);
                    var yRot = Random.Range(0, 360);
                    var treeType = GetTreeToSpawn(positioningFromMiddle);
                    if (positioningFromMiddle < halfMapSizeX - 10.0f)
                    {
                        if (Network.peerType == NetworkPeerType.Disconnected)
                        {
                            SpawnTree(treeType, xPos, yPos, zPos, scale, yRot);
                        }
                        else
                        {
                            networkView.RPC("SpawnTree", RPCMode.All, treeType, xPos, yPos, zPos, scale, yRot);
                        }
                    }
                }
            }
        }

        [RPC]
        private void SpawnTree(int treeType, float xPos, float yPos, float zPos, float scale, int yRot)
        {
            var newObject = Instantiate(TreePrefabs[treeType], new Vector3(xPos, yPos, zPos), Quaternion.Euler(0, yRot, 0)) as GameObject;
            newObject.transform.localScale = new Vector3(scale, scale, scale);
            newObject.transform.parent = TreeTypeCollectorObjects[treeType].transform;
        }

        // Update is called once per frame
        private void Update()
        {
        }

        #endregion
    }
}