using NUnit.Framework;
using UnityEngine;

public sealed class DifficultyProfileTests 
{
    [Test]
    public void NewProfile_HasPositiveDefaults()
    {
        DifficultyProfile profile =
            ScriptableObject.CreateInstance<DifficultyProfile>();
        try
        {
            Assert.That(profile.PlayerMaxHealth,
                Is.GreaterThan(0));
            Assert.That(profile.LevelDuration,
                Is.GreaterThan(0f));
            Assert.That(profile.RequiredScore,
                Is.GreaterThan(0));
        }
        finally
        {
            Object.DestroyImmediate(profile);
        }
    }
}
