using UnityEngine;

public class WindowBehaviour : MonoBehaviour
{
    private Animator animator;
    private AudioSource windowOpen;
    private AudioSource windowClose;

    void Start()
    {
        animator = GetComponent<Animator>();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        windowOpen = audioSources[0];
        windowClose = audioSources[1];
    }

    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("WindowOpen", true);
        windowOpen.Play();
    }

    void OnTriggerExit(Collider other)
    {
        animator.SetBool("WindowOpen", false);
        windowClose.Play();
    }
}
