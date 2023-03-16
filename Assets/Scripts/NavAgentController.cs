using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavAgentController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    private void Start()
    {

    }

    public void SetNavDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

}
