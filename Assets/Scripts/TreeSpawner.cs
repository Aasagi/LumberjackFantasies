using UnityEngine;
using System.Collections;

public class TreeSpawner : MonoBehaviour
{
    public Terrain terrain;
    public GameObject treePrefab;
	// Use this for initialization
	void Start () {
	    if (treePrefab != null)
	    {
            for(var i = 0; i < 500; i++)
            {
                var xPos = Random.Range(-80, 80);
                var zPos = Random.Range(-80, 80);
                var yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos));
                Instantiate(treePrefab, new Vector3(xPos, yPos, zPos), new Quaternion());
            }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
