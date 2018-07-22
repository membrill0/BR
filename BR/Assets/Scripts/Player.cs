using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    private AgeTrack track
    {
        get { return AgeTrack.instance; }
    }
    public MainUI MainUI
    {
        get { return MainUI.instance; }
    }


    [SyncVar]
    public string Name;

    public ClanSheet ClanSheet;
    public GameObject ClanSheetPrefab;

    

    void Start()
    {
        if (isServer)
        {
            Debug.Log("Server");
        }

        if(isLocalPlayer)
        {
            Debug.Log("LocalPlayer");
            CmdSetName(Preferences.instance.PlayerName);
        }
        else
        {
            gameObject.name = Name;
        }
        PlayerConnected();
    }

    /// <summary>
    /// Tell the server to set the name
    /// </summary>
    /// <param name="name"></param>
    [Command]
    public void CmdSetName(string name)
    {
        RpcSetName(name);
    }
    /// <summary>
    /// Tell all clients to set the name
    /// </summary>
    /// <param name="name"></param>
    [ClientRpc]
    public void RpcSetName(string name)
    {
        Name = name;
        gameObject.name = name;
        
    }

    public void AddPlayerToTrack()
    {
        if(track.Players.Contains(this)==false)
        {
            track.Players.Add(this);
        }
    }

    private void OnDestroy()
    {
        PlayerDisconnected();
    }

    public void PlayerDisconnected()
    {
        if (track.Players.Contains(this) == true)
        {
            track.Players.Remove(this);
        }
        DebugSystem.instance.Log(this.Name + " disconnected!");
    }

    public void PlayerConnected()
    {
        AddPlayerToTrack();
        if (isServer)
        {
            ClanSheet = Instantiate(ClanSheetPrefab).GetComponent<ClanSheet>();
            ClanSheet.Player = this;
            NetworkServer.SpawnWithClientAuthority(ClanSheet.gameObject, connectionToClient);
            
            
        }
        if(isLocalPlayer)
        {
            CmdSetCurrentPhase();
        }
        else
        {

        }
        

        DebugSystem.instance.Log("player connected!");
    }

    [Command]
    public void CmdSetCurrentPhase()
    {
        track.SetCurrentPhase();
    }

    [TargetRpc]
    public void TargetSendMessageToClient(NetworkConnection target, string msg)
    {
        DebugSystem.instance.Log(msg);
    }

}
