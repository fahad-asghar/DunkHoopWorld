using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public static RewardManager instance;

    [HideInInspector] public int coins = 0;
    [HideInInspector] public int diamonds = 0;

    [SerializeField] Text coinsText;
    [SerializeField] Text diamondsText;

    private void Awake()
    {
        instance = this;

        if(PlayerPrefs.HasKey("Coins"))
            coins = PlayerPrefs.GetInt("Coins");
        if (PlayerPrefs.HasKey("Diamonds"))
            diamonds = PlayerPrefs.GetInt("Diamonds");

        coinsText.text = coins.ToString();
        diamondsText.text = diamonds.ToString();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        coinsText.text = coins.ToString();
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void AddDiamonds(int amount)
    {
        diamonds += amount;
        diamondsText.text = diamonds.ToString();
        PlayerPrefs.SetInt("Diamonds", diamonds);
    }
}
