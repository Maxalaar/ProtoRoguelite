using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/StatisticModifierSO")]
public class StatisticModifierSO : ScriptableObject
{
    #region Fields

    #region Serialized Fields
    #endregion Serialized Fields

    #region Private Fields
    private string _name;
    private string _description;
    private Image _image;

    private Statistic[] _statistics;
    private float _value;
    private float _duration;
    #endregion Private Fields

    #region Properties
    public string Name => _name;
    public string Description => _description;
    public Image Image => _image;
    public Statistic[] Statistics => _statistics;
    public float Value => _value;
    public float Duration => _duration;
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Unity Interface
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    #endregion Public Methods

    #endregion Methods
}

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
    public void addStatistic(Statistic statistic)
    {
        if (statistic.Type == _targetStatistics)
        {
            if (_statistis.Contains(statistic) == true)
            {
                Debug.LogWarning("Attempt to add to the StatisticModifier a Statistic that is already modified by it");
                return;
            }
            if (statistic.StatisticModifiers.Contains(this) == true)
            {
                Debug.LogWarning("Attempt to add to the Statistic a StatisticModifier that is already modified by it");
                return;
            }
            _statistis.Add(statistic);
            statistic.StatisticModifiers.Add(this);
            statistic.computeCurrentValue();
        }
        else
        {
            Debug.LogWarning("Miss match between StatisticModifier._targetStatistics and Statistic._type");
            return;
        }
    }
    public void supStatistic(Statistic statistic)
    {
        if (statistic.Type == _targetStatistics)
        {
            if (_statistis.Contains(statistic) == false)
            {
                Debug.LogWarning("Attempt to sup Statistic to the StatisticModifier but is not modified by it");
                return;
            }
            if (statistic.StatisticModifiers.Contains(this) == false)
            {
                Debug.LogWarning("Attempt to sup StatisticModifier to the Statistic but is not modified by it");
                return;
            }
            _statistis.Remove(statistic);
            statistic.StatisticModifiers.Remove(this);
            statistic.computeCurrentValue();
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
