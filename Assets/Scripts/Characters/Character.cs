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
    [SerializeField] private GameObject _target = null;
    [SerializeField] private Statistic _damage;
    [SerializeField] private Statistic _rangeMin;
    [SerializeField] private Statistic _rangeMax;
    [SerializeField] private Statistic _areaOfEffect;
    [SerializeField] private Team _team;
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
    public Team Team
    { 
        get { return _team; }
        set { _team = value; }
    }
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface
    private void Start()
    {
        _mainManager = MainManager.instance;
        _characterManager = _mainManager?.CharacterManager;
    }

    private void Awake()
    {
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
        if (_target != null)
        {
            _navMeshAgent.SetDestination(_target.transform.position);
        }
    }

    public void setTargetRandomAdeversaryCharacter()
    {
        if (_navMeshAgent == null)
        {
            Debug.LogWarning("Attempt to add target character to character but he as no navMeshAgent.");
            return;
        }

        if (_team == null)
        {
            Debug.LogWarning("Attempt to add target character to character but he as no team.");
            return;
        }

        List<Team> adeversaryTeams = _team.AdeversaryTeams;

        if (adeversaryTeams.Count <= 0)
        {
            Debug.LogWarning("Attempt to add target character to character but his team does not have an adeversary team.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, adeversaryTeams.Count);
        Team randomAdversaryTeam = adeversaryTeams[randomIndex];

        List<Character> adeversaryCharacters = randomAdversaryTeam.Characters;

        if (adeversaryCharacters.Count <= 0)
        {
            Debug.LogWarning("Attempt to add target character to character but adeversary team has no character.");
            return;
        }

        randomIndex = UnityEngine.Random.Range(0, adeversaryCharacters.Count);
        _target = adeversaryCharacters[randomIndex].gameObject;
    }
    #endregion Public Methods

    #endregion Methods
}
