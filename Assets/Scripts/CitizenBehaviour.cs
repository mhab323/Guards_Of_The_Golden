using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CitizenBehaviour : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;
    public Animator animator;
    float distanceToTarget;
    public Transform[] targets;
    int n = 0;
    int size;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        size = targets.Length;
    }
    // Update is called once per frame
    void Update()
    {
        distanceToTarget = agent.transform.position.magnitude - targets[n].position.magnitude;
        Debug.Log("Distance to Target: " + distanceToTarget);
        if (distanceToTarget < 0.5f)
        {

            animator.SetBool("HasReached", true);
            StartCoroutine(Wait());

        }
        else
        {
            animator.SetBool("HasReached", false);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("PreWarm");

            agent.SetDestination(targets[0].position);
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        if (n < size - 1)
        {
            n++;
            agent.SetDestination(targets[n].position);
        }
        
    }
}
