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
    private List<Statistic> _statistics;
    #endregion Private Fields

    #region Properties
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Constructor
    public StatisticModifier()
    {

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
