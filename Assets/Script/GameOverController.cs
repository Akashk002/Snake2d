using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public ScoreController scoreController;
    public TMP_Text score1Text, score2Text, highScore, WinnerText;

    private void OnEnable()
    {

    }

    public void GameOver(bool snake2Win, bool GameDraw = false)
    {
        AudioManager.Instance.Play(SoundType.GameOver);

        gameObject.SetActive(true);
        Time.timeScale = 0;
        UpdateScore();
        if (WinnerText)
        {
            SetWinner(snake2Win, GameDraw);
        }
    }

    private void SetWinner(bool snake2Win, bool GameDraw)
    {
        var winner = (snake2Win) ? "Orange" : " Yellow";
        WinnerText.text = "Winner : " + winner + "Snake";

        if (GameDraw)
        {
            WinnerText.text = "Game Draw";
        }
    }

    private void UpdateScore()
    {
        score1Text.text = scoreController.score1.ToString();

        if (score2Text)
        {
            score2Text.text = scoreController.score2.ToString();

        }
        else
        {
            PlayerPrefs.SetInt("Score", scoreController.score1);
            int HighScore = PlayerPrefs.GetInt("HighScore", 0);
            highScore.text = HighScore.ToString();

            if (scoreController.score1 > HighScore)
            {
                PlayerPrefs.SetInt("HighScore", scoreController.score1);
                highScore.text = scoreController.score1.ToString();
            }
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
