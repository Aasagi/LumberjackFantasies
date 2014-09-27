using UnityEngine;
using System.Collections;

public class TreeSpawner : MonoBehaviour
{

    public GameObject treePrefab;
	// Use this for initialization
	void Start () {
	    if (treePrefab != null)
	    {
            for(var i = 0; i < 20; i++)
	        Instantiate(treePrefab,
	            new Vector3(Random.Range(-20.0f, 20.0f), 0.0f, Random.Range(-20.0f, 20.0f)),
	            new Quaternion());
	    }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
