using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public sealed class SceneLoadingTests
{
    [UnityTest]
    public IEnumerator Lab12Scene_ContainsActiveGameSession()
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(
            "Lab12_AutomatedTesting", LoadSceneMode.Single);
        yield return loading;
        yield return null;

        GameSession session =
            Object.FindFirstObjectByType<GameSession>();
        Assert.That(session, Is.Not.Null);
        Assert.That(session.enabled, Is.True);
        Assert.That(session.Health, Is.GreaterThan(0));
        Assert.That(session.RemainingTime, Is.GreaterThan(0f));
    }
}