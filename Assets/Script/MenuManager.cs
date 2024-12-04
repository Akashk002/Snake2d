using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statisticPanel;

    public void OpenSinglePlayerMode()
    {
        AudioManager.Instance.Play(SoundType.Click);
        SceneManager.LoadScene("SinglePlayerGame");
    }

    public void OpenMultiPlayerMode()
    {
        AudioManager.Instance.Play(SoundType.Click);
        SceneManager.LoadScene("MultiPlayerGame");
    }

    public void OpenStatistic()
    {
        AudioManager.Instance.Play(SoundType.Click);
        statisticPanel.SetActive(true);
    }

    public void CloseStatistic()
    {
        AudioManager.Instance.Play(SoundType.Click);
        statisticPanel.SetActive(false);
    }

    public void QuitGame()
    {
        AudioManager.Instance.Play(SoundType.Click);
        Application.Quit();
    }
}

