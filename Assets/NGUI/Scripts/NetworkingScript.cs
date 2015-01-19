using UnityEngine;
using System.Collections;

public class NetworkingScript : MonoBehaviour
{
    public string connectionIp = "127.0.0.1";
    public int connectionPort = 25001;

    public const string GameServerName = "Lumberjack Fantasies";
    void Awake()
    {
        MasterServer.RequestHostList(GameServerName);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {

        if (GUILayout.Button("Start Server"))
        {
            // Use NAT punchthrough if no public IP present
            Network.InitializeServer(32, 25002, !Network.HavePublicAddress());
            MasterServer.RegisterHost(GameServerName, "Canadian lumberjacks", "All is welcome!");
        }

        var data = MasterServer.PollHostList();
        // Go through all the hosts in the host list
        foreach (var element in data)
        {
            GUILayout.BeginHorizontal();
            var name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
            GUILayout.Label(name);
            GUILayout.Space(5);
            var hostInfo = "";
            hostInfo = "[";
            foreach (var host in element.ip)
                hostInfo = hostInfo + host + ":" + element.port + " ";
            hostInfo = hostInfo + "]";
            GUILayout.Label(hostInfo);
            GUILayout.Space(5);
            GUILayout.Label(element.comment);
            GUILayout.Space(5);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Connect"))
            {
                // Connect to HostData struct, internally the correct method is used (GUID when using NAT).
                Network.Connect(element);
            }
            GUILayout.EndHorizontal();
        }
    }
}
