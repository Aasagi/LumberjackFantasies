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
            for(var i = 0; i < 1000; i++)
            {
                var xPos = Random.Range(-80, 80);
                var zPos = Random.Range(-80, 80);
                var yPos = terrain.SampleHeight(new Vector3(xPos, 0.0f, zPos));
                var scale = Random.Range(1.0f, 2.0f);
                var newObject = Instantiate(treePrefab, new Vector3(xPos, yPos, zPos), new Quaternion()) as GameObject;
                newObject.transform.localScale = new Vector3(scale, scale, scale);
            }
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
