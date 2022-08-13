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
        private List<Character> _collidingCharacters;

        private Coroutine _attackCoroutine;
        private Coroutine _cooldownCoroutine;
        private PolygonCollider2D _collider = null;

        private Mesh _mesh = null;
        private MeshFilter _meshFilter = null;
        private MeshRenderer _meshRenderer = null;
        #endregion Private Fields

        #region Properties
        public Statistic Reach => _reach;
        #endregion Properties

        public int _pointsInArcNumber = 5;

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

            if (_collidingCharacters.Count == 0)
                _owner.Target = characterCollision.transform.gameObject;

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

        private void DisableCoAttack()
        {
            if (_attackCoroutine == null)
            {
                Debug.LogWarning("Attempt to stop _attackCoroutine but it is not active");
                return;
            }

            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
            _owner.StartMoving();
        }
        #endregion Unity Interface

        #region Private Methods
        private void GenerateCollider()
        {
            if (_collider == null)
            {
                _collider = gameObject.AddComponent<PolygonCollider2D>();
            }
            _collider.transform.rotation = _owner.transform.rotation;

            Vector2[] points = new Vector2[_pointsInArcNumber + 2];

            points[0] = Vector2.zero;

            for (int i = 1; i < _pointsInArcNumber + 2; i++)
            {
                float angleTemp = _angle.Current * ((float)(i-1) / (float)_pointsInArcNumber) - 0.5f * _angle.Current;
                points[i] = _reach.Current * new Vector2(Mathf.Cos(angleTemp), Mathf.Sin(angleTemp)); // /!\
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

            UpdateMeshColor(_minAlphaValue);
        }

        private IEnumerator CoAttack()
        {
            _owner.StopMoving();

            // attack anticipation
            int alphaNumSteps = 30;
            float alphaTimeSteps = _attackAnticipation.Current / alphaNumSteps;
            for (int i = 0; i < alphaNumSteps; i++)
            {
                yield return new WaitForSeconds(alphaTimeSteps);
                UpdateMeshColor(((float)i/alphaNumSteps) * (_maxAlphaValue - _minAlphaValue) + _minAlphaValue);
            }

            //create a temporary array in case _collidingCharacters list is modified
            Character[] collidingCharactersTemp = _collidingCharacters.ToArray();
            foreach (Character collidingChararcter in collidingCharactersTemp)
            {
                collidingChararcter.TakeDamage(Mathf.RoundToInt(_damage.Current));
            }

            UpdateMeshColor(0f);
            DisableCoAttack();

            _cooldownCoroutine = StartCoroutine(CoCooldown());
        }

        private IEnumerator CoCooldown()
        {
            UpdateMeshColor(0f);
            yield return new WaitForSeconds(_attackCooldown.Current);
            StopCoroutine(_cooldownCoroutine);
            _cooldownCoroutine = null;
            UpdateMeshColor(_minAlphaValue);
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

            _attackCoroutine = null;
            _cooldownCoroutine = null;

            _collidingCharacters = new List<Character>();


            GenerateCollider();
        }

        public void UpdateWeapon()
        {
            if (_collidingCharacters.Count > 0)
            {
                _owner.StopMoving();
            }
            else
            {
                _owner.StartMoving();
            }

            if (_collidingCharacters.Count > 0 && _attackCoroutine == null && _cooldownCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(CoAttack());
            }
            else if (_collidingCharacters.Count == 0 && _attackCoroutine != null && _cooldownCoroutine == null)
            {   
                UpdateMeshColor(_minAlphaValue);
                DisableCoAttack();
            }
        }

        public void RotateTowardTarget(Transform target)
        {
            Vector3 targetPos = target.position;
            Vector3 weaponPos = transform.position;

            Vector3 targetRelativePos = new Vector3(targetPos.x - weaponPos.x, targetPos.y - weaponPos.y, 0);

            float newAngle = Mathf.Atan2(targetRelativePos.y, targetRelativePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
        }

        public void UpdateMeshColor(float alphaValue = float.NaN)
        {
            if (alphaValue != float.NaN)
            {
                _curentAlphaValue = alphaValue;
            }

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