using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClanSheetUI : MonoBehaviour
{
    public ClanSheet ClanSheet;
    public GameObject RemoveButton;

    public Player Player;
	// Use this for initialization
	void Start () {
		
	}

    public void RemoveClanSheetUI()
    {
        ClanSheet.RpcRemoveFromTrack();
       // Destroy(this.gameObject);
    }

    public void DisableRemoveButton()
    {
        RemoveButton.SetActive(false);
    }

    public void Test()
    {
        Player.AskToAssignToSheet(ClanSheet);
    }

}
