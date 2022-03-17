using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    public GameObject number;
    public float speed;
    [SerializeField] private GameObject _hero;
    [SerializeField] private float _timeMoveAfter;
    [SerializeField] private GameObject _HeroForMaterial;
    [SerializeField] private Material _matHeroDead;
    [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private int[] _excludeNumberOfAIHero;

    private int _numberHero;
    private float persentContinueMove;

    private bool _changeSpeed;
    private float _changeLocalPersent;
    private Animator _animator;
    private SkinnedMeshRenderer _materialHero;
    private bool dead;

    public enum AIMode
    {
        idle,
        Run,
        Finish,
        Dead
    }

    public AIMode aiMode;
    

    private void Start()
    {
        _textMesh = number.GetComponent<TextMeshPro>();

        NumberHero = Random.Range(1, 456);

        for (int i = 0; i < _excludeNumberOfAIHero.Length; i++)
        {
            if (NumberHero == _excludeNumberOfAIHero[i])
            {
                if (i == 0)
                    NumberHero++;
                else
                    NumberHero--;
            }
        }

        speed = ChangeSpeed();

        aiMode = AIMode.idle;

        _changeSpeed = false;
        persentContinueMove = Random.Range(0f, 0.5f);
        _animator = _hero.GetComponent<Animator>();
        _materialHero = _HeroForMaterial.GetComponent<SkinnedMeshRenderer>();
        dead = false;
    }

    private void Update()
    {
        if (aiMode == AIMode.Finish)
        {
            this.enabled = false;
            _animator.SetBool("active", false);
        }
        else if (aiMode == AIMode.Dead)
        {
            if (!dead)
                DeadHero();
        }
        else if (CoreGame.S.timerMode == CoreGame.TimerMode.GreenZone && !CoreGame.S.firstRun)
        {
            Move();
            aiMode = AIMode.Run;

            if (_changeSpeed)
            {
                speed = ChangeSpeed();
                _changeSpeed = false;
            }  
        }
        else if (CoreGame.S.timerMode == CoreGame.TimerMode.RedZone)
        {
            if (!_changeSpeed)
            {
                aiMode = AIMode.idle;
                _animator.SetBool("active", false);

                speed = ChangeSpeed();
                _changeSpeed = true;

                _changeLocalPersent = Random.Range(0f, 1f);
                if (_changeLocalPersent <= persentContinueMove)
                {
                    StartCoroutine(ContinueMove());
                }
            }
        }
    }

    private void Move()
    {
        _animator.SetBool("active", true);
        transform.Translate(transform.forward * Time.deltaTime * speed);
    }

    IEnumerator ContinueMove()
    {
        float beginTime = Time.time;
        while (Time.time - _timeMoveAfter <= beginTime)
        {
            Move();
            yield return null;
        }
        aiMode = AIMode.Dead;
    }

    private void DeadHero()
    {
        _animator.SetBool("dead", true);
        _materialHero.material = _matHeroDead;
        dead = true;
    }

    private void FinishGame()
    {
        aiMode = AIMode.Finish;
        _animator.SetBool("active", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RedLine")
        {
            FinishGame();
        }
    }

    public float ChangeSpeed()
    {
        float tempSpeedAdding = PlayersManager.S._idActivePlayer / 10.0f - 0.1f;
        //Debug.Log(tempSpeedAdding);
        return Random.Range(HeroMove.S.speed * (0.7f - tempSpeedAdding), HeroMove.S.speed * (1.7f - tempSpeedAdding));

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
