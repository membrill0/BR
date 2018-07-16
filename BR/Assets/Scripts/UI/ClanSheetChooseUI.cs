using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClanSheetChooseUI : MonoBehaviour
{
    public GameObject ClanSheetUIPrefab;
    public Transform ClanSheetList;

    public GameObject AddButton;
	
    public void AddClanSheetUI(ClanSheet clanSheet)
    {
        ClanSheetUI csUI = Instantiate(ClanSheetUIPrefab).GetComponent<ClanSheetUI>();
        csUI.ClanSheet = clanSheet;
        csUI.transform.SetParent(ClanSheetList);
        clanSheet.ClanSheetUI = csUI;
    }

    public void AddClanButton()
    {
        AgeTrack.instance.AddRemoveClanSheets(true);
    }
    
    void Start()
    {
    //    AddButton.SetActive(false);
        transform.localPosition = Vector3.zero;
    }

    void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
}
