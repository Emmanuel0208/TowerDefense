using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playerMoney = 100;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("No hay suficiente dinero!");
            return false;
        }
    }

    public void EarnMoney(int amount)
    {
        playerMoney += amount;
        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "$" + playerMoney.ToString();
    }
}
