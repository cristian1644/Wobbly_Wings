using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using System.Threading.Tasks;
using Firebase.Database;

public class LoginManager : MonoBehaviour
{
    public GameObject loginPanel;       // Riferimento al pannello del login
    public InputField emailInput;       // Riferimento al campo email
    public InputField passwordInput;    // Riferimento al campo password
    public Text errorMessage;           // Riferimento al testo degli errori

    private FirebaseAuth auth;          // Riferimento a Firebase Authentication
    private bool isFirebaseReady = false;  // Variabile per sapere se Firebase � pronto

    public GameObject userPanel;         // Riferimento al pannello utente
    public Text usernameText;           // Riferimento al testo dello username
    public Text highScoreText;          // Riferimento al testo del punteggio

    private bool isUserLoggedIn = false; // Indica se l'utente � loggato

    private DatabaseReference databaseReference;




    private async void Start()
    {
        // Inizializza Firebase prima di fare altre operazioni
        await InitializeFirebase();

        // Una volta che Firebase � pronto, carica il punteggio
        if (isFirebaseReady)
        {
            LoadHighScore();
        }
        else
        {
            Debug.LogError("Firebase non � pronto. Impossibile procedere.");
            return;
        }

        if (!isFirebaseReady)
        {
            Debug.LogError("Firebase is not ready yet.");
            return; // Fermiamo il codice se Firebase non � pronto
        }

        // Verifica se i riferimenti sono inizializzati
        if (loginPanel == null || userPanel == null)
        {
            Debug.LogError("LoginPanel or UserPanel are not assigned in the Inspector!");
            return; // Fermiamo il codice se uno dei pannelli non � stato assegnato
        }

        // Controlliamo se l'utente � gi� loggato (se � presente la chiave "UserLoggedIn" in PlayerPrefs)
        if (PlayerPrefs.GetString("UserLoggedIn", "false") == "true")
        {
            isUserLoggedIn = true;
            loginPanel.SetActive(false);
            userPanel.SetActive(true);

            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                UpdateUserInfo(); // Aggiorna i dati dell'utente
            }
        }
        else
        {
            loginPanel.SetActive(true);
            userPanel.SetActive(false);
        }

        errorMessage.gameObject.SetActive(false);
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
            databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            isFirebaseReady = true;  // Imposta Firebase come pronto
            Debug.Log("Firebase initialized successfully!");
        }
        else
        {
            // Gestisci gli errori se Firebase non � correttamente inizializzato
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
            }
            else
            {
                Debug.Log("Login successful!");
                // Puoi aggiungere azioni da eseguire dopo un login corretto, ad esempio cambiare scena.
            }

            if (task.IsCompleted && !task.IsFaulted)
            {
                isUserLoggedIn = true; // L'utente � ora loggato

                // Salviamo lo stato di login
                PlayerPrefs.SetString("UserLoggedIn", "true"); // Imposta "true" per indicare che l'utente � loggato
                PlayerPrefs.Save(); // Salva subito i dati

                // Nascondi il pannello di login
                loginPanel.SetActive(false);

                // Mostra il pannello utente
                userPanel.SetActive(true);

                // Recupera i dati dell'utente
                FirebaseUser user = auth.CurrentUser;
                if (user != null)
                {
                    string userId = user.UserId;
                    DatabaseReference userRef = databaseReference.Child("users").Child(userId);

                    // Verifica se i dati dell'utente esistono nel database
                    userRef.GetValueAsync().ContinueWithOnMainThread(task2 =>
                    {
                        if (task2.IsCompleted && task2.Result.Exists)
                        {
                            // Se i dati esistono gi�, non fare nulla
                            Debug.Log("User data already exists in database.");
                        }
                        else
                        {
                            // Se i dati non esistono, creali con il punteggio iniziale di 0
                            UserData userData = new UserData(user.Email, 0); // Record iniziale = 0
                                                                             // Salva i dati nel database
                            userRef.SetRawJsonValueAsync(JsonUtility.ToJson(userData));
                        }
                    });

                    // Aggiorna i dati utente (es. username e punteggio)
                    UpdateUserInfo();
                }
            }
        });
    }


    public void OnLogoutButtonClicked()
    {
        // Recupera il punteggio attuale dal pannello
        int currentHighScore = int.Parse(highScoreText.text.Split(':')[1].Trim());

        // Salva il punteggio in PlayerPrefs
        PlayerPrefs.SetInt("HighScore", currentHighScore);
        PlayerPrefs.Save();

        // Esegui il logout
        auth.SignOut();
        Debug.Log("User logged out.");

        isUserLoggedIn = false; // L'utente non � pi� loggato

        // Nascondi il pannello utente
        userPanel.SetActive(false);

        // Mostra il pannello di login
        loginPanel.SetActive(true);

        // Rimuovi la chiave di login in PlayerPrefs
        PlayerPrefs.SetString("UserLoggedIn", "false");
        PlayerPrefs.Save();
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
            return; // Non fare nulla se Firebase non � pronto
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
            return; // Non fare nulla se Firebase non � pronto
        }

        if (isUserLoggedIn)
        {
            // Se l'utente � loggato, mostra direttamente il pannello utente
            userPanel.SetActive(true);
            loginPanel.SetActive(false);
            Debug.Log("User is already logged in. Opening user panel.");
        }
        else
        {
            // Se l'utente non � loggato, mostra il pannello di login
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
            // Imposta lo username
            usernameText.text = $"Username: {user.Email}";

            // Recupera il punteggio dal database
            string userId = user.UserId;
            DatabaseReference userRef = databaseReference.Child("users").Child(userId);

            userRef.GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Error retrieving user data: " + task.Exception?.Message);
                }
                else
                {
                    DataSnapshot snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        string json = snapshot.GetRawJsonValue();
                        UserData userData = JsonUtility.FromJson<UserData>(json);

                        // Imposta il punteggio da Firebase
                        highScoreText.text = $"High Score: {userData.highScore}";

                        // Aggiorna anche PlayerPrefs con il punteggio
                        PlayerPrefs.SetInt("HighScore", userData.highScore);
                        PlayerPrefs.Save();
                    }
                    else
                    {
                        // Se non ci sono dati, carica il punteggio da PlayerPrefs
                        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
                        highScoreText.text = $"High Score: {savedHighScore}";
                    }
                }
            });
        }
    }




    public void UpdateHighScore(int newScore)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            string userId = user.UserId;

            // Leggi i dati dell'utente dal database
            databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    // Converte i dati JSON in oggetto UserData
                    string json = task.Result.GetRawJsonValue();
                    UserData userData = JsonUtility.FromJson<UserData>(json);

                    // Confronta il punteggio attuale con quello salvato nel database
                    if (newScore > userData.highScore)
                    {
                        // Se il nuovo punteggio � maggiore, aggiorna il punteggio nel database
                        userData.highScore = newScore;

                        // Salva il nuovo punteggio nel database
                        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(JsonUtility.ToJson(userData));

                        // Aggiorna il pannello utente
                        highScoreText.text = $"High Score: {newScore}";
                    }
                }
                else
                {
                    Debug.LogWarning("User data not found in database.");
                }
            });
        }
    }

    private void LoadHighScore()
    {
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);  // 0 � il valore di default
        highScoreText.text = $"High Score: {savedHighScore}";
    }


}

[System.Serializable]
public class UserData
{
    public string email;     // Email dell'utente
    public int highScore;    // Record dell'utente

    // Costruttore per inizializzare i dati
    public UserData(string email, int highScore)
    {
        this.email = email;
        this.highScore = highScore;
    }
}
