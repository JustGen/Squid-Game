using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager S;

    [Header("Panels")]
    [SerializeField] private GameObject _gamePanel;
    [HideInInspector] public bool gamePanelStatus;

    [SerializeField] private GameObject _startPanel;
    [HideInInspector] public bool startPanelStatus;

    [SerializeField] private GameObject _deadPanel;
    [HideInInspector] public bool deadPanelStatus;

    [SerializeField] private GameObject _finishPanel;
    [HideInInspector] public bool finishPanelStatus;

    [SerializeField] private GameObject _endGamePanel;
    [HideInInspector] public bool endGamePanelStatus;

    [SerializeField] private GameObject _shopPanel;
    [HideInInspector] public bool shopPanelStatus;

    [SerializeField] private GameObject _settingsPanel;
    [HideInInspector] public bool settingsPanelStatus;

    [SerializeField] private GameObject _playersShopPanel;
    [HideInInspector] public bool playersShopPanelStatus;

    [Header("Other")]
    [SerializeField] private GameObject _txtTapToRun;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Text _txtCoinsGetAfterLevel;
    [SerializeField] private GameObject _coinsGetAfterLevelMenu;
    [SerializeField] private Text _txtCoinsGetAfterLevelMenu;

    private Animation _animationArrow;
    private Animation _animationUI;
    private int _multiply;
    private int _coinsGetAfterLevel;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        StartPanelActivator(true);
        GameStartPanelActivator(false);

        _animationArrow = _arrow.GetComponent<Animation>();
        _animationUI = _coinsGetAfterLevelMenu.GetComponent<Animation>();
    }

    public void StartPanelActivator(bool status)
    {
        _startPanel.SetActive(status);
        startPanelStatus = status;            
    }

    public void GameStartPanelActivator(bool status)
    {
        _gamePanel.SetActive(status);
        gamePanelStatus = status;
    }

    public void TxtTapToRunActivator(bool status)
    {
        _txtTapToRun.SetActive(status);
    }

    public void DeadPanelActivator(bool status)
    {
        _deadPanel.SetActive(status);
        deadPanelStatus = status;

        if (status)
            AudioBox.S.AudioPlayPanelsUI(1);
    }

    public void FinishPanelActivator(bool status)
    {
        _finishPanel.SetActive(status);
        finishPanelStatus = status;

        if (status)
        {
            _animationArrow.Play();
            //AudioBox.S.AudioPlayBG();
        }  
        else
        {
            _animationArrow.Stop();
        }    
    }

    public void EndGamePanelActivator(bool status)
    {
        _endGamePanel.SetActive(status);
        endGamePanelStatus = status;

        if (status)
            CoreGame.S.Money += 2000;
    }

    public void ShopPanelActivator(bool status)
    {
        //AudioBox.S.AudioPlayButtonUI();

        _shopPanel.SetActive(status);
        shopPanelStatus = status;

        _startPanel.SetActive(!status);
        //GameStartPanelActivator(!status);
    }

    public void SettingsPanelActivator(bool status)
    {
        //AudioBox.S.AudioPlayButtonUI();

        _settingsPanel.SetActive(status);
        settingsPanelStatus = status;

        _startPanel.SetActive(!status);
        //GameStartPanelActivator(!status);
    }

    public void PlayersShopPanelActivator(bool status)
    {
        //AudioBox.S.AudioPlayButtonUI();

        _playersShopPanel.SetActive(status);
        playersShopPanelStatus = status;

        //PlayersManager.S.RunPlayersManager();
        _startPanel.SetActive(!status);
        //GameStartPanelActivator(!status);
    }

    public void BackToMenu(int idOpenPanel)
    {
        if (idOpenPanel == 1)
        {
            ShopPanelActivator(false);
        }
        else if (idOpenPanel == 2)
        {
            SettingsPanelActivator(false);
        }
        else if (idOpenPanel == 3)
        {
            PlayersShopPanelActivator(false);
            //PlayersManager.S.StopPlayersManager();
        }
        else
        {
            Debug.Log("No Id Panels, or incorrect codding");
        }

        //AudioBox.S.AudioPlayButtonUI();
    }

    public void TxtCoinsGetAfterLevelMenu(bool status)
    {
        _coinsGetAfterLevelMenu.SetActive(status);

        if (status)
        {
            StartCoroutine(AnimationGetReward());
        }
    }

    IEnumerator AnimationGetReward()
    {
        _animationUI.Play();
        yield return new WaitForSeconds(1.25f);

        _animationUI.Stop();
        StartCoroutine(AddingCoins());
        TxtCoinsGetAfterLevelMenu(false);
    }

    IEnumerator AddingCoins()
    {
        int coinsLeft = CoinsGetAfterLevel;
        while (coinsLeft > 0)
        {
            CoreGame.S.Money += 4;
            coinsLeft -= 4;
            yield return new WaitForSeconds(0.01f);
        }

        CoinsGetAfterLevel = 60;
    }

    public void StartGame()
    {
        //AudioBox.S.AudioPlayButtonUI();

        StartPanelActivator(false);
        GameStartPanelActivator(true);
        TxtTapToRunActivator(true);
        
        AudioBox.S.AudioStopBG();
    }

    public void Multiply()
    {
        AudioBox.S.AudioStopPanelsUI();
        StartCoroutine(MultiplyCorotine());
    }

    IEnumerator MultiplyCorotine()
    {
        CoinsGetAfterLevel = 60;
        CoreGame.S.Money += 60;
        _animationArrow.Stop();
        _multiply = 1;

        float arrowRotZ = _arrow.transform.eulerAngles.z;
        arrowRotZ = Mathf.Repeat(arrowRotZ + 180, 360) - 180;


        if (Mathf.Abs(arrowRotZ) <= 11.3f && Mathf.Abs(arrowRotZ) >= 0.0f)
        {
            _multiply = 5;
        }
        else if (Mathf.Abs(arrowRotZ) > 11.3f && Mathf.Abs(arrowRotZ) <= 42.0f)
        {
            _multiply = 3;
        }
        else if (Mathf.Abs(arrowRotZ) > 42.0f && Mathf.Abs(arrowRotZ) <= 90.0f)
        {
            _multiply = 2;
        }

        yield return new WaitForSeconds(0.1f);
        
        CoreGame.S.NextLevel();
        GoogleADMob.S.ShowRewardedVideo(1);
    }

    public void GetReward()
    {
        CoinsGetAfterLevel *= _multiply;
        CoreGame.S.Money -= 120;
        TxtCoinsGetAfterLevelMenu(true);
    }

    private int CoinsGetAfterLevel
    {
        get
        {
            return _coinsGetAfterLevel;
        }

        set
        {
            _coinsGetAfterLevel = value;
            _txtCoinsGetAfterLevel.text = value.ToString();
            _txtCoinsGetAfterLevelMenu.text = value.ToString();
        }
    }
}
