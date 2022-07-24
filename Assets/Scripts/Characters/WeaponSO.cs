namespace ProtoRoguelite.Characters.Weapons
{
    using ProtoRoguelite.Statistics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/Characters/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private Statistic _damage = null;
        [SerializeField] private Statistic _reach = null;
        [SerializeField] private Statistic _angle = null;
        [SerializeField] private Statistic _attackCooldown = null;
        [SerializeField] private Statistic _attackAnticipation = null;
        [SerializeField] private Statistic _knockback = null;
        #endregion Serialized Fields

        #region Private Fields
        #endregion Private Fields

        #region Properties
        public Statistic Damage => _damage;
        public Statistic Reach => _reach;
        public Statistic Angle => _angle;
        public Statistic AttackCooldown => _attackCooldown;
        public Statistic AttackAnticipation => _attackAnticipation;
        public Statistic Knockback => _knockback;
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Constructor
        #endregion Constructor

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        #endregion Public Methods

        #endregion Methods
    }
}