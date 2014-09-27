using UnityEngine;
using System.Collections;

public class LumberjackLevler : MonoBehaviour {
    private int _logs;
    public float LogsToLevel = 10;
    public float LevelRequirementIncrement = 10;

    public GameObject EffectPrefab;
    public Transform EffectSpawnLocation;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.Alpha1))
	    {
	        GiveLog(1);
	    }
	}

    public void GiveLog(int nbrOfLogs)
    {
        Debug.Log("Log given");

        _logs += nbrOfLogs;

        if (_logs >= LogsToLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        LogsToLevel += LogsToLevel + LevelRequirementIncrement;

        Instantiate(EffectPrefab, EffectSpawnLocation.position, EffectSpawnLocation.rotation);
    }
}
