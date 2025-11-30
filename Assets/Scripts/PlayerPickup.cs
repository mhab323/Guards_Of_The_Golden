using UnityEngine;
using UnityEngine.UI;

public class PlayerPickup : MonoBehaviour
{
    [Header("References")]
    public PlayerWeapon playerWeapon;    // PlayerWeapon on the same player
    public GameObject sword;            // LongSword in the scene
    public Text messageText;            // your MessageDisplay Text

    [Header("Settings")]
    public float pickupDistance = 2f;   // how close you must be

    void Update()
    {
        if (sword == null) return;  // already picked up

        float dist = Vector3.Distance(transform.position, sword.transform.position);

        // In range → show message
        if (dist <= pickupDistance)
        {
            if (messageText != null)
                messageText.text = "Press F to pick up sword";

            // Press F to pick up
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (playerWeapon != null)
                {
                    Debug.Log("Picking up sword via distance check");
                    playerWeapon.Equip(sword);

                    // Stop showing the sword on the ground
                    // (since Equip parents it to the hand)
                    if (messageText != null)
                        messageText.text = "";

                    sword = null; // mark as picked up
                }
                else
                {
                    Debug.LogWarning("PlayerPickup: playerWeapon reference is missing!");
                }
            }
        }
        else
        {
            // Too far → clear message
            if (messageText != null && messageText.text == "Press F to pick up sword")
                messageText.text = "";
        }
    }
}
