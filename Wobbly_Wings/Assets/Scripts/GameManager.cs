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
    public GameObject pauseButton;
    public GameObject characterMenuButton;

    public Text highScoreLabelText;  // Testo fisso: "High Score:"
    public Text highScoreValueText;  // Punteggio più alto variabile


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
        pauseButton.SetActive(true);
        characterMenuButton.SetActive(false);

        highScoreLabelText.gameObject.SetActive(false);  
        highScoreValueText.gameObject.SetActive(false);  

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
        // Salva il punteggio più alto
        UpdateHighScore();

        pauseButton.SetActive(false);
        gameOver.SetActive(true);
        playButton.SetActive(true);
        DisplayHighScore();
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void UpdateHighScore()
    {
        // Carica il punteggio più alto salvato, di default è 0
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Se il punteggio attuale è maggiore del punteggio più alto, aggiorna il punteggio più alto
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();  // Salva i dati
        }
    }

    // Metodo per caricare e visualizzare il punteggio più alto
    public void DisplayHighScore()
    {
        // Carica il punteggio più alto
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        highScoreValueText.text = highScore.ToString();  // Visualizza il punteggio più alto
        highScoreLabelText.gameObject.SetActive(true);  // Attiva "High Score:" (etichetta fissa)
        highScoreValueText.gameObject.SetActive(true);  // Attiva il punteggio più alto
    }

}



