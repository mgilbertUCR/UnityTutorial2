using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotCounter : MonoBehaviour
{
    public static BotCounter instance { get; private set; }
    public static int BotCount;
    
    Text Bots;

    //BotCounter BotCount = BotCountText.GetComponent<Text>(); 

    void Awake()
    {
        instance = this;
        Bots = GetComponent<Text>();
    }

    public void UpdateBotCount(int value)
    {
        BotCount += value;

        if (BotCount >= 1)
        {
            Bots.text = BotCount.ToString();
        }
        else
        {
            Bots.text = "YOU WIN! Press Esc to exit.";
        }
             
    }
}
