using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void PauseGame()
    {
        AudioManager.Instance.Play(SoundType.Click);
        AudioManager.Instance.Play(SoundType.Pause);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        AudioManager.Instance.Play(SoundType.Click);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        AudioManager.Instance.Play(SoundType.Click);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        AudioManager.Instance.Play(SoundType.Click);
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
