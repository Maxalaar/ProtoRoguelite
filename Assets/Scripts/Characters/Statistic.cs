using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Statistic
{
    #region Fields

    #region Serialized Fields
    #endregion Serialized Fields

    #region Private Fields
    [SerializeField] private StatisticsEnum _type;
    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _base;
    [SerializeField] private float _current;
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
        
        _current +=  statisticModifier.Value * _base;
    }

    public void RemoveStatisticModifier(StatisticModifier statisticModifier)
    {
        _statisticModifiers.Remove(statisticModifier);
        
        _current -=  statisticModifier.Value * _base;
    }

    public bool ContainsStatisticModifier(StatisticModifier statisticModifier)
    {
        return _statisticModifiers.Contains(statisticModifier);
    }
    #endregion Public Methods

    #endregion Methods
}
