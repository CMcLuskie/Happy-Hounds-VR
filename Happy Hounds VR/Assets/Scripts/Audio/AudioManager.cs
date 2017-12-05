using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField]
    protected AudioSource dogAudio;
    [SerializeField]
    protected AudioSource tabletAudio;
    [SerializeField]
    protected AudioSource playerAudio;
    public AudioClip barkSound;
    public AudioClip eatSound;
    public AudioClip drinkSound;
    public AudioClip whistleSound;
    public AudioClip clickSound;

    [HideInInspector]
    public enum ClipNames { Bark, Eating, Drinking, Whistle, Click };
    [HideInInspector]
    public enum AudioSources { Player, Dog, Tablet };

    public void PlayClip(AudioSources sourceName, ClipNames clipName)
    {
        switch (sourceName)
        {
            case AudioSources.Dog:
                switch (clipName)
                {
                    case ClipNames.Bark:
                        dogAudio.PlayOneShot(barkSound);
                        break;
                    case ClipNames.Eating:
                        dogAudio.PlayOneShot(eatSound);
                        break;
                    case ClipNames.Drinking:
                        dogAudio.PlayOneShot(drinkSound);
                        break;
                    default:
                        Debug.Log("DOG CANT FUCKING MAKE SOUND TO THAT");
                        break;
                }
                break;
            case AudioSources.Player:
                switch (clipName)
                {
                    case ClipNames.Whistle:
                        playerAudio.PlayOneShot(whistleSound);
                        break;
                    default:
                        Debug.Log("PLAYER CANT FUCKING MAKE SOUND TO THAT");
                        break;
                }
                break;
            case AudioSources.Tablet:
                switch (clipName)
                {
                    case ClipNames.Click:
                        playerAudio.PlayOneShot(clickSound);
                        break;
                    default:
                        Debug.Log("TABLET CANT FUCKING MAKE SOUND TO THAT");
                        break;
                }
                break;
        }

    }
}
