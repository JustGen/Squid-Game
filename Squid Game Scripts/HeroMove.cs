using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroMove : MonoBehaviour
{
    public static HeroMove S;

    public int speed;

    [SerializeField] private GameObject number;
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _HeroForMaterial;
    [SerializeField] private Material _matHeroDead;
    [SerializeField] private Material _matHeroAlive;
    [SerializeField] private TextMeshPro _textMesh;

    [Header("Rotate Hero Component")]
    [SerializeField] private GameObject _heroBody;
    [SerializeField] private GameObject _heroRotStart;
    [SerializeField] private GameObject _heroRotLeft;
    [SerializeField] private GameObject _heroRotRight;
    private Transform _currRotateHeroBody;

    private int _numberHero;
    private Animator _animator;
    [HideInInspector] public SkinnedMeshRenderer materialHero;
    private Vector2 _touchPositionBegin;
    
    private float offsetX;
    private bool _wallLeft;
    private bool _wallRight;

    private void Awake()
    {
        S = this;

        materialHero = _HeroForMaterial.GetComponent<SkinnedMeshRenderer>();
    }

    private void Start()
    {
        NumberHero = 999;
        PlayersManager.S.LoadMaterialForNewHero(-1, false);

        _animator = _hero.GetComponent<Animator>();
        _wallLeft = false;
        _wallRight = false;      
    }

    private void Update()
    {
        if (Input.touchCount == 1 && CoreGame.S.adClose && MenuManager.S.gamePanelStatus && !EventSystem.current.IsPointerOverGameObject()) 
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (CoreGame.S.firstRun)
                    {
                        MenuManager.S.TxtTapToRunActivator(false);
                        Timer.S.StartGame();
                        CoreGame.S.firstRun = false;

                        PlayersManager.S.StartCoroutine(PlayersManager.S.JumpCoolDownByTime());
                        PlayersManager.S.StartCoroutine(PlayersManager.S.PushCoolDownByTime());
                    }

                    _touchPositionBegin = touch.position;
                    _currRotateHeroBody = _heroBody.transform;
                    offsetX = _touchPositionBegin.x * 0.1f;

                    if (CoreGame.S.gameMode == CoreGame.GameMode.idle)
                    {
                        CoreGame.S.gameMode = CoreGame.GameMode.Run;
                        _animator.SetBool("active", true);
                        AudioBox.S.AudioPlayHero();
                    }

                    if (CoreGame.S.timerMode == CoreGame.TimerMode.RedZone)
                    {
                        CoreGame.S.gameMode = CoreGame.GameMode.Run;
                        _animator.SetBool("active", true);
                        DeadHero();
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    TouchPhaseMove(touch);
                    break;

                case TouchPhase.Ended:
                    if (CoreGame.S.gameMode != CoreGame.GameMode.Dead)
                    {
                        _animator.SetBool("active", false);

                        if (CoreGame.S.gameMode != CoreGame.GameMode.Finish)
                            CoreGame.S.gameMode = CoreGame.GameMode.idle;
                    }

                    AudioBox.S.AudioStopHero();
                    break;
            }
        }
    }

    private void Move(Touch touch)
    {
        if (!_wallLeft && !_wallRight)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed);
            _heroBody.transform.rotation = Quaternion.Lerp(_currRotateHeroBody.rotation, _heroRotStart.transform.rotation, Time.deltaTime * speed);

            if (touch.position.x > _touchPositionBegin.x + offsetX)
            {
                transform.Translate(transform.right * Time.deltaTime * (speed * 0.75f));
                _heroBody.transform.rotation = Quaternion.Lerp(_currRotateHeroBody.rotation, _heroRotLeft.transform.rotation, Time.deltaTime * speed);
            }
            else if (touch.position.x < _touchPositionBegin.x - offsetX)
            {
                transform.Translate(-transform.right * Time.deltaTime * (speed * 0.75f));
                _heroBody.transform.rotation = Quaternion.Lerp(_currRotateHeroBody.rotation, _heroRotRight.transform.rotation, Time.deltaTime * speed);
            }
        }
        else if (_wallLeft)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed);

            if (touch.position.x > _touchPositionBegin.x + offsetX)
            {
                transform.Translate(transform.right * Time.deltaTime * (speed * 0.75f));
            }
        }
        else if (_wallRight)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed);

            if (touch.position.x < _touchPositionBegin.x - offsetX)
            {
                transform.Translate(-transform.right * Time.deltaTime * (speed * 0.75f));
            }
        }
    }

    public void TouchPhaseMove(Touch touch)
    {
        if (CoreGame.S.gameMode != CoreGame.GameMode.Dead)
        {
            if (CoreGame.S.gameMode != CoreGame.GameMode.Finish)
                Move(touch);
        }
        else
        {
            if (CoreGame.S.timerMode != CoreGame.TimerMode.TimeOver)
                DeadHero();
        }
    }

    public void DeadHero()
    {
        _animator.SetBool("active", false);
        _animator.SetBool("dead", true);
        AudioBox.S.AudioStopHero();
        PlayersManager.S.LoadMaterialForNewHero(-1, true);
        CoreGame.S.gameMode = CoreGame.GameMode.Dead;
        MenuManager.S.DeadPanelActivator(true);
        MenuManager.S.GameStartPanelActivator(false);
    }

    public IEnumerator Jump()
    {
        if (_animator.GetBool("active") == true)
        {
            _animator.SetBool("jump", true);

            yield return new WaitForSeconds(1f);

            _animator.SetBool("jump", false);
        }
    }

    public IEnumerator Push()
    {
        if (_animator.GetBool("active") == true)
        {
            _animator.SetBool("push", true);

            yield return new WaitForSeconds(1f);

            _animator.SetBool("push", false);
        }
    }

    public void Respawn()
    {
        _animator.SetBool("active", false);
        _animator.SetBool("dead", false);
        PlayersManager.S.LoadMaterialForNewHero(-1, false);
        CoreGame.S.gameMode = CoreGame.GameMode.idle; 
    }

    private void FinishGame()
    {
        AudioBox.S.AudioPlayPanelsUI(2);
        AudioBox.S.AudioStopHero();
        _animator.SetBool("active", false);
        CoreGame.S.gameMode = CoreGame.GameMode.Finish;
        CoreGame.S.timerMode = CoreGame.TimerMode.idle;
        Timer.S.StopAllCoroutines();
        Timer.S.RedPlaneActivator(false);

        if (CoreGame.S.Level != 13)
            MenuManager.S.FinishPanelActivator(true);
        else
            MenuManager.S.EndGamePanelActivator(true);

        MenuManager.S.GameStartPanelActivator(false);

        AudioBox.S.AudioStopSiren();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedLine")
        {
            FinishGame();
        }

        if (other.tag == "Wall Left")
        {
            _wallLeft = true;
        }
        else if (other.tag == "Wall Right")
        {
            _wallRight = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Wall Left")
        {
            _wallLeft = false;
        }
        else if (other.tag == "Wall Right")
        {
            _wallRight = false;
        }
    }

    public int NumberHero
    {
        get
        {
            return _numberHero;
        }

        set
        {
            _numberHero = value;

            if (_numberHero < 10)
            {
                _textMesh.text = "00" + _numberHero.ToString();
            }
            else if (_numberHero < 100)
            {
                _textMesh.text = "0" + _numberHero.ToString();
            }
            else
            {
                _textMesh.text = _numberHero.ToString();
            }
            
        }
    }
}
