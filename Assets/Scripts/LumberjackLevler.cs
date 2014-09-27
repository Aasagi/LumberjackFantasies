using UnityEngine;
using System.Collections;

public class LumberjackLevler : MonoBehaviour {
    private int logs;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GiveLog(int nbrOfLogs)
    {
        logs += nbrOfLogs;
    }
}
