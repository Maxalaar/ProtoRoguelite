using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Statistic
{
    #region Fields

    #region Serialized Fields
    [SerializeField] private float _min;
    [SerializeField] private float _max;
    [SerializeField] private float _base;
    #endregion Serialized Fields

    #region Private Fields
    private float _current;
    #endregion Private Fields

    #region Properties
    public float Min => _min;
    public float Max => _max;
    public float Base => _base;
    public float Current => _current;
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Constructor
    public Statistic(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _base = baseValue;
        _min = minValue;
        _max = maxValue;

        _current = _base;
    }
    #endregion Constructor

    #region Unity Interface
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    //TEST
    public void AddModifier(float value)
    {
        _current += _base * value;
    }
    #endregion Public Methods

    #endregion Methods
}
