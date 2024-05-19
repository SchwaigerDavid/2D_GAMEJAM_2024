using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // "instance" is necessary so we can set the "Instance" depending on whether it is null
    private static SoundManager instance;
    public static SoundManager Instance {
        get {
            if(instance == null) {
                GameObject soundManagerObject = new GameObject("SoundManager");
                instance = soundManagerObject.AddComponent<SoundManager>();
            }
            return instance;
        }
    }

    // If more than one SoundManager has been manually created, it is deleted
    private void Awake() {
        if(instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    public void playRandom(string soundDirectory) {
        playRandom(soundDirectory, 1.0);
    }

    public void playRandom(string soundDirectory, double volume) {
        AudioClip[] audioFiles = Resources.LoadAll<AudioClip>(soundDirectory);

        if(audioFiles.Length == 0) {
            Debug.LogWarning("There are no audio files at " + soundDirectory);
        }

        GameObject tempAudioSourceObj = new GameObject("TempAudioObj");
        AudioSource audioSource = tempAudioSourceObj.AddComponent<AudioSource>();
        
        int choice = Random.Range(0, audioFiles.Length);
        audioSource.clip = audioFiles[choice];
        audioSource.volume = (float)volume;
        audioSource.Play();

        tempAudioSourceObj.AddComponent<AutoAudioDestroyer>();
    }
}

public class AutoAudioDestroyer : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(!audioSource.isPlaying) {
            Destroy(gameObject);
        }
    }
}
