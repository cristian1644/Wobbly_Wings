using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    public InputField emailInput;
    public InputField passwordInput;
    public Text messageText;

    // Avvio di Firebase all'inizio
    void Start()
    {
        // Ottieni l'istanza di FirebaseAuth
        auth = FirebaseAuth.DefaultInstance;
    }

    // Funzione per il login
    public void SignInWithEmailPassword()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Verifica e effettua il login
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                messageText.text = "Login failed: " + task.Exception?.InnerExceptions[0].Message;
            }
            else
            {
                // Qui usiamo task.Result per ottenere il risultato dell'autenticazione
                FirebaseUser newUser = task.Result.User;
                messageText.text = "Login successful! Welcome, " + newUser.DisplayName;
                // Puoi ora gestire la sessione dell'utente
            }
        });
    }

    // Funzione per la registrazione
    public void RegisterWithEmailPassword()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Verifica e crea un nuovo utente
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                messageText.text = "Registration failed: " + task.Exception?.InnerExceptions[0].Message;
            }
            else
            {
                // Qui usiamo task.Result per ottenere il risultato della registrazione
                FirebaseUser newUser = task.Result.User;
                messageText.text = "Registration successful! Welcome, " + newUser.DisplayName;
                // Puoi ora gestire la sessione dell'utente
            }
        });
    }
}
