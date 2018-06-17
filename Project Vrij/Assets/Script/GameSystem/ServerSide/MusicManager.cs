using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public float volume;
    public AudioClip[] songs;
    public AudioMixer mixer;
    private AudioSource source;
    private int currIndex = 0;
    private bool active = false;
    private bool started = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }
    
    public void StartPlaying()
    {
        if (!started)
        {
            started = true;
            source.PlayOneShot(songs[currIndex]);
            if (currIndex < songs.Length-1)
            {
                currIndex++;
            }
            else
            {
                currIndex = 0;
            }
            active = true;
        }
    }

    public void PausePlay(bool play)
    {
        if (started)
        {
            if (!play)
            {
                //StopAllCoroutines();
                active = false;
                //StartCoroutine(FadeOut());
                source.Pause();
            }
            else
            {
                //StopAllCoroutines();
                active = true;
                //StartCoroutine(FadeIn());
                source.Play();
            }
        }
    }

    void Update()
    {
        if(active)
        {
            if(!source.isPlaying)
            {
                source.PlayOneShot(songs[currIndex]);
                if (currIndex < songs.Length-1)
                {
                    currIndex++;
                }
                else
                {
                    currIndex = 0;
                }
            }
        }
    }

    private IEnumerator FadeIn()
    {
        source.Play();
        float currTime = Time.time;
        float vol = 0f;
        while (vol != volume)
        {
            vol = Mathf.SmoothStep(-80, volume, (Time.time - currTime));
            mixer.SetFloat("AmbienceVolume", vol);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float currTime = Time.time;
        float vol = 0f;
        while (vol != volume)
        {
            vol = Mathf.SmoothStep(volume, -80f, (Time.time - currTime));
            mixer.SetFloat("AmbienceVolume", vol);
            yield return null;
        }
        source.Pause();
    }
}
