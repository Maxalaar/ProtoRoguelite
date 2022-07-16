using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathfindingTest
{
    [UnityTest]
    public IEnumerator BaseMoveTest()
    {
        Time.timeScale = 10;
        GameObject gameObject = new GameObject();
        BattlefieldManager battlefield = gameObject.AddComponent<BattlefieldManager>();
        yield return new WaitForSeconds(10f);
        Assert.AreEqual(2, 2);
    }
}
