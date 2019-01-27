using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    public enum AudioFile { 
        MainTheem,
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

    private static readonly AudioManager instance = new AudioManager();

    public static AudioManager GetInstance()
    {
        return instance;
    }
}
