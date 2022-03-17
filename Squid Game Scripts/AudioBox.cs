using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBox : MonoBehaviour
{
    public static AudioBox S;

    [Header("Sources")]
    [SerializeField] private AudioSource _audioSourceBG;
    [SerializeField] private AudioSource _audioSourceSiren;
    [SerializeField] private AudioSource _audioSourceHero;
    [SerializeField] private AudioSource _audioSourceUIPanels;
    [SerializeField] private AudioSource _audioSourceUIButtons;

    [Header("Clips")]
    [SerializeField] private AudioClip _musicBG;
    [SerializeField] private AudioClip _voiceSIREN_1;
    [SerializeField] private AudioClip _voiceSIREN_2;
    [SerializeField] private AudioClip _heroSteps;
    [SerializeField] private AudioClip _uiWin;
    [SerializeField] private AudioClip _uiLose;
    [SerializeField] private AudioClip _uiClick;


    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        _audioSourceBG.clip = _musicBG;
        _audioSourceHero.clip = _heroSteps;
        _audioSourceUIButtons.clip = _uiClick;
        AudioPlayBG();
    }

    //###########################-BG-#######################################
    public void AudioPlayBG()
    {
        StartCoroutine(CoroutineAudioPlayBG());
    }

    private IEnumerator CoroutineAudioPlayBG()
    {
        yield return null;
        if (PlayerPrefs.GetInt("Music") == 1)
            _audioSourceBG.Play();
    }

    public void AudioStopBG()
    {
        _audioSourceBG.Pause();
    }

    //###########################-HERO-#######################################
    public void AudioPlayHero()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
            _audioSourceHero.Play();
    }

    public void AudioStopHero()
    {
        StartCoroutine(CoroutineAudioStopHero());
    }

    private IEnumerator CoroutineAudioStopHero()
    {
        yield return null;
        _audioSourceHero.Stop();
    }

    //###########################-Panels UI-#######################################
    public void AudioPlayPanelsUI(int idPanel)
    {
        //1 - dead, 2 - finish
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            if (idPanel == 1)
            {
                _audioSourceUIPanels.clip = _uiLose;
                _audioSourceUIPanels.Play();
            }
            else if (idPanel == 2)
            {
                _audioSourceUIPanels.clip = _uiWin;
                _audioSourceUIPanels.Play();
            }
            else
            {
                Debug.Log("Not exict id Planel! Check it!");
            }
        }
    }

    public void AudioStopPanelsUI()
    {
        _audioSourceUIPanels.Stop();
    }

    //###########################-Buttons UI-#######################################
    public void AudioPlayButtonUI()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            _audioSourceUIButtons.Stop();
            _audioSourceUIButtons.Play();
        }
    }

    //###########################-SIREN-#######################################
    public void AudioPlaySiren(int type, bool justPlay)
    {
        StartCoroutine(CoroutineAudioPlaySiren(type, justPlay));
    }

    private IEnumerator CoroutineAudioPlaySiren(int type, bool justPlay)
    {
        yield return null;
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            if (justPlay)
            {
                _audioSourceSiren.Play();
            }
            else
            {
                switch (type)
                {
                    case 1:
                        _audioSourceSiren.clip = _voiceSIREN_1;
                        break;

                    case 2:
                        _audioSourceSiren.clip = _voiceSIREN_2;
                        break;
                }

                _audioSourceSiren.Play();
            }
        }
    }

    public void AudioStopSiren()
    {
        _audioSourceSiren.Stop();
    }

    public void AudioPitchSiren(bool first)
    {
        if (!first)
        {
            if (_audioSourceSiren.pitch < 1.4f)
                _audioSourceSiren.pitch += .05f;
        }
        else
        {
            _audioSourceSiren.pitch = 1;
        }
    }

    public void AudioPauseSiren()
    {
        StartCoroutine(CoroutineAudioPauseSiren());
    }

    private IEnumerator CoroutineAudioPauseSiren()
    {
        yield return null;
        _audioSourceSiren.Pause();
    }

}
