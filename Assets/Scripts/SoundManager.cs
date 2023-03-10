using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("SFXs")]
    public AudioClip goalSound;
    public AudioClip slotSound;
    public AudioClip swooshSound;
    public AudioClip buyProp;
    public AudioClip buyError;

    [Space]
    [Header("BG MUSICS")]
    public AudioClip bgMusic;
    public AudioClip bgFeverMusic;

    void Awake()
    {
        instance = this;    
    }

    public void PlaySound(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source, 4);
    }

    public void PlaySlotMachineSound()
    {
        transform.GetChild(1).GetComponent<AudioSource>().Play();
    }

    public void StopSlotMachineSound()
    {
        transform.GetChild(1).GetComponent<AudioSource>().Stop();
    }

    public void ChangeBG(AudioClip clip)
    {
        transform.GetChild(0).GetComponent<AudioSource>().clip = clip;
        transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
}
