using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClanSheetChooseUI : MonoBehaviour
{
    public GameObject ClanSheetUIPrefab;
    public Transform ClanSheetList;
    public Player Player;
    public GameObject AddButton;
	
    public void AddClanSheetUI(ClanSheet clanSheet)
    {
        ClanSheetUI csUI = Instantiate(ClanSheetUIPrefab).GetComponent<ClanSheetUI>();
        csUI.Player = Player;
        csUI.ClanSheet = clanSheet;
        csUI.transform.SetParent(ClanSheetList);
        clanSheet.ClanSheetUI = csUI;
    }

    public void AddClanButton()
    {
        //AgeTrack.instance.AddRemoveClanSheets(true);
    }
    
    public void Initialize()
    {
        if (Player.isServer == false)
        {
            AddButton.SetActive(false);
        }
        transform.localPosition = Vector3.zero;
    }

    void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
}
