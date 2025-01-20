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

    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

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

        // Salva il personaggio selezionato prima di tornare alla home
        int selectedCharacter = GetSelectedCharacter(); // Recupera il personaggio selezionato
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter); // Salva l'indice del personaggio
        PlayerPrefs.Save(); // Assicurati di salvare subito

        // Torna alla scena di home
        SceneManager.LoadScene("SampleScene");

        Invoke("UpdateCharacterOnHome", 0.1f);

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

    // Metodo per ottenere il personaggio selezionato
    private int GetSelectedCharacter()
    {
        // Qui puoi fare la logica per determinare quale personaggio è stato scelto
        if (player.sprites == player.birdSprites)
            return 0; // Bird
        if (player.sprites == player.blueBirdSprites)
            return 1; // BlueBird
        if (player.sprites == player.orangeBirdSprites)
            return 2; // OrangeBird
        if (player.sprites == player.beeSprites)
            return 3; // Bee

        return 0; // Default, nel caso non venga trovato un personaggio
    }

    // Funzione che forza l'aggiornamento del personaggio dopo il caricamento della scena
    private void UpdateCharacterOnHome()
    {
        // Carica il personaggio selezionato da PlayerPrefs
        int selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0); // Carica l'indice salvato, di default 0
        Player player = FindObjectOfType<Player>();

        if (player != null)
        {
            switch (selectedCharacter)
            {
                case 0:
                    player.ChangeCharacter(player.birdSprites); // Seleziona il personaggio Bird
                    break;
                case 1:
                    player.ChangeCharacter(player.blueBirdSprites); // Seleziona il personaggio BlueBird
                    break;
                case 2:
                    player.ChangeCharacter(player.orangeBirdSprites); // Seleziona il personaggio OrangeBird
                    break;
                case 3:
                    player.ChangeCharacter(player.beeSprites); // Seleziona il personaggio Bee
                    break;
                default:
                    player.ChangeCharacter(player.birdSprites); // Se non c'è selezione, imposta il personaggio di default
                    break;
            }
        }
    }

    // Funzione per cambiare gli sprite in base al personaggio scelto
    public void ChangeCharacter(Sprite[] newSprites)
    {
        sprites = newSprites; // Cambia l'array di sprite
        spriteIndex = 0; // Resetta l'indice per partire dal primo sprite
        spriteRenderer.sprite = sprites[spriteIndex]; // Imposta il primo sprite
    }
}
