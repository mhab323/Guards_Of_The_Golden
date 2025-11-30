using UnityEngine;
using UnityEngine.UI;   // <-- add this

public class BruteBehaviour : MonoBehaviour
{
    Animator animator;
    public GameObject Player;

    [Header("Dialogue UI")]
    public Text messageText;   // drag your UI Text here
    [TextArea]
    public string dialogue = "There is a close cave with treasure inside.";
    public float dialogueTime = 4f; // how long to show it

    bool hasSpoken = false;
    float dialogueTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator == null || Player == null) return;

        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // Player close -> talk
        if (distance < 4f)
        {
            // Set state to talking (you decide what 1 means in your Animator)
            if (animator.GetInteger("State") != 1)
            {
                animator.SetInteger("State", 1);
            }

            // Show dialogue once when we first get close
            if (!hasSpoken)
            {
                hasSpoken = true;
                dialogueTimer = dialogueTime;

                if (messageText != null)
                    messageText.text = dialogue;
            }
        }
        else
        {
            // Player far -> idle
            if (animator.GetInteger("State") != 0)
            {
                animator.SetInteger("State", 0);
            }
        }

        // Handle hiding the dialogue after some time
        if (hasSpoken && dialogueTimer > 0f)
        {
            dialogueTimer -= Time.deltaTime;
            if (dialogueTimer <= 0f && messageText != null)
            {
                messageText.text = "";
            }
        }
    }
}
