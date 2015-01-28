using UnityEngine;

public class CreateGameButtonScript : MonoBehaviour 
{
    #region Methods
    private void OnClick()
    {
        // Use NAT punchthrough if no public IP present
        Network.InitializeServer(32, 25002, !Network.HavePublicAddress());
        MasterServer.RegisterHost(NetworkingScript.GameServerName, "Canadian lumberjacks", "All is welcome!");
    }
    #endregion
}
