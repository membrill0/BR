using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class ClanSheet : NetworkBehaviour
{
    [ContextMenuItem("Pick Cards", "PickCards")]
    [ContextMenuItem("Send Next Draft", "SendNextDraft")]
    public Player Player;
    public Clan PlayerClan;
    public int CardsToConsider = 1;
    public AgeTrack AgeTrack
    {
        get { return AgeTrack.instance; }
    }
    public ClanSheet NextPlayer;
    public ClanSheetUI ClanSheetUI;
    public NetworkIdentity Identity;

    [SerializeField]
    private int rageTrack;
    public int RageTrack
    {
        get
        {
            return rageTrack;
        }
        set
        {
            if (value < 0)
            {
                rageTrack = 0;
            }
            else if (value > 12)
            {
                rageTrack = 12;
            }
            else
            {
                rageTrack = value;
            }
        }
    }
    public Stat RageStat;
    public Stat AxesStat;
    public Stat HornsStat;

    public CardsSlot NextDraft;
    public CardsSlot Draft;
    public CardsSlot ConsiderCards;
    public CardsSlot Hand;
    public List<UpgradeSlot> LeaderUpgrades;
    public List<UpgradeSlot> WarriorUpgrades;
    public List<UpgradeSlot> ClanUpgrades;
    public List<UpgradeSlot> ShipUpgrades;
    public List<UpgradeSlot> MonsterUpgrades;

    public enum Status
    {
        Idle = 0,
        Considering = 1,
        CardsPicked = 2,
        AwaitingNextDraft = 3,
        ReadyToPlay = 4
    }

    public Status PlayerStatus = Status.Idle;

    //public void ConsiderCard(Card card)
    //{
    //    if (PlayerStatus == Status.Considering)
    //    {
    //        if (ConsiderCards.Cards.Count < CardsToConsider)
    //        {
    //            card.MoveCard(Draft, ConsiderCards);
    //        }
    //        else
    //        {
    //            ConsiderCards.Cards[ConsiderCards.Cards.Count - 1].MoveCard(ConsiderCards, Draft);
    //            card.MoveCard(Draft, ConsiderCards);
    //        }
    //    }
    //}

    //public void ReturnConsideredCard(Card card)
    //{
    //    if (PlayerStatus == Status.Considering)
    //    {
    //        card.MoveCard(ConsiderCards, Draft);
    //    }
    //}

    //public void PickCards()
    //{
    //    if (ConsiderCards.Cards.Count == CardsToConsider && PlayerStatus == Status.Considering)
    //    {
    //        foreach (Card card in ConsiderCards.Cards.ToList())
    //        {
    //            card.MoveCard(ConsiderCards, Hand);
    //        }
    //        PlayerStatus = Status.CardsPicked;
    //    }
    //}

    //public void SendNextDraft()
    //{
    //    if (PlayerStatus == Status.CardsPicked)
    //    {
    //        if (Draft.Cards.Count > 2)
    //        {
    //            foreach (Card nextDraftCard in Draft.Cards.ToList())
    //            {
    //                nextDraftCard.MoveCard(Draft, NextPlayer.NextDraft);
    //            }
    //            PlayerStatus = Status.AwaitingNextDraft;
    //            AgeTrack.GodsGiftsMoveForward();
    //        }
    //        else
    //        {
    //            foreach (Card remainingCard in Draft.Cards.ToList())
    //            {
    //                remainingCard.MoveCard(Draft, AgeTrack.Ages[remainingCard.Age - 1]);
    //            }
    //            PlayerStatus = Status.ReadyToPlay;
    //        }
    //    }
    //}

    //public void GetNextDraft()
    //{
    //    if (PlayerStatus == Status.AwaitingNextDraft)
    //    {
    //        foreach (Card nextDraftCard in NextDraft.Cards.ToList())
    //        {
    //            nextDraftCard.MoveCard(NextDraft, Draft);
    //        }
    //        PlayerStatus = Status.Considering;
    //    }
    //}
    //[ClientRpc]
    //public void RpcInstantiateClanSheet(string name, Clan clan, int numOfPlayers)
    //{
    //    Player.Name = name;
    //    PlayerClan = clan;
    //    if (numOfPlayers == 2)
    //    {
    //        CardsToConsider = 2;
    //    }
    //    else
    //    {
    //        CardsToConsider = 1;
    //    }
    //    transform.SetParent(AgeTrack.instance.ClanSheetsParent.transform);
    //    gameObject.name = (PlayerClan.ClanName + "_" + Player.Name);
    //    PlayerStatus = Status.Idle;
    //    AgeTrack.instance.ClanSheets.Add(this);
    //}


    void Awake()
    {
        Identity = GetComponent<NetworkIdentity>();
        AddToTrack();
    }

    public void AddToTrack()
    {
        if(AgeTrack.ClanSheets.Contains(this) == false)
        {
            AgeTrack.ClanSheets.Add(this);
            transform.SetParent(AgeTrack.ClanSheetsParent.transform);
            MainUI.instance.ClanSheetChooseUI.AddClanSheetUI(this);
        }
    }
    [ClientRpc]
    public void RpcRemoveFromTrack()
    {
        if (AgeTrack.ClanSheets.Contains(this) == true)
        {
            AgeTrack.ClanSheets.Remove(this);
            Destroy(ClanSheetUI.gameObject);
            Destroy(this.gameObject);
        }
    }
}
