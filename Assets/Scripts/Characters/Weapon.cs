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
        private void Update()
        {
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            GenerateCollider();
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
        }
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods

        #endregion Methods
    }
}