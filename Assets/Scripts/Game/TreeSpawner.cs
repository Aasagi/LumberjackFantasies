using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeSpawner : MonoBehaviour
{
    public Terrain terrain;
    public List<GameObject> TreePrefabs;
    // Use this for initialization
    void Start()
    {
        if (TreePrefabs.Count > 0)
        {
            var mapSizeX = terrain.terrainData.size.x;
            var mapSizeZ = terrain.terrainData.size.z;
            var halfMapSizeX = terrain.terrainData.size.x * 0.5f - 20.0f;
            var halfMapSizeZ = terrain.terrainData.size.z * 0.5f - 20.0f;
            var numberOfTrees = (mapSizeX + (Math.Abs(mapSizeZ - mapSizeX))) * 10;
            for (var i = 0; i < numberOfTrees; i++)
            {
                var xPos = Random.Range(-halfMapSizeX, halfMapSizeX);
                var zPos = Random.Range(-halfMapSizeZ, halfMapSizeZ);
                var positioningFromMiddle = halfMapSizeX - new Vector2(xPos, zPos).magnitude;
                var yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos));
                var scale = Random.Range(0.8f, 1.5f);
                if (positioningFromMiddle < halfMapSizeX - 10.0f)
                {
                    var treeToSpawn = GetTreeToSpawn(positioningFromMiddle);

                    var newObject = Instantiate(TreePrefabs[treeToSpawn], new Vector3(xPos, yPos, zPos), new Quaternion()) as GameObject;
                    newObject.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

    private int GetTreeToSpawn(float positioningFromMiddle)
    {
        var chance = Random.value;

        if (positioningFromMiddle < 30)
        {
            if (chance <= 0.2)
                return 2;
            if (chance <= 0.4)
                return 1;
        }
        else if (positioningFromMiddle < 60)
        {
            if (chance <= 0.1)
                return 2;
            if (chance <= 0.3)
                return 1;
        }

        if (chance <= 0.05)
            return 2;
        if (chance <= 0.15)
            return 1;

        return 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
