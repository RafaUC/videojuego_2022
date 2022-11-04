using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager obj;

    public AudioClip saltar;
    public AudioClip dano;
    public AudioClip muerte;

    private AudioSource audioSrc;

    private void Awake(){
        obj = this;
        audioSrc = gameObject.AddComponent<AudioSource>();
    }

    public void playSaltar(){playSound(saltar);}

    public void playHurt(){playSound(dano);}

    public void playMuerte(){playSound(muerte);}

    public void playSound(AudioClip clip){
        audioSrc.PlayOneShot(clip);
    }
}
