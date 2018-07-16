using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Card : NetworkBehaviour
{
    [ContextMenuItem("Card Action","CardAction")]
    public string Name;
    [Range(1,3)]
    public int Age = 1;

    public int NumberOfPlayersInGame = 2;
    
    public enum CardStatus
    {
        InBox = 0,
        Draft = 1,
        NextDraft = 2,
        Consider = 3,
        Hand = 4,
        Played = 5,
        Discarded = 6
    }
    [SyncVar]
    public CardStatus Status = 0;

    public ClanSheet CardOwner;

    public void SetCardStatus()
    {
        ClanSheet clanSheet = GetComponentInParent<ClanSheet>();
        AgeTrack ageTrack = GetComponentInParent<AgeTrack>();
        if (clanSheet != null)
        {
            CardOwner = clanSheet;
            if (clanSheet.Draft.Cards.Contains(this))
            {
                Status = CardStatus.Draft;
                return;
            }
            if(clanSheet.NextDraft.Cards.Contains(this))
            {
                Status = CardStatus.NextDraft;
                return;
            }
            if(clanSheet.ConsiderCards.Cards.Contains(this))
            {
                Status = CardStatus.Consider;
                return;
            }
            if (clanSheet.Hand.Cards.Contains(this))
            {
                Status = CardStatus.Hand;
                return;
            }
            foreach(UpgradeSlot leaderUpgradeslot in clanSheet.LeaderUpgrades )
            {
                if(leaderUpgradeslot.Upgrade == this)
                {
                    Status = CardStatus.Played;
                    return;
                }
            }
            foreach (UpgradeSlot warriorUpgradeslot in clanSheet.WarriorUpgrades)
            {
                if (warriorUpgradeslot.Upgrade == this)
                {
                    Status = CardStatus.Played;
                    return;
                }
            }
            foreach (UpgradeSlot clanUpgradeslot in clanSheet.ClanUpgrades)
            {
                if (clanUpgradeslot.Upgrade == this)
                {
                    Status = CardStatus.Played;
                    return;
                }
            }
            foreach (UpgradeSlot monsterUpgradeslot in clanSheet.MonsterUpgrades)
            {
                if (monsterUpgradeslot.Upgrade == this)
                {
                    Status = CardStatus.Played;
                    return;
                }
            }
            foreach (UpgradeSlot shipUpgradeslot in clanSheet.ShipUpgrades)
            {
                if (shipUpgradeslot.Upgrade == this)
                {
                    Status = CardStatus.Played;
                    return;
                }
            }
        }
        else
        {
            CardOwner = null;
            if (ageTrack.Ages[Age-1].Cards.Contains(this))
            {
                Status = CardStatus.Discarded;
                return;
            }
        }
    }

    public void MoveCard(List<Card> removeFrom, List<Card> addTo, Transform parent)
    {
        removeFrom.Remove(this);
        addTo.Add(this);
        transform.SetParent(parent);
        SetCardStatus();
    }

    public void MoveCard(List<Card> removeFromList, CardsSlot addTo)
    {
        removeFromList.Remove(this);
        addTo.Cards.Add(this);
        transform.SetParent(addTo.CardsParent.transform);
        SetCardStatus();
    }

    public void MoveCard(CardsSlot removeFrom, CardsSlot addTo)
    {
        removeFrom.Cards.Remove(this);
        addTo.Cards.Add(this);
        transform.SetParent(addTo.CardsParent.transform);
        SetCardStatus();
    }

    //public void CardAction()
    //{
    //    if (CardOwner != null)
    //    {
    //        if (Status == CardStatus.Draft)
    //        {
    //            CardOwner.ConsiderCard(this);
    //            return;
    //        }
    //        if(Status == CardStatus.Consider)
    //        {
    //            CardOwner.ReturnConsideredCard(this);
    //        }
    //    }
    //}

    [ClientRpc]
    public void RpcInstantiateCard()
    {
        Status = CardStatus.InBox;
        gameObject.name = (Age + "_" + Name);
        AgeTrack.instance.Ages[Age - 1].Cards.Add(this);
        transform.SetParent(AgeTrack.instance.Ages[Age-1].CardsParent.transform);
    }
}
