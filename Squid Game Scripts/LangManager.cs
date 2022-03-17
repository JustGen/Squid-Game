using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangManager : MonoBehaviour
{
    public static LangManager S;

    [SerializeField] private List<GameObject> _listOfTextGO;
    [SerializeField] private List<LangItem> _listOfLangItem;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        for (int i = 0; i < _listOfTextGO.Count; i++)
        {
            _listOfLangItem.Add(_listOfTextGO[i].GetComponent<LangItem>());
        }
    }

    public void ChangeGlobalLang(int idLang)
    {
        for (int i = 0; i < _listOfLangItem.Count; i++)
        {
            _listOfLangItem[i].ChangeNamebyidLang(idLang);
        }

        CoreGame.S.CountRespawn += 0;
        CoreGame.S.Level += 0;
        CoreGame.S.AliveAI += 0;

        PlayersManager.S.UpdateUI(PlayersManager.S._idActivePlayer);
    }
}
