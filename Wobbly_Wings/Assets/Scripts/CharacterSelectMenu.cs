using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    public Player player; // Riferimento al giocatore
    public Sprite[] birdSprites; // Sprite per il personaggio "Bird"
    public Sprite[] blueBirdSprites;
    public Sprite[] orangeBirdSprites;
    public Sprite[] beeSprites; 
    public Button birdButton; // Pulsante per selezionare il personaggio "Bird"
    public Button blueBirdButton;
    public Button orangeBirdButton;
    public Button beeButton; 
    public GameObject characterMenuButton;
    public GameObject characterSelectPanel; // Pannello del menu di selezione personaggio

    // Assicurati di avere un solo metodo Start()
    void Start()
    {
        // Associa i metodi di selezione ai pulsanti
        birdButton.onClick.AddListener(SelectBird);
        blueBirdButton.onClick.AddListener(SelectBlueBird);
        orangeBirdButton.onClick.AddListener(SelectOrangeBird);
        beeButton.onClick.AddListener(SelectBee);
    }

    // Funzione per selezionare il personaggio "Bird"
    void SelectBird()
    {
        player.ChangeCharacter(birdSprites); // Cambia gli sprite del personaggio con quelli del "Bird"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
        characterMenuButton.SetActive(true);
    }

    void SelectBlueBird()
    {
        player.ChangeCharacter(blueBirdSprites); // Cambia gli sprite del personaggio con quelli del "Bird"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
        characterMenuButton.SetActive(true);
    }

    void SelectOrangeBird()
    {
        player.ChangeCharacter(orangeBirdSprites); // Cambia gli sprite del personaggio con quelli del "Bird"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
        characterMenuButton.SetActive(true);
    }

    void SelectBee()
    {
        player.ChangeCharacter(beeSprites); // Cambia gli sprite del personaggio con quelli del "Cat"
        characterSelectPanel.SetActive(false); // Nascondi il menu dopo la selezione
        characterMenuButton.SetActive(true);
    }

    // Funzione per aprire il menu
    public void OpenMenu()
    {
        characterSelectPanel.SetActive(true); // Mostra il menu
        characterMenuButton.SetActive(false);
    }

    // Funzione per chiudere il menu
    public void CloseMenu()
    {
        characterSelectPanel.SetActive(false); // Mostra il menu
        characterMenuButton.SetActive(true);
    }
}

