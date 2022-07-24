namespace ProtoRoguelite.Characters.Weapons
{
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
        private Statistic _attackCooldown;
        private Statistic _anticipationDuration;

        private Character _owner;
        #endregion Private Fields

        #region Properties
        #endregion Properties

        public int pointsInArcNumber = 5;
        public float radius = 1;
        public float angle = 0.25f;

        #endregion Fields

        #region Methods

        #region Constructor
        #endregion Constructor

        #region Unity Interface
        private void Start()
        {
            _owner = GetComponentInParent<Character>();

            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            GenerateCollider();

            _damage = new Statistic(StatisticsEnum.Damage, 1);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Character characterCollision = collision.gameObject.GetComponent<Character>();
            if (characterCollision == null)
                return;

            if (_owner == null || _owner.Target == null 
                || characterCollision != _owner.Target.GetComponent<Character>())
                return;

            characterCollision.TakeDamage(Mathf.RoundToInt(_damage.Current));
        }
        #endregion Unity Interface

        #region Private Methods
        private void GenerateCollider()
        {
            PolygonCollider2D collider = gameObject.AddComponent<PolygonCollider2D>();

            Vector2[] points = new Vector2[pointsInArcNumber + 2];

            points[0] = Vector2.zero;

            for (int i = 1; i < pointsInArcNumber + 2; i++)
            {
                float angleTemp = angle * ((float)(i-1) / (float)pointsInArcNumber) - 0.5f * angle;
                points[i] = radius * new Vector2(Mathf.Cos(angleTemp), Mathf.Sin(angleTemp));
            }

            collider.points = points;

            collider.isTrigger = true;
        }
        #endregion Private Methods

        #region Public Methods
        public void RotateTowardTarget(Transform target)
        {
            Vector3 targetPos = target.position;
            Vector3 weaponPos = transform.position;

            Vector3 targetRelativePos = new Vector3(targetPos.x - weaponPos.x, targetPos.y - weaponPos.y, 0);

            float newAngle = Mathf.Atan2(targetRelativePos.y, targetRelativePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
        }
        #endregion Public Methods

        #endregion Methods
    }
}