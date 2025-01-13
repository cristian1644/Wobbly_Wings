using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    public Player player; // Riferimento al giocatore
    public Sprite[] birdSprites; // Sprite per il personaggio "Bird"
    public Sprite[] beeSprites; // Sprite per il personaggio "Cat"
    public Button birdButton; // Pulsante per selezionare il personaggio "Bird"
    public Button beeButton; // Pulsante per selezionare il personaggio "Cat"
    public GameObject characterSelectPanel; // Pannello del menu di selezione personaggio

    // Assicurati di avere un solo metodo Start()
    void Start()
    {
        // Associa i metodi di selezione ai pulsanti
        birdButton.onClick.AddListener(SelectBird);
        beeButton.onClick.AddListener(SelectBee);
    }

    // Funzione per selezionare il personaggio "Bird"
    void SelectBird()
    {
        player.ChangeCharacter(birdSprites); // Cambia gli sprite del personaggio con quelli del "Bird"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
    }

    // Funzione per selezionare il personaggio "Cat"
    void SelectBee()
    {
        player.ChangeCharacter(beeSprites); // Cambia gli sprite del personaggio con quelli del "Cat"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
    }

    // Funzione per aprire il menu
    public void OpenMenu()
    {
        characterSelectPanel.SetActive(true); // Mostra il menu
    }

    // Funzione per chiudere il menu
    public void CloseMenu()
    {
        characterSelectPanel.SetActive(false); // Mostra il menu
    }
}

