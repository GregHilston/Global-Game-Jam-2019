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
    private Dictionary<AudioFile, AudioSource> currentlyPlaying = new Dictionary<AudioFile, AudioSource>();


    public static AudioManager Instance
    {
        get
        {
            if (mInstance == null) {
                GameObject gameObject = new GameObject();
                mInstance = gameObject.AddComponent<AudioManager>();
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
        if (!currentlyPlaying.ContainsKey(audioFile)) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            currentlyPlaying.Add(audioFile, audioSource);

            string audioFilePath = MapAudioFileEnumToFilePath(audioFile);

            AudioClip clip = Resources.Load<AudioClip>(audioFilePath);


            audioSource.clip = clip;

            audioSource.PlayOneShot(clip);

            StartCoroutine(StartMethod(audioFile, clip.length));
        }
    }

    public void StopAudioFile(AudioFile audioFile)
    {
        if (currentlyPlaying.ContainsKey(audioFile))
        {
            currentlyPlaying[audioFile].Stop();
            currentlyPlaying.Remove(audioFile);
        }
    }

    private IEnumerator StartMethod(AudioFile audioFile, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);

        AudioFinished(audioFile);
    }
}
