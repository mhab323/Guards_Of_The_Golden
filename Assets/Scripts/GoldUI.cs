using UnityEngine;
using UnityEngine.UI;

public class GoldUI : MonoBehaviour
{
    public Text coinsText;

    void Start()
    {
        if (coinsText != null)
        {
            coinsText.text = "Gold: " + CoinBehavior.num_coins;
        }
    }

    // Optional: call this from other scripts when gold changes
    public void Refresh()
    {
        if (coinsText != null)
        {
            coinsText.text = "Gold: " + CoinBehavior.num_coins;
        }
    }
}
