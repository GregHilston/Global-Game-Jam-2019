using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioFile { 
        MainTheme,
        DoorOpening, 
        FootstepsLinolium,
        FootstepsCarpet,
        FootstepsWood,
        Kick,
        LiftingGrunt,
        MovingHeavyObject,
        PickupVase,
        Scraping, 
    }

    private AudioManager()
    {
        // Prevent outside instantiation
    }

    static AudioManager mInstance;
    private AudioSource audioSource;
    private List<AudioFile> currentlyPlaying = new List<AudioFile>();

    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null) {
                GameObject gameObject = new GameObject();
                mInstance = gameObject.AddComponent<AudioManager>();
                mInstance.audioSource = gameObject.AddComponent<AudioSource>();
            }

            return mInstance;
        }
    }

    private string MapAudioFileEnumToFilePath(AudioFile audioFile)
    {
        switch (audioFile)
        {
            case AudioFile.MainTheme:
                {
                    return "Main Theme";
                }
            case AudioFile.DoorOpening:
                {
                    return "Door Opening";
                }
            case AudioFile.FootstepsLinolium:
                {
                    return "Footsteps (Linolium)";
                }
            case AudioFile.FootstepsCarpet:
                {
                    return "Footsteps(carpet)";
                }
            case AudioFile.FootstepsWood:
                {
                    return "Footsteps(Wood)";
                }
            case AudioFile.Kick:
                {
                    return "Kick(player)";
                }
            case AudioFile.LiftingGrunt:
                {
                    return "Lifting Grunt(player)";
                }
            case AudioFile.MovingHeavyObject:
                {
                    return "Moving Heavy Object";
                }
            case AudioFile.PickupVase:
                {
                    return "Pickup(Vase)";
                }
            case AudioFile.Scraping:
                {
                    return "Scraping Sound";
                }

        }

        return "";
    }

    private void AudioFinished(AudioFile audioFile)
    {
        this.currentlyPlaying.Remove(audioFile);
    }

    public void PlayAudioFile(AudioFile audioFile)
    {
        if (!currentlyPlaying.Contains(audioFile)) { 
            string audioFilePath = MapAudioFileEnumToFilePath(audioFile);

            AudioClip clip = Resources.Load<AudioClip>(audioFilePath);

            mInstance.audioSource.clip = clip;

            mInstance.audioSource.PlayOneShot(clip);

            StartCoroutine(StartMethod(audioFile, clip.length));

            currentlyPlaying.Add(audioFile);
        }
    }

    private IEnumerator StartMethod(AudioFile audioFile, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        AudioFinished(audioFile);
    }
}
