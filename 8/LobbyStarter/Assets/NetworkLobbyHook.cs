using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        var lobbyData   = lobbyPlayer.GetComponent<LobbyPlayer>();
        var localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();

        localPlayer.pName   = lobbyData.playerName;
        localPlayer.pColour = ColorUtility.ToHtmlStringRGBA(lobbyData.playerColor);
    }
}