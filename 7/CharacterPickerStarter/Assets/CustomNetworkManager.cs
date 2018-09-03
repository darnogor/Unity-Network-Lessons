using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public class MessageTypes
{
    public const short PlayerPrefabSelect = MsgType.Highest;

    public class PlayerPrefabMsg : MessageBase
    {
        public short controllerID;
        public short prefabIndex;
    }
}


public class CustomNetworkManager : NetworkManager
{
    public short playerPrefabIndex;

    private int _selectedPlayerIndex = 0;

    private void OnGUI()
    {
        if (!isNetworkActive)
        {
            var palyerNames = spawnPrefabs.GetRange(1, spawnPrefabs.Count - 1).Select(s => s.name).ToArray();
            _selectedPlayerIndex = GUI.SelectionGrid(new Rect(Screen.width - 200, 10, 200, 50), _selectedPlayerIndex, palyerNames, 2);
            playerPrefabIndex = (short) (_selectedPlayerIndex + 1);
        }
    }


    public override void OnStartServer()
    {
        NetworkServer.RegisterHandler(MessageTypes.PlayerPrefabSelect, OnServerPrefabSelectReceived);
        base.OnStartServer();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        conn.RegisterHandler(MessageTypes.PlayerPrefabSelect, OnClientSelectPrefabReceived);
        base.OnClientConnect(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var prefabMsg = new MessageTypes.PlayerPrefabMsg
        {
            controllerID = playerControllerId
        };
        NetworkServer.SendToClient(conn.connectionId, MessageTypes.PlayerPrefabSelect, prefabMsg);
    }


    private void OnServerPrefabSelectReceived(NetworkMessage networkMessage)
    {
        var prefabMsg = networkMessage.ReadMessage<MessageTypes.PlayerPrefabMsg>();
        playerPrefab = spawnPrefabs[prefabMsg.prefabIndex];
        
        base.OnServerAddPlayer(networkMessage.conn, prefabMsg.controllerID);
    }


    private void OnClientSelectPrefabReceived(NetworkMessage networkMessage)
    {
        var prefabMsg = new MessageTypes.PlayerPrefabMsg
        {
            controllerID = networkMessage.ReadMessage<MessageTypes.PlayerPrefabMsg>().controllerID,
            prefabIndex  = playerPrefabIndex
        };

        client.Send(MessageTypes.PlayerPrefabSelect, prefabMsg);
    }


    public void SwitchPlayer(SetupLocalPlayer player, int id)
    {
        var prefab = spawnPrefabs[id];
        var playerObj = Instantiate(prefab, player.gameObject.transform.position, player.gameObject.transform.rotation);

        playerPrefab = prefab;
        Destroy(player.gameObject);
        NetworkServer.ReplacePlayerForConnection(player.connectionToClient, playerObj, 0);
    }
}
