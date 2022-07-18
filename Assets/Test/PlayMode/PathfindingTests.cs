using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class PathfindingTest
{
    [UnityTest]
    public IEnumerator BaseMoveTest()
    {
        Time.timeScale = 10;
        GameObject gameObject = new GameObject();
        Character a = new Character();
        // BattlefieldManager battlefield = gameObject.AddComponent<BattlefieldManager>();
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Example.prefab");
        yield return new WaitForSeconds(10f);
        Assert.AreEqual(2, 2);
    }
}
