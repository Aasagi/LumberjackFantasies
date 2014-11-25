using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public List<GameObject> SpawnableObjects;
    public Transform PlayerPosition;

    public int MinSpawn = 1;
    public int MaxSpawn = 1;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDeath()
    {
        SpawnPickups(PlayerPosition);
    }
    public void SpawnPickups(Transform playerPosition)
    {
        var numberOfPickups = Random.Range(MinSpawn, MaxSpawn + 1);

        for (var spawnIndex = 0; spawnIndex < numberOfPickups; spawnIndex++)
        {
            var objectToSpawn = Random.Range(0, SpawnableObjects.Count);
            var newObject = (GameObject)Instantiate(SpawnableObjects[objectToSpawn], transform.position, new Quaternion());
            var woodenLog = newObject.GetComponent<GoToPlayer>();
            if (woodenLog != null)
            {
                woodenLog.PlayerPosition = playerPosition;
            }
        }
    }
}
