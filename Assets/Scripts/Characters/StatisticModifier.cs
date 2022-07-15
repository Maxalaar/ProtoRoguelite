using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StatisticModifier
{
    #region Fields

    #region Serialized Fields
    private StatisticModifierSO _statModSO = null;
    #endregion Serialized Fields

    #region Private Fields
    private List<Statistic> _statistics;
    #endregion Private Fields

    #region Properties
    #endregion Properties

    #endregion Fields

    #region Methods

    #region Constructor
    //TEST
    public StatisticModifier(StatisticModifierSO statModSO, Character character)
    {
        _statModSO = statModSO;

        _statistics = new List<Statistic>();

        AddStatistic(character);
    }
    #endregion Constructor

    #region Unity Interface
    #endregion Unity Interface

    #region Private Methods
    #endregion Private Methods

    #region Public Methods
    public void AddStatistic(Character character)
    {
        if (_statModSO == null)
        {
            Debug.LogWarning("StatisticModifier._statModSO is null !");
            return;
        }

        PropertyInfo[] pptInfos = ReflectionManager.GetProperties<Character>(new List<System.Type> { typeof(Statistic) });

        if (pptInfos == null)
            return;

        for (int i = 0; i < pptInfos.Length; i++)
        {
            if (i == _statModSO.StatisticIndex)
            {
                _statistics.Add(pptInfos[i].GetValue(character) as Statistic);
            }
        }

        //TEST
        foreach (Statistic statistic in _statistics)
        {
            if (statistic == null)
                continue;

            Debug.Log(statistic.Current);

            statistic.AddModifier(_statModSO.Value);

            Debug.Log(statistic.Current);
        }
    }
    #endregion Public Methods

    #endregion Methods
}
