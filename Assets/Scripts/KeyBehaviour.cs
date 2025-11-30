using UnityEngine;
using UnityEngine.UI;

public class KeyBehaviour : MonoBehaviour
{
    public static bool hasKey = false;

    public float rotationSpeed = 90f; // degrees per second

    [Header("UI")]
    public Text messageText;              // drag message UI text here

    [Header("Guards")]
    public GameObject[] guardsToActivate; // drag guard objects here


    [Header("Audio")]
    public AudioSource audioSource;       // AudioSource to play the sound
    public AudioClip pickupClip;


    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        hasKey = true;
        Debug.Log("Key picked up!");

        if (audioSource != null)
        {
            if (pickupClip != null)
                audioSource.clip = pickupClip;   // set the clip if given

            audioSource.Play();                  // <-- this is what you wanted
        }

        if (messageText != null)
        {
            messageText.text = "Key picked up!";
            messageText.text = "Open the Treasure!";
        }

        //Wake up the guards
        foreach (GameObject g in guardsToActivate)
        {
            if (g != null)
                g.SetActive(true);
        }

        Destroy(gameObject,0.3f); // remove key
    }
}
