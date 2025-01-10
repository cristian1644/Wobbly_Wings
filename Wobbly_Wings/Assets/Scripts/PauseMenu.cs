using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    public Player player;
    public GameManager gameManager;

    public static bool isPaused = false;

    public void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        isPaused = false;
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
