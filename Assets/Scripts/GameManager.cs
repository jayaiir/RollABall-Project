using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public Player player;

    public int coins;

    public int[] potions = new int[3] { 0, 0, 0 };

    public GameObject endGamePanel;
    public TMP_Text coinText;
    public TMP_Text potionText;
    public TMP_Text shieldText;
    public TMP_Text shieldPlusText;
    public TMP_Text endGameText;

    public AudioClip gameOverSound;
    public AudioClip youWinSound;

    public bool gameOver;

    public TMP_Text levelTimerText;
    public float levelTimer = 90f;
    private bool isTimerRunning = true;

    // Start is called before the first frame update
    void Start()
    {
        coins = 0;
        gameOver = false;
        endGamePanel.SetActive(false);

    }

    // Update is called once per frame
    private void Update()
    {
        UpdateLevelTimer();
    }

    public void UpdateData(int hp)
    {
        hp = player.health;
        coinText.text = $"{coins}";
        potionText.text = $"{potions[0]}";
        shieldText.text = $"{potions[1]}";
        shieldPlusText.text = $"{potions[2]}";
        HandleGameOver();
        CheckLevelWin();
    }

    private void UpdateLevelTimer()
    {
        if (isTimerRunning && !gameOver)
        {
            levelTimer -= Time.deltaTime;
            if (levelTimer <= 0)
            {
                levelTimer = 0;
                isTimerRunning = false;
                HandleGameOver();
                
            }
            if (levelTimer > 10)
            {
                levelTimerText.text = $"{levelTimer:F0}";
                levelTimerText.color = Color.white;
            } else
            {
                levelTimerText.text = $"{levelTimer:F1}";
                levelTimerText.color = Color.red;
            }

            
        }
    }

    public void HandleGameOver()
    {
        // Check if the game is over and display the end game panel
        if (player.health == 0 || levelTimer <= 0)
        {
            player.audioSource.PlayOneShot(gameOverSound);
            endGameText.text = "Game Over!";
            endGameText.color = Color.red;
            endGamePanel.SetActive(true);
            gameOver = true;
        }
    }

    public void CheckLevelWin()
    {
        // Check if the player has won and display the end game panel
        if (coins == 10)
        {
            player.audioSource.PlayOneShot(youWinSound);
            endGameText.text = "You Win!";
            endGameText.color = Color.green;
            endGamePanel.SetActive(true);
            gameOver = true;
        }
    }

    public void Restart()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}

