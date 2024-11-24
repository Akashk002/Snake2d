using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Statistic : MonoBehaviour
{
    public TMP_Text highScoreText, scoreText;

    private void OnEnable()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        var lastScore = PlayerPrefs.GetInt("Score", 0);
        scoreText.text = lastScore.ToString();

        int HighScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = HighScore.ToString();

        if (lastScore > HighScore)
        {
            PlayerPrefs.SetInt("HighScore", lastScore);
            highScoreText.text = lastScore.ToString();
        }
    }

}
