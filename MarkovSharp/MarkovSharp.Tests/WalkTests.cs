﻿using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MarkovSharp.TokenisationStrategies;
using NUnit.Framework;

namespace MarkovSharp.Tests
{
    [TestFixture]
    public class WalkTests : BaseMarkovTests
    {
        private readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Test]
        public void BasicWalkOnTrainedModelGeneratesCorrectNumberOfLines()
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);
            var result = model.Walk(1);

            Assert.AreEqual(1, result.Count());
            Logger.Info(result.First());
        }

        [Test]
        public void WalkOnUntrainedModelIsEmpty()
        {
            var model = new StringMarkov();
            var result = model.Walk();

            CollectionAssert.AreEqual(new List<string> { string.Empty }, result);
        }
        
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]

        public void WalkOnTrainedModelGeneratesCorrectNumberOfLines(int lineCount)
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);
            var result = model.Walk(lineCount);

            Assert.AreEqual(lineCount, result.Count());
        }

        [TestCase(int.MinValue)]
        [TestCase(-1)]
        [TestCase(0)]
        public void MustCallWalkWithPositiveInteger(int lineCount)
        {
            var model = new StringMarkov();
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                var x = model.Walk(lineCount).ToList();
            });
            Assert.AreEqual("Invalid argument - line count for walk must be a positive integer\r\nParameter name: lines", ex.Message);
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        public void WalkCreatesNewContent(int walkCount)
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);
            model.EnsureUniqueWalk = true;

            var results = model.Walk(walkCount);
            
            CollectionAssert.IsNotSubsetOf(results, ExampleData);
            foreach (var result in results)
            {
                Assert.That(result, Is.Not.Empty);
                CollectionAssert.DoesNotContain(ExampleData, result);
            }
        }

        [Test]
        public void CanWalkWithUniqueOutputUsingSeed()
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);
            model.EnsureUniqueWalk = true;

            var results = model.Walk(1000, "This is a line");
            foreach (var result in results)
            {
                Assert.That(result, Is.StringStarting("This is a line"));
            }

            Assert.AreEqual(results.Count(), results.Distinct().Count());
        }

        [Test]
        public void CanWalkWithUniqueOutput()
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);
            model.EnsureUniqueWalk = true;

            var results = model.Walk(1000);
            Assert.AreEqual(results.Count(), results.Distinct().Count());
        }

        [Test]
        public void CanWalkUsingSeed()
        {
            var model = new StringMarkov();
            model.Learn(ExampleData);

            var results = model.Walk(100, "This is a line");

            Assert.AreEqual(100, results.Count());
            foreach (var result in results)
            {
                Assert.That(result, Is.StringStarting("This is a line"));
            }
        }
    }
}
