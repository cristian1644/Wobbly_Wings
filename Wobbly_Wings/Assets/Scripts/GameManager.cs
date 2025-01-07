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
    public GameObject pauseButton; // Nuovo riferimento al pulsante di pausa
    public GameObject pausePanel; // Nuovo riferimento al pannello di pausa
    public GameObject gameOver;

    private int score;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Pause(); // Il gioco parte in pausa
        pauseButton.SetActive(false); // Nasconde il pulsante di pausa inizialmente
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        pausePanel.SetActive(false); // Assicura che il pannello di pausa sia nascosto
        pauseButton.SetActive(true); // Mostra il pulsante di pausa

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
        pauseButton.SetActive(false); // Nasconde il pulsante di pausa
        Pause();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    // Nuova funzione per mettere il gioco in pausa
    public void OpenPauseMenu()
    {
        Pause();
        pausePanel.SetActive(true); // Mostra il pannello di pausa
        pauseButton.SetActive(false); // Nasconde il pulsante di pausa
    }

    // Nuova funzione per riprendere il gioco
    public void ClosePauseMenu()
    {
        pausePanel.SetActive(false); // Nasconde il pannello di pausa
        pauseButton.SetActive(true); // Mostra il pulsante di pausa
        Time.timeScale = 1f; // Riprende il gioco
        player.enabled = true;
    }

    // Funzione per riavviare il gioco dalla pausa
    public void RestartFromPause()
    {
        pausePanel.SetActive(false);
        Play(); // Richiama Play per resettare tutto
    }

    public void LoadHomeScene()
    {
        Time.timeScale = 1f; // Assicura che il tempo sia normale
        SceneManager.LoadScene("SampleScene"); // Sostituisci "HomeScene" con il nome effettivo della scena iniziale
    }
}



