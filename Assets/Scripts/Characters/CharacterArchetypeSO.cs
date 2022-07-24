namespace ProtoRoguelite.Characters
{
    using ProtoRoguelite.Characters.Weapons;
    using ProtoRoguelite.Statistics;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "CharacterArchetypeSO", menuName = "ScriptableObjects/Characters/CharacterArchetypeSO")]
    public class CharacterArchetypeSO : ScriptableObject
    {
        #region Fields

        #region Serialized Fields
        [SerializeField] private Statistic _maxHealth = null;
        [SerializeField] private Statistic _speed = null;
        [SerializeField] private Statistic _size = null;

        [SerializeField] private WeaponSO _weaponSO = null;
        #endregion Serialized Fields

        #region Private Fields
        #endregion Private Fields

        #region Properties
        public Statistic MaxHealth => _maxHealth;
        public Statistic Speed => _speed;
        public Statistic Size => _size;

        public WeaponSO WeaponSO => _weaponSO;
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