using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool DoublePlayerMode;
    public int player1Score;
    public int player2Score;
    public GameObject mainMenuPanel, statisticPanel, gameOverPanel;

    public int score1, score2;
    public TMP_Text scoreText1, scoreText2;
    public TMP_Text powerUp1, powerUp2;

    public Snake snake1, snake2;
    public Food food;
    public PowerUp powerUp;
    public int powerUpCoolDownTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void StartSinglePlayerGame()
    {
        mainMenuPanel.SetActive(false);
        DoublePlayerMode = false;
        snake1.gameObject.SetActive(true);
        food.gameObject.SetActive(true);
        StartPowerUpTimer();
    }

    public void StartMultiPlayerGame()
    {
        mainMenuPanel.SetActive(false);
        DoublePlayerMode = true;
        snake1.gameObject.SetActive(true);
        food.gameObject.SetActive(true);
        StartPowerUpTimer();

        snake2.gameObject.SetActive(true);
        scoreText2.gameObject.SetActive(true);
        powerUp2.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        snake1.gameObject.SetActive(false);
        food.gameObject.SetActive(false);
        powerUp.gameObject.SetActive(false);

        if (DoublePlayerMode)
        {
            snake2.gameObject.SetActive(false);
            scoreText2.gameObject.SetActive(false);
            powerUp2.gameObject.SetActive(false);
        }

        gameOverPanel.SetActive(true);
    }

    public void UpdateScore(int Val, bool player2 = false)
    {
        if (player2)
        {
            score2 += Val;
            if (score2 < 0) score2 = 0;
            scoreText2.text = "Score - " + score2;
        }
        else
        {
            score1 += Val;
            if (score1 < 0) score1 = 0;
            scoreText1.text = "Score - " + score1;
            SaveScore(score1);
        }
    }

    public void UpdatePowerUp(PowerUpType powerUpType, bool player2 = false)
    {
        if (player2)
        {
            powerUp2.text = "Power Up - " + powerUpType;
        }
        else
        {
            powerUp1.text = "Power Up - " + powerUpType;
        }
    }

    void SaveScore(int val)
    {
        int currentScore = PlayerPrefs.GetInt("Score", 0);
        currentScore += val;
        PlayerPrefs.SetInt("Score", currentScore);

        //int highScore = PlayerPrefs.GetInt("HighScore", 0);

        //if (currentScore > highScore)
        //{
        //    PlayerPrefs.SetInt("Score", currentScore);
        //}
    }

    void StartPowerUpTimer()
    {
        int randomTime = Random.Range(4, 6);
        Invoke("EnablePowerUp", randomTime);
    }

    void EnablePowerUp()
    {
        powerUp.gameObject.SetActive(true);
        Invoke("DisablePowerUp", powerUpCoolDownTime);
    }

    public void DisablePowerUp()
    {
        powerUp.gameObject.SetActive(false);
        StartPowerUpTimer();
        CancelInvoke("DisablePowerUp");
    }
}

