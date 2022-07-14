using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statistic
{
    #region Fields

    #region Serialized Fields
    #endregion Serialized Fields

    #region Private Fields
    private float _min;
    private float _max;
    private float _base;
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
    private Statistic(float baseValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _base = baseValue;
        _min = minValue;
        _max = maxValue;
    }
    #endregion Constructor

    #region Unity Interface
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods

    #endregion Public Methods

    #endregion Methods
}
