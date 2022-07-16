using ProtoRoguelite.Managers;
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
    [SerializeField] private Character _target;

    [SerializeField] private Statistic _damage;
    [SerializeField] private Statistic _rangeMin;
    [SerializeField] private Statistic _rangeMax;
    [SerializeField] private Statistic _areaOfEffect;
    #endregion Serialized Fields

    #region Private Fields
    private MainManager _mainManager = null;
    private CharacterManager _characterManager = null;

    private NavMeshAgent _navMeshAgent;
    #endregion Private Fields

    #region Properties
    public Statistic Damage => _damage;
    public Statistic RangeMin => _rangeMin;
    public Statistic RangeMax => _rangeMax;
    public Statistic AreaOfEffect => _areaOfEffect;

    public Team Team { get; set; } = null;
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface
    private void Start()
    {
        _mainManager = MainManager.instance;
        _characterManager = _mainManager?.CharacterManager;

        //navMeshAgent initialization
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
    }
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    public void UpdateCharacter()
    {
        if (_navMeshAgent == null)
            return;

        if (_target == null)
        {
            //try finding a target
            _target = _characterManager.FindTarget(this);

            if (_target == null)
                return;
        }

        //move to target
        _navMeshAgent.SetDestination(_target.transform.position);
    }
    #endregion Public Methods

    #endregion Methods
}
