using UnityEngine;
using System.Collections;

public class GoToPlayer : MonoBehaviour
{

    public Transform PlayerPosition;
    public float SpeedMultiplayer;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (PlayerPosition == null) return;
        
        var position = transform.position;

	    transform.position = Vector3.MoveTowards(position, PlayerPosition.position, Time.deltaTime * SpeedMultiplayer);
	}
}
