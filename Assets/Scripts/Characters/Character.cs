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

        [SerializeField] private GameObject _target;

        [SerializeField] private Statistic _maxHealth = null;
        [SerializeField] private Statistic _speed = null;
        [SerializeField] private Statistic _size = null;
        [SerializeField] private int _currentHealth = 10;

        private List<Character> _collidingCharacters;
        private PolygonCollider2D _collider = null;

        private bool _isAbleMove;
        private bool _isAbleAttack;
        private bool _isAttacking;

        [SerializeField] private Team _team;
        [SerializeField] private Weapon _weapon;
        #endregion Serialized Fields

        #region Private Fields
        private MainManager _mainManager = null;
        private CharacterManager _characterManager = null;
        #endregion Private Fields

        #region Properties
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public bool IsAbleAttack
        {
            get { return _isAbleAttack; }
            set { _isAbleAttack = value; }
        }

        public bool IsAbleMove
        {
            get { return _isAbleMove; }
            set { _isAbleMove = value; }
        }

        public bool IsAttacking
        {
            get { return _isAttacking; }
            set { _isAttacking = value; }
        }

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
            _characterManager.RemoveCharacter(this);
        }
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character characterCollision = collision.gameObject.GetComponent<Character>();
            
            if (characterCollision == null)
                return;
            
            if (!_team.AdversaryTeams.Contains(characterCollision.Team)
                || _collidingCharacters.Contains(characterCollision))
                return;

            _collidingCharacters.Add(characterCollision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Character characterCollision = collision.gameObject.GetComponent<Character>();
            
            if (characterCollision == null)
                return;

            if (!_team.AdversaryTeams.Contains(characterCollision.Team)
                || !_collidingCharacters.Contains(characterCollision))
                return;

            _collidingCharacters.Remove(characterCollision);
        }

        private void GenerateCollider()
        {
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<PolygonCollider2D>();
            }

            int pointsInArcNumber = 20;
            float radius = 5;

            Vector2[] points = new Vector2[pointsInArcNumber + 2];

            points[0] = Vector2.zero;

            for (int i = 1; i < pointsInArcNumber + 2; i++)
            {
                float angleTemp = radius * ((float)(i-1) / (float)pointsInArcNumber) - 0.5f * 2 * (float)Math.PI;
                points[i] = radius * new Vector2(Mathf.Cos(angleTemp), Mathf.Sin(angleTemp));
            }

            _collider.points = points;

            _collider.isTrigger = true;
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

            // GenerateCollider();
            _target = null;

            _maxHealth = characterArchetypeSO.MaxHealth;
            _currentHealth = Mathf.RoundToInt(_maxHealth.Base);
            _speed = characterArchetypeSO.Speed;
            _size = characterArchetypeSO.Size;

            _weapon.Init(characterArchetypeSO.WeaponSO, this);

            _textMeshHealth.text = _currentHealth.ToString();
            _collidingCharacters = new List<Character>();
            // _navMeshAgent.stoppingDistance = _weapon.Reach.Current * 0.8f;
            _isAbleMove = true;
            _isAbleAttack = true;
            _isAttacking = false;
        }

        public void UpdateCharacter()
        {
            // Set target
            if (_target == null || _target.gameObject.activeInHierarchy == false)
                // SetTargetRandomAdversary();
                SetTargetNearestAdversary();

            if (_target == null || _target.gameObject.activeInHierarchy == false)
            {
                _target = null;
                StopMoving();
                return;
            }

            if (_navMeshAgent != null && _navMeshAgent.enabled == true)
            {
                _navMeshAgent.SetDestination(_target.transform.position);
            }

            // Moving
            if (_weapon != null)
            {
                if (_weapon.CollidingCharacters.Count > 0)
                {
                    _isAbleMove = false;
                }
                else
                {
                    _isAbleMove = true;
                }
            }

            if (_isAbleMove == true)
            {
                StartMoving();
            }
            else
            {
                StopMoving();
            }

            // Attack
            if (_weapon != null)
            {
                _weapon.RotateTowardTarget(_target.transform);
                
                if (_weapon.CollidingCharacters.Count > 0 && _isAbleAttack)
                {
                    _weapon.Attack();
                }
                else if(_weapon.CollidingCharacters.Count == 0 && _isAttacking)
                {
                    _weapon.DisableAttack();
                }
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

        public void SetTargetRandomAdversary()
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

            List<Team> adversaryTeams = _team.AdversaryTeams;

            if (adversaryTeams.Count <= 0)
            {
                Debug.LogWarning("Attempt to add target character to character but his team does not have an adversary team.");
                return;
            }

            int randomIndex = UnityEngine.Random.Range(0, adversaryTeams.Count);
            Team randomAdversaryTeam = adversaryTeams[randomIndex];

            List<Character> adversaryCharacters = randomAdversaryTeam.Characters;

            randomIndex = UnityEngine.Random.Range(0, adversaryCharacters.Count);
            _target = adversaryCharacters[randomIndex].gameObject;
        }

        public void SetTargetNearestAdversary()
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

            List<Team> adversaryTeams = _team.AdversaryTeams;

            if (adversaryTeams.Count <= 0)
            {
                Debug.LogWarning("Attempt to add target character to character but his team does not have an adversary team.");
                return;
            }

            List<Character> adversaryCharacters = new List<Character>();
            
            if (_collidingCharacters.Count > 0)
            {
                adversaryCharacters = _collidingCharacters;
            }
            else
            {
                foreach (Team adversaryTeam in adversaryTeams)
                {
                    adversaryCharacters.AddRange(adversaryTeam.Characters);
                }
            }
            
            Character nearestCharacter = null;
            float nearestDistance = float.MaxValue;

            foreach (Character adversaryCharacter in adversaryCharacters)
            {
                float distanceTemp = Math.Abs(adversaryCharacter.transform.position.x - this.transform.position.x) + Math.Abs(adversaryCharacter.transform.position.y - this.transform.position.y) + Math.Abs(adversaryCharacter.transform.position.z - this.transform.position.z);
                if (distanceTemp < nearestDistance)
                {
                    nearestCharacter = adversaryCharacter;
                    nearestDistance = distanceTemp;
                }  
            }

            if (nearestCharacter == null)
            {
                _target = null;
            }
            else
            {
                _target = nearestCharacter.gameObject;
            }
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