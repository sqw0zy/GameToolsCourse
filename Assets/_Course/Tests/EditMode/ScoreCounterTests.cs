using NUnit.Framework;
using UnityEngine;
using Course.Gameplay;
using System;

public sealed class ScoreCounterTests
{
    [TestCase(5, 1, 1)]
    [TestCase(5, 3, 3)]
    [TestCase(10, 4, 4)]
    public void Add_PositiveAmount_ChangesScore(
        int target, int amount, int expected)
    {
        //Arrange
        var counter = new ScoreCounter(target);

        //Act
        bool completed = counter.Add(amount);

        //Assert
        Assert.That(counter.Score, Is.EqualTo(expected));
        Assert.That(completed, Is.False);
    }

    [Test]
    public void Add_WhenTargetReached_ReturnsTrue()
    {
        var counter = new ScoreCounter(5);
        bool completed = counter.Add(5);

        Assert.That(completed, Is.True);
        Assert.That(counter.IsComplete, Is.True);
    }

    [Test]
    public void Add_NonPositiveAmount_DoesNotChangeScore()
    {
        var counter = new ScoreCounter(5);

        bool completed = counter.Add(0);

        Assert.That(completed, Is.False);
        Assert.That(counter.Score, Is.Zero);
    }

    [Test]
    public void Constructor_NonPositiveTarget_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ScoreCounter(0));
    }
}
