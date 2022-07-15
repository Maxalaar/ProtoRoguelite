using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeSandbox : MonoBehaviour
{
    void Start()
    {
        Statistic domageStatistic1 = new Statistic(StatisticsEnum.Damage, 10f);
        Statistic domageStatistic2 = new Statistic(StatisticsEnum.Damage, 20f);

        Statistic domageArea1 = new Statistic(StatisticsEnum.AreaOfEffect, 5f);

        StatisticModifier domageModifier1 = new StatisticModifier(StatisticsEnum.Damage, 0.5f);
        StatisticModifier domageModifier2 = new StatisticModifier(StatisticsEnum.Damage, 2f);

        print("domageStatistic1: " + domageStatistic1.Base.ToString());
        print("domageArea1: " + domageArea1.Current.ToString());

        domageModifier1.addStatistic(domageStatistic1);
        domageModifier1.addStatistic(domageStatistic2);
        domageModifier1.addStatistic(domageArea1);

        print("domageStatistic1: " + domageStatistic1.Current.ToString());
        print("domageArea1: " + domageArea1.Current.ToString());

        domageModifier1.supStatistic(domageStatistic1);
        domageModifier2.addStatistic(domageStatistic2);

        print("domageStatistic1: " + domageStatistic1.Current.ToString());
        print("domageArea1: " + domageArea1.Current.ToString());


    }
}