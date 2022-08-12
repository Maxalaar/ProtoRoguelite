namespace ProtoRoguelite.Characters.Weapons
{
    using ProtoRoguelite.Statistics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Weapon : MonoBehaviour
    {
        #region Fields

        #region Serialized Fields
        #endregion Serialized Fields

        #region Private Fields
        private Statistic _damage;
        private Statistic _knockBack;
        private Statistic _reach;
        private Statistic _angle;
        private Statistic _attackCooldown;
        private Statistic _attackAnticipation;

        private float _maxAlphaValue = 0.25f;
        private float _minAlphaValue = 0.05f;
        private float _curentAlphaValue = 0.05f;

        private Character _owner;
        private List<Character> _collidingCharacters = new List<Character>();

        private Coroutine _attackCoroutine = null;

        private PolygonCollider2D _collider = null;
        private Mesh _mesh = null;
        private MeshFilter _meshFilter = null;
        private MeshRenderer _meshRenderer = null;
        #endregion Private Fields

        #region Properties
        public bool IsAttacking { get; private set; } = false;
        #endregion Properties

        public int pointsInArcNumber = 5;
        public float radius = 1;
        public float angle = 0.25f;

        #endregion Fields

        #region Methods

        #region Constructor
        #endregion Constructor

        #region Unity Interface
        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character characterCollision = collision.gameObject.GetComponent<Character>();
            if (characterCollision == null)
                return;

            if (_owner == null
                || _owner.Target == null
                || !_owner.Team.AdeversaryTeams.Contains(characterCollision.Team)
                || _collidingCharacters.Contains(characterCollision))
                return;

            _collidingCharacters.Add(characterCollision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Character characterCollision = collision.gameObject.GetComponent<Character>();
            if (characterCollision == null)
                return;

            if (_owner == null || _owner.Target == null
                || !_owner.Team.AdeversaryTeams.Contains(characterCollision.Team)
                || !_collidingCharacters.Contains(characterCollision))
                return;

            _collidingCharacters.Remove(characterCollision);
        }

        private void OnDisable()
        {
            if (_attackCoroutine == null)
                return;

            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
            IsAttacking = false;
        }
        #endregion Unity Interface

        #region Private Methods
        private void GenerateCollider()
        {
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<PolygonCollider2D>();
            }
            _collider.transform.localRotation = _owner.transform.localRotation;

            Vector2[] points = new Vector2[pointsInArcNumber + 2];

            points[0] = Vector2.zero;

            for (int i = 1; i < pointsInArcNumber + 2; i++)
            {
                float angleTemp = angle * ((float)(i-1) / (float)pointsInArcNumber) - 0.5f * angle;
                points[i] = radius * new Vector2(Mathf.Cos(angleTemp), Mathf.Sin(angleTemp));
            }

            _collider.points = points;

            _collider.isTrigger = true;

            GenerateMesh();
        }

        private void GenerateMesh()
        {

            if (_collider == null || _meshFilter == null || _meshRenderer == null)
            {
                return;
            }
        
            _mesh = _collider.CreateMesh(false, false);
            _meshFilter.mesh = _mesh;

            UpdateMeshColor();
        }

        private IEnumerator CoAttack()
        {
            int alphaNumSteps = 30;
            float alphaTimeSteps = _attackAnticipation.Current / alphaNumSteps;

            IsAttacking = true;
            _owner.StopMoving();

            // attack anticipation
            for (int i = 0; i < alphaNumSteps; i++)
            {
                yield return new WaitForSeconds(alphaTimeSteps);
                _curentAlphaValue = ((float)i/alphaNumSteps) * _maxAlphaValue;
                UpdateMeshColor();
            }
            _curentAlphaValue = _minAlphaValue;
            UpdateMeshColor();

            //if no enemies are in range anymore, return
            if (_collidingCharacters.Count == 0)
            {
                IsAttacking = false;
                _attackCoroutine = null;
                yield break; 
            }

            //create a temporary array in case _collidingCharacters list is modified
            Character[] collidingCharactersTemp = _collidingCharacters.ToArray();
            foreach (Character collidingChararcter in collidingCharactersTemp)
            {
                collidingChararcter.TakeDamage(Mathf.RoundToInt(_damage.Current));
            }

            //attack cooldown
            yield return new WaitForSeconds(_attackCooldown.Current);

            IsAttacking = false;
            _attackCoroutine = null;
        }
        #endregion Private Methods

        #region Public Methods
        public void Init(WeaponSO weaponSO, Character character)
        {
            if (weaponSO == null)
            {
                Debug.LogWarning("Weapon.Init has null weaponSO.");
                return;
            }

            _owner = character;

            _damage = weaponSO.Damage;
            _knockBack = weaponSO.Knockback;
            _reach = weaponSO.Reach;
            _angle = weaponSO.Angle;
            _attackCooldown = weaponSO.AttackCooldown;
            _attackAnticipation = weaponSO.AttackAnticipation;

            GenerateCollider();
        }

        public void UpdateWeapon()
        {
            if (_attackCoroutine != null)
                return;

            if (_collidingCharacters.Count == 0)
                return;

            _attackCoroutine = StartCoroutine(CoAttack());
        }

        public void RotateTowardTarget(Transform target)
        {
            Vector3 targetPos = target.position;
            Vector3 weaponPos = transform.position;

            Vector3 targetRelativePos = new Vector3(targetPos.x - weaponPos.x, targetPos.y - weaponPos.y, 0);

            float newAngle = Mathf.Atan2(targetRelativePos.y, targetRelativePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
        }

        public void UpdateMeshColor()
        {
            Color newColor = _owner.SpriteRenderer.color;
            newColor.a = _curentAlphaValue;

            float darkFactor = 0.5f;

            newColor.r *= darkFactor;
            newColor.g *= darkFactor;
            newColor.b *= darkFactor;

            _meshRenderer.material.color = newColor;
        }
        #endregion Public Methods

        #endregion Methods
    }
}