using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    Animator animator;
    AudioSource DoorSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        DoorSound = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("DoorOpens", true);
        DoorSound.PlayDelayed(0.5f);
    }
    void OnTriggerExit(Collider other)
    {
        animator.SetBool("DoorOpens", false);
        DoorSound.PlayDelayed(0.5f);
    }

}
