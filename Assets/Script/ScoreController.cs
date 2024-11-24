using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public int score1, score2;
    public TMP_Text score1Text, score2Text;
    public TMP_Text PowerUp1Text, PowerUp2Text;

    // Start is called before the first frame update
    void Start()
    {
        score1 = 0;
        score2 = 0;
    }

    public void UpdateScore(int Val, bool player2)
    {
        if (player2)
        {
            score2 += Val;
            if (score2 < 0) score2 = 0;
            score2Text.text = "Score - " + score2;
        }
        else
        {
            score1 += Val;
            if (score1 < 0) score1 = 0;
            score1Text.text = "Score - " + score1;
        }
    }

    public void UpdatePowerUp(PowerUpType powerUpType, bool player2)
    {
        if (player2)
        {
            PowerUp2Text.text = "PowerUp - " + powerUpType;
        }
        else
        {
            PowerUp1Text.text = "PowerUp - " + powerUpType;
        }
    }
}
