using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ReflectionManager : MonoBehaviour
{
    public static PropertyInfo[] GetProperties<T>(List<Type> specificTypes = null)
    {
        //get all properties of the type T
        PropertyInfo[] pptInfos = typeof(T).GetProperties();

        //if no specific type is wanted, return all properties
        if (specificTypes == null)
            return pptInfos;

        List<PropertyInfo> pptInfosList = new List<PropertyInfo>();
        pptInfosList.AddRange(pptInfos);

        List<PropertyInfo> pptToRemoveList = new List<PropertyInfo>();

        //go through all properties of the type
        foreach (PropertyInfo pptInfo in pptInfosList)
        {
            bool hasIncorrectType = true;

            //if the property's type matches a specific type, it won't be removed
            foreach (Type type in specificTypes)
            {
                if (pptInfo.PropertyType == type)
                    hasIncorrectType = false;
            }

            //add the incorrect type to the remove list
            if (hasIncorrectType)
            {
                pptToRemoveList.Add(pptInfo);
            }
        }

        //remove properties to remove from the list that will be returned
        foreach (PropertyInfo pptToRemove in pptToRemoveList)
        {
            if (pptInfosList.Contains(pptToRemove))
                pptInfosList.Remove(pptToRemove);
        }

        return pptInfosList.ToArray();
    }

    public static string[] GetPropertyNames(PropertyInfo[] pptInfos)
    {
        //get all names of the properties given
        string[] pptNames = new string[pptInfos.Length];

        for (int i = 0; i < pptInfos.Length; i++)
        {
            pptNames[i] = pptInfos[i].Name;
        }

        return pptNames;
    }
}
