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

    public GameObject userPanel;         // Riferimento al pannello utente
    public Text usernameText;           // Riferimento al testo dello username
    public Text highScoreText;          // Riferimento al testo del punteggio

    private bool isUserLoggedIn = false; // Indica se l'utente è loggato



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
            return;
        }

        // Resetta e nascondi il messaggio di errore predefinito
        errorMessage.text = "";
        errorMessage.gameObject.SetActive(false);

        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Gestisci tutti i tipi di errore
                FirebaseException firebaseEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx?.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.WrongPassword:
                        errorMessage.text = "The password is incorrect.";
                        break;
                    case AuthError.InvalidEmail:
                        errorMessage.text = "The email address is not valid.";
                        break;
                    case AuthError.UserNotFound:
                        errorMessage.text = "No user found with this email.";
                        break;
                    default:
                        errorMessage.text = "Login failed. Please try again.";
                        break;
                }

                // Rendi visibile il messaggio di errore
                errorMessage.gameObject.SetActive(true);

                //Debug.LogError("Login failed: " + task.Exception?.Message);
            }
            else
            {
                Debug.Log("Login successful!");
                // Puoi aggiungere azioni da eseguire dopo un login corretto, ad esempio cambiare scena.
            }

            if (task.IsCompleted && !task.IsFaulted)
            {
                isUserLoggedIn = true; // L'utente è ora loggato

                // Nascondi il pannello di login
                loginPanel.SetActive(false);

                // Mostra il pannello utente
                userPanel.SetActive(true);

                // Aggiorna i dati utente (es. username e punteggio)
                UpdateUserInfo();
            }

        });
    }

    public void OnLogoutButtonClicked()
    {
        auth.SignOut();
        Debug.Log("User logged out.");

        isUserLoggedIn = false; // L'utente non è più loggato

        // Nascondi il pannello utente
        userPanel.SetActive(false);

        // Mostra il pannello di login
        loginPanel.SetActive(true);
    }


    public void OnRegisterButtonClicked()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return;
        }

        // Resetta e nascondi il messaggio di errore predefinito
        errorMessage.text = "";
        errorMessage.gameObject.SetActive(false);

        string email = emailInput.text;
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                // Gestisci tutti i tipi di errore
                FirebaseException firebaseEx = task.Exception?.Flatten().InnerExceptions[0] as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx?.ErrorCode;

                switch (errorCode)
                {
                    case AuthError.InvalidEmail:
                        errorMessage.text = "The email address is not valid.";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        errorMessage.text = "The email address is already in use.";
                        break;
                    case AuthError.WeakPassword:
                        errorMessage.text = "The password is too weak. Please use a stronger password.";
                        break;
                    default:
                        errorMessage.text = "Registration failed. Please try again.";
                        break;
                }

                // Rendi visibile il messaggio di errore
                errorMessage.gameObject.SetActive(true);

                //Debug.LogError("Registration failed: " + task.Exception?.Message);
            }
            else
            {
                Debug.Log("Registration successful!");
                // Puoi aggiungere azioni da eseguire dopo la registrazione, come navigare al login o alla schermata principale.
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
        userPanel.SetActive(false);
    }

    // Metodo per aprire il menu di login
    public void OpenLoginPanel()
    {
        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Non fare nulla se Firebase non è pronto
        }

        if (isUserLoggedIn)
        {
            // Se l'utente è loggato, mostra direttamente il pannello utente
            userPanel.SetActive(true);
            loginPanel.SetActive(false);
            Debug.Log("User is already logged in. Opening user panel.");
        }
        else
        {
            // Se l'utente non è loggato, mostra il pannello di login
            loginPanel.SetActive(true);
            userPanel.SetActive(false);
            Debug.Log("User is not logged in. Opening login panel.");
        }
    }

    private void UpdateUserInfo()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            // Imposta lo username (puoi cambiarlo se hai altri dati da Firebase)
            usernameText.text = $"Username: {user.Email}";

            // Recupera il punteggio massimo (placeholder: impostalo manualmente per ora)
            highScoreText.text = "High Score: 0"; // In futuro, lo collegheremo a un database
        }
    }

}
