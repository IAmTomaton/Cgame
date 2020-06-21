using Cgame.Core;
using NUnit.Framework;
using OpenTK;
using System.Collections.Generic;

namespace NUnitTestProject
{
    class ColliderTests
    {
        [Test]
        public void TestСircularCollider()
        {
            var first = new Collider(10);
            var second = new Collider(10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(20, result.MtvLength);
        }

        [Test]
        public void TestRectangularCollider()
        {
            var first = new Collider(10, 10);
            var second = new Collider(10, 10);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(10, result.MtvLength);
        }

        [Test]
        public void TestRectangularRotationCollider()
        {
            var first = new Collider(10, 10).Transform(Vector2.Zero, 45);
            var second = first.Transform(new Vector2(10, 0), 0);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(2.9, result.MtvLength, 0.1);
        }

        [Test]
        public void TestArbitraryCollider()
        {
            var first = new Collider(new List<Vector2>() { new Vector2(10, 10), new Vector2(10, -10), new Vector2(0, 10) }, Vector2.Zero);
            var second = new Collider(new List<Vector2>() { new Vector2(10, 10), new Vector2(10, -10), new Vector2(0, 10) }, Vector2.Zero);

            var result = Collider.Collide(first, second);

            Assert.AreEqual(8.9, result.MtvLength, 0.1);
        }
    }
}
