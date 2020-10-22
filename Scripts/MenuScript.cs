using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public string game;

    public GameObject menuPanel;
    public GameObject optionsPanel;
    public Text legend;

    private int difficulty; // 1 - Easy; 2 - Normal; 3 - Hard

    public void OptionsButton()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        GetDifficulty();
        SetLegend();
    }

    public void MenuButton()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void LeftButton()
    {
        GetDifficulty();
        difficulty--;
        if (difficulty < 1) difficulty = 1;
        SetDifficulty();
        
    }

    public void RightButton()
    {
        GetDifficulty();
        difficulty++;
        if (difficulty > 3) difficulty = 3;
        SetDifficulty();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(game);
    }

    public void QuitGame()
    {

        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            //Application.Quit();
        }
    }

    void GetDifficulty()
    {
        if (PlayerPrefs.HasKey("Difficulty"))
        {
            difficulty = PlayerPrefs.GetInt("Difficulty");
        } else
        {
            difficulty = 2;
        }
    }

    public void SetDifficulty()
    {
        PlayerPrefs.SetInt("Difficulty", difficulty);
        SetLegend();
    }

    void SetLegend()
    {
        switch (difficulty)
        {
            case 1: legend.text = "Easy"; break;
            case 2: legend.text = "Normal"; break;
            case 3: legend.text = "Hard"; break;
        }
    }
}
