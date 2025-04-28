using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class Music : MonoBehaviour
{
    public AudioSource audioSource { get; private set; }
    public AudioClip music;
    public AudioClip hurryWarning;
    public AudioClip hurryMusic;
    public AudioClip introMusic;

    public AudioClip backgroundMusic;
    public AudioClip hurryBackgroundMusic;
    public AudioClip subAreaMusic;
    public AudioClip hurrySubAreaMusic;
    public AudioClip starPowerMusic;

    public bool subArea = false;
    public bool noBackgroundMusic;

    private AudioClip defaultMusic;
    private AudioClip overrideMusic = null;

    private bool isStopped = false;
    public bool hurryWarningPlaying { get; private set; } = false;

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

    public void PickMusic(AudioClip clip = null)
    {
        print(noBackgroundMusic);
        if (!noBackgroundMusic)
        {
            if (GameObject.FindWithTag("Player").GetComponent<Player>().starpower)
            {
                audioSource.clip = starPowerMusic;
            }
            else if (subArea)
            {
                if (GetComponent<Timer>().hurry)
                {
                    audioSource.clip = hurrySubAreaMusic;
                }
                else
                {
                    audioSource.clip = subAreaMusic;
                }
            }
            else if (GetComponent<Timer>().hurry)
            {
                audioSource.clip = hurryBackgroundMusic;
            }
            else
            {
                audioSource.clip = backgroundMusic;
            }
            audioSource.Play();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
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

        if (audioSource.clip != hurryWarning)
        {
            audioSource.loop = true;
            audioSource.Play();
            isStopped = false;
        }

        if (duration > 0)
        {
            Invoke(nameof(SwitchMusicBack), duration);
        } else
        {
            defaultMusic = music;
            this.music = music;
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
    }

    public IEnumerator ActivateHurryMusic()
    {
        // This is done to not override the star power music
        audioSource.Stop();
        noBackgroundMusic = true;
        audioSource.clip = hurryWarning;
        audioSource.loop = false;
        hurryWarningPlaying = true;
        audioSource.Play();

        print("setting noBackgroundMusic to true");

        //yield return new WaitForSeconds(hurryWarning.length);
        Timer timer = GetComponent<Timer>();
        while (timer.time >= timer.hurryTime - hurryWarning.length)
        {
            yield return null;
        }

        print("setting noBackgroundMusic to false");
        hurryWarningPlaying = false;
        noBackgroundMusic = false;
        audioSource.loop = true;
        PickMusic();
    }
}
