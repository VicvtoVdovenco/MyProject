using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    [HideInInspector]
    public Transform agentDestination;


    private void Start()
    {
        agent.SetDestination(agentDestination.position);
    }

    private void Update()
    {

    }

}
