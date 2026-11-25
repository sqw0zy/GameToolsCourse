using UnityEngine;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
public sealed class GameSessionPlayModeTests 
{
    [UnityTest]
    public IEnumerator ApplyProfile_InitializesRuntimeState()
    {
        DifficultyProfile profile =
            ScriptableObject.CreateInstance<DifficultyProfile>();
        var sessionObject = new GameObject("TestSession");
        sessionObject.SetActive(false);
        GameSession session =
            sessionObject.AddComponent<GameSession>();

        Assert.That(session.ApplyProfile(profile), Is.True);
        sessionObject.SetActive(true);
        yield return null;

        Assert.That(session.Score, Is.Zero);
        Assert.That(session.Health,
            Is.EqualTo(profile.PlayerMaxHealth));
        Assert.That(session.RemainingTime,
            Is.EqualTo(profile.LevelDuration).Within(0.01f));

        Object.Destroy(sessionObject);
        Object.Destroy(profile);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Pickup_WhenPlayerEnters_AddsScoreAndIsDestroyed()
    {
        DifficultyProfile profile =
            ScriptableObject.CreateInstance<DifficultyProfile>();
        ItemDefinition item =
            ScriptableObject.CreateInstance<ItemDefinition>();

        var sessionObject = new GameObject("TestSession");
        sessionObject.SetActive(false);
        GameSession session =
            sessionObject.AddComponent<GameSession>();
        session.ApplyProfile(profile);
        sessionObject.SetActive(true);

        GameObject player = GameObject.CreatePrimitive(
            PrimitiveType.Cube);
        player.name = "TestPlayer";
        player.tag = "Player";
        Rigidbody body = player.AddComponent<Rigidbody>();
        body.isKinematic = true;

        GameObject pickup = GameObject.CreatePrimitive(
            PrimitiveType.Sphere);
        pickup.name = "TestPickup";
        pickup.GetComponent<Collider>().isTrigger = true;
        pickup.AddComponent<ConfigurablePickup>()
            .Configure(item, session);

        player.transform.position = Vector3.zero;
        pickup.transform.position = Vector3.zero;
        Physics.SyncTransforms();

        yield return new WaitForFixedUpdate();
        yield return null;

        Assert.That(session.Score, Is.EqualTo(item.ScoreValue));
        yield return new WaitForSeconds(2.5f);
        Assert.That(pickup == null, Is.True);

        Object.Destroy(player);
        Object.Destroy(sessionObject);
        Object.Destroy(item);
        Object.Destroy(profile);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Awake_WithoutProfile_ReportsError()
    {
        LogAssert.Expect(LogType.Error,
            "DifficultyProfile is not assigned.");

        var sessionObject = new GameObject("InvalidSession");
        GameSession session =
            sessionObject.AddComponent<GameSession>();
        yield return null;

        Assert.That(session.enabled, Is.False);
        Object.Destroy(sessionObject);
        yield return null;
    }
}
