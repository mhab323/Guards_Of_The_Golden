using UnityEngine;
using UnityEngine.UI;

public class ChestBehavior : MonoBehaviour
{
    [Header("References")]
    public GameObject player;
    public Animator chestAnimator;
    public Text coinsText;
    public Text messageText;
    public GameObject portal;       // ADD THIS

    [Header("Chest Settings")]
    public int goldInChest = 10;

    bool isPlayerInRange = false;
    bool isOpened = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pressed F near chest. hasKey = " + KeyBehaviour.hasKey + ", isOpened = " + isOpened);

            if (!KeyBehaviour.hasKey)
            {
                ShowMessage("Can't open without key!");
            }
            else if (!isOpened)
            {
                isOpened = true;

                if (chestAnimator != null)
                    chestAnimator.SetTrigger("Open");

                CoinBehavior.num_coins += goldInChest;

                if (coinsText != null)
                    coinsText.text = "Gold: " + CoinBehavior.num_coins;

                ShowMessage("You opened the chest!");

                // 🔥 ACTIVATE THE PORTAL
                if (portal != null)
                {
                    portal.SetActive(true);
                    Debug.Log("Portal activated!");
                }
                else
                {
                    Debug.LogWarning("Portal reference is missing in ChestBehavior!");
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            ShowMessage("Press F to open the chest");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ShowMessage("");
        }
    }

    void ShowMessage(string text)
    {
        if (messageText != null)
            messageText.text = text;
    }
}
