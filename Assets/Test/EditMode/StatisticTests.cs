using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using ProtoRoguelite.Characters;
using ProtoRoguelite.Statistics;

public class StatisticTests
{
    [Test]
    public void BaseValue()
    {
        float baseValue = 10f;
        Statistic domageStatistic1 = new Statistic(StatisticsEnum.Damage, baseValue);
        Assert.AreEqual(baseValue, domageStatistic1.Base);
    }

    [Test]
    public void CurrentValue()
    {
        float baseValue = 10f;
        Statistic domageStatistic1 = new Statistic(StatisticsEnum.Damage, baseValue);
        
        StatisticModifier domageModifier1 = new StatisticModifier(StatisticsEnum.Damage, 0.3f);
        StatisticModifier domageModifier2 = new StatisticModifier(StatisticsEnum.Damage, 1.2f);

        domageModifier1.AddStatistic(domageStatistic1);
        domageModifier2.AddStatistic(domageStatistic1);
        
        float currentValue = baseValue + 0.3f * baseValue + 1.2f * baseValue;
        Assert.AreEqual(currentValue, domageStatistic1.Current);
    }
}