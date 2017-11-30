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

    enum ClipNames { Bark, Eating, Drinking, Whistle, Click };
    enum AudioSources { Player, Dog, Tablet };

    void PlayClip(AudioSources sourceName, ClipNames clipName)
    {
        switch (sourceName)
        {
            case AudioSources.Dog:
                dogAudio.Play();
                break;
            case AudioSources.Player:
                dogAudio.Play();
                break;
            case AudioSources.Tablet:
                dogAudio.Play();
                break;
        }

    }
}
