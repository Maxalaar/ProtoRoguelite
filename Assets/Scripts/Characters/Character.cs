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

    [SerializeField] private Statistic _damage;
    [SerializeField] private Statistic _rangeMin;
    [SerializeField] private Statistic _rangeMax;
    [SerializeField] private Statistic _areaOfEffect;    
    #endregion Serialized Fields

    #region Private Fields
    private NavMeshAgent _navMeshAgent;
    #endregion Private Fields

    #region Properties
    public Statistic Damage => _damage;
    public Statistic RangeMin => _rangeMin;
    public Statistic RangeMax => _rangeMax;
    public Statistic AreaOfEffect => _areaOfEffect;
    #endregion Properties

    //TEST
    public StatisticModifierSO TOREMOVE;

    #endregion Fields

    #region Methods

    #region Unity Interface
    private void Start()
    {
        //navMeshAgent initialization
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        StatisticModifier statMod = new StatisticModifier(TOREMOVE, this);
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
