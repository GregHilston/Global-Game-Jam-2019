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
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.DoorOpening:
                {
                    return "./Audio/Door Opening.mp3";
                }
            case AudioFile.FootstepsLinolium:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.FootstepsCarpet:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.FootstepsWood:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.Kick:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.LiftingGrunt:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.MovingHeavyObject:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.PickupVase:
                {
                    return "../Audio/Door Opening.mp3";
                }
            case AudioFile.Scraping:
                {
                    return "../Audio/Door Opening.mp3";
                }

        }

        return "";
    }

    public void PlayAudioFile(AudioFile audioFile)
    {
        Debug.Log(Application.dataPath);
        AudioClip clip = new WWW(@"file:///" + MapAudioFileEnumToFilePath(audioFile)).GetAudioClip();
        // AudioClip clip = (AudioClip)Resources.Load(audioFilePath);

        mInstance.audioSource.clip = clip;

        Debug.Log(clip);

        mInstance.audioSource.PlayOneShot(clip);
    }
}
