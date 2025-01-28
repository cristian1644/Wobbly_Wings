using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectMenu : MonoBehaviour
{
    public Player player; // Riferimento al giocatore
    public Sprite[] birdSprites; // Sprite per il personaggio "Bird"
    public Sprite[] blueBirdSprites;
    public Sprite[] orangeBirdSprites;
    public Sprite[] beeSprites;
    public Sprite[] greenBirdSprites;
    public Sprite[] whiteBirdSprites;

    public Button birdButton; // Pulsante per selezionare il personaggio "Bird"
    public Button blueBirdButton;
    public Button orangeBirdButton;
    public Button beeButton;
    public Button greenBirdButton;
    public Button whiteBirdButton;

    public GameObject characterMenuButton;
    public GameObject characterSelectPanel; // Pannello del menu di selezione personaggio

    // Assicurati di avere un solo metodo Start()
    void Start()
    {
        // Associa i metodi di selezione ai pulsanti
        birdButton.onClick.AddListener(SelectBird);
        blueBirdButton.onClick.AddListener(SelectBlueBird);
        orangeBirdButton.onClick.AddListener(SelectOrangeBird);
        greenBirdButton.onClick.AddListener(SelectGreenBird);
        whiteBirdButton.onClick.AddListener(SelectWhiteBird);
        beeButton.onClick.AddListener(SelectBee);
    }

    void SelectBird()
    {
        player.ChangeCharacter(birdSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 0); // Salva l'indice del personaggio scelto (0 = Bird)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
        characterMenuButton.SetActive(true);
    }

    void SelectBlueBird()
    {
        player.ChangeCharacter(blueBirdSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 1); // Salva l'indice del personaggio scelto (1 = BlueBird)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
        characterMenuButton.SetActive(true);
    }

    void SelectOrangeBird()
    {
        player.ChangeCharacter(orangeBirdSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 2); // Salva l'indice del personaggio scelto (2 = OrangeBird)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
        characterMenuButton.SetActive(true);
    }

    void SelectBee()
    {
        player.ChangeCharacter(beeSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 3); // Salva l'indice del personaggio scelto (3 = Bee)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
        characterMenuButton.SetActive(true);
    }

    void SelectGreenBird()
    {
        player.ChangeCharacter(greenBirdSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 4); // Salva l'indice del personaggio scelto (1 = BlueBird)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
        characterMenuButton.SetActive(true);
    }

    void SelectWhiteBird()
    {
        player.ChangeCharacter(whiteBirdSprites);
        PlayerPrefs.SetInt("SelectedCharacter", 5); // Salva l'indice del personaggio scelto (1 = BlueBird)
        PlayerPrefs.Save();
        characterSelectPanel.SetActive(false);
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

