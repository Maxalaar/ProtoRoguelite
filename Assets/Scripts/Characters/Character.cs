using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum StatisticsEnum
{
    Damage,
    RangeMin,
    RangeMax,
    AreaOfEffect,
}

public class Character : MonoBehaviour
{
    #region Fields

    #region Serialized Fields
    [SerializeField] private GameObject _target;
    #endregion Serialized Fields

    #region Private Fields
    private NavMeshAgent _navMeshAgent;
    #endregion Private Fields

    #region Properties
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface
    private void Start()
    {
        //navMeshAgent initialization
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
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    #endregion Public Methods

    #endregion Methods
}
