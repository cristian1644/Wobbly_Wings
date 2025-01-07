using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public GameObject playButton;
    public GameObject startGame;
    public GameObject gameOver;

    private int score;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Pause(); // Il gioco parte in pausa
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        startGame.SetActive(false);
        gameOver.SetActive(false);

        player.transform.position = new Vector3(0f, 0f, 0f);

        Time.timeScale = 1f;
        player.enabled = true;

        // Rimuove tutti i tubi esistenti
        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

}



