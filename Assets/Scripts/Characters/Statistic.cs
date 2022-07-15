using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistic
{
    #region Fields

    #region Serialized Fields
    #endregion Serialized Fields

    #region Private Fields
    private StatisticsEnum _type;
    private float _min;
    private float _max;
    private float _base;
    private float _current;
    List<StatisticModifier> _statisticModifiers = new List<StatisticModifier>();
    #endregion Private Fields

    #region Properties
    public StatisticsEnum Type => _type;
    public float Min => _min;
    public float Max => _max;
    public float Base => _base;
    public float Current => _current;
    public List<StatisticModifier> StatisticModifiers => _statisticModifiers;   //Add computeCurrentValue() here and remouve in StatisticModifier and modification for < O(n)

    #endregion Properties

    #endregion Fields

    #region Methods

    #region Constructor
    public Statistic(StatisticsEnum typeValue, float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _type = typeValue;
        _base = baseValue;
        _min = minValue;
        _max = maxValue;
        computeCurrentValue();
    }
    #endregion Constructor

    #region Unity Interface
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    public void computeCurrentValue()
    {
        float currentValueTemp = _base;
        foreach (StatisticModifier StatisticModifier in _statisticModifiers)
        {
            currentValueTemp += StatisticModifier.Value * _base;
        }
        
        _current =  currentValueTemp;
    }
    #endregion Public Methods

    #endregion Methods
}
