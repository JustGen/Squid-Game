using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager S;

    [SerializeField] private GameObject itemMusic;
    [SerializeField] private GameObject itemSound;
    [SerializeField] private AnimationClip _animationOn;
    [SerializeField] private AnimationClip _animationOff;
    [SerializeField] private Sprite _spriteOn;
    [SerializeField] private Sprite _spriteOff;
    [SerializeField] private Image _imageStatusRU;
    [SerializeField] private Image _imageStatusEN;


    private bool _firstRun;
    private int _statusMusic;
    private int _statusSound;
    private int _statusLang;

    private Animation _animationOfMusic;
    private Animation _animationOfSound;


    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        _firstRun = true;

        _animationOfMusic = itemMusic.GetComponent<Animation>();
        _animationOfSound = itemSound.GetComponent<Animation>();

        if (PlayerPrefs.HasKey("Music"))
        {
            _statusMusic = PlayerPrefs.GetInt("Music");
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        if (PlayerPrefs.HasKey("Sound"))
        {
            _statusSound = PlayerPrefs.GetInt("Sound");
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }

        MusicControl();
        SoundControl();
        

        if (PlayerPrefs.HasKey("Lang"))
        {
            _statusLang = PlayerPrefs.GetInt("Lang");
            ChangeLang(_statusLang);
        }
        else
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                ChangeLang(1);
            }
            else
            {
                ChangeLang(2);
            }      
        }

        _firstRun = false;
    }

    public void MusicControl()
    {
        if (StatusMusic == 1)
        {
            if (!_firstRun)
            {
                //AudioBox.S.AudioPlayButtonUI();
                StatusMusic = 0;
                _animationOfMusic.clip = _animationOff;
            }
            else
            {
                _animationOfMusic.clip = _animationOn;
            }

        } 
        else if (StatusMusic == 0)
        {
            if (!_firstRun)
            {
                //AudioBox.S.AudioPlayButtonUI();
                StatusMusic = 1;
                _animationOfMusic.clip = _animationOn;
            }
            else
            {
                _animationOfMusic.clip = _animationOff;
            }
        }

        _animationOfMusic.Play();
    }

    public void SoundControl()
    {
        if (StatusSound == 1)
        {
            if (!_firstRun)
            {
                //AudioBox.S.AudioPlayButtonUI();
                StatusSound = 0;
                _animationOfSound.clip = _animationOff;
            }
            else
            {
                _animationOfSound.clip = _animationOn;
            }
        }
        else if (StatusSound == 0)
        {
            if (!_firstRun)
            {
                //AudioBox.S.AudioPlayButtonUI();
                StatusSound = 1;
                _animationOfSound.clip = _animationOn;
            }
            else
            {
                _animationOfSound.clip = _animationOff;
            }
        }
        
        _animationOfSound.Play();
    }

    public void ChangeLang(int idLang)
    {
        if (idLang == 1)
        {
            _imageStatusRU.sprite = _spriteOn;
            _imageStatusEN.sprite = _spriteOff;
            PlayerPrefs.SetInt("Lang", idLang);
            LangManager.S.ChangeGlobalLang(idLang);
        }
        else if (idLang == 2)
        {
            _imageStatusEN.sprite = _spriteOn;
            _imageStatusRU.sprite = _spriteOff;
            PlayerPrefs.SetInt("Lang", idLang);
            LangManager.S.ChangeGlobalLang(idLang);
        }
        else
        {
            Debug.Log("Lang no found!");
        }

        //if (!_firstRun)
            //AudioBox.S.AudioPlayButtonUI();
    }

    private int StatusMusic
    {
        get
        {
            _statusMusic = PlayerPrefs.GetInt("Music");
            return _statusMusic;
        }

        set
        {
            _statusMusic = value;
            PlayerPrefs.SetInt("Music", value);

            if (_statusMusic == 0)
            {
                AudioBox.S.AudioStopBG();
            }
            else
            {
                AudioBox.S.AudioPlayBG();
            }
        }
    }

    private int StatusSound
    {
        get
        {
            _statusSound = PlayerPrefs.GetInt("Sound");
            return _statusSound;
        }

        set
        {
            _statusSound = value;
            PlayerPrefs.SetInt("Sound", value);
        }
    }
}
