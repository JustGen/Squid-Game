using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoreGame : MonoBehaviour
{
    public static CoreGame S;

    [SerializeField] private Text _txtMoney;
    [SerializeField] private Text _txtBalanceInPlayersShop;
    [SerializeField] private Text _txtCountRespawn;

    [SerializeField] private Text _txt_AlivePlayersInMenu;
    [SerializeField] private Text txt_Level;
    [SerializeField] private int[] alives;

    [Header("Generation Level")]
    [SerializeField] private GameObject _HERO;
    [SerializeField] private Transform _posHero;
    [SerializeField] private GameObject _parentAI;
    [SerializeField] private GameObject _aiHero;
    [SerializeField] private List<Transform> _spawnPoints;

    [HideInInspector] public bool firstRun;
    [HideInInspector] public bool adClose;

    private int _countAI;
    private int _aliveAI;
    private int _level;
    private int _money;
    private int _countRespawn;


    private List<GameObject> _aiModels;
    private GameObject _hero;

    public enum GameMode
    {
        idle,
        Run,
        Finish,
        Dead
    }

    public enum TimerMode
    {
        idle,
        RedZone,
        GreenZone,
        TimeOver
    }

    public GameMode gameMode;
    public TimerMode timerMode;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            Money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            PlayerPrefs.SetInt("Money", 0);
            Money = 0;
        }

        if (PlayerPrefs.HasKey("Level"))
        {
            Level = PlayerPrefs.GetInt("Level");
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            Level = 1;
        }

        adClose = true;

        ResetGameElements();
        GenerationLevel();
    }

    private void Awake()
    {
        S = this;
    }

    public void NextLevel()
    {
        if (Level != 13)
        {
            Level++;
            AliveAI = alives[Level - 1];

            Money += 60;
            ClearLevel();
            ResetGameElements();
            GenerationLevel();
        }
        else
        {
            Level = 1;
            AliveAI = alives[Level - 1];

            ClearLevel();
            ResetGameElements();
            GenerationLevel();
        }
    }

    public void RestartGame()
    {
        if (Level != 13)
        {
            Level = 1;
            AliveAI = alives[Level - 1];

            ClearLevel();
            ResetGameElements();
            GenerationLevel();
        }
    }

    public void Respawn()
    {
        if (CountRespawn != 0)
        {
            CountRespawn--;
            HeroMove.S.Respawn();
            MenuManager.S.DeadPanelActivator(false);
            MenuManager.S.GameStartPanelActivator(true);
        }
    }

    private void GenerationLevel()
    {
        _hero = Instantiate(_HERO, _posHero);

        _aiModels = new List<GameObject>();
        for (int i = 0; i < _countAI; i++)
        {
            GameObject goAI = Instantiate(_aiHero, _spawnPoints[i]);
            _aiModels.Add(goAI);
            goAI.transform.SetParent(_parentAI.transform);
        }

        LevelEnvRandom.S.ChangeMaterialsInScene();
    }

    private void ClearLevel()
    {
        Destroy(_hero);

        for (int i = 0; i < _countAI; i++)
        {
            Destroy(_aiModels[i]);
        }

        _aiModels.Clear();
    }

    private void ResetGameElements()
    {
        firstRun = true;

        gameMode = GameMode.idle;
        timerMode = TimerMode.idle;
        MenuManager.S.StartPanelActivator(true);
        MenuManager.S.DeadPanelActivator(false);
        MenuManager.S.FinishPanelActivator(false);
        MenuManager.S.EndGamePanelActivator(false);
        MenuManager.S.GameStartPanelActivator(false);

        Timer.S.TimerBuild();
        AudioBox.S.AudioStopSiren();

        Timer.S.StopAllCoroutines();
        Timer.S.RedPlaneActivator(false);
        PlayersManager.S.ResetCoolDowns();

        AliveAI = alives[Level - 1];

        if (alives[Level - 1] > _spawnPoints.Count)
        {
            _countAI = _spawnPoints.Count;
        }
        else
        {
            _countAI = alives[Level - 1];
        }

        CountRespawn = 3;
    }

    public void StopGame(bool pause)
    {
        if (pause)
        {
            adClose = false;
            AudioBox.S.AudioStopHero();
            AudioBox.S.AudioPauseSiren();
        }
        else
        {
            adClose = true;

            if (!firstRun)
                AudioBox.S.AudioPlaySiren(0, true);
        }
    }

    public void ChangeSpeedAI()
    {
        for (int i = 0; i < _countAI; i++)
        {
            AIMove tempMoveAI = _aiModels[i].GetComponent<AIMove>();
            tempMoveAI.speed = tempMoveAI.ChangeSpeed();
        }
    }

    public int AliveAI
    {
        get
        {
            return _aliveAI;
        }

        set
        {
            if (Level != 13)
            {
                _aliveAI = value;

                if (PlayerPrefs.GetInt("Lang") == 1)
                    _txt_AlivePlayersInMenu.text = "»√–Œ Œ¬ Œ—“¿ÀŒ—‹ " + alives[Level];
                else
                    _txt_AlivePlayersInMenu.text = "PLAYERS LEFT " + alives[Level];
            }
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }

        set
        {
            _level = value;
            PlayerPrefs.SetInt("Level", _level);

            if (PlayerPrefs.GetInt("Lang") == 1)
                txt_Level.text = "”–Œ¬≈Õ‹ " + value;
            else
                txt_Level.text = "LEVEL " + value;
        }
    }

    public int Money
    {
        get
        {
            return _money;
        }

        set
        {
            _money = value;
            _txtMoney.text = _money.ToString();
            _txtBalanceInPlayersShop.text = _money.ToString();
            PlayerPrefs.SetInt("Money", _money);
        }
    }

    public int CountRespawn
    {
        get
        {
            return _countRespawn;
        }

        set
        {
            _countRespawn = value;

            if (PlayerPrefs.GetInt("Lang") == 1)
                _txtCountRespawn.text = "ŒÒÚ‡ÎÓÒ¸ ÔÓÔ˚ÚÓÍ " + value.ToString() + "/ 3";
            else
                _txtCountRespawn.text = "Attempts left " + value.ToString() + "/ 3";
        }
    }
}
