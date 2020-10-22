using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{

    [Header("Paineis e Menu")]
    public GameObject pausePanel;
    public GameObject endGamePanel;
    public GameObject victoryPanel;
    public Text cherryNum;
    public Text enemyNum;
    public Text gemNum;
    public string cena;

    [Header("Mostradores UI")]
    public Image[] cherries;
    public Sprite cherry;
    public Image[] gems;
    public Sprite gem;
    public Sprite none;

    private bool isPaused;
    private int numGems;
    private int numEnemy;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        numGems = 0;
        numEnemy = CountEnemies();
        UpdateGemUI(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }
    }

    public void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } 
        else
        {
            isPaused = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        health = currentHealth;
        for (int i = 0; i < cherries.Length; i++)
        {
            if (i < currentHealth)
            {
                cherries[i].sprite = cherry;
            }
            else
            {
                cherries[i].sprite = none;
            }
        }
    }

    public void UpdateGemUI(int addGem)
    {
        numGems += addGem;

        for (int i = 0; i < gems.Length; i++)
        {
            if (i < numGems)
            {
                gems[i].sprite = gem;
            }
            else
            {
                gems[i].sprite = none;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player") && numGems >=3)
        {
            StartCoroutine("VictoryPanel");
        }
    }
    private int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("eagle").Length + GameObject.FindGameObjectsWithTag("frog").Length + GameObject.FindGameObjectsWithTag("opossum").Length;
    }

    IEnumerator VictoryPanel()
    {
        yield return new WaitForSeconds(1.5f);
        victoryPanel.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i <= health; i++)
        {
            cherryNum.text = "" + i;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.7f);
        int percentEnemies = ((numEnemy - CountEnemies()) * 100) / numEnemy; 
        for (int i = 0; i < percentEnemies ; i += 2)
        {
            enemyNum.text = "" + i ;
            yield return new WaitForSeconds(0.01f);
        }
        enemyNum.text = "" + percentEnemies;

        yield return new WaitForSeconds(0.7f);
        for (int i = 0; i <= numGems; i++)
        {
            gemNum.text = "" + i;
            yield return new WaitForSeconds(0.2f);
        }
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayerDead()
    {
        StartCoroutine("EndGamePanel");
    }

    IEnumerator EndGamePanel()
    {
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        endGamePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        endGamePanel.SetActive(false);
        victoryPanel.SetActive(false);
        pausePanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(cena);
    }
}
