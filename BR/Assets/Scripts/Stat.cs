using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Stat", menuName = "BloodRage/Stat")]
public class Stat : ScriptableObject
{
    [SerializeField]
    private int currentStatValue = 1;
    public int CurrentStatValue
    {
        get
        {
            return currentStatValue;
        }
        set
        {
            if (value < 1)
            {
                currentStatValue = 1;
            }
            else if(value > 6)
            {
                currentStatValue = 6;
            }
            else
            {
                currentStatValue = value;
            }
        }
    }
    public List<int> Bonus;
}
