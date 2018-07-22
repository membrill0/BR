using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AgeTrack : NetworkBehaviour
{
    public static AgeTrack instance = null;

    

    [ContextMenuItem("Unbox", "Unbox")]
    [ContextMenuItem("Deal","Deal")]
    [ContextMenuItem("Create Clan Sheets", "CreateClanSheets")]
    public Box Box;

   
    public List<Player> Players;

    public List<ClanSheet> ClanSheets;
    public GameObject ClanSheetsParent;

    public List<CardsSlot> Ages;

    public int CurrentAge = 1;

    public enum Phase
    {
        NotConnected = 0,
        Lobby = 1,
        GodsGifts = 2,
        Action = 3,
        Discard = 4,
        Quest = 5,
        Ragnarog = 6,
        ReleaseValhalla = 7
    }

    public Phase CurrentPhase = Phase.NotConnected;

    void Awake()
    {
        if(instance ==null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        
    }

    public void SetCurrentPhase()
    {
        if (isServer)
        {
            Phase phase = CurrentPhase;
            if (CurrentPhase == Phase.NotConnected)
            {
                if (Players.Count > 0)
                {
                    phase = Phase.Lobby;
                }
            }
            RpcSetCurrentPhase(phase);
        }
    }
    [ClientRpc]
    public void RpcSetCurrentPhase(Phase phase)
    {
        DebugSystem.instance.Log("Game phase swithing from " + CurrentPhase + " to "+phase);
        CurrentPhase = phase;
    }


    //void Update()
    //{ 

    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        if (isServer)
    //        {
    //            AddRemoveClanSheets(true);
    //        }
    //    }
    //}

    //public void AddRemoveClanSheets(bool add)
    //{
    //    if (isServer)
    //    {
    //        if (add == true)
    //        {
    //            if (ClanSheets.Count < Players.Count)
    //            {
    //                ClanSheet sheet = Instantiate(Box.ClanSheet).GetComponent<ClanSheet>();
    //                NetworkServer.Spawn(sheet.gameObject);
    //            }
    //        }
    //        else
    //        {
    //            if (ClanSheets.Count > 0)
    //            {
    //                ClanSheet sheet = ClanSheets[ClanSheets.Count - 1];
    //                sheet.RpcRemoveFromTrack();
    //            }
    //        }
    //    }
    //}


    //public void Unbox()
    //{
    //    foreach (GameObject cardObject in Box.Cards)
   //     {
   //         Card crd = cardObject.GetComponent<Card>();
   //         if (crd.NumberOfPlayersInGame <= Players.Count)
   //         {
   //             Card currentCard = Instantiate(cardObject).GetComponent<Card>();
   //             NetworkServer.Spawn(currentCard.gameObject);
   //             currentCard.RpcInstantiateCard();
   //         }
   //     }   
   // }

   // public void CreateClanSheets()
   // {
   //     for (int i = 0; i < Players.Count; i++)
   //     {
   //         ClanSheet sheet = Instantiate(Box.ClanSheet).GetComponent<ClanSheet>();
   //         NetworkServer.Spawn(sheet.gameObject);
   ////         sheet.RpcInstantiateClanSheet(Players[i].Name,Players[i].Clan,Players.Count);
   //     }
   //     for(int i = 0; i < ClanSheets.Count;i++)
   //     {
   //         if (i < ClanSheets.Count - 1)
   //         {
   //             ClanSheets[i].NextPlayer = ClanSheets[i + 1];
   //         }
   //         else
   //         {
   //             ClanSheets[i].NextPlayer = ClanSheets[0];
   //         }
   //     }
   // }

   // public void Deal()
   // {
   //     int currentPlayer = 0;
   //     for (int i = 0; i< Players.Count * 8;i++)
   //     {
   //         int remainingNumberOfCards = Ages[CurrentAge - 1].Cards.Count;
   //         Card dealingCard = Ages[CurrentAge - 1].Cards[Random.Range(0, remainingNumberOfCards)];
   //         Ages[CurrentAge - 1].Cards.Remove(dealingCard);

   //         ClanSheets[currentPlayer].Draft.Cards.Add(dealingCard);
   //         dealingCard.transform.SetParent(ClanSheets[currentPlayer].Draft.CardsParent.transform);
   //         dealingCard.SetCardStatus();
   //         currentPlayer++;
   //         if (currentPlayer >= Players.Count)
   //         {
   //             currentPlayer = 0;
   //         }
   //     }
   //     foreach(Card card in Ages[CurrentAge-1].Cards)
   //     {
   //         card.SetCardStatus();
   //     }
   //     foreach(ClanSheet sheet in ClanSheets)
   //     {
   //         sheet.PlayerStatus = ClanSheet.Status.Considering;
   //     }
   // }

   // private void NextPhase()
   // {
   //     if (CurrentPhase == Phase.GameStart)
   //     {
   //         if(Players.Count < 2)
   //         {
   //             DebugSystem.instance.Log("Not Enough Players!");
   //             return;
   //         }
   //         Unbox();
   //         CreateClanSheets();
   //     }
   //     if (CurrentPhase == Phase.GodsGifts)
   //     {
   //         Deal();
   //     }
   //     if (CurrentPhase == Phase.Action)
   //     {

   //     }
   //     if (CurrentPhase == Phase.Discard)
   //     {

   //     }
   //     if (CurrentPhase == Phase.Quest)
   //     {

   //     }
   //     if (CurrentPhase == Phase.Ragnarog)
   //     {

   //     }
   //     if (CurrentPhase == Phase.ReleaseValhalla)
   //     {

   //     }

   //     if (CurrentPhase < Phase.ReleaseValhalla)
   //     {
   //         CurrentPhase++; ;
   //     }
   //     DebugSystem.instance.Log("Swithing from phase " + (CurrentPhase - 1) + " to phase " + CurrentPhase);

   // }

   // //public void GodsGiftsMoveForward()
   // //{
   // //    bool nextDraft = false;

   // //    foreach(ClanSheet nextDraftSheet in ClanSheets)
   // //    {
   // //        if(nextDraftSheet.PlayerStatus == ClanSheet.Status.AwaitingNextDraft)
   // //        {
   // //            nextDraft = true;
   // //        }
   // //        else
   // //        {
   // //            nextDraft = false;
   // //            Debug.Log("Waiting for " + nextDraftSheet.Player.Name + " to pick cards.");
   // //            break;
   // //        }
   // //    }

   // //    if(nextDraft == true)
   // //    {
   // //        foreach(ClanSheet nextSheet in ClanSheets)
   // //        {
   // //            nextSheet.GetNextDraft();
   // //        }
   // //    }
   // //}

   // public override void OnStartServer()
   // {
   //     NetworkServer.Spawn(this.gameObject);
   // }

}

