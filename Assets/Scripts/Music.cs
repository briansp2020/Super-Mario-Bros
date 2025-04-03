using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }
    public AudioClip music;
    public AudioClip hurryWarning;
    public AudioClip hurryMusic;
    public AudioClip introMusic;

    private AudioClip defaultMusic;
    private AudioClip overrideMusic = null;

    private bool isStopped = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        defaultMusic = music;
    }
    private void Start()
    {
        audioSource.clip = music;
        audioSource.Play();
    }

    private void Update()
    {
        defaultMusic = Camera.main.GetComponent<Timer>().hurry ? hurryMusic : music;
    }

    public void PlayMusic(AudioClip music, float duration = -1, AudioClip hurryMusic = null)
    {
        if (overrideMusic)
        {
            audioSource.clip = overrideMusic;
        }
        else if (hurryMusic != null && GetComponent<Timer>().hurry)
        {
            audioSource.clip = hurryMusic;
        }
        else
        {
            audioSource.clip = music;
        }


        audioSource.loop = true;
        audioSource.Play();
        isStopped = false;

        if (duration > 0)
        {
            Invoke(nameof(SwitchMusicBack), duration);
        } else
        {
            defaultMusic = music;
        }
    }

    public void PlayOverrideMusic(AudioClip music, float duration)
    {
        overrideMusic = music;
        PlayMusic(music, duration);
    }

    private void SwitchMusicBack()
    {
        if (!isStopped)
        {
            overrideMusic = null;
            PlayMusic(defaultMusic, -1);
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        audioSource.loop = false;
        isStopped = true;
    }

    public IEnumerator ActivateHurryMusic()
    {
        // This is done to not override the star power music
        audioSource.clip = hurryWarning;
        audioSource.Play();
        
        yield return new WaitForSeconds(hurryWarning.length);

        PlayMusic(hurryMusic);
    }
}
