using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    private NavMeshAgent _navMeshAgent;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }

    private void Update()
    {
        if (_navMeshAgent == null || _target == null)
            return;

        _navMeshAgent.SetDestination(_target.transform.position);
    }
}
