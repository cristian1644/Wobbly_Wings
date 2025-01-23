using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System.Threading.Tasks;

public class FirebaseController : MonoBehaviour
{
    private static FirebaseController _instance;
    public static FirebaseController Instance => _instance; // Singleton access

    private DatabaseReference reference;
    private bool isFirebaseInitialized = false;

    private void Awake()
    {
        // Imposta il Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // Evita duplicati
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); // Mantieni l'istanza tra le scene
    }

    async void Start()
    {
        // Controlla le dipendenze di Firebase
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            // Ottieni l'istanza di Firebase
            FirebaseApp app = FirebaseApp.DefaultInstance;

            // Imposta l'URL del database
            string databaseURL = "https://login-53d3f-default-rtdb.europe-west1.firebasedatabase.app/"; // Sostituisci con il tuo URL
            FirebaseDatabase database = FirebaseDatabase.GetInstance(app, databaseURL); // Specifica l'URL

            // Configura la persistenza del database
            database.SetPersistenceEnabled(true);

            // Imposta la reference al database
            reference = database.RootReference;

            Debug.Log("Firebase Initialized successfully!");
            isFirebaseInitialized = true; // Segnala che Firebase è pronto
        }
        else
        {
            Debug.LogError("Firebase dependencies could not be resolved: " + dependencyStatus);
        }
    }

    // Funzione per verificare se Firebase è pronto
    public bool IsFirebaseReady()
    {
        return isFirebaseInitialized;
    }

    // Funzione per scrivere nel database
    public void WriteData(string userId, string username)
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized. Cannot write data.");
            return;
        }

        if (reference != null)
        {
            reference.Child("users").Child(userId).Child("username").SetValueAsync(username).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Data written successfully!");
                }
                else
                {
                    Debug.LogError("Error writing data: " + task.Exception);
                }
            });
        }
        else
        {
            Debug.LogError("Database reference is null!");
        }
    }

    // Funzione per leggere dal database
    public void ReadData(string userId)
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase is not initialized. Cannot read data.");
            return;
        }

        if (reference != null)
        {
            reference.Child("users").Child(userId).Child("username").GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error reading data: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("Username: " + snapshot.Value?.ToString());
                }
            });
        }
        else
        {
            Debug.LogError("Database reference is null!");
        }
    }
}
