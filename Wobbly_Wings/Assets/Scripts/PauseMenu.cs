using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    public Player player;
    public GameManager gameManager;
    public GameObject pauseButton;

    public AudioManager audioManager; // Riferimento all'AudioManager
    public UnityEngine.UI.Slider musicSlider; // Slider per la musica
    public UnityEngine.UI.Slider effectsSlider; // Slider per gli effetti

    public static bool isPaused = false;

    private void Start()
    {
        // Carica i valori salvati e aggiorna gli slider
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        effectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume", 0.5f);

        // Associa gli eventi degli slider
        musicSlider.onValueChanged.AddListener(audioManager.SetMusicVolume);
        effectsSlider.onValueChanged.AddListener(audioManager.SetEffectsVolume);
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        isPaused = false;
        pauseButton.SetActive(false);
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        isPaused = false;
        pauseMenu.SetActive(false);

        // Reset direzione (velocità del giocatore) e movimento
        Player playerScript = player.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.direction = Vector3.zero;  // Azzeriamo il movimento residuo
        }

        gameManager.Play();
    }
}
