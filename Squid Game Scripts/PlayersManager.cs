using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager S;

    [SerializeField] private GameObject _shop;
    [SerializeField] private Image _leftClick;
    [SerializeField] private Image _rightClick;

    [Header("Info Players")]
    [SerializeField] private List<GameObject> _listOfPlayers;

    [Header("Skills")]
    public GameObject skillJump;
    public GameObject skillPush;

    [Header("Dinamic Info")]
    [SerializeField] private GameObject _activePlayer;
    public int _idActivePlayer;

    private Color _noActiveElement;
    private Color _activeElement;

    private PlayerItem _currentPlayerItem;
    private PlayerItem _currentPlayerChoose;

    private bool _cooldownPushActive;
    private bool _cooldownJumpActive;

    private float _timeCoolDownJump = 25.0f;
    private float _timeCoolDownPush = 15.0f;

    private void Awake()
    {
        S = this;

        _noActiveElement = new Color(1f, 1f, 1f, 0.5f);
        _activeElement = new Color(1f, 1f, 1f, 1f);
    }

    private void Start()
    {
        _cooldownJumpActive = false;
        _cooldownPushActive = false;

        LoadSavesOfBuyers();
        LoadSavesOfChoosers();

        //_idActivePlayer = 1;
        SpawnPlayerUI(_idActivePlayer);

        UpdateUI(_idActivePlayer);
    }

    //public void RunPlayersManager()
    //{
    //    _idActivePlayer = 1;
    //    SpawnPlayerUI(_idActivePlayer);

    //    LoadSavesOfBuyers();
    //    LoadSavesOfChoosers();

    //    UpdateUI(_idActivePlayer);
    //}

    //public void StopPlayersManager()
    //{
    //    Destroy(_activePlayer);
    //    Debug.Log("close Panel Shop Players");
    //}

    private void LoadSavesOfBuyers()
    {
        for (int i = 0; i < _listOfPlayers.Count; i++)
        {
            PlayerItem _tempPlayerItem = _listOfPlayers[i].GetComponent<PlayerItem>();

            if (PlayerPrefs.HasKey("BuyItem" + (i + 1).ToString()))
            {
                if (PlayerPrefs.GetInt("BuyItem" + (i + 1).ToString()) == 1)
                {
                    _tempPlayerItem.buy = true;
                }
                else
                {
                    _tempPlayerItem.buy = false;
                }
            }
            else
            {
                if (i == 0)
                {
                    PlayerPrefs.SetInt("BuyItem1", 1);
                }
                else
                {
                    PlayerPrefs.SetInt("BuyItem" + (i + 1).ToString(), 0);
                }
            }
        }
    }

    private void LoadSavesOfChoosers()
    {
        for (int i = 0; i < _listOfPlayers.Count; i++)
        {
            PlayerItem _tempPlayerItem = _listOfPlayers[i].GetComponent<PlayerItem>();

            if (PlayerPrefs.HasKey("ChooseItem" + (i + 1).ToString()))
            {
                if (PlayerPrefs.GetInt("ChooseItem" + (i + 1).ToString()) == 1)
                {
                    _tempPlayerItem.choose = true;
                    _idActivePlayer = i + 1;
                    _currentPlayerChoose = _tempPlayerItem;
                }
                else
                {
                    _tempPlayerItem.choose = false;
                }
            }
            else
            {
                if (i == 0)
                {
                    PlayerPrefs.SetInt("ChooseItem1", 1);
                    _idActivePlayer = 1;
                    _currentPlayerChoose = _tempPlayerItem;
                }
                else
                {
                    PlayerPrefs.SetInt("ChooseItem" + (i + 1).ToString(), 0);
                }
            }
        }
    }

    private void SpawnPlayerUI(int idPlayer)
    {
        _activePlayer = Instantiate(_listOfPlayers[idPlayer - 1], _shop.transform);
        _activePlayer.transform.SetParent(_shop.transform);
        _currentPlayerItem = _activePlayer.GetComponent<PlayerItem>();
        _idActivePlayer = _currentPlayerItem.idPlayer;
    }

    public void UpdateUI(int idPlayer)
    {
        PlayerItem tempPlayerItem = _currentPlayerItem.GetComponent<PlayerItem>();

        if (PlayerPrefs.GetInt("BuyItem" + idPlayer.ToString()) == 1)
        {
            _currentPlayerItem.buy = true;
            _currentPlayerItem.btnBuy.SetActive(false);
            _currentPlayerItem.btnChoose.SetActive(true);
        }
        else if (CoreGame.S.Money >= _currentPlayerItem.price)
        {
            tempPlayerItem.btnBuy.GetComponent<Image>().color = _activeElement;
            tempPlayerItem.btnName.GetComponent<Text>().color = _activeElement;
            tempPlayerItem.iconMoney.GetComponent<Image>().color = _activeElement;
            tempPlayerItem.txtPrice.GetComponent<Text>().color = _activeElement;
        }
        else
        {
            tempPlayerItem.btnBuy.GetComponent<Image>().color = _noActiveElement;
            tempPlayerItem.btnName.GetComponent<Text>().color = _noActiveElement;
            tempPlayerItem.iconMoney.GetComponent<Image>().color = _noActiveElement;
            tempPlayerItem.txtPrice.GetComponent<Text>().color = _noActiveElement;
        }

        if (PlayerPrefs.GetInt("ChooseItem" + idPlayer.ToString()) == 1)
        {
            _currentPlayerItem.btnChoose.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Lang") == 1)
        {
            tempPlayerItem.nameBuyBtn.text = "Купить";
            tempPlayerItem.nameChooseBtn.text = "Выбрать";
            tempPlayerItem.nameSkill1.text = "Прыжок";
            tempPlayerItem.nameSkill2.text = "Толкнуть";
        }
        else
        {
            tempPlayerItem.nameBuyBtn.text = "Buy";
            tempPlayerItem.nameChooseBtn.text = "Choose";
            tempPlayerItem.nameSkill1.text = "Jump";
            tempPlayerItem.nameSkill2.text = "Push";
        }
    }

    public void NextPlayer()
    {
        if (_idActivePlayer < _listOfPlayers.Count)
        {
            Destroy(_activePlayer);

            AudioBox.S.AudioPlayButtonUI();

            _idActivePlayer++;
            SpawnPlayerUI(_idActivePlayer);

            if (_idActivePlayer == _listOfPlayers.Count)
            {
                _rightClick.color = _noActiveElement;
            }
            else if (_idActivePlayer == 2)
            {
                _leftClick.color = _activeElement;
            }

            UpdateUI(_idActivePlayer);
        }
    }

    public void PreviousPlayer()
    {
        if (_idActivePlayer > 1)
        {
            Destroy(_activePlayer);

            AudioBox.S.AudioPlayButtonUI();

            _idActivePlayer--;
            SpawnPlayerUI(_idActivePlayer);

            if (_idActivePlayer == 1)
            {
                _leftClick.color = _noActiveElement;
            }
            else if (_idActivePlayer == _listOfPlayers.Count - 1)
            {
                _rightClick.color = _activeElement;
            }

            UpdateUI(_idActivePlayer);
        }
    }

    public void BuyPlayer(int idPlayer)
    {
        if (CoreGame.S.Money >= _currentPlayerItem.price)
        {
            AudioBox.S.AudioPlayButtonUI();

            CoreGame.S.Money -= _currentPlayerItem.price;
            _currentPlayerItem.buy = true;
            _currentPlayerItem.btnBuy.SetActive(false);
            _currentPlayerItem.btnChoose.SetActive(true);

            PlayerPrefs.SetInt("BuyItem" + idPlayer.ToString(), 1);
        }
    }

    public void ChoosePlayer(int idPlayer)
    {
        AudioBox.S.AudioPlayButtonUI();

        for (int i = 0; i < _listOfPlayers.Count; i++)
        {
            if (idPlayer == i + 1)
            {
                _currentPlayerChoose = _currentPlayerItem;
                PlayerPrefs.SetInt("ChooseItem" + (i + 1).ToString(), 1);
                _currentPlayerItem.choose = true;
                LoadMaterialForNewHero(idPlayer, false);
            }
            else
            {
                PlayerPrefs.SetInt("ChooseItem" + (i + 1).ToString(), 0);
                _currentPlayerItem.choose = false;
            }
        }

        UpdateUI(idPlayer);
    }

    public void LoadMaterialForNewHero(int idPlayer, bool dead)
    {
        if (idPlayer > 0)
        {
            if (!dead)
                HeroMove.S.materialHero.material = _listOfPlayers[idPlayer - 1].GetComponent<PlayerItem>().materialPlayerAlive;
            else
                HeroMove.S.materialHero.material = _listOfPlayers[idPlayer - 1].GetComponent<PlayerItem>().materialPlayerDead;
        }
        else
        {
            if (!dead)
                HeroMove.S.materialHero.material = _currentPlayerChoose.materialPlayerAlive;
            else
                HeroMove.S.materialHero.material = _currentPlayerChoose.materialPlayerDead; 
        }

        HeroMove.S.NumberHero = _currentPlayerChoose.namePlayer;
        HeroMove.S.speed = 2 * _currentPlayerChoose.speed;
        CoreGame.S.ChangeSpeedAI();

        if (_currentPlayerChoose.skillJump)
        {
            //skillJump.GetComponent<Image>().color = _noActiveElement;
            skillJump.SetActive(true);
        }    
        else
        {
            skillJump.SetActive(false);
        }

        if (_currentPlayerChoose.skillPush)
        {
            //skillPush.GetComponent<Image>().color = _noActiveElement;
            skillPush.SetActive(true);
        }   
        else
        {
            skillPush.SetActive(false);
        }   
    }

    public void SkillJumpOn()
    {
        if (_currentPlayerChoose.skillJump && !_cooldownJumpActive && CoreGame.S.gameMode == CoreGame.GameMode.Run)
        {
            StartCoroutine(JumpCoolDownByTime());
            HeroMove.S.StartCoroutine(HeroMove.S.Jump());
        }
    }

    public void SkillPushOn()
    {
        if (_currentPlayerChoose.skillPush && !_cooldownPushActive && CoreGame.S.gameMode == CoreGame.GameMode.Run)
        {
            StartCoroutine(PushCoolDownByTime());
            HeroMove.S.StartCoroutine(HeroMove.S.Push());
        }
    }

    public IEnumerator JumpCoolDownByTime()
    {
        _cooldownJumpActive = true;
        skillJump.GetComponent<Image>().fillAmount = 0.2f;
        skillJump.GetComponent<Outline>().enabled = false;

        StartCoroutine(TimerForJump());

        yield return new WaitForSeconds(_timeCoolDownJump);

        _cooldownJumpActive = false;
        skillJump.GetComponent<Image>().fillAmount = 1f;
        skillJump.GetComponent<Outline>().enabled = true;
    }

    private IEnumerator TimerForJump()
    {
        Image tempImageJump = skillJump.GetComponent<Image>();
        float imageSkillJump = tempImageJump.fillAmount;
        float stepFilling = 0.8f / _timeCoolDownJump;

        while (tempImageJump.fillAmount < 1f)
        {
            tempImageJump.fillAmount += stepFilling;
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator PushCoolDownByTime()
    {
        _cooldownPushActive = true;
        skillPush.GetComponent<Image>().fillAmount = 0.2f;
        skillPush.GetComponent<Outline>().enabled = false;

        StartCoroutine(TimerForPush());

        yield return new WaitForSeconds(_timeCoolDownPush);

        _cooldownPushActive = false;
        skillPush.GetComponent<Image>().fillAmount = 1f;
        skillPush.GetComponent<Outline>().enabled = true;
    }

    private IEnumerator TimerForPush()
    {
        Image tempImagePush = skillPush.GetComponent<Image>();
        float imageSkillPush = tempImagePush.fillAmount;
        float stepFilling = 0.8f / _timeCoolDownPush;

        while (tempImagePush.fillAmount < 1f)
        {
            tempImagePush.fillAmount += stepFilling;
            Debug.Log(_timeCoolDownPush);
            yield return new WaitForSeconds(1f);
        }
    }

    public void ResetCoolDowns()
    {
        StopCoroutine(JumpCoolDownByTime());
        StopCoroutine(TimerForJump());
        _cooldownJumpActive = false;
        skillJump.GetComponent<Image>().fillAmount = 0.2f;
        skillJump.GetComponent<Outline>().enabled = false;

        StopCoroutine(PushCoolDownByTime());
        StopCoroutine(TimerForPush());
        _cooldownPushActive = false;
        skillPush.GetComponent<Image>().fillAmount = 0.2f;
        skillPush.GetComponent<Outline>().enabled = false;
    }

}
