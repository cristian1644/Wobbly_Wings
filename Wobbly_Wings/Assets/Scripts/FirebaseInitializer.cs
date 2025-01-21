using Firebase;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    // FirebaseApp è l'oggetto che rappresenta l'intera configurazione di Firebase
    public FirebaseApp firebaseApp;

    void Start()
    {
        // Inizializza Firebase
        InitializeFirebase();
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            Debug.Log("Firebase initialized successfully!");
            firebaseApp = app;
        });
    }
}

