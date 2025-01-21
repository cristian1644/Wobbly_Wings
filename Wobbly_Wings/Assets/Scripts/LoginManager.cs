using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    public GameObject loginPanel;       // Riferimento al pannello del login
    public InputField emailInput;       // Riferimento al campo email
    public InputField passwordInput;    // Riferimento al campo password
    public Text errorMessage;           // Riferimento al testo degli errori

    private FirebaseAuth auth;          // Riferimento a Firebase Authentication
    private bool isFirebaseReady = false;  // Variabile per sapere se Firebase è pronto

    private async void Start()
    {
        // Inizializza Firebase in modo asincrono, controllando le dipendenze
        await InitializeFirebase();
        errorMessage.gameObject.SetActive(false); // Nascondi il messaggio di errore all'inizio
    }

    // Metodo per inizializzare Firebase
    private async Task InitializeFirebase()
    {
        // Controlla e risolvi le dipendenze di Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            // Se le dipendenze sono corrette, inizializza Firebase Auth
            auth = FirebaseAuth.DefaultInstance;
            isFirebaseReady = true;  // Imposta Firebase come pronto
            Debug.Log("Firebase initialized successfully!");
        }
        else
        {
            // Gestisci gli errori se Firebase non è correttamente inizializzato
            Debug.LogError("Firebase dependencies could not be resolved: " + dependencyStatus);
        }
    }

    // Metodo per il login
    public void OnLoginButtonClicked()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Non fare nulla se Firebase non è pronto
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        // Effettua il login con l'email e la password
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Mostra l'errore in caso di fallimento
                errorMessage.text = "Login failed: " + task.Exception?.Message;
                errorMessage.gameObject.SetActive(true);
            }
            else
            {
                // Successo nel login, passa alla schermata principale o altro
                Debug.Log("Login successful!");
                // Ad esempio, puoi passare a un'altra scena o abilitare un pannello
                // Puoi fare altre operazioni dopo il login
            }
        });
    }

    // Metodo per la registrazione
    public void OnRegisterButtonClicked()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Non fare nulla se Firebase non è pronto
        }

        string email = emailInput.text;
        string password = passwordInput.text;

        // Effettua la registrazione dell'utente
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Mostra l'errore in caso di fallimento
                errorMessage.text = "Registration failed: " + task.Exception?.Message;
                errorMessage.gameObject.SetActive(true);
            }
            else
            {
                // Successo nella registrazione, passa alla schermata principale o altro
                Debug.Log("Registration successful!");
                // Puoi navigare al menu di login o home
            }
        });
    }

    // Metodo per tornare alla home dal menu di login
    public void BackToHome()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Non fare nulla se Firebase non è pronto
        }

        loginPanel.SetActive(false);  // Nascondi il menu di login
    }

    // Metodo per aprire il menu di login
    public void OpenLoginPanel()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Non fare nulla se Firebase non è pronto
        }

        loginPanel.SetActive(true);    // Mostra il pannello di login
    }
}
