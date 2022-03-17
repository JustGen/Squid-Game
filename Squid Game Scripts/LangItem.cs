using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LangItem : MonoBehaviour
{
    [SerializeField] private string _nameRU;
    [SerializeField] private string _nameEN;

    [SerializeField] private Text _thisText;

    public void ChangeNamebyidLang(int idLang)
    {
        switch (idLang)
        {
            case 1:
                _thisText.text = _nameRU;
                break;

            case 2:
                _thisText.text = _nameEN;
                break;
        }
    }
}
