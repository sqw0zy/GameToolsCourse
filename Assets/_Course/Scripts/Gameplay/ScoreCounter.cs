using NUnit.Framework.Constraints;
using UnityEngine;
using System;

namespace Course.Gameplay
{
    public sealed class ScoreCounter
    {
        public int Score { get; private set; }
        public int RequiredScore { get; }
        public bool IsComplete => Score >= RequiredScore;

        public ScoreCounter(int requiredScore)
        {
            if (requiredScore <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requiredScore));
            }
            RequiredScore = requiredScore;
        }

        public bool Add(int amount)
        {
            if (amount <= 0) return false;

            Score += amount;
            return IsComplete;
        }
    }
}
