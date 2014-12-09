using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.MainMenu;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public List<GameObject> Cameras;
    public List<GameObject> Players;
    public List<ScoreDisplay> PlayerDisplays; 

	// Use this for initialization
	void Start ()
	{
	    SetupPlayers();

	    SetupCameras();
	}

    private void SetupPlayers()
    {
        var playerPrefab = Resources.Load("Prefabs/Player") as GameObject;

        for (var i = 0; i < NumberOfPlayersManager.NumberOfPlayers; i++)
        {
            var player =
                Instantiate(playerPrefab,
                    new Vector3(i == 0 ? 5.0f : -5.0f, Terrain.activeTerrain.SampleHeight(new Vector3(5.0f, 0.0f, 0.0f)),
                        0.0f), new Quaternion()) as GameObject;

            AssignButtons(player, i+1);
            player.GetComponentInChildren<Lumberjack>().Display = PlayerDisplays[i];
            player.GetComponentInChildren<LumberjackLevler>().ScoreDisplay = PlayerDisplays[i];
            Players.Add(player);
        }
    }

    private static void AssignButtons(GameObject player, int playerIndex)
    {
        player.GetComponentInChildren<Lumberjack>().AttackInputButton = "Chop" + playerIndex;
        var controller = player.GetComponent<Assets.Scripts.Game.ThirdPersonController>();
        controller.VerticalAxisName = "VerticalMovement" + playerIndex;
        controller.HorizontalAxisName = "HorizontalMovement" + playerIndex;
        controller.AttackButton = "Chop" + playerIndex;
        controller.JumpButton = "Jump" + playerIndex;
    }

    private void SetupCameras()
    {
        var cameraPrefab = Resources.Load("Prefabs/PlayerCamera") as GameObject;

        for (var i = 0; i < NumberOfPlayersManager.NumberOfPlayers; i++)
        {
            var camera = Instantiate(cameraPrefab, new Vector3(), new Quaternion()) as GameObject;
            Cameras.Add(camera);
            camera.GetComponent<SmoothFollow>().target = Players[i].transform;
        }

        if (NumberOfPlayersManager.NumberOfPlayers == 2)
        {
            Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
            Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
        }
        else if (NumberOfPlayersManager.NumberOfPlayers == 3)
        {
            Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            Cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
        }
        else if (NumberOfPlayersManager.NumberOfPlayers == 4)
        {
            Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
            Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
            Cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
            Cameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
        }
    }

    // Update is called once per frame
	void Update () 
    {
	
	}
}
