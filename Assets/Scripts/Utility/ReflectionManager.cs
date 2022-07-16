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

        //go through all properties of the type
        foreach (PropertyInfo pptInfo in pptInfos)
        {
            //if the property's type matches a specific type, add it to the list
            foreach (Type type in specificTypes)
            {
                if (pptInfo.PropertyType == type)
                {
                    pptInfosList.Add(pptInfo);
                    break;
                }
            }
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
