using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class LevelSynchronizer : MonoBehaviour 
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        networkView.group = 1;
    }

	private void OnClick () 
    {
        networkView.RPC("LoadLevel", RPCMode.All, "GameScene");
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    [RPC]
    void LoadLevel(string levelName)
    {
        // There is no reason to send any more data over the network on the default channel,
        // because we are about to load the level, thus all those objects will get deleted anyway
  //      Network.SetSendingEnabled(0, false);

        // We need to stop receiving because first the level must be loaded first.
        // Once the level is loaded, rpc's and other state update attached to objects in the level are allowed to fire
//        Network.isMessageQueueRunning = false;

        Application.LoadLevel(levelName);

        //Invoke("AfterLoad", 2.0f);
    }

    void AfterLoad()
    {
        // Allow receiving data again
        Network.isMessageQueueRunning = true;
        // Now the level has been loaded and we can start sending out data to clients
        Network.SetSendingEnabled(0, true);
    }

    void OnDisconnectedFromServer()
    {
        Application.LoadLevel("DisconnectedMenuScene");
    }
}
