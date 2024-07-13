using System.Collections.Generic;
using NUnit.Framework;
using software.anthill;
using UnityEngine;

namespace Tests
{
    public class PathTests
    {
        private readonly Vector3[] _emptyPath = new List<Vector3>().ToArray();

        private readonly Vector3[] _onePointPath = new List<Vector3>()
        {
            Vector3.one
        }.ToArray();
    
        private readonly Vector3[] _pathOfLengthOfTwo = new List<Vector3>()
        {
            new(0f, 0f, 0f),
            new(1f, 0f, 0f),
            new(1f, 1f, 0f)
        }.ToArray();
    
        [Test]
        public void ShouldReturnZeroLengthForEmptyPath()
        {
            Assert.That(Path.Length(_emptyPath), Is.EqualTo(0));
        }

        [Test]
        public void ShouldReturnZeroLengthForOnePointPath()
        {
            Assert.That(Path.Length(_onePointPath), Is.EqualTo(0));
        }
    
        [Test]
        public void ShouldCalculateLengthOfPath()
        {
            Assert.That(Path.Length(_pathOfLengthOfTwo), Is.EqualTo(2.0f));
        }
    
        [Test]
        public void ShouldReturnEmptyPathOnEmptyPathLimiting()
        {
            Assert.That(Path.Limit(_emptyPath, 5.0f), Is.Empty);
        }
    
        [Test]
        public void ShouldReturnTheSamePathOnOnePointPathLimiting()
        {
            Assert.That(Path.Limit(_onePointPath, 1.5f), Is.EqualTo(_onePointPath));
        }
    
        [Test]
        public void ShouldNotLimitIfPathLengthShorterThenLimit()
        {
            Assert.That(Path.Limit(_pathOfLengthOfTwo, 5.0f), Is.EqualTo(_pathOfLengthOfTwo));
        }
    
        [Test]
        public void ShouldLimitIfPathLengthExceedsLimit()
        {
            var longPath = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0)
            };

            var expectedPath = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0)
            };
            Assert.That(Path.Limit(longPath, 2.0f), Is.EqualTo(expectedPath));
        }
    
        [Test]
        public void ShouldLimitToExpectedLengthIfCutIsOnTheLastSegment()
        {
            var longPath = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(6, 8, 0)
            };

            var expectedPath = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(3, 4, 0)
            };
            
            Assert.That(Path.Limit(longPath, 5.0f), Is.EqualTo(expectedPath));
        }
    
        [Test]
        public void ShouldAlsoLimitFromEnd()
        {
            var longPath = new[]
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0)
            };

            var expectedPath = new[]
            {
                new Vector3(1, 0, 0),
                new Vector3(2, 0, 0),
                new Vector3(3, 0, 0)
            };
            Assert.That(Path.LimitFromEnd(longPath, 2.0f), Is.EqualTo(expectedPath));
        }
    }
}
