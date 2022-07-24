namespace ProtoRoguelite.Statistics
{
    using ProtoRoguelite.Characters;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatisticModifier
    {
        #region Fields

        #region Serialized Fields
        #endregion Serialized Fields

        #region Private Fields
        private StatisticsEnum _targetStatistics;
        List<Statistic> _statistis = new List<Statistic>();
        private float _value;
        #endregion Private Fields

        #region Properties
        public float Value => _value;
        #endregion Properties

        #endregion Fields

        #region Methods

        #region Constructor
        public StatisticModifier(StatisticsEnum targetStatistics, float value)
        {
            _targetStatistics = targetStatistics;
            _value = value;
        }
        #endregion Constructor

        #region Unity Interface
        #endregion Unity Interface

        #region Private Methods
        #endregion Private Methods

        #region Public Methods
        public void AddStatistic(Statistic statistic)
        {
            if (statistic.Type == _targetStatistics)
            {
                if (_statistis.Contains(statistic) == true)
                {
                    Debug.LogWarning("Attempt to add to the StatisticModifier a Statistic that is already modified by it");
                    return;
                }
                if (statistic.ContainsStatisticModifier(this) == true)
                {
                    Debug.LogWarning("Attempt to add to the Statistic a StatisticModifier that is already modified by it");
                    return;
                }
                _statistis.Add(statistic);
                statistic.AddStatisticModifier(this);
            }
            else
            {
                Debug.LogWarning("Miss match between StatisticModifier._targetStatistics and Statistic._type");
                return;
            }
        }
        public void RemoveStatistic(Statistic statistic)
        {
            if (statistic.Type == _targetStatistics)
            {
                if (_statistis.Contains(statistic) == false)
                {
                    Debug.LogWarning("Attempt to remove Statistic to the StatisticModifier but is not modified by it");
                    return;
                }
                if (statistic.ContainsStatisticModifier(this) == false)
                {
                    Debug.LogWarning("Attempt to remove StatisticModifier to the Statistic but is not modified by it");
                    return;
                }
                _statistis.Remove(statistic);
                statistic.RemoveStatisticModifier(this);
            }
            else
            {
                Debug.LogWarning("Miss match between StatisticModifier._targetStatistics and Statistic._type");
                return;
            }
        }
        #endregion Public Methods

        #endregion Methods
    }
}