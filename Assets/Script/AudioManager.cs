using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;
    [SerializeField] private AudioClip tenSeconds;

    private void Awake()
    {
        instance = this; 
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioSounds sound)
    {
        switch (sound) {
            case AudioSounds.TEN_SECONDS:
                //audioSource.resource = tenSeconds;
                audioSource.PlayOneShot(tenSeconds); 
                break;
            default:
                Debug.Log($"«вук {sound} не найден.") ;
                break;
        }
    }

}
