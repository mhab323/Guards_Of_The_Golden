using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    public Text messageText;                  // drag MessageDisplay Text here
    public string pickupMessage = "Press F to pick up sword";

    bool isPlayerInRange = false;
    PlayerWeapon playerWeapon;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword trigger ENTER: " + other.name);

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerWeapon = other.GetComponent<PlayerWeapon>();

            if (messageText != null)
                messageText.text = pickupMessage;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Sword trigger EXIT: " + other.name);

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerWeapon = null;

            if (messageText != null)
                messageText.text = "";
        }
    }

    void Update()
    {
        if (isPlayerInRange && playerWeapon != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Picking up sword!");

            playerWeapon.Equip(gameObject);

            if (messageText != null)
                messageText.text = "";

            enabled = false;
        }
    }
}
