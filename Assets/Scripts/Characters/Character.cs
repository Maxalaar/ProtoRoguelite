namespace ProtoRoguelite.Characters
{
    using ProtoRoguelite.Characters.Weapons;
    using ProtoRoguelite.Managers;
    using ProtoRoguelite.Statistics;
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
        Speed,
        Size,
        Reach,
        Angle,
        AttackCooldown,
        AttackAnticipation,
        Knockback,
    }

    public class Character : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TextMesh _textMeshHealth;

        [SerializeField] private GameObject _target = null;

        [SerializeField] private Statistic _maxHealth = null;
        [SerializeField] private Statistic _speed = null;
        [SerializeField] private Statistic _size = null;
        [SerializeField] private int _currentHealth = 10;

        [SerializeField] private Team _team;
        [SerializeField] private Weapon _weapon;
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;
        private CharacterManager _characterManager = null;
        #endregion Private Fields

        #region Properties
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public GameObject Target
        {
            get { return _target; }
            set { _target = value; }
        }

        public Team Team
        {
            get { return _team; }
            set { _team = value; }
        }

        public Weapon Weapon => _weapon;

        // public GameObject Target => _target;
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
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
        }
        #endregion Unity Interface

        #region Private Methods
        private void Die()
        {
            _target = null;
            _characterManager.RemoveCharacter(this);
        }
        #endregion Private Methods

        #region Public Methods
        public void Init(CharacterArchetypeSO characterArchetypeSO)
        {
            if (characterArchetypeSO == null)
            {
                Debug.LogWarning("Character.Init has null characterArchetypeSO.");
                return;
            }

            _maxHealth = characterArchetypeSO.MaxHealth;
            _currentHealth = Mathf.RoundToInt(_maxHealth.Base);
            _speed = characterArchetypeSO.Speed;
            _size = characterArchetypeSO.Size;

            _weapon.Init(characterArchetypeSO.WeaponSO, this);

            _textMeshHealth.text = _currentHealth.ToString();
            // _navMeshAgent.stoppingDistance = _weapon.Reach.Current * 0.8f;
            StartMoving();
        }

        public void UpdateCharacter()
        {

            if (_target == null || _target.gameObject.activeInHierarchy == false)
                SetTargetRandomAdeversary();

            if (_target == null || _target.gameObject.activeInHierarchy == false)
            {
                _target = null;
                return;
            }

            if (_navMeshAgent != null && _navMeshAgent.enabled == true)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }

            if (_weapon != null)
            {
                _weapon.UpdateWeapon();
                _weapon.RotateTowardTarget(_target.transform);
            }
        }

        public void StopMoving()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.velocity = new Vector3(0, 0, 0);
        }

        public void StartMoving()
        {
            _navMeshAgent.isStopped = false;
        }

        public void SetTargetRandomAdeversary()
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
                //Debug.LogWarning("Attempt to add target character to character but adeversary team has no character.");
                return;
            }

            randomIndex = UnityEngine.Random.Range(0, adeversaryCharacters.Count);
            _target = adeversaryCharacters[randomIndex].gameObject;
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            _textMeshHealth.text = _currentHealth.ToString();

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        #endregion Public Methods

        #endregion Methods
    }
}