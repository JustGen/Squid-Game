using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer S;

    [SerializeField] private Text _txtMinutes;
    [SerializeField] private Text _txtSeconds;
    [SerializeField] private Text _txtTimeIsOver;
    [SerializeField] private float _timeRound;
    [SerializeField] private GameObject _redPlane;
    [SerializeField] private float _timeDeadZone;
    [SerializeField] private float _timeRun;
    [SerializeField] private float _timeReactionPlayer;
    public bool deadZone;

    //private Image _imageBoard;
    private float _currTime;
    private float _minutes;
    private float _seconds;
    private int _round;

    private float _timeOffset;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        //_imageBoard = _timerBoard.GetComponent<Image>();
        RedPlaneActivator(false);
        deadZone = false;
        CoreGame.S.timerMode = CoreGame.TimerMode.GreenZone;
        TimerBuild();
        _round = 1;
    }

    IEnumerator TimerTiker()
    {
        while (CoreGame.S.gameMode != CoreGame.GameMode.Finish)
        {
            yield return new WaitForSeconds(_timeRun - _timeOffset);
            RedZoneActive(true);

            yield return new WaitForSeconds(_timeReactionPlayer);
            if (CoreGame.S.gameMode == CoreGame.GameMode.Run)
            {
                CoreGame.S.gameMode = CoreGame.GameMode.Dead;
            }

            yield return new WaitForSeconds(_timeDeadZone - _timeOffset);
            RedZoneActive(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(TimerTiker());
        StartCoroutine(TimerRun());
        //CoreGame.S.gameMode = CoreGame.GameMode.Run;
        CoreGame.S.timerMode = CoreGame.TimerMode.GreenZone;
        AudioBox.S.AudioPlaySiren(1, false);
        AudioBox.S.AudioPitchSiren(true);
    }

    public void RedPlaneActivator(bool status)
    {
        _redPlane.SetActive(status);
        _redPlane.SetActive(status);
    }

    private void RedZoneActive(bool status)
    {
        RedPlaneActivator(status);
        
        deadZone = status;
        if (deadZone)
        {
            CoreGame.S.timerMode = CoreGame.TimerMode.RedZone;
            AudioBox.S.AudioPlaySiren(2, false);
        }
        else
        {
            CoreGame.S.timerMode = CoreGame.TimerMode.GreenZone;
            AudioBox.S.AudioPlaySiren(1, false);
            AudioBox.S.AudioPitchSiren(false);

            if (_timeOffset <= 1.6f)
                _timeOffset += 0.2f;
        }

        _round++;
    }

    public void TimerBuild()
    {
        Minutes = (int)(_timeRound / 60);
        Seconds = _timeRound - Minutes * 60;
        _currTime = _timeRound;
        _txtTimeIsOver.text = ":";

        _timeOffset = 0;
    }

    private float Seconds
    {
        get
        {
            return _seconds;
        }

        set
        {
            _seconds = (int)value;

            if (_seconds < 10)
            {
                _txtSeconds.text = "0" + _seconds.ToString();
            }
            else
            {
                _txtSeconds.text = _seconds.ToString();
            }
        }
    }

    private float Minutes
    {
        get
        {
            return _minutes;
        }

        set
        {
            _minutes = value;

            if (_minutes < 10)
            {
                _txtMinutes.text = "0" + _minutes.ToString();
            }
            else
            {
                _txtMinutes.text = _minutes.ToString();
            }
        }
    }

    IEnumerator TimerRun()
    {
        while (_currTime >= 0)
        {
            _currTime -= Time.deltaTime;
            Minutes = (int)(_currTime / 60);
            Seconds = _currTime - Minutes * 60;

            yield return null;
        }

        if (_currTime <= 0)
        {
            TimeEnd();
        }
    }

    public void TimeEnd()
    {
        if (PlayerPrefs.GetInt("Lang") == 1)
            _txtTimeIsOver.text = "ÂÐÅÌß ÂÛØËÎ!";
        else
            _txtTimeIsOver.text = "Time is over!";

        _txtMinutes.text = "";
        _txtSeconds.text = "";
        StopCoroutine(TimerRun());
        CoreGame.S.timerMode = CoreGame.TimerMode.TimeOver;
        HeroMove.S.DeadHero();
    }
}
