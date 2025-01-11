using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip flapSound;
    public AudioClip collisionSound;
    public AudioClip scoreSound;

    public AudioSource musicSource;   // AudioSource per la musica di sottofondo
    public AudioSource effectsSource; // AudioSource per gli effetti sonori

    private AudioSource audioSource;
    public AudioSource flap;
    public AudioSource collision;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlapSound()
    {
        flap.PlayOneShot(flapSound);
    }

    public void PlayCollisionSound()
    {
        collision.PlayOneShot(collisionSound);
    }

    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }

    // Metodo per aggiornare il volume della musica
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume); // Salva il valore
        PlayerPrefs.Save(); // Assicura il salvataggio immediato
    }

    // Metodo per aggiornare il volume degli effetti sonori
    public void SetEffectsVolume(float volume)
    {
        flap.volume = volume;
        collision.volume = volume;
        PlayerPrefs.SetFloat("EffectsVolume", volume); // Salva il valore
        PlayerPrefs.Save(); // Assicura il salvataggio immediato
    }
}
