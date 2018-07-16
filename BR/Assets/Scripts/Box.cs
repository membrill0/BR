using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Box", menuName = "BloodRage/Box")]
public class Box : ScriptableObject
{
    public List<GameObject> Cards;
    public GameObject ClanSheet;
    public List<Clan> Clans;
}
