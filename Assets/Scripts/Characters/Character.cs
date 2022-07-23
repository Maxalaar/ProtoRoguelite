using ProtoRoguelite.Characters.Weapons;
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
    Health,
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
    [SerializeField] private int _health = 10;
    [SerializeField] private Team _team;
    #endregion Serialized Fields

    #region Private Fields
    private MainManager _mainManager = null;
    private CharacterManager _characterManager = null;

    private NavMeshAgent _navMeshAgent;

    private Weapon _weapon = null;
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

    public GameObject Target => _target;
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface
    private void Start()
    {
        _mainManager = MainManager.instance;
        _characterManager = _mainManager?.CharacterManager;
        _weapon = GetComponentInChildren<Weapon>();
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
    private void Die()
    {
        _characterManager.RemoveCharacter(this);
    }
    #endregion Private Methods

    #region Public Methods
    public void UpdateCharacter()
    {
        if (_target == null || _target.gameObject.activeInHierarchy == false)
            SetTargetRandomAdeversaryCharacter();

        if (_target == null)
            return;

        Vector3 targetPos = _target.transform.position;

        _navMeshAgent.SetDestination(targetPos);

        if (_weapon == null)
            return;

        _weapon.RotateTowardTarget(_target.transform);
    }

    public void SetTargetRandomAdeversaryCharacter()
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

    public void TakeDamage(int damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            Die();
        }
    }
    #endregion Public Methods

    #endregion Methods
}
