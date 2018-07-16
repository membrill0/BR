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
    public GameObject ClanSheetPrefab;
    [SyncVar (hook = "SetGameObjectName")]
    public string Name;

    public ClanSheet ClanSheet;

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
            Debug.Log("notLocalPlayer");
            gameObject.name = Name;
        }


        if (track.Players.Contains(this) == false)
        {
            track.Players.Add(this);
        }

    }

    /// <summary>
    /// Client sets the player name. TODO: get the name from the playerprefs (random for now)
    /// </summary>
    [Command]
    public void CmdSetName(string name)
    {
        Name = name;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                DebugSystem.instance.Log(Name + "----" + gameObject.name);

                foreach (Player pl in track.Players)
                {
                    DebugSystem.instance.Log(pl.Name);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            foreach (ClanSheet sheet in track.ClanSheets)
            {
                if (sheet.Player == null)
                {
                    AskToAssignToSheet(sheet);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (ClanSheet != null)
            {
                AskToPickClan(track.Box.Clans[Random.Range(0, track.Box.Clans.Count)]);
            }
        }
        
    }

    public void AskToPickClan(Clan clan)
    {
        if(isLocalPlayer)
        {
            int i = 0;
            foreach(Clan cln in track.Box.Clans)
            {
                if(cln == clan)
                {
                    CmdPickClan(i);
                    break;
                }
                i++;
            }
        }
    }

    [Command]
    public void CmdPickClan(int index)
    {
        bool clanTaken = false;
        foreach(ClanSheet sheet in track.ClanSheets)
        {
            if(sheet.PlayerClan == track.Box.Clans[index])
            {
                clanTaken = true;
                break;
            }
        }
        if (clanTaken == false)
        {
            RpcPickClan(index);
        }
        else
        {
            TargetSendMessageToClient(connectionToClient,"Clan is already taken!");
        }
    }
    [ClientRpc]
    public void RpcPickClan(int index)
    {
        ClanSheet.PlayerClan = track.Box.Clans[index];
    }

    public void AskToAssignToSheet(ClanSheet sheet)
    {
        if (isLocalPlayer)
        {
            if (sheet.Player == null)
            {
                CmdAssignPlayerToSheet(sheet.Identity);
            }
        }
    }

    [Command]
    public void CmdAssignPlayerToSheet(NetworkIdentity identity)
    {
        RpcAssignPlayerToSheet(identity);
    }
    [ClientRpc]
    public void RpcAssignPlayerToSheet(NetworkIdentity identity)
    {
        if (ClanSheet != null)
        {
            ClanSheet.Player = null;
        }
        ClanSheet sheet = identity.GetComponent<ClanSheet>();
        sheet.Player = this;
        ClanSheet = sheet;

    }
    
    /// <summary>
    /// SyncVar hook to name the gameobject same as the player name
    /// </summary>
    /// <param name="name"></param>
    public void SetGameObjectName(string name)
    {
        Name = name;
        gameObject.name = Name;
    }

    [TargetRpc]
    public void TargetSendMessageToClient(NetworkConnection target, string msg)
    {
        DebugSystem.instance.Log(msg);
    }

}
