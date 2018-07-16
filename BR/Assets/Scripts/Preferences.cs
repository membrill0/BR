using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preferences : MonoBehaviour
{
    public static Preferences instance;
    public string PlayerName = "";
    public string[] RandomNames = {"Comi", "Zika", "Shomy", "Eric M. Lang", "Batman", "Daum"};

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public InputField PlayerNameInputField;
	// Use this for initialization
	void Start ()
    {
	    if(PlayerPrefs.HasKey("PlayerName") == true)
        {
            PlayerName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            PlayerName = RandomNames[Random.Range(0,RandomNames.Length)];
            PlayerPrefs.SetString("PlayerName", PlayerName);
        }

        //PlayerNameText.text = PlayerName;
        PlayerNameInputField.text = PlayerName;
    }
	

    public void ChangePlayerName(string playerName)
    {
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerName = PlayerPrefs.GetString("PlayerName");
    }
}
