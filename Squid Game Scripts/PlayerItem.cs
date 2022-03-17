using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    public int idPlayer;
    public int namePlayer;
    public int price;
    public int force;
    public int speed;
    public int dexterity;
    public bool skillJump;
    public bool skillPush;

    [Header("Components")]
    public Material materialPlayerAlive;
    public Material materialPlayerDead;
    public GameObject btnChoose;

    [Header("Button Buy Changer")]
    public GameObject btnBuy;
    public GameObject btnName;
    public GameObject iconMoney;
    public GameObject txtPrice;

    [Header("Button Buy Changer")]
    public Text nameBuyBtn;
    public Text nameChooseBtn;
    public Text nameSkill1;
    public Text nameSkill2;

    [Header("Prossesing")]
    public bool buy;
    public bool choose;

    public void Buy()
    {
        PlayersManager.S.BuyPlayer(idPlayer);
    }

    public void Choose()
    {
        PlayersManager.S.ChoosePlayer(idPlayer);
    }
}
