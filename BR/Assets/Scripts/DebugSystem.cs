using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugSystem : MonoBehaviour
{
    public static DebugSystem instance = null;
    public Text text;
    public float Timer = 10f;
    private float timer = 10f;
    void Awake()
    {
        timer = Timer;
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

    public void Log(string str)
    {
        if (text != null)
        {
            text.text += ("\n" + str);
            timer = Timer;
        }
        Debug.Log(str);
    }

    private void Update()
    {
        if (text.text != "")
        {
            timer -= Time.deltaTime;
            if (timer < 0f)
            {
                text.text = "";
                timer = Timer;
            }
        }
    }
}
