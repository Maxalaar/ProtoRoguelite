namespace ProtoRoguelite.Statistics
{
    using ProtoRoguelite.Characters;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class Statistic : ISerializationCallbackReceiver
    {
        #region Fields

        #region Serialized Fields
        [SerializeField, HideInInspector] private bool _serialized = false;

        [SerializeField] private StatisticsEnum _type;
        [SerializeField] private float _min = float.MinValue;
        [SerializeField] private float _max = float.MaxValue;
        [SerializeField] private float _base = 0f;
        #endregion Serialized Fields

        #region Private Fields
        private float _current = float.NaN;
        List<StatisticModifier> _statisticModifiers = new List<StatisticModifier>();
        #endregion Private Fields

        #region Properties
        public StatisticsEnum Type => _type;
        public float Min => _min;
        public float Max => _max;
        public float Base => _base;
        public float Current => _current;

        #endregion Properties

        #endregion Fields

        #region Methods

        #region Constructor
        public Statistic(StatisticsEnum typeValue, float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            _type = typeValue;
            _base = baseValue;
            _current = _base;
            _min = minValue;
            _max = maxValue;
        }
        #endregion Constructor

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods

        public void AddStatisticModifier(StatisticModifier statisticModifier)
        {
            _statisticModifiers.Add(statisticModifier);

            _current += statisticModifier.Value * _base;
        }

        public void RemoveStatisticModifier(StatisticModifier statisticModifier)
        {
            _statisticModifiers.Remove(statisticModifier);

            _current -= statisticModifier.Value * _base;
        }

        public bool ContainsStatisticModifier(StatisticModifier statisticModifier)
        {
            return _statisticModifiers.Contains(statisticModifier);
        }

        public void OnBeforeSerialize()
        {
            if (_serialized)
                return;

            _min = float.MinValue;
            _max = float.MaxValue;
            _base = 0f;

            _serialized = true;
        }

        public void OnAfterDeserialize() 
        {
            _current = _base;
        }
        #endregion Public Methods

        #endregion Methods
    }
}