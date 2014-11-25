using UnityEngine;
using System.Collections;

public class ReplayButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        Time.timeScale = 1;     
        Application.LoadLevel(Application.loadedLevel);
    }
}
