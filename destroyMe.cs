using UnityEngine;
using System.Collections;

public class destroyMe : MonoBehaviour {
	
	public float Countdown;
	

	void Start () {
			Destroy(gameObject, Countdown);
	}
	

}
