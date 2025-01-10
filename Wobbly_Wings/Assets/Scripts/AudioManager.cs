using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip flapSound;
    public AudioClip collisionSound;
    public AudioClip scoreSound;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFlapSound()
    {
        audioSource.PlayOneShot(flapSound);
    }

    public void PlayCollisionSound()
    {
        audioSource.PlayOneShot(collisionSound);
    }

    public void PlayScoreSound()
    {
        audioSource.PlayOneShot(scoreSound);
    }
}
