using System;
using System.Collections;
using NUnit.Framework;
using Tests.Editor;

namespace Modules.Converter.Tests
{
    public class CouldDownTests
    {
        private static IEnumerable TickSource() => new object[]
        {
            new object[] { 1, 1, 1 },
            new object[] { 5, 6, 1 },
        }.FormatAsObjects(args => $"interval: {args[0]}; deltaTime: {args[1]}; expectedCompleteCount: {args[2]}");

        [TestCaseSource(nameof(TickSource))]
        public void Tick(float interval, float deltaTime, int expectedCompleteCount)
        {
            var couldDown = new CouldDown(interval);
            var completeCount = 0;
            couldDown.OnComplete += () => completeCount++;
            couldDown.Start();

            couldDown.Tick(deltaTime);

            Assert.AreEqual(expectedCompleteCount, completeCount);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void WhenIncorrectIntervalThenException(float interval) => Assert.Throws<ArgumentException>(() => new CouldDown(interval));

        [TestCase(-1)]
        public void WhenIncorrectDeltaTimeThenException(float deltaTime)
        {
            var couldDown = new CouldDown(1);
            
            Assert.Throws<ArgumentException>(() => couldDown.Tick(deltaTime));
        }
    }
}