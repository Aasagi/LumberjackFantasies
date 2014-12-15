using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.MainMenu;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public List<GameObject> Cameras;
    public List<GameObject> Players;
    public List<ScoreDisplay> PlayerDisplays;
    public List<Material> PlayerMaterials;

    public UISprite TwoPlayerSplitSprite;
    public UISprite FourPlayerSplitSprite;

    // Use this for initialization
    void Start()
    {
        SetSpriteActives();

        SetupPlayers();

        SetupCameras();
    }

    private void SetSpriteActives()
    {
        for (var i = NumberOfPlayersManager.NumberOfPlayers; i < PlayerDisplays.Count; i++)
        {
            PlayerDisplays[i].gameObject.SetActive(false);
        }

        if (NumberOfPlayersManager.NumberOfPlayers < 3 && FourPlayerSplitSprite != null)
        {
            NGUITools.SetActive(FourPlayerSplitSprite.gameObject, false);
        }
        if (NumberOfPlayersManager.NumberOfPlayers < 2 && TwoPlayerSplitSprite != null)
        {
            NGUITools.SetActive(TwoPlayerSplitSprite.gameObject, false);
        }
    }

    private void SetupPlayers()
    {
        var playerPrefab = Resources.Load("Prefabs/Player") as GameObject;

        for (var i = 0; i < NumberOfPlayersManager.NumberOfPlayers; i++)
        {
            var player =
                Instantiate(playerPrefab, GetPlayerStartingPosition(i + 1), GetPlayerStartingRotation(i + 1)) as GameObject;

            AssignButtons(player, i + 1);
            var lumberjack = player.GetComponentInChildren<Lumberjack>();
            lumberjack.Display = PlayerDisplays[i];
            if (lumberjack.LumberJackModel != null)
            {
                lumberjack.LumberJackModel.renderer.material = PlayerMaterials[i];
            }
            player.GetComponentInChildren<LumberjackLevler>().ScoreDisplay = PlayerDisplays[i];
            Players.Add(player);
        }
    }

    private Quaternion GetPlayerStartingRotation(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return Quaternion.LookRotation(new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            case 2:
                return Quaternion.LookRotation(new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            case 3:
                return Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f), new Vector3(0.0f, 1.0f, 0.0f));
            case 4:
                return Quaternion.LookRotation(new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, 1.0f, 0.0f));
        }

        return new Quaternion();
    }

    private Vector3 GetPlayerStartingPosition(int playerNumber)
    {
        switch (playerNumber)
        {
            case 1:
                return new Vector3(5.0f, Terrain.activeTerrain.SampleHeight(new Vector3(5.0f, 0.0f, 0.0f)), 0.0f);
            case 2:
                return new Vector3(-5.0f, Terrain.activeTerrain.SampleHeight(new Vector3(-5.0f, 0.0f, 0.0f)), 0.0f);
            case 3:
                return new Vector3(0.0f, Terrain.activeTerrain.SampleHeight(new Vector3(0.0f, 0.0f, 5.0f)), 5.0f);
            case 4:
                return new Vector3(0.0f, Terrain.activeTerrain.SampleHeight(new Vector3(0.0f, 0.0f, -5.0f)), -5.0f);
        }

        return new Vector3();
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

        switch (NumberOfPlayersManager.NumberOfPlayers)
        {
            case 1:
                Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
                Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
                break;
            case 3:
                Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                Cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                break;
            case 4:
                Cameras[0].GetComponent<Camera>().rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                Cameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                Cameras[2].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
                Cameras[3].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
