using UnityEngine;
using UnityEditor;
using System.Linq;
using NUnit.Framework;
public sealed class ConfigurationAssetsTests 
{
    [Test]
    public void ItemDefinitions_HaveUniqueNonEmptyIds()
    {
        ItemDefinition[] items = AssetDatabase
            .FindAssets("t:ItemDefinition")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<ItemDefinition>)
            .Where(item => item != null)
            .ToArray();

        Assert.That(items, Has.Length.GreaterThanOrEqualTo(3));
        Assert.That(items.Select(item => item.Id),
            Is.All.Not.Empty);
        Assert.That(items.Select(item => item.Id), Is.Unique);
        Assert.That(items.Select(item => item.ScoreValue),
            Is.All.GreaterThan(0));
    }

    [Test]
    public void NpcConfigs_PreserveDistanceAndSpeedInvariants()
    {
        string[] paths = AssetDatabase.FindAssets("t:NpcConfig")
            .Select(AssetDatabase.GUIDToAssetPath).ToArray();

        Assert.That(paths, Has.Length.GreaterThanOrEqualTo(2));
        foreach (string path in paths)
        {
            NpcConfig config =
                AssetDatabase.LoadAssetAtPath<NpcConfig>(path);
            Assert.That(config.ChaseSpeed,
                Is.GreaterThanOrEqualTo(config.PatrolSpeed), path);
            Assert.That(config.LoseDistance,
                Is.GreaterThan(config.DetectionDistance), path);
        }
    }
}
