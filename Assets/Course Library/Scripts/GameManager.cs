using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject defeatScreen;
    private void Start()
    {
        Time.timeScale = 0f;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        titleScreen.gameObject.SetActive(false);
    }
    public void DefeatGame()
    {
        Time.timeScale = 0f;
        defeatScreen.gameObject.SetActive(true);
    }
}